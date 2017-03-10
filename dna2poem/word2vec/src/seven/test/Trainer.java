package seven.test;

import java.io.File;
import java.io.FileInputStream;
import java.io.FileOutputStream;
import java.io.OutputStream;
import java.io.PrintWriter;
import java.util.Random;

public class Trainer {
	private static final int EXP_TABLE_SIZE = 1000;
	private static final double MAX_EXP = 6;

	private double[] syn0, syn1, syn1neg, exptbl;
	private double alpha, starting_alpha, sample;
	private boolean cbow, binary, hs;
	private int layer, window, negative, threads, iter;
	private long min_count, min_reduce;
	private String trainfile;
	private VocabTable vocab_table;
	private UnigramTable unigram_table;
	private long word_count_actual, train_words;
	private boolean running;

	public Trainer() {
		this(
			/* alpha = */ 0.025, /* sample = */ 1e-3, /* layer = */ 100,
			/* min_count = */ 5, /* min_reduce = */ 1, /* hs = */ false,
			/* cbow = */ true, /* window = */ 5, /* negative = */ 5,
			/* binary = */ false, /* iter = */ 5, /* threads = */ 12);
	}

	public Trainer set(String key, String value) {
		if ("alpha".equals(key)) {
			this.alpha = Double.parseDouble(value);
			return this;
		}
		if ("sample".equals(key)) {
			this.sample = Double.parseDouble(value);
			return this;
		}
		if ("layer".equals(key)) {
			this.layer = Integer.parseInt(value);
			return this;
		}
		if ("min_count".equals(key)) {
			this.min_count = Integer.parseInt(value);
			return this;
		}
		if ("min_reduce".equals(key)) {
			this.min_reduce = Integer.parseInt(value);
			return this;
		}
		if ("hs".equals(key)) {
			this.hs = Boolean.parseBoolean(value);
			return this;
		}
		if ("cbow".equals(key)) {
			this.cbow = Boolean.parseBoolean(value);
			return this;
		}
		if ("window".equals(key)) {
			this.window = Integer.parseInt(value);
			return this;
		}
		if ("negative".equals(key)) {
			this.negative = Integer.parseInt(value);
			return this;
		}
		if ("binary".equals(key)) {
			this.binary = Boolean.parseBoolean(value);
			return this;
		}
		if ("iter".equals(key)) {
			this.iter = Integer.parseInt(value);
			return this;
		}
		if ("threads".equals(key)) {
			this.threads = Integer.parseInt(value);
			return this;
		}
		if ("trainfile".equals(key)) {
			this.trainfile = value;
			return this;
		}
		return this;
	}

	public Trainer(double alpha, double sample, int layer, long min_count, long min_reduce, boolean hs, boolean cbow,
			int window, int negative, boolean binary, int iter, int threads) {
		this.alpha = alpha;
		this.starting_alpha = alpha;
		this.sample = sample;
		this.layer = layer;
		if (this.layer <= 0)
			this.layer = 100;
		this.min_count = min_count;
		this.min_reduce = min_reduce;
		this.window = window;
		this.negative = negative;
		this.binary = binary;
		this.cbow = cbow;
		this.hs = hs;
		this.iter = iter;
		this.threads = threads;
		this.trainfile = null;
		this.vocab_table = null;
		this.unigram_table = null;
		this.syn0 = this.syn1 = this.syn1neg = this.exptbl = null;
		this.running = false;
		this.word_count_actual = this.train_words = 0;

		this.exptbl = new double[EXP_TABLE_SIZE + 1];
		for (int i = 0; i < EXP_TABLE_SIZE; i++) {
			// Precompute the exp() table
			this.exptbl[i] = Math.exp((i * 2.0 / EXP_TABLE_SIZE - 1) * MAX_EXP);
			// Precompute f(x) = x / (x + 1)
			this.exptbl[i] = this.exptbl[i] / (this.exptbl[i] + 1);
		}
	}

	private void initNet() {
		int a, b, n;
		Random rnd = new Random();
		n = vocab_table.size();
		syn0 = new double[n * layer];
		syn1 = new double[n * layer];
		syn1neg = new double[n * layer];
		for (a = 0; a < n; a++) {
			for (b = 0; b < layer; b++) {
				syn0[a * layer + b] = (rnd.nextInt(0xffff) / 65536.0 - 0.5) / layer;
			}
		}
		vocab_table.buildBinaryTree();
	}

	public Trainer netlink() {
		initNet();
		if (negative > 0) {
			unigram_table = new UnigramTable(vocab_table);
		} else {
			unigram_table = new UnigramTable();
		}
		return this;
	}

	public Trainer reset() {
		vocab_table = null;
		return this;
	}

	public Trainer load(boolean new_one) {
		return load(trainfile, true, new_one);
	}

	public Trainer load(String filename, boolean count, boolean new_one) {
		if (vocab_table == null || new_one) {
			vocab_table = new VocabTable();
		}
		if (count) {
			train_words = vocab_table.count(filename, min_reduce, min_count);
		} else {
			vocab_table.load(filename);
		}
		return this;
	}

	public Trainer train() {
		if (running) {
			return this;
		}
		running = true;
		TrainerThread[] workers = new TrainerThread[threads];
		for (int i = 0; i < threads; i++) {
			workers[i] = new TrainerThread(i, this);
			workers[i].start();
		}
		try {
			for (int i = 0; i < threads; i++) {
				workers[i].join();
			}
		} catch (InterruptedException e) {
			e.printStackTrace();
		}
		running = false;
		return this;
	}

	public void result() {
		int a = 0;
		for (VocabWord v : vocab_table) {
			v.vec = new double[layer];
			System.arraycopy(syn0, a * layer, v.vec, 0, layer);
			// normalize vector
			double mod = 0;
			for (double x : v.vec) {
				mod += x * x;
			}
			mod = Math.sqrt(mod);
			for (int j = 0; j < v.vec.length; j++) {
				v.vec[j] /= mod;
			}
			a ++;
		}
	}

	public Trainer saveVocabs(String filename) {
		vocab_table.save(filename);
		return this;
	}

	public Trainer saveClasses(String filename, int classes) {
		// Run K-means on the word vectors
		int a, b, c, d;
		int clcn = classes, iter = 10, closeid;
		int n = vocab_table.size();
		int[] centcn = new int[classes];
		int[] cl = new int[n];
		double closev, x;
		double[] cent = new double[classes * layer];
		for (a = 0; a < n; a++)
			cl[a] = a % clcn;
		for (a = 0; a < iter; a++) {
			for (b = 0; b < clcn * layer; b++) {
				cent[b] = 0;
			}
			for (b = 0; b < clcn; b++) {
				centcn[b] = 1;
			}
			for (c = 0; c < n; c++) {
				for (d = 0; d < layer; d++) {
					cent[layer * cl[c] + d] += syn0[c * layer + d];
				}
				centcn[cl[c]]++;
			}
			for (b = 0; b < clcn; b++) {
				closev = 0;
				for (c = 0; c < layer; c++) {
					cent[layer * b + c] /= centcn[b];
					closev += cent[layer * b + c] * cent[layer * b + c];
				}
				closev = Math.sqrt(closev);
				for (c = 0; c < layer; c++) {
					cent[layer * b + c] /= closev;
				}
			}
			for (c = 0; c < n; c++) {
				closev = -10;
				closeid = 0;
				for (d = 0; d < clcn; d++) {
					x = 0;
					for (b = 0; b < layer; b++) {
						x += cent[layer * d + b] * syn0[c * layer + b];
					}
					if (x > closev) {
						closev = x;
						closeid = d;
					}
				}
				cl[c] = closeid;
			}
		}
		try {
			PrintWriter writer = new PrintWriter(filename);
			a = 0;
			for (VocabWord v : vocab_table) {
				writer.printf("%s %d\n", v.word, cl[a]);
				a++;
			}
			writer.close();
		} catch (Exception e) {
			e.printStackTrace();
			// TODO: handle i/o error
		}
		return this;
	}

	public Trainer saveVector(String filename) {
		try {
			if (binary) {
				FileOutputStream writer = new FileOutputStream(filename);
				int a = 0;
				writer.write((vocab_table.size() + "").getBytes());
				writer.write(' ');
				writer.write((layer + "").getBytes());
				writer.write('\n');
				for (VocabWord v : vocab_table) {
					writer.write(v.word.getBytes());
					writer.write(' ');
					for (int b = 0; b < layer; b++) {
                                               /*
                                                * if you would like to make output compatible with Google word2vec in C version,
                                                * use wirte_int:
                                                * write_int(writer, Float.floatToRawIntBits((float)syn0[a * layer + b]));
                                                */
						write_long(writer, Double.doubleToRawLongBits(syn0[a * layer + b]));
					}
					a++;
					writer.write('\n');
				}
				writer.close();
			} else {
				PrintWriter writer = new PrintWriter(filename);
				int a = 0;
				writer.printf("%d %d\n", vocab_table.size(), layer);
				for (VocabWord v : vocab_table) {
					writer.print(v.word);
					writer.print(' ');
					for (int b = 0; b < layer; b++) {
						writer.printf("%f ", syn0[a * layer + b]);
					}
					a++;
					writer.print('\n');
				}
				writer.close();
			}
		} catch (Exception e) {
			e.printStackTrace();
			// TODO: handle i/o error
		}
		return this;
	}

	public static void write_long(OutputStream out, long x) throws Exception {
		out.write((int) ((x >>> 0) & 0xff));
		out.write((int) ((x >>> 8) & 0xff));
		out.write((int) ((x >>> 16) & 0xff));
		out.write((int) ((x >>> 24) & 0xff));
		out.write((int) ((x >>> 32) & 0xff));
		out.write((int) ((x >>> 40) & 0xff));
		out.write((int) ((x >>> 48) & 0xff));
		out.write((int) ((x >>> 56) & 0xff));
	}

	protected static void write_int(OutputStream out, int x) throws Exception {
		out.write((int) ((x >>> 0) & 0xff));
		out.write((int) ((x >>> 8) & 0xff));
		out.write((int) ((x >>> 16) & 0xff));
		out.write((int) ((x >>> 24) & 0xff));
	}

	protected static class TrainerThread extends Thread {
		private static final int MAX_SENTENCE_LENGTH = 4096;

		private int id, iter, layer, threads, window, negative;
		private boolean cbow, hs;
		private long train_words;
		private String trainfile;
		private Trainer trainer;
		private Random rnd;

		public TrainerThread(int worker_id, Trainer trainer) {
			this.id = worker_id;
			this.iter = trainer.iter;
			this.layer = trainer.layer;
			this.window = trainer.window;
			this.threads = trainer.threads;
			this.cbow = trainer.cbow;
			this.hs = trainer.hs;
			this.trainfile = trainer.trainfile;
			this.train_words = trainer.train_words;
			this.negative = trainer.negative;
			this.trainer = trainer;
			this.rnd = new Random();
		}

		@Override
		public void run() {
			int a, b, d, cw, word, last_word, sentence_length = 0, sentence_position = 0;
			int n = trainer.vocab_table.size();
			long word_count = 0, last_word_count = 0;
			int[] sen = new int[MAX_SENTENCE_LENGTH + 1];
			int l1, l2, c, target, label, local_iter = iter;
			double f, g;
			double[] neu1 = new double[layer];
			double[] neu1e = new double[layer];
			try {
				FileInputStream fi;
				fi = new FileInputStream(new File(trainfile));
				int file_size;
				file_size = fi.available();
				fi.skip(file_size / threads * id);
				Tokenizer tokenizer = new Tokenizer(fi);
				while (true) {
					synchronized (trainer) {
						if (word_count - last_word_count > 10000) {
							trainer.word_count_actual += word_count - last_word_count;
							last_word_count = word_count;
							trainer.alpha = trainer.starting_alpha
									* (1 - trainer.word_count_actual / (double) (iter * train_words + 1));
							if (trainer.alpha < trainer.starting_alpha * 0.0001) {
								trainer.alpha = trainer.starting_alpha * 0.0001;
							}
						}
						if (sentence_length == 0) {
							while (true) {
								word = trainer.vocab_table.indexOf(tokenizer.next());
								if (tokenizer.eof())
									break;
								if (word == -1)
									continue;
								word_count++;
								if (word == 0)
									break;
								// The subsampling randomly discards frequent
								// words while keeping the ranking same
								if (trainer.sample > 0) {
									double ran = (Math.sqrt(trainer.vocab_table.get(word).count
											/ (trainer.sample * trainer.train_words)) + 1)
											* (trainer.sample * trainer.train_words)
											/ trainer.vocab_table.get(word).count;
									if (ran < rnd.nextInt(0xffff) / 65536.0)
										continue;
								}
								sen[sentence_length] = word;
								sentence_length++;
								if (sentence_length >= MAX_SENTENCE_LENGTH)
									break;
							}
							sentence_position = 0;
						}
						if (tokenizer.eof() || word_count > train_words / threads) {
							trainer.word_count_actual += word_count - last_word_count;
							local_iter--;
							if (local_iter == 0)
								break;
							word_count = 0;
							last_word_count = 0;
							sentence_length = 0;
							fi.close();
							fi = new FileInputStream(new File(trainfile));
							fi.skip(file_size / threads * id);
							continue;
						}
						word = sen[sentence_position];
						if (word == -1)
							continue;
						for (c = 0; c < layer; c++)
							neu1[c] = 0;
						for (c = 0; c < layer; c++)
							neu1e[c] = 0;
						b = rnd.nextInt(window);
						if (cbow) { // train the cbow architecture
							// in -> hidden
							cw = 0;
							for (a = b; a < window * 2 + 1 - b; a++)
								if (a != window) {
									c = sentence_position - window + a;
									if (c < 0)
										continue;
									if (c >= sentence_length)
										continue;
									last_word = sen[c];
									if (last_word == -1)
										continue;
									for (c = 0; c < layer; c++) {
										neu1[c] += trainer.syn0[c + last_word * layer];
									}
									cw++;
								}
							if (cw != 0) {
								for (c = 0; c < layer; c++) {
									neu1[c] /= cw;
								}
								if (hs)
									for (d = 0; d < trainer.vocab_table.get(word).codelen; d++) {
										f = 0;
										l2 = trainer.vocab_table.get(word).point[d] * layer;
										// Propagate hidden -> output
										for (c = 0; c < layer; c++)
											f += neu1[c] * trainer.syn1[c + l2];
										if (f <= -MAX_EXP)
											continue;
										else if (f >= MAX_EXP)
											continue;
										else
											f = trainer.exptbl[(int) ((f + MAX_EXP) * (EXP_TABLE_SIZE / MAX_EXP / 2))];
										// 'g' is the gradient multiplied by the
										// learning rate
										g = (1 - trainer.vocab_table.get(word).code[d] - f) * trainer.alpha;
										// Propagate errors output -> hidden
										for (c = 0; c < layer; c++)
											neu1e[c] += g * trainer.syn1[c + l2];
										// Learn weights hidden -> output
										for (c = 0; c < layer; c++)
											trainer.syn1[c + l2] += g * neu1[c];
									}
								// NEGATIVE SAMPLING
								if (negative > 0)
									for (d = 0; d < negative + 1; d++) {
										if (d == 0) {
											target = word;
											label = 1;
										} else {
											target = trainer.unigram_table.random(rnd);
											if (target == 0)
												target = rnd.nextInt(n - 1) + 1;
											if (target == word)
												continue;
											label = 0;
										}
										l2 = target * layer;
										f = 0;
										for (c = 0; c < layer; c++)
											f += neu1[c] * trainer.syn1neg[c + l2];
										if (f > MAX_EXP)
											g = (label - 1) * trainer.alpha;
										else if (f < -MAX_EXP)
											g = (label - 0) * trainer.alpha;
										else
											g = (label - trainer.exptbl[(int) ((f + MAX_EXP)
													* (EXP_TABLE_SIZE / MAX_EXP / 2))]) * trainer.alpha;
										for (c = 0; c < layer; c++)
											neu1e[c] += g * trainer.syn1neg[c + l2];
										for (c = 0; c < layer; c++)
											trainer.syn1neg[c + l2] += g * neu1[c];
									}
								// hidden -> in
								for (a = b; a < window * 2 + 1 - b; a++) {
									if (a != window) {
										c = sentence_position - window + a;
										if (c < 0)
											continue;
										if (c >= sentence_length)
											continue;
										last_word = sen[c];
										if (last_word == -1)
											continue;
										for (c = 0; c < layer; c++)
											trainer.syn0[c + last_word * layer] += neu1e[c];
									}
								}
							}
						} else { // train skip-gram
							for (a = b; a < window * 2 + 1 - b; a++)
								if (a != window) {
									c = sentence_position - window + a;
									if (c < 0)
										continue;
									if (c >= sentence_length)
										continue;
									last_word = sen[c];
									if (last_word == -1)
										continue;
									l1 = last_word * layer;
									for (c = 0; c < layer; c++)
										neu1e[c] = 0;
									// HIERARCHICAL SOFTMAX
									if (hs)
										for (d = 0; d < trainer.vocab_table.get(word).codelen; d++) {
											f = 0;
											l2 = trainer.vocab_table.get(word).point[d] * layer;
											// Propagate hidden -> output
											for (c = 0; c < layer; c++)
												f += trainer.syn0[c + l1] * trainer.syn1[c + l2];
											if (f <= -MAX_EXP)
												continue;
											else if (f >= MAX_EXP)
												continue;
											else
												f = trainer.exptbl[(int) ((f + MAX_EXP)
														* (EXP_TABLE_SIZE / MAX_EXP / 2))];
											// 'g' is the gradient multiplied by
											// the learning rate
											g = (1 - trainer.vocab_table.get(word).code[d] - f) * trainer.alpha;
											// Propagate errors output -> hidden
											for (c = 0; c < layer; c++)
												neu1e[c] += g * trainer.syn1[c + l2];
											// Learn weights hidden -> output
											for (c = 0; c < layer; c++)
												trainer.syn1[c + l2] += g * trainer.syn0[c + l1];
										}
									// NEGATIVE SAMPLING
									if (negative > 0)
										for (d = 0; d < negative + 1; d++) {
											if (d == 0) {
												target = word;
												label = 1;
											} else {
												target = trainer.unigram_table.random(rnd);
												if (target == 0)
													target = rnd.nextInt(n - 1) + 1;
												if (target == word)
													continue;
												label = 0;
											}
											l2 = target * layer;
											f = 0;
											for (c = 0; c < layer; c++)
												f += trainer.syn0[c + l1] * trainer.syn1neg[c + l2];
											if (f > MAX_EXP)
												g = (label - 1) * trainer.alpha;
											else if (f < -MAX_EXP)
												g = (label - 0) * trainer.alpha;
											else
												g = (label - trainer.exptbl[(int) ((f + MAX_EXP)
														* (EXP_TABLE_SIZE / MAX_EXP / 2))]) * trainer.alpha;
											for (c = 0; c < layer; c++)
												neu1e[c] += g * trainer.syn1neg[c + l2];
											for (c = 0; c < layer; c++)
												trainer.syn1neg[c + l2] += g * trainer.syn0[c + l1];
										}
									// Learn weights input -> hidden
									for (c = 0; c < layer; c++)
										trainer.syn0[c + l1] += neu1e[c];
								}
						}
						sentence_position++;
						if (sentence_position >= sentence_length) {
							sentence_length = 0;
							continue;
						}
					} // synchronized
				} // while
				fi.close();
			} catch (Exception e) {
				e.printStackTrace();
			} // try
		}
	}

	public static void main(String[] args) {
		// wget http://mattmahoney.net/dc/text8.zip
		// unzip text8.zip
		String path = "";
		Trainer t = new Trainer();
		t.set("binary", "true").set("trainfile", path + "text8").load(path + "text8", true, true).netlink().train();
		t.saveVocabs(path + "vocab.txt");
		t.saveClasses(path + "classes.txt", 500);
		t.saveVector(path + "vector.bin");

		VocabTable v = new VocabTable();
		v.vector(path + "vector.bin", 4, true);
		VocabWord[] ws = v.suggested(new String[] {"world", "war"}, 100);
		for (VocabWord word : ws) {
			System.out.printf("%20s %f\n", word.word, word.score);
		}
	}
}

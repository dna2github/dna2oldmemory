package seven.test;

import java.util.Random;

public class UnigramTable {
	private static final int TABLE_SIZE = 100000000;
	private int[] table;

	public UnigramTable(VocabTable vocab_table) {
		this.table = new int[TABLE_SIZE];
		init(vocab_table);
	}

	public UnigramTable() {
		this.table = new int[TABLE_SIZE];
	}

	private void init(VocabTable vocab_table) {
		int a, i, n;
		double train_words_pow = 0;
		double d1, power = 0.75;
		n = vocab_table.size();
		for (a = 0; a < n; a++)
			train_words_pow += Math.pow(vocab_table.get(a).count, power);
		i = 0;
		d1 = Math.pow(vocab_table.get(i).count, power) / train_words_pow;
		for (a = 0; a < TABLE_SIZE; a++) {
			table[a] = i;
			if (a * 1.0 / TABLE_SIZE > d1) {
				i++;
				d1 += Math.pow(vocab_table.get(i).count, power) / train_words_pow;
			}
			if (i >= n)
				i = n - 1;
		}
	}

	public int random(Random rnd) {
		return table[rnd.nextInt(TABLE_SIZE)];
	}
}

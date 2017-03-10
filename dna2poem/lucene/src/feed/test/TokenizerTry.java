package feed.test;

import java.io.IOException;
import java.io.StringReader;

import org.apache.lucene.analysis.Analyzer;
import org.apache.lucene.analysis.TokenStream;
import org.apache.lucene.analysis.Tokenizer;
import org.apache.lucene.analysis.standard.StandardAnalyzer;
import org.apache.lucene.analysis.tokenattributes.CharTermAttribute;
import org.apache.lucene.analysis.tokenattributes.OffsetAttribute;
import org.apache.lucene.analysis.tokenattributes.PositionIncrementAttribute;
import org.apache.lucene.analysis.tokenattributes.TypeAttribute;

public class TokenizerTry {
	private static class MyTokenizer extends Tokenizer {
		private final CharTermAttribute termAtt = addAttribute(CharTermAttribute.class);
		private final OffsetAttribute offsetAtt = addAttribute(OffsetAttribute.class);
		private final PositionIncrementAttribute posIncrAtt = addAttribute(PositionIncrementAttribute.class);
		private final TypeAttribute typeAtt = addAttribute(TypeAttribute.class);
		
		public MyTokenizer() {
		}
		
		char[] buf = null;
		int pos = 0;

		@Override
		public boolean incrementToken() throws IOException {
			clearAttributes();
			if (buf == null) {
				buf = new char[3];
				pos = 0;
			}
			int n = input.read(buf, 0, 3);
			if (n < 0) {
				buf = null;
				return false;
			}
			termAtt.copyBuffer(buf, 0, n);
			posIncrAtt.setPositionIncrement(n);
			offsetAtt.setOffset(pos, pos+n);
			typeAtt.setType("<codon>");
			pos += 3;
			return true;
		}
		
	}
	
	private static class MyAnalyzer extends Analyzer {

		@Override
		protected TokenStreamComponents createComponents(String field) {
			MyTokenizer tokenizer = new MyTokenizer();
			return new TokenStreamComponents(tokenizer);
		}
		
	}
	private static void printTokens(Analyzer analyzer, String string) throws Exception {
		TokenStream ts = analyzer.tokenStream("default", new StringReader(string));
		CharTermAttribute termAtt = ts.getAttribute(CharTermAttribute.class);
		ts.reset();
		while (ts.incrementToken()) {
			System.out.print(termAtt.toString());
			System.out.print("-");
		}
		System.out.println();
		analyzer.close();
	}

	public static void main(String[] args) throws Exception {
		printTokens(new StandardAnalyzer(), "hello world, for a token stream outputs.");
		printTokens(new MyAnalyzer(), "atgcattttcgcatgcagtagtcgatta");
	}
}

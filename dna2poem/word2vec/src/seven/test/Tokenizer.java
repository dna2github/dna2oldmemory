package seven.test;

import java.io.IOException;
import java.io.InputStream;
import java.io.UnsupportedEncodingException;

public class Tokenizer {
	public static final int BUF_WINDOW_LENGTH = 4096;
	public static final int BUF_LENGTH = BUF_WINDOW_LENGTH * 2;
	public static final int MAX_WORD_LENGTH = BUF_WINDOW_LENGTH / 2;

	private InputStream input;
	private byte[] buf;
	private int windowlen;
	private int page;
	private int cursor;

	public Tokenizer(InputStream input) {
		this.input = input;
		buf = new byte[BUF_LENGTH];
		firstload();
	}

	public boolean eof() {
		try {
			if (input.available() == 0 && cursor >= windowlen) {
				return true;
			}
		} catch (IOException e) {
			return true;
		}
		return false;
	}

	public int seek() {
		return page * BUF_WINDOW_LENGTH + cursor;
	}

	private void postcut() {
		if (windowlen > BUF_WINDOW_LENGTH) {
			System.arraycopy(buf, BUF_WINDOW_LENGTH, buf, 0, BUF_WINDOW_LENGTH);
			windowlen -= BUF_WINDOW_LENGTH;
			cursor -= BUF_WINDOW_LENGTH;
			page ++;
		}
	}

	private void preload() {
		try {
			int read = input.read(buf, BUF_WINDOW_LENGTH, BUF_WINDOW_LENGTH);
			windowlen += read;
		} catch (IOException e) {
			// TODO: handle i/o error
		}
	}

	private void firstload() {
		try {
			int read = input.read(buf, 0, BUF_WINDOW_LENGTH);
			windowlen = read;
			cursor = 0;
			page = 0;
		} catch (IOException e) {
			// TODO: handle i/o error
		}
	}

	public String next() {
		return this.next("UTF-8");
	}

	public String next(String encoding) {
		postcut();
		byte[] str = new byte[MAX_WORD_LENGTH];
		int start = cursor;
		int cur = 0;
		byte ch = 0;
		while (!eof()) {
			ch = buf[start++];
			if (start == BUF_WINDOW_LENGTH) preload();
			if (ch == 13) continue;
			if (ch == ' ' || ch == '\t') {
				if (cur > 0) break;
				cursor = start;
				continue;
			} else if (ch == '\n') {
				if (cur > 0) break;
				cursor = start;
				return "</s>";
			}
			if (cur < MAX_WORD_LENGTH) str[cur++] = ch;
		}
		if (start == cursor) return null;
		byte[] str_one = new byte[start - cursor - 1];
		System.arraycopy(buf, cursor, str_one, 0, start - cursor - 1);
		cursor = start;
		if (ch == '\n') cursor --;
		try {
			return new String(str_one, encoding);
		} catch (UnsupportedEncodingException e) {
			throw new RuntimeException(String.format("Unsupported charset: %s", encoding));
		}
	}
}

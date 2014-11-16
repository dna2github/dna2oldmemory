/**
 * @author J.Y.Liu
 * @date 2010.01.21
 */
package ljy.mobile.expcalc;

import java.security.MessageDigest;
import java.security.NoSuchAlgorithmException;

public class STR_codec {
	public static String md5(String _plain) {
		return Encode("MD5",_plain);
	}
	
	public static String sha256(String _plain) {
		return Encode("SHA-256",_plain);
	}
	
	public static String sha512(String _plain) {
		return Encode("SHA-512",_plain);
	}
	
	public static String encode_selector(String str,
										String method) {
		if(method.equals("sha512")) return sha512(str);
		else if(method.equals("sha256")) return sha256(str);
		else if(method.equals("md5")) return md5(str);
		else return "";
	}
	
	public static String byteArrayToHexString(byte[] b) {
		char[] hex = { '0', '1', '2', '3',
					   '4', '5', '6', '7',
					   '8', '9', 'A', 'B',
					   'C', 'D', 'E', 'F' };
		char[] newChar = new char[b.length * 2];
		for (int i = 0; i < b.length; i++) {
			newChar[2 * i] = hex[(b[i] & 0xf0) >> 4];
			newChar[2 * i + 1] = hex[b[i] & 0xf];
		}
		return new String(newChar);
	}
	
	private static String Encode(String _code, String _message) {
		MessageDigest md;
		String encode = null;
		try {
			md = MessageDigest.getInstance(_code);
			encode = byteArrayToHexString(
					md.digest(_message.getBytes()));
		} catch (NoSuchAlgorithmException e) {
			System.out.println("(D) codec >> failed | Encode");
		}
		return encode;
	}

	// myself hash function
	public static int do_self_hash(String s) {
		if(s==null) {
			return 0;
		}
		byte[] b=s.getBytes();
		int r=0;
		int t=0;
		int p=0;
		for(int i=0;i<(b.length-1)/8+1;i++) {
			t=0;
			p=0;
			for(int j=0;j<8;j++) {
				if(i*8+j>=b.length) break;
				t+=(b[i*8+j]%16)<<p;
				p+=log2(b[i*8+j]%16);
			}
			t&=0x7fffffff;
			r+=t;
			r&=0x7fffffff;
		}
		return r;
	}
	
	private static int log2(int x) {
		int p;
		p=-1;
		while(x>0) {
			x/=2;
			p++;
		}
		return p;
	}
}

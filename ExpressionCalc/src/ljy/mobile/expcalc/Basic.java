/**
 * @author J.Y.Liu
 * @date 2010.08.14
 */
package ljy.mobile.expcalc;

public class Basic {
	public static double basicValExp(String _num) {
		double _r = 0.0;
		try {
			_r = Double.valueOf(_num).doubleValue();
		}catch (Exception e) {
			_r = 0.0;
		}
		return _r;
	}
	public static double basicHexValExp(String _num) {
		double _r = 0.0;
		if(_num==null) return _r;
		if(_num.length()<3) return _r;
		
		char x;
		x = _num.charAt(0);
		if(x!='&') return _r;
		x = _num.charAt(1);
		if(x!='h') return _r;
		for(int i=2;i<_num.length();i++) {
			x = _num.charAt(i);
			System.out.println("( " + x + " ) >> r = " + _r);
			if(!charHex(x)) return 0.0;
			x -= 48;
			if(x >= 49) x -= 39;
			_r = _r*16+x;
		}
		return _r;
	}
	
	public static boolean charNum(char _x) {
		return (_x>='0' && _x<='9');
	}
	public static boolean charHex(char _x) {
		return (charNum(_x) || (_x>='a' && _x<='f'));
	}
	public static boolean charLCase(char _x) {
		return (_x>='a' && _x<='z');
	}
	
	public static double basicSin(double _x) {
		return Math.sin(_x);
	}
	public static double basicCos(double _x) {
		return Math.cos(_x);
	}
	public static double basicAtn(double _x) {
		return Math.atan(_x);
	}
	public static double basicLog(double _x) {
		return Math.log(_x);
	}
	
	public static String varnameWithoutType(String _name) {
		if(_name==null) return null;
		if(_name.length()==0) return _name;
		char x;
		x = _name.charAt(_name.length()-1);
		if(!Basic.charLCase(x) && !Basic.charNum(x)) {
			String _tmp = _name.substring(0, _name.length()-1);
			return _tmp;
		} else {
			return _name;
		}
	}
}

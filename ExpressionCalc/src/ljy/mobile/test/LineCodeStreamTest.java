/**
 * @author J.Y.Liu
 * @date 2010.08.16
 */
package ljy.mobile.test;

import ljy.mobile.expcalc.LineCodeStream;

public class LineCodeStreamTest {

	public static void main(String[] args) {
		LineCodeStream lcs;
		lcs = new LineCodeStream("set s00.xxxx <> 2.0/function(a*sin(x),b,c) & chr(34)+\"missing\" and i>=7");
		while(lcs.hasNextWord() && lcs.getError()==0) {
			System.out.println(lcs.getWord()+"\t( status = "+lcs.getLastStatus()+")");
		}
		if(lcs.getError()!=0) {
			System.out.println(lcs.getErrorDescription());
		}
		System.out.println("---- Test 2 ----");
		lcs.setLineCode("end					 ");
		System.out.println(lcs.getWord()+"\t( status = "+lcs.getLastStatus()+")");
		System.out.println(lcs.hasNextWord());
		System.out.println(lcs.getWord()+"\t( status = "+lcs.getLastStatus()+")");
		System.out.println("( error = "+lcs.getError()+")");
		System.out.println("---- Test 3 ----");
		lcs.setLineCode("a b + d e 12  ");
		System.out.println(lcs.getWord()+"\t( status = "+lcs.getLastStatus()+")");
		System.out.println(lcs.hasNextWord());
		System.out.println(lcs.getBefore("d")+"\t( status = "+lcs.getLastStatus()+")");
		System.out.println(lcs.getRemain());
		System.out.println("( error = "+lcs.getError()+")");
	}

}

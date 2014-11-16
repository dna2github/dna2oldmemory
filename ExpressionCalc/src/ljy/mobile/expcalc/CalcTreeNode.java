/**
 * @author J.Y.Liu
 * @date 2010.08.14
 */
package ljy.mobile.expcalc;

public class CalcTreeNode {
	private CalcTreeNode(int _type, int _deep) {
		type = _type;
		deep = _deep;
		parent = null;
		left = null;
		right = null;
	}
	public static CalcTreeNode newNode(int _type, int _deep) {
		return new CalcTreeNode(_type,_deep);
	}
	
	public int type;
	public int deep;
	public Object data;
	public int flag;
	
	public CalcTreeNode parent;
	public CalcTreeNode left;
	public CalcTreeNode right;
}

/*
 * ----------- Operator --- 
 * and or xor
 * not
 * > < >= <= =
 * + -
 * mod \
 * * /
 * ^
 * +/-
 */

/*
 * ----------- Value ---
 * Variable
 * Function 
 */
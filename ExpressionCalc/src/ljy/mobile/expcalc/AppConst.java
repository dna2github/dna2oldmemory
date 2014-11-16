/**
 * @author J.Y.Liu
 * @date 2010.08.30
 */
package ljy.mobile.expcalc;

public final class AppConst {
	public static final int SYMBOL_TYPE_ID_ERROR = -1;
	public static final int SYMBOL_TYPE_ID_OPERATOR = 0;
	public static final int SYMBOL_TYPE_ID_STRING = 1;
	public static final int SYMBOL_TYPE_ID_INTEGER = 2;
	public static final int SYMBOL_TYPE_ID_REAL = 3;
	public static final int SYMBOL_TYPE_ID_HEX = 4;
	public static final int SYMBOL_TYPE_ID_SYMBOL = 5;
	public static final int SYMBOL_TYPE_ID_PART = 98;
	public static final int SYMBOL_TYPE_ID_REMAIN = 99;
	
	public static final int CALCTREENODE_TYPE_OPERATOR = 0;
	public static final int CALCTREENODE_TYPE_OPERATOR_COMA = 0;
	public static final int CALCTREENODE_TYPE_OPERATOR_ADD = 4;
	public static final int CALCTREENODE_TYPE_OPERATOR_SUB = 4;
	public static final int CALCTREENODE_TYPE_OPERATOR_MUL = 6;
	public static final int CALCTREENODE_TYPE_OPERATOR_DIV = 6;
	public static final int CALCTREENODE_TYPE_OPERATOR_POSITIVE = 8;
	public static final int CALCTREENODE_TYPE_OPERATOR_NEGTIVE = 8;
	
	public static final int CALCTREENODE_TYPE_VALUE = 100;
	public static final int CALCTREENODE_TYPE_VALUE_IMDVALUE = 101;
	public static final int CALCTREENODE_TYPE_VALUE_IMDVALUE_STRING = 102;
	public static final int CALCTREENODE_TYPE_VALUE_VARIABLE = 103;
	public static final int CALCTREENODE_TYPE_VALUE_FUNCTION = 104;
}

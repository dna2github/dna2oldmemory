/**
 * @author J.Y.Liu
 * @date 2010.08.16
 */
package ljy.mobile.test;

import ljy.mobile.expcalc.AppConst;
import ljy.mobile.expcalc.Basic;
import ljy.mobile.expcalc.CalcTree;
import ljy.mobile.expcalc.CalcTreeNode;
import ljy.mobile.expcalc.LineCodeStream;
import ljy.mobile.expcalc.SymbolTable;
import ljy.mobile.expcalc.SymbolTableNode;

public class SyCoExp3in1Test {
	public static void main(String[] args) {
		SymbolTableNode stn;
		String tmp;
		int deep;
		char cur;
		
		SymbolTable st;
		CalcTree ct;
		LineCodeStream lcs;
		
		st = new SymbolTable();
		ct = new CalcTree();
		
		stn = st.add("hello", null);
		stn.data = Double.valueOf(20.0);
		stn.type = 0;
		stn = st.add("world", null);
		stn.data = Double.valueOf(10.0);
		stn.type = 0;
		stn = st.add("sqr", null);
		stn.data = Integer.valueOf(1); // function id
		stn.type = 1;
		stn = st.add("cube", null);
		stn.data = Integer.valueOf(2);
		stn.type = 1;
		stn = st.add("double", null);
		stn.data = Integer.valueOf(3);
		stn.type = 1;
		
		lcs = new LineCodeStream("hello+cube(world) - (world + 2)*sqr(2) - sqr(cube(double(2)))/hello");
		
		deep = 0;
		cur = 0;
		while(lcs.hasNextWord() && lcs.getError()==0) {
			tmp = lcs.getWord();
			if(tmp==null) break;
			switch(lcs.getLastStatus()) {
			case 0:
				cur = tmp.charAt(0);
				switch(cur) {
				case '+':
					ct.pushNode(AppConst.CALCTREENODE_TYPE_OPERATOR_ADD, deep, Integer.valueOf(10));
					break;
				case '-':
					ct.pushNode(AppConst.CALCTREENODE_TYPE_OPERATOR_SUB, deep, Integer.valueOf(11));
					break;
				case '*':
					ct.pushNode(AppConst.CALCTREENODE_TYPE_OPERATOR_MUL, deep, Integer.valueOf(14));
					break;
				case '/':
					ct.pushNode(AppConst.CALCTREENODE_TYPE_OPERATOR_DIV, deep, Integer.valueOf(15));
					break;
				case '(':
					deep++;
					break;
				case ')':
					deep--;
					break;
				}
				break;
			case 1:
				break; // not used
			case 2:
			case 3:
				ct.pushNode(AppConst.CALCTREENODE_TYPE_VALUE_IMDVALUE, deep, Double.valueOf(Basic.basicValExp(tmp)));
				break;
			case 4:
				break; // not used
			case 5:
				stn = st.find(tmp, null);
				if(stn.type==0) {
					ct.pushNode(AppConst.CALCTREENODE_TYPE_VALUE_VARIABLE, deep, tmp); 
				} else {
					ct.pushNode(AppConst.CALCTREENODE_TYPE_VALUE_FUNCTION, deep, tmp);
				}
				break;
			}
		}
		
		double _val = 0.0;
		_val = calcTreeValue(ct.getRoot(),st);
		System.out.println(lcs.getLineCode() + " = " + _val);
	}

	public static double calcTreeValue(CalcTreeNode _opr, SymbolTable _st) {
		double _left = 0.0;
		double _right = 0.0;
		if(_opr == null) {
			return 0.0;
		} else if(_opr.type > AppConst.CALCTREENODE_TYPE_VALUE) {
			switch(_opr.type) {
			case AppConst.CALCTREENODE_TYPE_VALUE_IMDVALUE:
				return ((Double)_opr.data).doubleValue();
			case AppConst.CALCTREENODE_TYPE_VALUE_VARIABLE:
				SymbolTableNode _stn;
				_stn = _st.add((String)_opr.data, null);
				return ((Double)_stn.data).doubleValue();
			}
		}else {
			_left = calcTreeValue(_opr.left,_st);
			_right = calcTreeValue(_opr.right,_st);
			int _m = ((Integer)_opr.data).intValue();
			
			double _tmpval;
			switch(_m) {
			case 0:
				// assume function has only parameter
				_tmpval = functionJump(_st,(String)_opr.left.data,_right); 
				System.out.println(" --> "+_opr.left.data+"("+_right+")="+_tmpval);
				return _tmpval;
			case 10:
				_tmpval = _left+_right;
				System.out.println(" --> "+_left+"+"+_right+"="+_tmpval);
				return _tmpval;
			case 11:
				_tmpval = _left-_right;
				System.out.println(" --> "+_left+"-"+_right+"="+_tmpval);
				return _tmpval;
			case 14:
				_tmpval = _left*_right;
				System.out.println(" --> "+_left+"*"+_right+"="+_tmpval);
				return _tmpval;
			case 15:
				_tmpval = _left/_right;
				System.out.println(" --> "+_left+"/"+_right+"="+_tmpval);
				return _tmpval;
			}
		}
		return 0.0;
	}
	
	public static double functionJump(SymbolTable _st, String _name, double _para) {
		SymbolTableNode _stn;
		int function_id;
		double x;
		
		_stn = _st.find(_name, null);
		if(_stn==null) return 0.0;
		
		function_id = ((Integer)_stn.data).intValue();
		x = _para;
		switch(function_id) {
		case 1:
			return x*x;
		case 2:
			return x*x*x;
		case 3:
			return 2*x;
		}
		return 0.0;
	}
}

/**
 * @author J.Y.Liu
 * @date 2010.08.14
 */
package ljy.mobile.test;

import ljy.mobile.expcalc.AppConst;
import ljy.mobile.expcalc.Basic;
import ljy.mobile.expcalc.CalcTree;
import ljy.mobile.expcalc.CalcTreeNode;

public class CalcTreeTest {

	public static void main(String[] args) {
		String exp = "(((5-3)+9)/2.0-0.5)*20/5.0+2";
		CalcTree ct = new CalcTree();
		boolean realnum = false;
		int deep = 0;
		int st_pos = 0;
		char cur;
		String tmp;
		
		//String errstr = "";
		//boolean err = false;;

		for(int i=0;i<exp.length();i++) {
			cur = exp.charAt(i);
			if(cur=='.') {
				if(realnum) {
					st_pos = i+1;
					realnum = false;
				} else {
					realnum = true;
				}
			} else if(cur>='0' && cur<='9') {
				if(i==exp.length()-1) {
					tmp = exp.substring(st_pos, i+1);
					ct.pushNode(AppConst.CALCTREENODE_TYPE_VALUE_IMDVALUE,
							deep, Double.valueOf(Basic.basicValExp(tmp)));
				}
			} else {
				if(st_pos<i) {
					tmp = exp.substring(st_pos, i);
					ct.pushNode(AppConst.CALCTREENODE_TYPE_VALUE_IMDVALUE,
							deep, Double.valueOf(Basic.basicValExp(tmp)));
				}
				switch(cur) {
				case ')':
					deep--;
					realnum = false;
					break;
				case '(':
					deep++;
					realnum = false;
					break;
				case '+':
					ct.pushNode(AppConst.CALCTREENODE_TYPE_OPERATOR_ADD,
							deep, Integer.valueOf(10));
					break;
				case '-':
					ct.pushNode(AppConst.CALCTREENODE_TYPE_OPERATOR_SUB,
							deep, Integer.valueOf(11));
					break;
				case '*':
					ct.pushNode(AppConst.CALCTREENODE_TYPE_OPERATOR_MUL,
							deep, Integer.valueOf(14));
					break;
				case '/':
					ct.pushNode(AppConst.CALCTREENODE_TYPE_OPERATOR_DIV,
							deep, Integer.valueOf(15));
					break;
				}
				st_pos = i+1;
				realnum = false;
			}
		}
		
		double _val = 0.0;
		_val = calcTreeValue(ct.getRoot());
		System.out.println(exp + " = " + _val);
	}

	public static double calcTreeValue(CalcTreeNode _opr) {
		double _left = 0.0;
		double _right = 0.0;
		if(_opr == null) {
			return 0.0;
		} else if(_opr.type > AppConst.CALCTREENODE_TYPE_VALUE) {
			return ((Double)_opr.data).doubleValue();
		}else {
			_left = calcTreeValue(_opr.left);
			_right = calcTreeValue(_opr.right);
			int _m = ((Integer)_opr.data).intValue();
			switch(_m) {
			case 10:
				return (_left+_right);
			case 11:
				return (_left-_right);
			case 14:
				return (_left*_right);
			case 15:
				return (_left/_right);
			}
		}
		return 0.0;
	}
}

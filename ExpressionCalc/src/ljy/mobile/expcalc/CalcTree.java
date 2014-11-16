/**
 * @author J.Y.Liu
 * @date 2010.08.14
 */
package ljy.mobile.expcalc;

public class CalcTree {

	public CalcTree() {
		initialize();
	}
	public void initialize() {
		root = null;
		current = null;
	}
	
	public boolean pushNode(int _type, int _deep, Object _data) {
		boolean _r = false;
		if(_type < AppConst.CALCTREENODE_TYPE_VALUE) {
			// push operator
			_r = pushOperatorNode(_type,_deep,_data);
		} else {
			// push value
			_r = pushValueNode(_type,_deep,_data);
		}
		return _r;
	}
	private boolean pushValueNode(int _type, int _deep, Object _val) {
		CalcTreeNode _newone = CalcTreeNode.newNode(_type, _deep);
		if(_newone==null) return false;
		_newone.data = _val;
		
		if(current==null) {
			// first push
			_newone.left=root;
			current = _newone;
			root = current;
		} else if(current.type == AppConst.CALCTREENODE_TYPE_VALUE_FUNCTION ){
			// push first value for a function
			if(pushOperatorNode(AppConst.CALCTREENODE_TYPE_OPERATOR_COMA,
					_deep, Integer.valueOf(0))) {
				_newone.parent = current;
				current.right = _newone;
			} else {
				return false;
			}
		} else if(current.type < AppConst.CALCTREENODE_TYPE_VALUE) {
			// push value for an operator
			if(current.right==null) {
				_newone.parent = current;
				current.right = _newone;
			} else {
				// error : too many parameter for the operator
				return false;
			}
		} else {
			// error for push value
			return false;
		}
		current = _newone;
		return true;
	}
	private boolean pushOperatorNode(int _type, int _deep, Object _opr) {
		CalcTreeNode _newone = CalcTreeNode.newNode(_type, _deep);
		if(_newone==null) return false;
		_newone.data = _opr;
		
		if(current==null) {
			// first push
			if((current.flag & 0x01) == 1) {
				_newone.left=current;
				current = _newone;
				root = current;
			} else {
				// error : lose previous parameter
				return false;
			}
		} else if(current.type >= AppConst.CALCTREENODE_TYPE_VALUE) {
			// push operator for a value
			if((current.flag & 0x01) == 1) {
				// error : unsupported for previous parameter
				return false;
			} else {
				while(current!=null) {
					current=current.parent;
					if(current==null) {
						_newone.left = root;
						root = _newone;
					} else {
						if(_newone.deep>current.deep ||
								(_newone.deep==current.deep &&
										_newone.type>current.type)) {
							_newone.parent = current;
							_newone.left = current.right;
							current.right = _newone;
							break;
						}
					}
				}
			}
		} else if((current.flag & 0x01) == 1) {
			// single parameter operator
			if(current.right!=null) {
				current.right = _newone;
			} else {
				// error : too many parameter for the operator
				return false;
			}
		} else {
			// error for push operator
			return false;
		}
		current = _newone;
		return true;
	}
	
	public CalcTreeNode getRoot() {
		return root;
	}
	public CalcTreeNode getCurrent() {
		return current;
	}

	private CalcTreeNode root;
	private CalcTreeNode current;
}

/**
 * @author J.Y.Liu
 * @date 2010.08.14
 */
package ljy.mobile.expcalc;

public class SymbolTableNode {
	public static final int TYPE_NONE = -1;
	
	private SymbolTableNode(String _name) {
		setName(_name);
		type = TYPE_NONE;
		data = null;
		
		isStatic = false;
		isConst = false;
		isArray = false;
	}
	public static SymbolTableNode newNode(String _name) {
		return new SymbolTableNode(_name);
	}
	
	public void setName(String _name) {
		name = _name;
		namehash = STR_codec.do_self_hash(Basic.varnameWithoutType(_name));
	}
	public String getName() {
		return name;
	}
	public boolean cmpName(String _name) {
		if(_name==null && name==null) return true;
		if(_name==null || name==null) return false;
		String nameA,nameB;
		nameA=Basic.varnameWithoutType(_name);
		nameB=Basic.varnameWithoutType(name);
		if(nameA.equals(nameB)) return true;
		return false;
	}
	public boolean cmpName(String _name, int _namehash) {
		if(namehash==_namehash) {
			return cmpName(_name);
		}
		return false;
	}

	public boolean isStatic;
	public boolean isConst;
	public boolean isArray;
	public int type;
	private String name; // name = VarName + "@" + scope
	private int namehash;
	public Object data;
}

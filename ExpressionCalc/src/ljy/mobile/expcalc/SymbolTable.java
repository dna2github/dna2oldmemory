/**
 * @author J.Y.Liu
 * @date 2010.08.14
 */
package ljy.mobile.expcalc;

import java.util.Iterator;
import java.util.Vector;

public class SymbolTable {
	public SymbolTable() {
		table = new Vector();
	}
	
	public SymbolTableNode add(String _name, String _scope) {
		if(table==null) return null;
		
		String _varname;
		if(_scope!=null) {
			_varname = _scope + "@" + _name;
		} else {
			_varname = _name + "";
		}
		
		SymbolTableNode _cur;
		_cur = find(_name,_scope);
		if(_cur==null) {
			_cur = SymbolTableNode.newNode(_varname);
			table.add(_cur);
		}
		return _cur;
	}
	
	public SymbolTableNode add_directly(String _name, String _scope) {
		if(table==null) return null;
		
		String _varname;
		if(_scope!=null) {
			_varname = _scope + "@" + _name ;
		} else {
			_varname = _name + "";
		}
		
		SymbolTableNode _cur;
		_cur = SymbolTableNode.newNode(_varname);
		table.add(_cur);
		return _cur;
	}
	
	public SymbolTableNode find(String _name, String _scope) {
		if(table==null) return null;
		
		String _varname;
		if(_scope!=null) {
			_varname = _scope + "@" + _name;
		} else {
			_varname = _name + "";
		}
		
		Iterator _i;
		SymbolTableNode _cur;
		
		_i = table.iterator();
		while(_i.hasNext()) {
			_cur = (SymbolTableNode)_i.next();
			if(_cur == null) {
				continue;
			} else {
				if(_cur.cmpName(_varname)) return _cur;
			}
		}
		return null;
	}
	
	public boolean del(String _name, String _scope) {
		if(table==null) return false;
		
		String _varname;
		if(_scope!=null) {
			_varname = _scope + "@" + _name;
		} else {
			_varname = _name + "";
		}
		
		Iterator _i;
		SymbolTableNode _cur;
		
		_i = table.iterator();
		while(_i.hasNext()) {
			_cur = (SymbolTableNode)_i.next();
			if(_cur == null) {
				continue;
			} else {
				if(_cur.cmpName(_varname)) {
					_i.remove();
					return true;
				}
			}
		}
		return false;
	}
	
	public void clear() {
		if(table==null) return;
		table.clear();
	}
	
	public Vector getTable() {
		return table;
	}
	
	private Vector table;
}

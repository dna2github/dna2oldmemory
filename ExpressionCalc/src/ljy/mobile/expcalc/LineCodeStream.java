/**
 * @author J.Y.Liu
 * @date 2010.08.16
 */
package ljy.mobile.expcalc;

public class LineCodeStream {
	public static final int ERR_UNKNOWN = 9999;
	public static final int ERR_STRING_QUOTE_MISSING = 1;
	public static final int ERR_REAL_TOO_MANY_DOT = 2;
	public static final int ERR_SYMBOL_NAME_NUMBER_AHEAD = 3;
	public static final int ERR_HEX_FORMAT = 4;
	
	public LineCodeStream(String _lineCode) {
		setLineCode(_lineCode);
	}
	public LineCodeStream() {
		setLineCode(null);
	}
	
	public void setLineCode(String _lineCode) {
		lineCode = _lineCode;
		if(lineCode!=null) {
			lineCode=lineCode.toLowerCase().trim();
		}
		reset();
	}
	public void reset() {
		status = 0;
		pos = 0;
		cur = 0;
	}
	
	public String getLineCode() {
		return lineCode;
	}
	
	public boolean hasNextWord() {
		if(lineCode==null) return false;
		return (pos < lineCode.length());
	}
	
	public String getRemain() {
		if(lineCode==null) return null;
		if(!hasNextWord()) return null;
		String tmp = lineCode.substring(pos).trim();
		// status = 99 : remain
		status = AppConst.SYMBOL_TYPE_ID_REMAIN;
		pos = lineCode.length();
		return tmp;
	}
	
	public String getBefore(String _before) {
		status = AppConst.SYMBOL_TYPE_ID_ERROR;
		if(_before==null) return null;
		int tmp_pos = pos;
		String tmp = null;
		String part = "";
		while(hasNextWord()) {
			tmp = getWord();
			if(_before.equals(tmp)) {
				if(part.length()>0) {
					part=part.substring(0,part.length()-1);
				}
				status = AppConst.SYMBOL_TYPE_ID_PART;
				return part;
			}
			part += tmp+"\000";
		}
		status = AppConst.SYMBOL_TYPE_ID_ERROR;
		pos = tmp_pos; // rollback
		return null; // not find the string of _before
	}
	
	public String getWord() {
		if(!hasNextWord()) return null;
		
		char current = 0;
		String tmp = null;
		
		// skip space and tab
		current = lineCode.charAt(pos);
		while(current == ' ' || current == '\011') {
			pos++;
			current = lineCode.charAt(pos);
		}
		cur = pos;
		
		// decide the status
		// status = 0 : normal
		//          1 : string
		//          2 : integer
		//          3 : real
		//          4 : hex
		//          5 : symbol
		//         99 : remain
		if(current=='"') {
			status = AppConst.SYMBOL_TYPE_ID_STRING;
		} else if(current>='0' && current<='9') {
			status = AppConst.SYMBOL_TYPE_ID_INTEGER;
		} else if(current>='a' && current<='z') {
			status = AppConst.SYMBOL_TYPE_ID_SYMBOL;
		} else if(current=='&'){
			status = AppConst.SYMBOL_TYPE_ID_HEX;
		} else if(current=='.') {
			status = AppConst.SYMBOL_TYPE_ID_REAL;
		} else {
			status = AppConst.SYMBOL_TYPE_ID_OPERATOR;
		}
		
		// get a word
		switch(status) {
		case 0: // other
			cur++;
			// operator with two characters such as <= >= <>
			if(current=='<') {
				current = lineCode.charAt(cur);
				if(current=='=' || current=='>') {
					cur++;
				}
			} else if(current=='>') {
				current = lineCode.charAt(cur);
				if(current=='=') {
					cur++;
				}
			}
			break;
		case 1: // string
			while(true) {
				cur++;
				if(cur>=lineCode.length()) {
					err = ERR_STRING_QUOTE_MISSING;
					break;
				}
				current = lineCode.charAt(cur);
				if(current=='"') break;
			}
			if(err==0) cur++;
			break;
		case 3: // real
			cur++;
			if(cur<lineCode.length()) {
				current = lineCode.charAt(cur);
				if(!Basic.charNum(current)) {
					// the dot is an operator
					status = AppConst.SYMBOL_TYPE_ID_OPERATOR;
					break;
				}
			} else {
				break;
			}
			// there is no break here
		case 2: // integer
			while(true) {
				cur++;
				if(cur>=lineCode.length()) break;
				current = lineCode.charAt(cur);
				if(!Basic.charNum(current)) {
					if(current=='.') {
						if(status==2) {
							status=AppConst.SYMBOL_TYPE_ID_REAL;
						} else {
							err = ERR_REAL_TOO_MANY_DOT;
							break;
						}
					} else if(Basic.charLCase(current)) {
						err = ERR_SYMBOL_NAME_NUMBER_AHEAD;
						break;
					} else {
						break;
					}
				}
			}
			break;
		case 4: // hex
			cur++;
			if(cur>=lineCode.length()) {
				status = AppConst.SYMBOL_TYPE_ID_OPERATOR; // for operator &
				break;
			}
			current = lineCode.charAt(cur);
			if(current==' ' || current=='\t') {
				status = AppConst.SYMBOL_TYPE_ID_OPERATOR; // for operator &
				break;
			}
			if(current!='h') {
				err = ERR_HEX_FORMAT;
				break; // TODO: error for hex
			}
			while(true) {
				cur++;
				if(cur>=lineCode.length()) {
					if(cur-pos<3) {
						// TODO: error for hex
						err = ERR_HEX_FORMAT;
					}
					break;
				}
				current = lineCode.charAt(cur);
				if(!Basic.charHex(current)) {
					if(Basic.charLCase(current) || current=='.') {
						// TODO: error for hex
						err = ERR_HEX_FORMAT;
						break;
					} else {
						break;
					}
				}
			}
			break;
		case 5: // symbol
			while(true) {
				cur++;
				if(cur>=lineCode.length()) break;
				current = lineCode.charAt(cur);
				if(!Basic.charNum(current) && !Basic.charLCase(current)) {
					break;
				}
			}
			if(current=='$' || current=='%' || current=='#' || current=='&' || current=='!') {
				cur++;
			}
			break;
		}
		
		if(err==0) {
			tmp = lineCode.substring(pos,cur);
			pos=cur;
		} else {
			status = -1;
		}
		
		return tmp;
	}
	
	public int getLastStatus() {
		return status;
	}
	public int getError() {
		return err;
	}
	public String getErrorDescription() {
		switch(err) {
		case 0:
			return "successfully";
		case ERR_UNKNOWN:
			return "(unknown)";
		case ERR_STRING_QUOTE_MISSING:
			return "right quote is missing for this string";
		case ERR_REAL_TOO_MANY_DOT:
			return "find too many dot appear for real number";
		case ERR_SYMBOL_NAME_NUMBER_AHEAD:
			return "symbol name must start with a letter";
		case ERR_HEX_FORMAT:
			return "hex format is invalid";
		}
		return null;
	}

	private String lineCode;
	private int status;
	private int pos;
	private int cur;
	
	private int err;
}

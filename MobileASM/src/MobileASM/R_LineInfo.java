package MobileASM;
/**
 * @author J.Y.Liu
 * @at Hefei, Anhui, China
 * @version v.2009[1st]
 * @date 2009.03
 * @class R_LineInfo
 * @function A Struct of A Line
 */
public class R_LineInfo {

    public String operator;
    public byte opr_num;
    public byte opr_no;

    public byte opr_type_1;
    public String opr_1;
    public byte opr_type_2;
    public String opr_2;
    
    public R_LineInfo() {
        clear();
    }

    public void clear() {
        operator = "";
        opr_num = -1;
        opr_no = -1;
        opr_type_1 = -1;
        opr_1 = "";
        opr_type_2 = -1;
        opr_2 = "";
    }

}
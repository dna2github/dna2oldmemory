package MobileASM;
/**
 * @author J.Y.Liu
 * @at Hefei, Anhui, China
 * @version v.2009[1st]
 * @date 2009.03
 * @class R_Interpreter
 * @function Interpret a line
 */
public class R_Interpreter {

    public final int DATA_BASE = 10;
    
    public R_LineInfo linedata;

    public R_Interpreter() {
        linedata = new R_LineInfo();
        clear();
    }

    public void clear() {
        linedata.clear();
    }

    public void get_line_info(String data) {
        String tmp;
        tmp = data + " ";
        linedata.clear();
        get_operator(tmp);
        if(linedata.operator.charAt(0)=='!') return;
        int pos = tmp.indexOf(' ');
        tmp = tmp.substring(pos + 1)+",";
        get_oprand(tmp);
    }

    void get_operator(String data) {
        int pos = data.indexOf(' ');
        String tmp = data.substring(0, pos);
        byte r = check_operator(tmp);
        linedata.opr_no = (byte)(r + (byte)0);
        if(r==-1) tmp="! Invalid Operator";
        linedata.operator = tmp;
        linedata.opr_num = get_opr_num(tmp);
    }

    void get_oprand(String data) { // in data: no operator!
        String tmp = data.trim();
        String tmp2;
        if(linedata.operator.equals("print")) {
            tmp = tmp.substring(0, tmp.length()-2);
            switch(tmp.charAt(0)) {
                case ':': // base = 20
                    interpret_oprand(tmp.substring(1));
                    linedata.opr_1 = linedata.opr_2 + "";
                    linedata.opr_type_1 = (byte)(linedata.opr_type_2 + 20);
                    break;
                case '"': // base = 41
                    linedata.opr_1 = tmp.substring(1);
                    linedata.opr_type_1 = 41;
                    break;
                case 39: // ' base = 40
                    linedata.opr_1 = tmp.substring(1,2);
                    linedata.opr_type_1 = 40;
                    break;
                default: // base = 0
                    interpret_oprand(tmp.trim());
                    linedata.opr_1 = linedata.opr_2 + "";
                    linedata.opr_type_1 = (byte)(linedata.opr_type_2 + 0);
            }
        } else {
            int num = -1;
            int pos = 0;
            if(data.trim().charAt(0)==',') num=0;
            if(linedata.opr_num==0 && num==-1) {linedata.operator="! Invalid Oprand Number";return;}
            if(linedata.opr_num>0 && num==0) {linedata.operator="! Invalid Oprand Number";return;}
            if(linedata.opr_num==0) return;
            num = 1;
            pos = tmp.indexOf(',');
            tmp2 = tmp.substring(pos+1);
            while(tmp2.length()>0) {
                pos = tmp2.indexOf(',');
                tmp2 = tmp2.substring(pos+1);
                num += 1;
            }
            if(linedata.opr_num != num) linedata.operator="! Invalid Oprand Number";
            pos = tmp.indexOf(',');
            tmp2 = tmp.substring(0, pos).trim();
            interpret_oprand(tmp2);
            linedata.opr_1 = linedata.opr_2 + "";
            linedata.opr_type_1 = (byte)(linedata.opr_type_2 + (byte)0);
            if (num>1) {
                tmp2 = tmp.substring(pos+1, tmp.length()-1).trim();
                interpret_oprand(tmp2);
            }
        }
    }

    void interpret_oprand(String data) {
        String tmp = data.trim();
        int base = 0;
        int r = -1;
        if(tmp.charAt(0)=='[' && tmp.charAt(tmp.length()-1)==']') {
            base = DATA_BASE;
            tmp = tmp.substring(1, tmp.length()-1).trim();
        }
        r = check_oprand(tmp);
        if(r==-1) { linedata.operator="! Invalid Oprand";linedata.opr_type_1=-1;}
        base+=r;
        linedata.opr_2 = tmp.trim();
        linedata.opr_type_2 = (byte)base;
    }

    int check_oprand(String data) {
        if(data.equals("eax")) return 1; // return data;
        else if(data.equals("ebx")) return 1; // return data;
        else if(data.equals("ecx")) return 1; // return data;
        else if(data.equals("edx")) return 1; // return data;
        else if(data.equals("esi")) return 1; // return data;
        else if(data.equals("edi")) return 1; // return data;
        else if(data.equals("ebp")) return 1; // return data;
        else if(data.equals("esp")) return 1; // return data;
        else if(data.equals("eip")) return 1; // return data;
        else {
            byte isnum = 1;
            int pos = 0;
            if(data.charAt(0)=='-') pos = 1;
            if(data.length()>5+pos) return -1;
            for (int i=pos;i<data.length();i++)
                if((data.charAt(i)<48 || data.charAt(i)>57)) {
                    isnum = 0;
                    break;
                }
            if (data.length()==5+pos) {
                if(pos==1 && data.compareTo("-32768")>0) return -1;
                if(pos==0 && data.compareTo("65535")>0) return -1;
            }
            if (isnum == 1) return 0;
        }
        return -1;
    }

    byte check_operator(String data) {
        if(data.equals("cls")) return 1;
        else if(data.equals("print")) return 2;
        //else if(data.equals("input")) return 3;
        else if(data.equals("end")) return 4;
        else if(data.equals("exit")) return 4;
        else if(data.equals("mov")) return 5;
        else if(data.equals("push")) return 6;
        else if(data.equals("pop")) return 7;
        else if(data.equals("add")) return 8;
        else if(data.equals("adc")) return 9;
        else if(data.equals("inc")) return 10;
        else if(data.equals("sub")) return 11;
        else if(data.equals("sbb")) return 12;
        else if(data.equals("dec")) return 13;
        else if(data.equals("neg")) return 14;
        else if(data.equals("cmp")) return 15;
        else if(data.equals("test")) return 16;
        else if(data.equals("and")) return 17;
        else if(data.equals("or")) return 18;
        else if(data.equals("xor")) return 19;
        else if(data.equals("not")) return 20;
        else if(data.equals("mul")) return 21;
        else if(data.equals("div")) return 22;
        else if(data.equals("imul")) return 23;
        else if(data.equals("idiv")) return 24;
        else if(data.equals("shl")) return 25;//or 1
        else if(data.equals("shr")) return 26;//or 1
        else if(data.equals("sal")) return 27;//or 1
        else if(data.equals("sar")) return 28;//or 1
        else if(data.equals("rol")) return 29;//or 1
        else if(data.equals("ror")) return 30;//or 1
        else if(data.equals("jna")) return 31;
        else if(data.equals("ja")) return 32;
        else if(data.equals("jnl")) return 33;
        else if(data.equals("jl")) return 34;
        else if(data.equals("jng")) return 35;
        else if(data.equals("jg")) return 36;
        else if(data.equals("jmp")) return 37;
        else if(data.equals("jcxz")) return 38;
        else if(data.equals("jcxnz")) return 39;
        else if(data.equals("jne")) return 40;
        else if(data.equals("je")) return 41;
        else if(data.equals("jnz")) return 40;
        else if(data.equals("jz")) return 41;
        else if(data.equals("js")) return 42;
        else if(data.equals("jc")) return 43;
        else if(data.equals("jp")) return 44;
        else if(data.equals("jo")) return 45;
        else if(data.equals("jns")) return 46;
        else if(data.equals("jnc")) return 47;
        else if(data.equals("jnp")) return 48;
        else if(data.equals("jno")) return 49;
        else if(data.equals("call")) return 50;
        else if(data.equals("ret")) return 51;
        return -1;
    }
    
    byte get_opr_num(String data) {
        if(data.equals("cls")) return 0;
        else if(data.equals("print")) return 1;
        //else if(data.equals("input")) return 1;
        else if(data.equals("end")) return 0;
        else if(data.equals("exit")) return 0;
        else if(data.equals("mov")) return 2;
        else if(data.equals("push")) return 1;
        else if(data.equals("pop")) return 1;
        else if(data.equals("add")) return 2;
        else if(data.equals("adc")) return 2;
        else if(data.equals("inc")) return 1;
        else if(data.equals("sub")) return 2;
        else if(data.equals("sbb")) return 2;
        else if(data.equals("dec")) return 1;
        else if(data.equals("neg")) return 1;
        else if(data.equals("cmp")) return 2;
        else if(data.equals("test")) return 2;
        else if(data.equals("and")) return 2;
        else if(data.equals("or")) return 2;
        else if(data.equals("xor")) return 2;
        else if(data.equals("not")) return 1;
        else if(data.equals("mul")) return 2;
        else if(data.equals("div")) return 2;
        else if(data.equals("imul")) return 2;
        else if(data.equals("idiv")) return 2;
        else if(data.equals("shl")) return 2; //or 1
        else if(data.equals("shr")) return 2; //or 1
        else if(data.equals("sal")) return 2; //or 1
        else if(data.equals("sar")) return 2; //or 1
        else if(data.equals("rol")) return 2; //or 1
        else if(data.equals("ror")) return 2; //or 1
        else if(data.equals("jna")) return 1;
        else if(data.equals("ja")) return 1;
        else if(data.equals("jnl")) return 1;
        else if(data.equals("jl")) return 1;
        else if(data.equals("jng")) return 1;
        else if(data.equals("jg")) return 1;
        else if(data.equals("jmp")) return 1;
        else if(data.equals("jcxz")) return 1;
        else if(data.equals("jcxnz")) return 1;
        else if(data.equals("jne")) return 1;
        else if(data.equals("je")) return 1;
        else if(data.equals("jnz")) return 1;
        else if(data.equals("jz")) return 1;
        else if(data.equals("js")) return 1;
        else if(data.equals("jc")) return 1;
        else if(data.equals("jp")) return 1;
        else if(data.equals("jo")) return 1;
        else if(data.equals("jns")) return 1;
        else if(data.equals("jnc")) return 1;
        else if(data.equals("jnp")) return 1;
        else if(data.equals("jno")) return 1;
        else if(data.equals("call")) return 1;
        else if(data.equals("ret")) return 0;
        return -1;
    }
}
package MobileASM;

import javax.microedition.lcdui.*;
/**
 * @author J.Y.Liu
 * @at Hefei, Anhui, China
 * @version v.2009[1st]
 * @date 2009.04
 * @class R_ExeCode
 * @function Run Code Proccess
 */
// eip,esp,ebp
// eax,ecx,ebx,edx
// cs=0,ss=0,ds=0
// esi,edi

// flag
/* | 0  | 1  | 2  | 3  | 4  | 5  | 6  | 7  | 8  | 9  | 10 | 11 | 12 | 13 | 14 | 15 |
 *   CF        PF        AF        ZF   SF   TF   IF   DF   OF
 */

// cls ;clear display string

// input ...
//  e.g input eax, input [eax]

// print ...
//  e.g print a number: print eax, print 128
//  e.g pirnt a character:
//        print :n(n=0 to 65535),print :[eax], print '; -> ';'
//  e.g print a string: pirnt "a, print "hello

// end/exit
// end the program

// mov, push, pop, (lea)
// add, adc, inc, sub, sbb, dec, neg
// cmp, test
// mul, div
// rep-
// and, or, xor, not
// shl_0+, shr_+0, sal_0+, shr_h->l,h=h,cf=l
// rol, ror
// jmp line#, jxx, call line#, ret, loop
//      jna     CF==1 || ZF==1
//      ja      CF==0 && ZF==0
//      jl      SF^OF==1 && ZF==0
//      jnl     SF^OF==0  ||  ZF==1
//      jg      SF^OF==0 && ZF==0
//      jng     SF^OF==1  ||  ZF==1

// not support string opcode
// not support [ebp+ebx]... and [ebx]...

public class R_ExeCode{
    public final int MAX_LINE = 1000;
    public final int MAX_RUN_COUNTER = 30000;
    public final int MAX_SHOW_STR_LEN = 2000;

    String e_code[] = new String[MAX_LINE];

    public Form frm = new Form("Result:");
    String output;
    String lout;
    R_RunStack runstack = new R_RunStack(); // contain reg
    R_Interpreter runint = new R_Interpreter();

    int counter;
    boolean isover;

    public R_ExeCode() {
        codeclear();
        clear();
        frm.addCommand(new Command("Close",Command.OK,1));
    }
    public void codeclear() {
        for(int i=0;i<MAX_LINE;i++) {
            e_code[i]="";
        }
    }
    public void clear() {
        isover = false;
        runstack.clear();
        runint.clear();
        output = "";
        lout = "";
        frm.deleteAll();
        counter = 0;
    }
    public void run() {
        isover = false;
        while(isover==false)
            exe();
    }
    void exe() {
        // ! -> Error
        // . -> End
//System.out.println("\n-- P:" + runstack.eip+"\n-- C:"+counter);
        runstack.eip++;
        counter++;
        if(runstack.eip>=MAX_LINE) {
            output = "Line: "+(runstack.eip+1) + "\n! Not Find: end or exit\n! eip Overflow";
            exit();
            return;
        }
        if(e_code[runstack.eip].length()==0) {
            output = "Line: "+(runstack.eip+1) + "\n! Not Find: end or exit";
            exit();
            return;
        }
        runint.get_line_info(e_code[runstack.eip].trim());
        if(runint.linedata.operator.charAt(0)=='!') {
            output = "Line: "+(runstack.eip+1) + "\n" + runint.linedata.operator;
            exit();
            return;
        }
        if(output.length()>MAX_SHOW_STR_LEN) output = output.substring(output.length()-MAX_SHOW_STR_LEN);
        switch(runint.linedata.opr_no) {
            case 1:
                cls();
                break;
            case 2:
                lout = print(runint.linedata);
                break;
            //case 3:
            case 4:
                exit();
                /// exit  proccess
                /// return
                return;
            case 5:
                lout = mov(runint.linedata);
                break;
            case 6:
                lout = push(runint.linedata);
                break;
            case 7:
                lout = pop(runint.linedata);
                break;
            case 8:
                lout = add(runint.linedata);
                break;
            case 9:
                lout = adc(runint.linedata);
                break;
            case 10:
                lout = inc(runint.linedata);
                break;
            case 11:
                lout = sub(runint.linedata);
                break;
            case 12:
                lout = sbb(runint.linedata);
                break;
            case 13:
                lout = dec(runint.linedata);
                break;
            case 14:
                lout = neg(runint.linedata);
                break;
            case 15:
                lout = cmp(runint.linedata);
                break;
            case 16:
                lout = test(runint.linedata);
                break;
            case 17:
                lout = and(runint.linedata);
                break;
            case 18:
                lout = or(runint.linedata);
                break;
            case 19:
                lout = xor(runint.linedata);
                break;
            case 20:
                lout = not(runint.linedata);
                break;
            case 21:
                lout = mul(runint.linedata);
                break;
            case 22:
                lout = div(runint.linedata);
                break;
            case 23:
                lout = imul(runint.linedata);
                break;
            case 24:
                lout = idiv(runint.linedata);
                break;
            case 25:
                lout = shl(runint.linedata);
                break;
            case 26:
                lout = shr(runint.linedata);
                break;
            case 27:
                lout = sal(runint.linedata);
                break;
            case 28:
                lout = sar(runint.linedata);
                break;
            case 29:
                lout = rol(runint.linedata);
                break;
            case 30:
                lout = ror(runint.linedata);
                break;
            case 31:
                lout = jna(runint.linedata);
                break;
            case 32:
                lout = ja(runint.linedata);
                break;
            case 33:
                lout = jnl(runint.linedata);
                break;
            case 34:
                lout = jl(runint.linedata);
                break;
            case 35:
                lout = jng(runint.linedata);
                break;
            case 36:
                lout = jg(runint.linedata);
                break;
            case 37:
                lout = jmp(runint.linedata);
                break;
            case 38:
                lout = jcxz(runint.linedata);
                break;
            case 39:
                lout = jcxnz(runint.linedata);
                break;
            case 40:
                lout = jne(runint.linedata);
                break;
            case 41:
                lout = je(runint.linedata);
                break;
            case 42:
                lout = js(runint.linedata);
                break;
            case 43:
                lout = jc(runint.linedata);
                break;
            case 44:
                lout = jp(runint.linedata);
                break;
            case 45:
                lout = jo(runint.linedata);
                break;
            case 46:
                lout = jns(runint.linedata);
                break;
            case 47:
                lout = jnc(runint.linedata);
                break;
            case 48:
                lout = jnp(runint.linedata);
                break;
            case 49:
                lout = jno(runint.linedata);
                break;
            case 50:
                lout = call(runint.linedata);
                break;
            case 51:
                lout = ret();
                break;
            default:
                lout = "! Invalid Operator";
        }
        if(counter>MAX_RUN_COUNTER) {
            output = "! I'm too tired to work";
            exit();
            return;
        }
        if(lout.length()==0) return;
        if(lout.charAt(0)=='!') {
            output = "Line: "+(runstack.eip+1) + "\n" + lout;
            exit();
            return;
        }
        return;
    }

    int read_reg(String data) {
        if(data.equals("eax")) return runstack.eax;
        else if(data.equals("ebx")) return runstack.ebx;
        else if(data.equals("ecx")) return runstack.ecx;
        else if(data.equals("edx")) return runstack.edx;
        else if(data.equals("esi")) return runstack.esi;
        else if(data.equals("edi")) return runstack.edi;
        else if(data.equals("ebp")) return runstack.ebp;
        else if(data.equals("esp")) return runstack.esp;
        else if(data.equals("eip")) return runstack.eip;
        return 0;
    }
    int s2n(String data) {
        int sign = 1;
        int r = 0;
        if(data.charAt(0)=='-') sign = -1;
        for(int i=(1-sign)/2;i<data.length();i++)
            r = r * 10 + (data.charAt(i)-48);
        return r*sign;
    }
    void write_reg(String data, int val) {
        if(data.equals("eax")) runstack.eax = val;
        else if(data.equals("ebx")) runstack.ebx = val;
        else if(data.equals("ecx")) runstack.ecx = val;
        else if(data.equals("edx")) runstack.edx = val;
        else if(data.equals("esi")) runstack.esi = val;
        else if(data.equals("edi")) runstack.edi = val;
        else if(data.equals("ebp")) runstack.ebp = val;
        else if(data.equals("esp")) runstack.esp = val;
        else if(data.equals("eip")) runstack.eip = val;
        return;
    }
    int read_opr(String data, byte mode) {
        if(mode==0) return s2n(data);
        if(mode==1) return read_reg(data);
        return 0;
    }

    void  cls() {
        output = "";
    }
    String  print(R_LineInfo data) {
        int tmp;
        switch(data.opr_type_1) {
            case 0:
                output+=data.opr_1;
                break;
            case 1:
                tmp = read_reg(data.opr_1);
                output+="" + tmp;
                break;
            case 10:
                tmp = s2n(data.opr_1);
                if(tmp<0 || tmp>runstack.MAX_MEM-1) return "! Out of Memory Range";
                tmp = runstack.mread(tmp);
                output+="" + tmp;
                break;
            case 11:
                tmp = read_reg(data.opr_1);
                if(tmp<0 || tmp>runstack.MAX_MEM-1) return "! Out of Memory Range";
                tmp = runstack.mread(tmp);
                output+="" + tmp;
                break;
            case 20:
                tmp = s2n(data.opr_1);
                if(tmp<0 || tmp>65535) return "! Invalid Character Code";
                output+=(char)tmp;
                break;
            case 21:
                tmp = read_reg(data.opr_1);
                if(tmp<0 || tmp>65535) return "! Invalid Character Code";
                output+=(char)tmp;
                break;
            case 30:
                tmp = s2n(data.opr_1);
                if(tmp<0 || tmp>runstack.MAX_MEM-1) return "! Out of Memory Range";
                tmp = runstack.mread(tmp);
                if(tmp<0 || tmp>65535) return "! Invalid Character Code";
                output+=(char)tmp;
                break;
            case 31:
                tmp = read_reg(data.opr_1);
                if(tmp<0 || tmp>runstack.MAX_MEM-1) return "! Out of Memory Range";
                tmp = runstack.mread(tmp);
                if(tmp<0 || tmp>65535) return "! Invalid Character Code";
                output+=(char)tmp;
                break;
            case 40:
                output+=data.opr_1;
                break;
            case 41:
                output+=data.opr_1;
        }
        return "";
    }
    //String  input(R_LineInfo data) {
    //    return "";
    //}
    void  exit() {
        isover = true;
        output+="\n-- Stop --";
        frm.append(output);
    }

    String  mov(R_LineInfo data) {
        if(data.opr_type_1==0) return "! Invalid Oprand";
        int tmp = read_opr(data.opr_2, (byte)(data.opr_type_2 % runint.DATA_BASE));
        int tmp2 = -1;
        if(data.opr_type_1>=runint.DATA_BASE) tmp2 = read_opr(data.opr_1, (byte)(data.opr_type_1 % runint.DATA_BASE));
        if(tmp2==-1) {
            if(data.opr_1.equals("esp") && data.opr_type_2==0) return "! Invalid Oprand";
            if(data.opr_1.equals("esp") && data.opr_type_2>=runint.DATA_BASE) return "! Invalid Oprand";
            if(data.opr_1.equals("eip") && data.opr_type_2==0) return "! Invalid Oprand";
            if(data.opr_1.equals("eip") && data.opr_type_2>=runint.DATA_BASE) return "! Invalid Oprand";
            write_reg(data.opr_1, tmp);
        }
        else {
            if(tmp2<0 || tmp2>runstack.MAX_MEM-1) return "! Out of Memory Range";
            runstack.mwrite(tmp2, tmp);
        }
        return "";
    }
    String  push(R_LineInfo data) {
        int tmp = read_opr(data.opr_1, (byte)(data.opr_type_1 % runint.DATA_BASE));
        if(data.opr_type_1>=runint.DATA_BASE){
            if(tmp<0 || tmp>runstack.MAX_MEM-1) return "! Out of Memory Range";
            tmp = runstack.mread(tmp);
        }
        tmp = runstack.push_1U(tmp);
        if(tmp==-1) return "! Out of Memory Range";
        return "";
    }
    String  pop(R_LineInfo data) {
        if(data.opr_type_1==0) return "! Invalid Oprand";
        if(data.opr_type_1==1) {
            if(runstack.esp>=runstack.MAX_MEM-1) return "! Out of Memory Range";
            write_reg(data.opr_1, runstack.pop_1U());
        } else {
            int tmp = read_opr(data.opr_1, (byte)(data.opr_type_1 % runint.DATA_BASE));
            if(tmp<0 || tmp>runstack.MAX_MEM-1) return "! Out of Memory Range";
            runstack.mwrite(tmp, runstack.pop_1U());
        }
        return "";
    }

    String  add(R_LineInfo data) {
        int tmp_r = 0;
        int tmp2_r = 0;
        int tmp = read_opr(data.opr_1, (byte)(data.opr_type_1 % runint.DATA_BASE));
        tmp_r = tmp;
        if(data.opr_type_1>=runint.DATA_BASE) {
            if(tmp<0 || tmp>runstack.MAX_MEM-1) return "! Out of Memory Range";
            tmp_r=runstack.mread(tmp);
        }
        int tmp2 = read_opr(data.opr_2, (byte)(data.opr_type_2 % runint.DATA_BASE));
        tmp2_r = tmp2;
        if(data.opr_type_2>=runint.DATA_BASE) {
            if(tmp2<0 || tmp2>runstack.MAX_MEM-1) return "! Out of Memory Range";
            tmp2_r=runstack.mread(tmp2);
        }
        tmp_r +=tmp2_r;
            if(tmp_r>32767 || tmp_r<-32768) runstack.CF(1);
                                       else runstack.CF(0);
        if(tmp_r>65535 || tmp_r<-32768) {
            tmp_r &= 65535;
            runstack.OF(1);
        } else runstack.OF(0);
        change_flag(tmp_r);
        switch(data.opr_type_1) {
            case 1:
                write_reg(data.opr_1, tmp_r);
                break;
            case 10:
                runstack.mwrite(tmp, tmp_r);
                break;
            case 11:
                runstack.mwrite(tmp, tmp_r);
        }
        return "";
    }
    String  adc(R_LineInfo data) {
        int tmp_r = 0;
        int tmp2_r = 0;
        int tmp = read_opr(data.opr_1, (byte)(data.opr_type_1 % runint.DATA_BASE));
        tmp_r = tmp;
        if(data.opr_type_1>=runint.DATA_BASE) {
            if(tmp<0 || tmp>runstack.MAX_MEM-1) return "! Out of Memory Range";
            tmp_r=runstack.mread(tmp);
        }
        int tmp2 = read_opr(data.opr_2, (byte)(data.opr_type_2 % runint.DATA_BASE));
        tmp2_r = tmp2;
        if(data.opr_type_2>=runint.DATA_BASE) {
            if(tmp2<0 || tmp2>runstack.MAX_MEM-1) return "! Out of Memory Range";
            tmp2_r=runstack.mread(tmp2);
        }
        tmp_r +=tmp2_r+runstack.OF(-1);
            if(tmp_r>32767 || tmp_r<-32768) runstack.CF(1);
                                       else runstack.CF(0);
        if(tmp_r>65535 || tmp_r<-32768) {
            tmp_r &= 65535;
            runstack.OF(1);
        } else runstack.OF(0);
        change_flag(tmp_r);
        switch(data.opr_type_1) {
            case 1:
                write_reg(data.opr_1, tmp_r);
                break;
            case 10:
                runstack.mwrite(tmp, tmp_r);
                break;
            case 11:
                runstack.mwrite(tmp, tmp_r);
        }
        return "";
    }
    String  inc(R_LineInfo data) {
        int tmp_r = 0;
        int tmp = read_opr(data.opr_1, (byte)(data.opr_type_1 % runint.DATA_BASE));
        tmp_r = tmp;
        if(data.opr_type_1>=runint.DATA_BASE) {
            if(tmp<0 || tmp>runstack.MAX_MEM-1) return "! Out of Memory Range";
            tmp_r=runstack.mread(tmp);
        }
        tmp_r ++;
            if(tmp_r>32767 || tmp_r<-32768) runstack.CF(1);
                                       else runstack.CF(0);
        if(tmp_r>65535 || tmp_r<-32768) {
            tmp_r &= 65535;
            runstack.OF(1);
        } else runstack.OF(0);
        change_flag(tmp_r);
        switch(data.opr_type_1) {
            case 1:
                write_reg(data.opr_1, tmp_r);
                break;
            case 10:
                runstack.mwrite(tmp, tmp_r);
                break;
            case 11:
                runstack.mwrite(tmp, tmp_r);
        }
        return "";
    }
    String  sub(R_LineInfo data) {
        int tmp_r = 0;
        int tmp2_r = 0;
        int tmp = read_opr(data.opr_1, (byte)(data.opr_type_1 % runint.DATA_BASE));
        tmp_r = tmp;
        if(data.opr_type_1>=runint.DATA_BASE) {
            if(tmp<0 || tmp>runstack.MAX_MEM-1) return "! Out of Memory Range";
            tmp_r=runstack.mread(tmp);
        }
        int tmp2 = read_opr(data.opr_2, (byte)(data.opr_type_2 % runint.DATA_BASE));
        tmp2_r = tmp2;
        if(data.opr_type_2>=runint.DATA_BASE) {
            if(tmp2<0 || tmp2>runstack.MAX_MEM-1) return "! Out of Memory Range";
            tmp2_r=runstack.mread(tmp2);
        }
        tmp_r -=tmp2_r;
            if(tmp_r>32767 || tmp_r<-32768) runstack.CF(1);
                                       else runstack.CF(0);
        if(tmp_r>65535 || tmp_r<-32768) {
            tmp_r &= 65535;
            runstack.OF(1);
        } else runstack.OF(0);
        change_flag(tmp_r);
        switch(data.opr_type_1) {
            case 1:
                write_reg(data.opr_1, tmp_r);
                break;
            case 10:
                runstack.mwrite(tmp, tmp_r);
                break;
            case 11:
                runstack.mwrite(tmp, tmp_r);
        }
        return "";
    }
    String  sbb(R_LineInfo data) {
        int tmp_r = 0;
        int tmp2_r = 0;
        int tmp = read_opr(data.opr_1, (byte)(data.opr_type_1 % runint.DATA_BASE));
        tmp_r = tmp;
        if(data.opr_type_1>=runint.DATA_BASE) {
            if(tmp<0 || tmp>runstack.MAX_MEM-1) return "! Out of Memory Range";
            tmp_r=runstack.mread(tmp);
        }
        int tmp2 = read_opr(data.opr_2, (byte)(data.opr_type_2 % runint.DATA_BASE));
        tmp2_r = tmp2;
        if(data.opr_type_2>=runint.DATA_BASE) {
            if(tmp2<0 || tmp2>runstack.MAX_MEM-1) return "! Out of Memory Range";
            tmp2_r=runstack.mread(tmp2);
        }
        tmp_r -=tmp2_r-runstack.CF(-1);
            if(tmp_r>32767 || tmp_r<-32768) runstack.CF(1);
                                       else runstack.CF(0);
        if(tmp_r>65535 || tmp_r<-32768) {
            tmp_r &= 65535;
            runstack.OF(1);
        } else runstack.OF(0);
        change_flag(tmp_r);
        switch(data.opr_type_1) {
            case 1:
                write_reg(data.opr_1, tmp_r);
                break;
            case 10:
                runstack.mwrite(tmp, tmp_r);
                break;
            case 11:
                runstack.mwrite(tmp, tmp_r);
        }
        return "";
    }
    String  dec(R_LineInfo data) {
        int tmp_r = 0;
        int tmp = read_opr(data.opr_1, (byte)(data.opr_type_1 % runint.DATA_BASE));
        tmp_r = tmp;
        if(data.opr_type_1>=runint.DATA_BASE) {
            if(tmp<0 || tmp>runstack.MAX_MEM-1) return "! Out of Memory Range";
            tmp_r=runstack.mread(tmp);
        }
        tmp_r --;
            if(tmp_r>32767 || tmp_r<-32768) runstack.CF(1);
                                       else runstack.CF(0);
        if(tmp_r>65535 || tmp_r<-32768) {
            tmp_r &= 65535;
            runstack.OF(1);
        } else runstack.OF(0);
        change_flag(tmp_r);
        switch(data.opr_type_1) {
            case 1:
                write_reg(data.opr_1, tmp_r);
                break;
            case 10:
                runstack.mwrite(tmp, tmp_r);
                break;
            case 11:
                runstack.mwrite(tmp, tmp_r);
        }
        return "";
    }
    String  neg(R_LineInfo data) { //////////////////////////////////////////
        int tmp_r = 0;
        int tmp = read_opr(data.opr_1, (byte)(data.opr_type_1 % runint.DATA_BASE));
        tmp_r = tmp;
        if(data.opr_type_1>=runint.DATA_BASE) {
            if(tmp<0 || tmp>runstack.MAX_MEM-1) return "! Out of Memory Range";
            tmp_r=runstack.mread(tmp);
        }
        tmp_r = -tmp_r;
            if(tmp_r>32767 || tmp_r<-32768) runstack.CF(1);
                                       else runstack.CF(0);
        if(tmp_r>65535 || tmp_r<-32768) {
            tmp_r &= 65535;
            runstack.OF(1);
        } else runstack.OF(0);
        change_flag(tmp_r);
        switch(data.opr_type_1) {
            case 1:
                write_reg(data.opr_1, tmp_r);
                break;
            case 10:
                runstack.mwrite(tmp, tmp_r);
                break;
            case 11:
                runstack.mwrite(tmp, tmp_r);
        }
        return "";
    }
    String  cmp(R_LineInfo data) {
        int tmp = read_opr(data.opr_1, (byte)(data.opr_type_1 % runint.DATA_BASE));
        if(data.opr_type_1>=runint.DATA_BASE) {
            if(tmp<0 || tmp>runstack.MAX_MEM-1) return "! Out of Memory Range";
            tmp=runstack.mread(tmp);
        }
        int tmp2 = read_opr(data.opr_2, (byte)(data.opr_type_2 % runint.DATA_BASE));
        if(data.opr_type_2>=runint.DATA_BASE) {
            if(tmp2<0 || tmp2>runstack.MAX_MEM-1) return "! Out of Memory Range";
            tmp2=runstack.mread(tmp2);
        }
        tmp = tmp - tmp2;
        if (tmp<0) runstack.CF(1);
        else runstack.CF(0);
        if(tmp>65535 || tmp<-32768) runstack.OF(1);
        else runstack.OF(0);
        tmp &= 65535;
        change_flag(tmp);
        return "";
    }
    String  test(R_LineInfo data) {
        int tmp = read_opr(data.opr_1, (byte)(data.opr_type_1 % runint.DATA_BASE));
        if(data.opr_type_1>=runint.DATA_BASE) {
            if(tmp<0 || tmp>runstack.MAX_MEM-1) return "! Out of Memory Range";
            tmp=runstack.mread(tmp);
        }
        int tmp2 = read_opr(data.opr_2, (byte)(data.opr_type_2 % runint.DATA_BASE));
        if(data.opr_type_2>=runint.DATA_BASE) {
            if(tmp2<0 || tmp2>runstack.MAX_MEM-1) return "! Out of Memory Range";
            tmp2=runstack.mread(tmp2);
        }
        tmp &= tmp2;
        tmp &= 65535;
        if (tmp<0 || tmp>32767) runstack.CF(1);
        else runstack.CF(0);
        runstack.OF(0);
        tmp &= 65535;
        change_flag(tmp);
        return "";
    }
    String  and(R_LineInfo data) {
        int tmp_r = 0;
        int tmp2_r = 0;
        int tmp = read_opr(data.opr_1, (byte)(data.opr_type_1 % runint.DATA_BASE));
        tmp_r = tmp;
        if(data.opr_type_1>=runint.DATA_BASE) {
            if(tmp<0 || tmp>runstack.MAX_MEM-1) return "! Out of Memory Range";
            tmp_r=runstack.mread(tmp);
        }
        int tmp2 = read_opr(data.opr_2, (byte)(data.opr_type_2 % runint.DATA_BASE));
        tmp2_r = tmp2;
        if(data.opr_type_2>=runint.DATA_BASE) {
            if(tmp2<0 || tmp2>runstack.MAX_MEM-1) return "! Out of Memory Range";
            tmp2_r=runstack.mread(tmp2);
        }
        tmp_r &=tmp2_r;
        tmp_r &=65535;
            if(tmp_r>32767 || tmp_r<-32768) runstack.CF(1);
                                       else runstack.CF(0);
            runstack.OF(0);
        change_flag(tmp_r);
        switch(data.opr_type_1) {
            case 1:
                write_reg(data.opr_1, tmp_r);
                break;
            case 10:
                runstack.mwrite(tmp, tmp_r);
                break;
            case 11:
                runstack.mwrite(tmp, tmp_r);
        }
        return "";
    }
    String  or(R_LineInfo data) {
        int tmp_r = 0;
        int tmp2_r = 0;
        int tmp = read_opr(data.opr_1, (byte)(data.opr_type_1 % runint.DATA_BASE));
        tmp_r = tmp;
        if(data.opr_type_1>=runint.DATA_BASE) {
            if(tmp<0 || tmp>runstack.MAX_MEM-1) return "! Out of Memory Range";
            tmp_r=runstack.mread(tmp);
        }
        int tmp2 = read_opr(data.opr_2, (byte)(data.opr_type_2 % runint.DATA_BASE));
        tmp2_r = tmp2;
        if(data.opr_type_2>=runint.DATA_BASE) {
            if(tmp2<0 || tmp2>runstack.MAX_MEM-1) return "! Out of Memory Range";
            tmp2_r=runstack.mread(tmp2);
        }
        tmp_r |=tmp2_r;
        tmp_r &=65535;
            if(tmp_r>32767 || tmp_r<-32768) runstack.CF(1);
                                       else runstack.CF(0);
            runstack.OF(0);
        change_flag(tmp_r);
        switch(data.opr_type_1) {
            case 1:
                write_reg(data.opr_1, tmp_r);
                break;
            case 10:
                runstack.mwrite(tmp, tmp_r);
                break;
            case 11:
                runstack.mwrite(tmp, tmp_r);
        }
        return "";
    }
    String  xor(R_LineInfo data) {
        int tmp_r = 0;
        int tmp2_r = 0;
        int tmp = read_opr(data.opr_1, (byte)(data.opr_type_1 % runint.DATA_BASE));
        tmp_r = tmp;
        if(data.opr_type_1>=runint.DATA_BASE) {
            if(tmp<0 || tmp>runstack.MAX_MEM-1) return "! Out of Memory Range";
            tmp_r=runstack.mread(tmp);
        }
        int tmp2 = read_opr(data.opr_2, (byte)(data.opr_type_2 % runint.DATA_BASE));
        tmp2_r = tmp2;
        if(data.opr_type_2>=runint.DATA_BASE) {
            if(tmp2<0 || tmp2>runstack.MAX_MEM-1) return "! Out of Memory Range";
            tmp2_r=runstack.mread(tmp2);
        }
        tmp_r ^=tmp2_r;
        tmp_r &=65535;
            if(tmp_r>32767 || tmp_r<-32768) runstack.CF(1);
                                       else runstack.CF(0);
            runstack.OF(0);
        change_flag(tmp_r);
        switch(data.opr_type_1) {
            case 1:
                write_reg(data.opr_1, tmp_r);
                break;
            case 10:
                runstack.mwrite(tmp, tmp_r);
                break;
            case 11:
                runstack.mwrite(tmp, tmp_r);
        }
        return "";
    }
    String  not(R_LineInfo data) {
        int tmp_r = 0;
        int tmp = read_opr(data.opr_1, (byte)(data.opr_type_1 % runint.DATA_BASE));
        tmp_r = tmp;
        if(data.opr_type_1>=runint.DATA_BASE) {
            if(tmp<0 || tmp>runstack.MAX_MEM-1) return "! Out of Memory Range";
            tmp_r=runstack.mread(tmp);
        }
        tmp_r = ~tmp_r;
        tmp_r &= 65535;
            if(tmp_r>32767 || tmp_r<-32768) runstack.CF(1);
                                       else runstack.CF(0);
        runstack.OF(0);
        change_flag(tmp_r);
        switch(data.opr_type_1) {
            case 1:
                write_reg(data.opr_1, tmp_r);
                break;
            case 10:
                runstack.mwrite(tmp, tmp_r);
                break;
            case 11:
                runstack.mwrite(tmp, tmp_r);
        }
        return "";
    }
    String  mul(R_LineInfo data) { // not used
        return "";
    }
    String  div(R_LineInfo data) { // not used
        return "";
    }
    String  imul(R_LineInfo data) { // not used
        return "";
    }
    String  idiv(R_LineInfo data) { // not used
        return "";
    }
    String  shl(R_LineInfo data) { // not used
        return "";
    }
    String  shr(R_LineInfo data) { // not used
        return "";
    }
    String  sal(R_LineInfo data) { // not used
        return "";
    }
    String  sar(R_LineInfo data) { // not used
        return "";
    }
    String  rol(R_LineInfo data) { // not used
        return "";
    }
    String  ror(R_LineInfo data) { // not used
        return "";
    }
    String  jna(R_LineInfo data) {
        int tmp = read_opr(data.opr_1, (byte)(data.opr_type_1 % runint.DATA_BASE));
        if(data.opr_type_1>=runint.DATA_BASE) {
            if(tmp<0 || tmp>runstack.MAX_MEM-1) return "! Out of Memory Range";
            tmp=runstack.mread(tmp);
        }
        if(tmp<1 || tmp>=MAX_LINE) return "! Out of Line Range";
        if(e_code[tmp-1].length()==0) return "! Out of Line Range";
        if(runstack.CF(-1)==1 || runstack.ZF(-1)==1) runstack.eip = tmp-2;
        return "";
    }
    String  ja(R_LineInfo data) {
        int tmp = read_opr(data.opr_1, (byte)(data.opr_type_1 % runint.DATA_BASE));
        if(data.opr_type_1>=runint.DATA_BASE) {
            if(tmp<0 || tmp>runstack.MAX_MEM-1) return "! Out of Memory Range";
            tmp=runstack.mread(tmp);
        }
        if(tmp<1 || tmp>=MAX_LINE) return "! Out of Line Range";
        if(e_code[tmp-1].length()==0) return "! Out of Line Range";
        if(runstack.CF(-1)==0 && runstack.ZF(-1)==0) runstack.eip = tmp-2;
        return "";
    }
    String  jnl(R_LineInfo data) {
        int tmp = read_opr(data.opr_1, (byte)(data.opr_type_1 % runint.DATA_BASE));
        if(data.opr_type_1>=runint.DATA_BASE) {
            if(tmp<0 || tmp>runstack.MAX_MEM-1) return "! Out of Memory Range";
            tmp=runstack.mread(tmp);
        }
        if(tmp<1 || tmp>=MAX_LINE) return "! Out of Line Range";
        if(e_code[tmp-1].length()==0) return "! Out of Line Range";
        if((runstack.SF(-1) & runstack.OF(-1))==0 || runstack.ZF(-1)==1) runstack.eip = tmp-2;
        return "";
    }
    String  jl(R_LineInfo data) {
        int tmp = read_opr(data.opr_1, (byte)(data.opr_type_1 % runint.DATA_BASE));
        if(data.opr_type_1>=runint.DATA_BASE) {
            if(tmp<0 || tmp>runstack.MAX_MEM-1) return "! Out of Memory Range";
            tmp=runstack.mread(tmp);
        }
        if(tmp<1 || tmp>=MAX_LINE) return "! Out of Line Range";
        if(e_code[tmp-1].length()==0) return "! Out of Line Range";
        if((runstack.SF(-1) & runstack.OF(-1))==1 && runstack.ZF(-1)==0) runstack.eip = tmp-2;
        return "";
    }
    String  jng(R_LineInfo data) {
        int tmp = read_opr(data.opr_1, (byte)(data.opr_type_1 % runint.DATA_BASE));
        if(data.opr_type_1>=runint.DATA_BASE) {
            if(tmp<0 || tmp>runstack.MAX_MEM-1) return "! Out of Memory Range";
            tmp=runstack.mread(tmp);
        }
        if(tmp<1 || tmp>=MAX_LINE) return "! Out of Line Range";
        if(e_code[tmp-1].length()==0) return "! Out of Line Range";
        if((runstack.SF(-1) & runstack.OF(-1))==1 || runstack.ZF(-1)==1) runstack.eip = tmp-2;
        return "";
    }
    String  jg(R_LineInfo data) {
        int tmp = read_opr(data.opr_1, (byte)(data.opr_type_1 % runint.DATA_BASE));
        if(data.opr_type_1>=runint.DATA_BASE) {
            if(tmp<0 || tmp>runstack.MAX_MEM-1) return "! Out of Memory Range";
            tmp=runstack.mread(tmp);
        }
        if(tmp<1 || tmp>=MAX_LINE) return "! Out of Line Range";
        if(e_code[tmp-1].length()==0) return "! Out of Line Range";
        if((runstack.SF(-1) & runstack.OF(-1))==0 && runstack.ZF(-1)==0) runstack.eip = tmp-2;
        return "";
    }
    String  jmp(R_LineInfo data) {
//System.out.println("-- jmp");
        int tmp = read_opr(data.opr_1, (byte)(data.opr_type_1 % runint.DATA_BASE));
        if(data.opr_type_1>=runint.DATA_BASE) {
            if(tmp<0 || tmp>runstack.MAX_MEM-1) return "! Out of Memory Range";
            tmp=runstack.mread(tmp);
        }
        if(tmp<1 || tmp>=MAX_LINE) return "! Out of Line Range";
        if(e_code[tmp-1].length()==0) return "! Out of Line Range";
//System.out.println("-- A "+tmp+"/"+runstack.eip);
        runstack.eip = tmp-2;
        return "";
    }
    String  jcxz(R_LineInfo data) {
        int tmp = read_opr(data.opr_1, (byte)(data.opr_type_1 % runint.DATA_BASE));
        if(data.opr_type_1>=runint.DATA_BASE) {
            if(tmp<0 || tmp>runstack.MAX_MEM-1) return "! Out of Memory Range";
            tmp=runstack.mread(tmp);
        }
        if(tmp<1 || tmp>=MAX_LINE) return "! Out of Line Range";
        if(e_code[tmp-1].length()==0) return "! Out of Line Range";
        if(runstack.ecx==0) runstack.eip = tmp-2;
        return "";
    }
    String  jcxnz(R_LineInfo data) {
        int tmp = read_opr(data.opr_1, (byte)(data.opr_type_1 % runint.DATA_BASE));
        if(data.opr_type_1>=runint.DATA_BASE) {
            if(tmp<0 || tmp>runstack.MAX_MEM-1) return "! Out of Memory Range";
            tmp=runstack.mread(tmp);
        }
        if(tmp<1 || tmp>=MAX_LINE) return "! Out of Line Range";
        if(e_code[tmp-1].length()==0) return "! Out of Line Range";
        if(runstack.ecx!=0) runstack.eip = tmp-2;
        return "";
    }
    String  jne(R_LineInfo data) {
        int tmp = read_opr(data.opr_1, (byte)(data.opr_type_1 % runint.DATA_BASE));
        if(data.opr_type_1>=runint.DATA_BASE) {
            if(tmp<0 || tmp>runstack.MAX_MEM-1) return "! Out of Memory Range";
            tmp=runstack.mread(tmp);
        }
        if(tmp<1 || tmp>=MAX_LINE) return "! Out of Line Range";
        if(e_code[tmp-1].length()==0) return "! Out of Line Range";
        if(runstack.ZF(-1)==1) runstack.eip = tmp-2;
        return "";
    }
    String  je(R_LineInfo data) {
        int tmp = read_opr(data.opr_1, (byte)(data.opr_type_1 % runint.DATA_BASE));
        if(data.opr_type_1>=runint.DATA_BASE) {
            if(tmp<0 || tmp>runstack.MAX_MEM-1) return "! Out of Memory Range";
            tmp=runstack.mread(tmp);
        }
        if(tmp<1 || tmp>=MAX_LINE) return "! Out of Line Range";
        if(e_code[tmp-1].length()==0) return "! Out of Line Range";
        if(runstack.ZF(-1)==0) runstack.eip = tmp-2;
        return "";
    }
    String  js(R_LineInfo data) {
        int tmp = read_opr(data.opr_1, (byte)(data.opr_type_1 % runint.DATA_BASE));
        if(data.opr_type_1>=runint.DATA_BASE) {
            if(tmp<0 || tmp>runstack.MAX_MEM-1) return "! Out of Memory Range";
            tmp=runstack.mread(tmp);
        }
        if(tmp<1 || tmp>=MAX_LINE) return "! Out of Line Range";
        if(e_code[tmp-1].length()==0) return "! Out of Line Range";
        if(runstack.SF(-1)==1) runstack.eip = tmp-2;
        return "";
    }
    String  jc(R_LineInfo data) {
        int tmp = read_opr(data.opr_1, (byte)(data.opr_type_1 % runint.DATA_BASE));
        if(data.opr_type_1>=runint.DATA_BASE) {
            if(tmp<0 || tmp>runstack.MAX_MEM-1) return "! Out of Memory Range";
            tmp=runstack.mread(tmp);
        }
        if(tmp<1 || tmp>=MAX_LINE) return "! Out of Line Range";
        if(e_code[tmp-1].length()==0) return "! Out of Line Range";
        if(runstack.CF(-1)==1) runstack.eip = tmp-2;
        return "";
    }
    String  jp(R_LineInfo data) {
        int tmp = read_opr(data.opr_1, (byte)(data.opr_type_1 % runint.DATA_BASE));
        if(data.opr_type_1>=runint.DATA_BASE) {
            if(tmp<0 || tmp>runstack.MAX_MEM-1) return "! Out of Memory Range";
            tmp=runstack.mread(tmp);
        }
        if(tmp<1 || tmp>=MAX_LINE) return "! Out of Line Range";
        if(e_code[tmp-1].length()==0) return "! Out of Line Range";
        if(runstack.PF(-1)==1) runstack.eip = tmp-2;
        return "";
    }
    String  jo(R_LineInfo data) {
        int tmp = read_opr(data.opr_1, (byte)(data.opr_type_1 % runint.DATA_BASE));
        if(data.opr_type_1>=runint.DATA_BASE) {
            if(tmp<0 || tmp>runstack.MAX_MEM-1) return "! Out of Memory Range";
            tmp=runstack.mread(tmp);
        }
        if(tmp<1 || tmp>=MAX_LINE) return "! Out of Line Range";
        if(e_code[tmp-1].length()==0) return "! Out of Line Range";
        if(runstack.OF(-1)==1) runstack.eip = tmp-2;
        return "";
    }
    String  jns(R_LineInfo data) {
        int tmp = read_opr(data.opr_1, (byte)(data.opr_type_1 % runint.DATA_BASE));
        if(data.opr_type_1>=runint.DATA_BASE) {
            if(tmp<0 || tmp>runstack.MAX_MEM-1) return "! Out of Memory Range";
            tmp=runstack.mread(tmp);
        }
        if(tmp<1 || tmp>=MAX_LINE) return "! Out of Line Range";
        if(e_code[tmp-1].length()==0) return "! Out of Line Range";
        if(runstack.SF(-1)==0) runstack.eip = tmp-2;
        return "";
    }
    String  jnc(R_LineInfo data) {
        int tmp = read_opr(data.opr_1, (byte)(data.opr_type_1 % runint.DATA_BASE));
        if(data.opr_type_1>=runint.DATA_BASE) {
            if(tmp<0 || tmp>runstack.MAX_MEM-1) return "! Out of Memory Range";
            tmp=runstack.mread(tmp);
        }
        if(tmp<1 || tmp>=MAX_LINE) return "! Out of Line Range";
        if(e_code[tmp-1].length()==0) return "! Out of Line Range";
        if(runstack.CF(-1)==0) runstack.eip = tmp-2;
        return "";
    }
    String  jnp(R_LineInfo data) {
        int tmp = read_opr(data.opr_1, (byte)(data.opr_type_1 % runint.DATA_BASE));
        if(data.opr_type_1>=runint.DATA_BASE) {
            if(tmp<0 || tmp>runstack.MAX_MEM-1) return "! Out of Memory Range";
            tmp=runstack.mread(tmp);
        }
        if(tmp<1 || tmp>=MAX_LINE) return "! Out of Line Range";
        if(e_code[tmp-1].length()==0) return "! Out of Line Range";
        if(runstack.PF(-1)==0) runstack.eip = tmp-2;
        return "";
    }
    String  jno(R_LineInfo data) {
        int tmp = read_opr(data.opr_1, (byte)(data.opr_type_1 % runint.DATA_BASE));
        if(data.opr_type_1>=runint.DATA_BASE) {
            if(tmp<0 || tmp>runstack.MAX_MEM-1) return "! Out of Memory Range";
            tmp=runstack.mread(tmp);
        }
        if(tmp<1 || tmp>=MAX_LINE) return "! Out of Line Range";
        if(e_code[tmp-1].length()==0) return "! Out of Line Range";
        if(runstack.OF(-1)==0) runstack.eip = tmp-2;
        return "";
    }
    String  call(R_LineInfo data) {
        int tmp = read_opr(data.opr_1, (byte)(data.opr_type_1 % runint.DATA_BASE));
        if(data.opr_type_1>=runint.DATA_BASE) {
            if(tmp<0 || tmp>runstack.MAX_MEM-1) return "! Out of Memory Range";
            tmp=runstack.mread(tmp);
        }
        if(tmp<1 || tmp>=MAX_LINE) return "! Out of Line Range";
        if(e_code[tmp-1].length()==0) return "! Out of Line Range";
        int r = runstack.push_1U(runstack.eip+1);
        if(r==-1) return "! Out of Memory Range";
        runstack.eip = tmp-2;
        return "";
    }
    String  ret() {
        if(runstack.esp>=runstack.MAX_MEM-1) return "! Out of Memory Range";
        int tmp = runstack.pop_1U();
        if(tmp<1 || tmp>=MAX_LINE) return "! Out of Line Range";
        if(e_code[tmp-1].length()==0) return "! Out of Line Range";
        runstack.eip = tmp-1;
        return "";
    }
    
// flag
/* | 0  | 1  | 2  | 3  | 4  | 5  | 6  | 7  | 8  | 9  | 10 | 11 | 12 | 13 | 14 | 15 |
 *   CF        PF        AF        ZF   SF   TF   IF   DF   OF
 */
    void change_flag(int data) {
        change_SF(data);
        change_ZF(data);
        change_PF(data);
    }
    void change_SF(int data) {
        int r;
        r=(data>>15) % 2;
        runstack.SF(r);
    }
    void change_ZF(int data) {
        if(data==0) runstack.ZF(1);
        else runstack.ZF(0);
    }
    void change_PF(int data) {
        int r = data + 0;
        int num = 0;
        while (r>0) {
            num+=r % 2;
            r>>=1;
        }
        if(num%2==0) runstack.PF(1);
        else runstack.PF(0);
    }
}
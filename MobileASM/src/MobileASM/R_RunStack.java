package MobileASM;
/**
 * @author J.Y.Liu
 * @at Hefei, Anhui, China
 * @version v.2009[1st]
 * @date 2009.03
 * @class R_RunStack
 * @function Stack for Running
 * @coment Stack:8KU, Function:1KU, Variable:6KU, Const:1KU
 */
public class R_RunStack {
    public final int MAX_MEM = 16384; //Size: 16KU

    public int memory[] = new int[MAX_MEM];
    
    public int esp, ebp;
    public int eip, esi, edi;
    public int eax, ebx, ecx, edx;
    int flag;
    
    public R_RunStack() {
        for(int i=0;i<MAX_MEM;i++)
            memory[i] = 0;
        clear();
    }
    public void clear() {
        esp = 16383;
        eip = -1;
        ebp=esi=edi=eax=ebx=ecx=edx=0;
        flag = 0;
    }
    public void mwrite(int seek,int data) {
        if (seek<0 || seek>=MAX_MEM) return;
        memory[seek] = data;
    }
    public int mread(int seek) {
        if (seek<0 || seek>=MAX_MEM) return -1;
        return memory[seek];
    }
   public int push_1U(int data) {
        if(esp<=0 || esp>=MAX_MEM) return -1;
        memory[esp] = data;
        esp--;
        return 0;
    }
    public int pop_1U() {
        if(esp<0 || esp>=MAX_MEM-1) return -1;
        esp++;
        return memory[esp];
    }

    public byte CF(int val) {
        if(val==-1) return (byte)(flag % 2);
        if(val==0) {flag &= 65534; return 0;}
        if(val==1) {flag |= 1; return 1;}
        return -1;
    }
    public byte PF(int val) {
        int tmp = (flag<<2);
        if(val==-1) return (byte)(tmp % 2);
        if(val==0) {flag &= 65531; return 0;}
        if(val==1) {flag |= 4; return 1;}
        return -1;
    }
    public byte AF(int val) {
        int tmp = (flag<<4);
        if(val==-1) return (byte)(tmp % 2);
        if(val==0) {flag &= 65519; return 0;}
        if(val==1) {flag |= 16; return 1;}
        return -1;
    }
    public byte ZF(int val) {
        int tmp = (flag<<6);
        if(val==-1) return (byte)(tmp % 2);
        if(val==0) {flag &= 65471; return 0;}
        if(val==1) {flag |= 64; return 1;}
        return -1;
    }
    public byte SF(int val) {
        int tmp = (flag<<7);
        if(val==-1) return (byte)(tmp % 2);
        if(val==0) {flag &= 65407; return 0;}
        if(val==1) {flag |= 64; return 1;}
        return -1;
    }
    public byte TF(int val) {
        int tmp = (flag<<8);
        if(val==-1) return (byte)(tmp % 2);
        if(val==0) {flag &= 65279; return 0;}
        if(val==1) {flag |= 128; return 1;}
        return -1;
    }
    public byte IF(int val) {
        int tmp = (flag<<9);
        if(val==-1) return (byte)(tmp % 2);
        if(val==0) {flag &= 65023; return 0;}
        if(val==1) {flag |= 256; return 1;}
        return -1;
    }
    public byte DF(int val) {
        int tmp = (flag<<10);
        if(val==-1) return (byte)(tmp % 2);
        if(val==0) {flag &= 64511; return 0;}
        if(val==1) {flag |= 512; return 1;}
        return -1;
    }
    public byte OF(int val) {
        int tmp = (flag<<11);
        if(val==-1) return (byte)(tmp % 2);
        if(val==0) {flag &= 63487; return 0;}
        if(val==1) {flag |= 1024; return 1;}
        return -1;
    }
}
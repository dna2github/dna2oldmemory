package MobileASM;

import javax.microedition.lcdui.*;
import javax.microedition.midlet.*;

/**
 * @author J.Y.Liu
 * @at Hefei, Anhui, China
 * @version v.2009[1st]
 * @date 2009.03
 * @class RunEditor
 * @function Editor & Diplay Result
 */
public class RunEditor extends Canvas implements CommandListener {
    public final int MAX_LINE = 1000;
    public final int MAX_CHAR_PER_LINE = 200;
    public final int MAX_STRING_LEN = 256;
    
    public final int KEY_SOFT_LEFT = -6;
    public final int KEY_SOFT_RIGHT = -7;
    public final int KEY_DEL = -8;
    public final int KEY_UP = -1;
    public final int KEY_DOWN = -2;
    public final int KEY_LEFT = - 3;
    public final int KEY_RIGHT = -4;
    public final int KEY_OK = -5;

    RunMain midlet;

    R_ExeCode runcode = new R_ExeCode();

    Form frm_help;

    int e_page = 0;
    int e_index = 0;
    int e_totalline = 0;

    int openmenu = 0;
    int selectmenu = 0;
    String menu_str[] = {"Run","New","Help","Exit"};

    String txt_input = "";
    //  txt_copy = ""; // Copy a line
    TextBox textBox;

    public SCR_SET scrset;

    public RunEditor(RunMain midlet_) {
        try {
            midlet = midlet_;
            this.setFullScreenMode(true);
            scrset = new SCR_SET(this.getWidth(),this.getHeight());
            scrset.line_show = (int)(scrset.h_scr / 23) - 1;
            scrset.line_char = (int)(scrset.w_scr / 6);
            textBox = new TextBox("Insert Codes", "", MAX_CHAR_PER_LINE, TextField.ANY);
            textBox.addCommand(new Command("OK", Command.BACK, 1));
            frm_help = new Form("Help");
            frm_help.addCommand(new Command("Close",Command.BACK, 1));
            frm_help.setCommandListener(this);
            frm_help.append("Register:\neip, esp, ebp, eax, ecx, ebx, edx, esi, edi\n");
            frm_help.append("Flag:\nCF PF ZF SF OF\n");
            frm_help.append("Assembly:\n cls: clear display with string\n print: print a string\ne.g\n  print eax\n  print 154\n  print [eax]\n  print [5]\n  print :num | print :65 -> A\n  print '(character) | print 'A -> A\n  print \"(string) | print \"Hello - > Hello\n exit(end): quit the program\n");
            frm_help.append(" mov, push, pop\n add, adc, inc\n sub, sbb, dec, neg\n and, or, xor, not\n neg, cmp, test\n jmp, call, ret\n jna, ja, jnl, jl, jng, jg\n jne, je, jnz, jz\n jcxz, jcxnz\n js, jc, jp, jo\n jns, jnc, jnp, jno\n");
            frm_help.append("\nSimple Program:\nmov eax,1\nmov ecx,0\nadd ecx,eax\ncmp eax,100\nja 8\nadd eax,1\njmp 3\nprint \"1+2+3+...+100=\npirnt ecx\nexit");
            setCommandListener(this);
        } catch(Exception e) {
            e.printStackTrace();
        }
    }
    
    public void paint(Graphics g) {
        int i;
        int j = e_page * scrset.line_show;
        int k = scrset.line_char;
        String tmp = "";
        p_clear(g);
        g.setColor(scrset.c_editor_litback);
        g.fillRect(0, e_index*23, this.getWidth(), 23);
        g.setColor(scrset.c_editor_word);
        for (i=j;(i<j+scrset.line_show && i<256);i++) {
            tmp = runcode.e_code[i];
            if (tmp.length() == 0)
                break;
            else {
                if (tmp.length()>k-8)
                    g.drawString((i+1)+"."+tmp.substring(0, k-8)+"...", 1, (i-j)*23, Graphics.TOP | Graphics.LEFT);
                else
                    g.drawString((i+1)+"."+tmp, 1, (i-j)*23, Graphics.TOP | Graphics.LEFT);
            }
        }
        if (openmenu==0) {
            g.drawString(" Menu", 1, scrset.line_show*23+5, Graphics.TOP | Graphics.LEFT);
            g.drawString("Run", this.getWidth()-4 , scrset.line_show*23+5, Graphics.TOP | Graphics.RIGHT);
        } else {
            g.drawString(" < > " + menu_str[selectmenu], 1, scrset.line_show*23+5, Graphics.TOP | Graphics.LEFT);
            g.drawString("Cancel", this.getWidth()-4 , scrset.line_show*23+5, Graphics.TOP | Graphics.RIGHT);
        }
    }
    
    private void p_clear(Graphics g) {
        g.setColor(scrset.c_editor_back);
        g.fillRect(0, 0, this.getWidth(), this.getHeight());
    }

    protected  void keyPressed(int keyCode) {
        int tmp = 0;
        switch (keyCode) {
            case KEY_UP:
                if (e_index==0) {
                    if (e_page>0) {
                      e_page--;
                      e_index = scrset.line_show-1;
                      repaint();
                    }
                } else {
                    e_index--;
                    repaint();
                }
                break;
            case KEY_DOWN:
                tmp = scrset.line_show*e_page+e_index+1;
                if (tmp<MAX_LINE && tmp<=e_totalline ) {
                    if (e_index==scrset.line_show-1) {
                        e_page++;
                        e_index = 0;
                        repaint();
                    } else {
                        e_index++;
                        repaint();
                    }
                }
                break;
            case KEY_LEFT:
              if(openmenu==0) {
                if (e_page>0) {
                    e_page--;
                    e_index = scrset.line_show-1;
                    repaint();
                }
              } else {
                  selectmenu=(selectmenu+3)%4;
                  repaint();
              }
                break;
            case KEY_RIGHT:
             if(openmenu==0) {
                tmp = scrset.line_show*(e_page+1);
                if (tmp<MAX_LINE && tmp<e_totalline) {
                    e_page++;
                    e_index = 0;
                    repaint();
                }
             } else {
                 selectmenu=(selectmenu+1)%4;
                 repaint();
             }
                break;
        }
        // System.out.println(e_page +", "+ e_index + "| " + scrset.line_show);
    }

    protected  void keyReleased(int keyCode) {
        switch (keyCode) {
            case KEY_OK:
                if (openmenu==0) {
                    textBox.setString(""); //e_code[e_page * scrset.line_show + e_index]);
                    Display.getDisplay(midlet).setCurrent(textBox);
                    textBox.setCommandListener(this);
                } else {
                 openmenu=0;
                 switch(selectmenu) {
                     case 0: // Run
                         runcode.run();
                         Display.getDisplay(midlet).setCurrent(runcode.frm);
                         runcode.frm.setCommandListener(this);
                         break;
                     case 1: // New
                         e_page = 0;
                         e_index = 0;
                         e_totalline = 0;
                         runcode.clear();
                         runcode.codeclear();
                         break;
                     case 2: // Help
System.out.println("-- Null???");
                         Display.getDisplay(midlet).setCurrent(frm_help);
System.out.println("-- A");
                         break;
                     case 3: // Exit
                         midlet.notifyDestroyed();
                 }
                 repaint();
                }
                break;
            case KEY_DEL:
                int tmp = scrset.line_show*e_page+e_index;
                if (tmp<e_totalline)
                    CodeEmptyLine(tmp);
                repaint();
                break;
            case KEY_SOFT_LEFT: //Go to menu
             if(openmenu==0) {
                 openmenu=1;
                 repaint();
             } else {
                 openmenu=0;
                 switch(selectmenu) {
                     case 0: // Run
                         runcode.run();
                         Display.getDisplay(midlet).setCurrent(runcode.frm);
                         break;
                     case 1: // New
                         e_page = 0;
                         e_index = 0;
                         e_totalline = 0;
                         runcode.clear();
                         runcode.codeclear();
                         break;
                     case 2: // Help
                         frm_help.setCommandListener(this);
                         Display.getDisplay(midlet).setCurrent(frm_help);
                         break;
                     case 3: // Exit
                         midlet.notifyDestroyed();
                 }
                 repaint();
             }
                break; ///////////////////////////////////////////////////
                //midlet.notifyDestroyed();
            case KEY_SOFT_RIGHT: //Go to menu
                if(openmenu==0){
                    runcode.run();
                    Display.getDisplay(midlet).setCurrent(runcode.frm);
                    runcode.frm.setCommandListener(this);
                } else {
                    openmenu=0;
                    repaint();
                }
        }
    }

    protected  void keyRepeated(int keyCode) {
    }

    public void commandAction(Command command, Displayable displayable) {
        int tmp = 0;
        if (command.getLabel().equals("OK")) { //Finished Input
            txt_input = textBox.getString()+"\012";
            Display.getDisplay(midlet).setCurrent(this);
            tmp = scrset.line_show*e_page+e_index;
            if (txt_input.length()>1) {
                int pos = txt_input.indexOf(10);
                String linedata;
                while(pos>=0 && tmp<MAX_LINE) {
                    linedata = txt_input.substring(0, pos);
                    txt_input = txt_input.substring(pos+1);
                    if (linedata.trim().length()>0) {
                        CodeInsertLine(tmp, linedata);
                        e_index++;
                        tmp++;
                    }
                    pos = txt_input.indexOf(10);
                }
            } else {
            //    if (tmp<e_totalline)
            //        CodeEmptyLine(tmp);
            }
            e_page += (int)(e_index/scrset.line_show);
            e_index = e_index % scrset.line_show;
            tmp = scrset.line_show*e_page+e_index;
            if (tmp==MAX_LINE)
                if (e_index==0) {
                    if (e_page>0) {
                      e_page--;
                      e_index = scrset.line_show-1;
                    }
                } else
                    e_index--;
            repaint();
            return;
        }
        if(command.getLabel().equals("Close")) {
            Display.getDisplay(midlet).setCurrent(this);
            runcode.clear();
        }
    }

    public void CodeEmptyLine(int line_num) {
        int i;
        e_totalline--;
        for (i=line_num+1;i<256;i++) {
            runcode.e_code[i-1]=runcode.e_code[i];
        }
        runcode.e_code[MAX_LINE-1] = "";
    }

    public void CodeInsertLine(int line_num, String data) {
        if (data.length()==0) return;
        if (e_totalline<MAX_LINE)
            e_totalline++;
            for(int i=MAX_LINE-1;i>line_num;i--)
                runcode.e_code[i] = runcode.e_code[i-1];
        runcode.e_code[line_num] = data;
    }

}

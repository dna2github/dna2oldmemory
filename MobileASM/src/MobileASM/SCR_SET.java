package MobileASM;

/**
 * @author J.Y.Liu
 * @at Hefei, Anhui, China
 * @version v.2009[1st]
 * @date 2009.03
 * @class SCR_SET
 * @function setup to adapt to the size of one screen
 */
public class SCR_SET{
    public int w_scr; // w = width
    public int h_scr; // h = height

    public int line_show; // the number of lines for codes to display
    public int line_char; // the number of chars per line

    public int c_menu_back; // c = color
    public int c_menu_litback;
    public int c_menu_word;

    public int c_editor_word;
    public int c_editor_litback;
    public int c_editor_back;

    public int c_result_back;
    public int c_result_word;

    public SCR_SET(int wd, int ht) {
        w_scr = wd;
        h_scr = ht;
        line_show = 10;
        line_char = 20;
        c_menu_back = 0x80;
        c_menu_litback = 0x60;
        c_menu_word = 0xC0C0C0;
        c_editor_back = 0x80;
        c_editor_litback = 0x60;
        c_editor_word = 0xC0C0C0;
        c_result_back = 0x0;
        c_result_word = 0xC0C0C0;
    } 
}
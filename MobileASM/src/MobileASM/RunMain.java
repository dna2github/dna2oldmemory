package MobileASM;

import javax.microedition.midlet.*;
import javax.microedition.lcdui.*;

/**
 * @author J.Y.Liu
 * @at Hefei, Anhui, China
 * @version v.2009[1st]
 * @date 2009.03
 * @class RunMain
 * @function as Main() to run the program
 */
public class RunMain extends MIDlet {
    RunPage run_page;

    public RunMain() {
        run_page = new RunPage(this);
    }

    public void startApp() {
        System.out.println("-- DEBUG -- " + (3<<1));
        Display.getDisplay(this).setCurrent(run_page);
    }

    public void pauseApp() {
    }

    public void destroyApp(boolean unconditional) {
    }
}

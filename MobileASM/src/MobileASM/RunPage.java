/*
 * To change this template, choose Tools | Templates
 * and open the template in the editor.
 */

package MobileASM;

import javax.microedition.lcdui.*;
import javax.microedition.midlet.*;

/**
 * @author J.Y.Liu
 * @at Hefei, Anhui, China
 * @version v.2009[1st]
 * @date 2009.03
 * @class RunPage
 * @function Cover Page
 */

public class RunPage extends Canvas implements CommandListener {
    RunMain midlet;
    Image img_title; // 128*160 Image

    public RunPage(RunMain midlet_) {
        try {
            midlet = midlet_;
            this.setFullScreenMode(true);
            img_title = Image.createImage("/MobileASM/title.png");
	    // Add the Exit command
	    // addCommand(new Command("Go", Command.SCREEN, 1));
	    // addCommand(new Command("Go", Command.SCREEN, 1));
	    // Set up this canvas to listen to command events
	    // setCommandListener(this);
        } catch(Exception e) {
            e.printStackTrace();
        }
    } 
    
    public void paint(Graphics g) {
        int wd = this.getWidth();
        int ht = this.getHeight();
        g.setColor(255, 255, 255);
        g.fillRect(0, 0, this.getWidth(), this.getHeight());
        // show the picture
        wd = (wd-img_title.getWidth())/2;
        ht = (ht-img_title.getHeight())/2;
        g.drawImage(img_title, wd, ht, Graphics.LEFT | Graphics.TOP);
    }
    
    protected  void keyPressed(int keyCode) {
            RunEditor run_editor = new RunEditor(midlet);
            Display.getDisplay(midlet).setCurrent(run_editor);
    }

    public void commandAction(Command command, Displayable displayable) {
    }

}
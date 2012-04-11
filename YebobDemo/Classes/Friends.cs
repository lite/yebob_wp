using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Phone.Controls;

using cocos2d;

namespace YebobDemo.Classes
{
    public class Friends : Main
    {
        WebBrowser browser = new WebBrowser();
        
        public Friends()
        {
        }

        public static new CCLayer node()
        {
            Game ret = new Game();

            if (ret.init())
            {
                return ret;
            }
            return null;
        }

        public override bool init()
        {
            if (!base.init())
            {
                return false;
            }

            //CCNode node = 
            //this.addChild(browser);
            
            browser.Navigate(new Uri("http://alpha.yebob.com"));

            return true;
        }

    }
}


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using cocos2d;
using Yebob;

namespace YebobDemo.Classes
{
    class MainMenu : Main
    {
        YebobUI browser;

        public static new CCLayer node()
        {
            MainMenu ret = new MainMenu();
            if (ret.init())
            {
                return ret;
            }

            return null;
        }
        public override bool init()
        {
            CCSprite logo = CCSprite.spriteWithFile(@"Images\logo");
            logo.position = new CCPoint(240, 650);
            addChild(logo);

            CCMenuItem button1 = CCMenuItemImage.itemFromNormalImage(@"Images\loginButton", @"Images\loginButton", this, loginCallback);
            Scale(button1);
            CCMenuItem button2 = CCMenuItemImage.itemFromNormalImage(@"Images\playButton", @"Images\playButton", this, playCallback);
            Scale(button2);
            CCMenuItem button3 = CCMenuItemImage.itemFromNormalImage(@"Images\aboutButton", @"Images\aboutButton", this, aboutCallback);
            Scale(button3);
            CCMenu menu = CCMenu.menuWithItems(button1, button2, button3);

            menu.alignItemsVerticallyWithPadding(15f);
            menu.position = new CCPoint(240, 257);
            this.addChild(menu);

            return base.init();
        }

        void loginCallback(CCObject sender)
        {
            MessageBox.Show("YebobDemo", "loginCallback");
            //YebobUI.Show("http://alpha.yebob.com");
            CCScene pScene = CCScene.node();
            pScene.addChild(YebobDemo.Classes.Friends.node());
            CCDirector.sharedDirector().pushScene(pScene);

            //OpenXLive.Forms.XLiveFormFactory.Factory.ShowForm("Logon");
            //CCDirector.sharedDirector().pause();
            //CCDirector.sharedDirector().runningScene.visible = false;
        }

        void playCallback(CCObject sender)
        {
            CCScene pScene = CCScene.node();
            pScene.addChild(YebobDemo.Classes.Game.node());
            CCDirector.sharedDirector().pushScene(pScene);
        }

        void aboutCallback(CCObject sender)
        {
            MessageBox.Show("YebobDemo", "http://www.yebob.com");
        }

    }
}

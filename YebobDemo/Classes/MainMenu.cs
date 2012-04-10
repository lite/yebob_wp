using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using cocos2d;
//using OpenXLive;

namespace YebobDemo.Classes
{
    class MainMenu : Main
    {
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


            CCMenuItemImage Lobby = CCMenuItemImage.itemFromNormalImage(
               @"images\Lobby",
               @"images\Lobby1",
               this,
               new SEL_MenuHandler(LobbyCallback));
            Lobby.position = new CCPoint(0, 20);

            CCMenuItemImage Achievements = CCMenuItemImage.itemFromNormalImage(
                                          @"images\Achievements",
                                          @"images\Achievements1",
                                          this,
                                          new SEL_MenuHandler(AchievementsCallback));
            Achievements.position = new CCPoint(0, 20);

            CCMenuItemImage Leaderboards = CCMenuItemImage.itemFromNormalImage(
                                       @"images\Leaderboards",
                                       @"images\Leaderboards1",
                                       this,
                                       new SEL_MenuHandler(LeaderboardsCallback));
            Leaderboards.position = new CCPoint(0, 20);

            CCMenu OpenXLiveMenu = CCMenu.menuWithItems(Lobby, Leaderboards, Achievements);
            OpenXLiveMenu.alignItemsHorizontallyWithPadding(40);
            OpenXLiveMenu.position = new CCPoint(240, 60);
            //this.addChild(OpenXLiveMenu, 1);

            return base.init();
        }

        void loginCallback(CCObject sender)
        {
            //OpenXLive.Forms.XLiveFormFactory.Factory.ShowForm("Logon");
            CCDirector.sharedDirector().pause();
            CCDirector.sharedDirector().runningScene.visible = false;
        }

        void playCallback(CCObject sender)
        {
            CCScene pScene = CCScene.node();
            pScene.addChild(YebobDemo.Classes.Game.node());
            CCDirector.sharedDirector().pushScene(pScene);
        }

        void aboutCallback(CCObject sender)
        {
            //MessageBox.Show("about");
        }

        public virtual void AchievementsCallback(CCObject pSender)
        {
            //OpenXLive.Forms.XLiveFormFactory.Factory.ShowForm("Achievements");
            CCDirector.sharedDirector().pause();
            CCDirector.sharedDirector().runningScene.visible = false;
        }

        public virtual void LeaderboardsCallback(CCObject pSender)
        {
            //OpenXLive.Forms.XLiveFormFactory.Factory.ShowForm("Leaderboard");
            CCDirector.sharedDirector().pause();
            CCDirector.sharedDirector().runningScene.visible = false;
        }

        public virtual void LobbyCallback(CCObject pSender)
        {
            //OpenXLive.Forms.XLiveFormFactory.Factory.ShowForm("Lobby");
            CCDirector.sharedDirector().pause();
            CCDirector.sharedDirector().runningScene.visible = false;
        }
    }
}

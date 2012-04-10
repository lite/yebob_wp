using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using cocos2d;
//using OpenXLive;

namespace YebobDemo.Classes
{
    class GameOver : Main
    {
        int currentScore;

        public bool initWithScore(int lastScore)
        {
            if (!base.init())
            {
                return false;
            }

            currentScore = lastScore;

            //CCSpriteBatchNode spriteManager = (CCSpriteBatchNode)getChildByTag((int)tags.kSpriteManager);

            //CCSprite title = CCSprite.spriteWithTexture(spriteManager.Texture, new CCRect(600, 180, 240, 73));
            //Scale(title);
            //title.scaleX = 1.8f;
            //title.scaleY = 2.3f;
            //spriteManager.addChild(title, 5);
            //title.position = new CCPoint(240, 650);
            CCSprite title = CCSprite.spriteWithFile(@"Images\gameOver");
            title.position = new CCPoint(240, 650);
            addChild(title);

            CCLabelBMFont scoreLabel = CCLabelBMFont.labelWithString(currentScore.ToString(), "Fonts/bitmapFont");
            scoreLabel.scaleX = sx * 1.5f;
            scoreLabel.scaleY = sy * 1.5f;
            addChild(scoreLabel, 5, (int)tags.kScoreLabel);
            scoreLabel.position = new CCPoint(240, 450);

            CCMenuItem button1 = CCMenuItemImage.itemFromNormalImage(@"Images\playAgainButton", @"Images\playAgainButton", this, playAgainCallback);
            Scale(button1);
            CCMenuItem button2 = CCMenuItemImage.itemFromNormalImage(@"Images\submitScoreButton", @"Images\submitScoreButton", this, submitScoreCallback);
            Scale(button2);
            CCMenu menu = CCMenu.menuWithItems(button1, button2);

            menu.alignItemsVerticallyWithPadding(15f);
            menu.position = new CCPoint(240, 237);

            this.addChild(menu);

            CCMenuItemImage Lobby = CCMenuItemImage.itemFromNormalImage(
   @"images\Lobby",
   @"images\Lobby1",
   this,
   new SEL_MenuHandler(LobbyCallback));
            Lobby.position = new CCPoint(0, 40);

            CCMenuItemImage Achievements = CCMenuItemImage.itemFromNormalImage(
                                          @"images\Achievements",
                                          @"images\Achievements1",
                                          this,
                                          new SEL_MenuHandler(AchievementsCallback));
            Achievements.position = new CCPoint(0, 40);

            CCMenuItemImage Leaderboards = CCMenuItemImage.itemFromNormalImage(
                                       @"images\Leaderboards",
                                       @"images\Leaderboards1",
                                       this,
                                       new SEL_MenuHandler(LeaderboardsCallback));
            Leaderboards.position = new CCPoint(0, 40);

            CCMenu OpenXLiveMenu = CCMenu.menuWithItems(Lobby, Leaderboards, Achievements);
            OpenXLiveMenu.alignItemsHorizontallyWithPadding(40);
            OpenXLiveMenu.position = new CCPoint(240, 60);
            //this.addChild(OpenXLiveMenu, 1);

            return true;
        }

        void playAgainCallback(CCObject sender)
        {
            CCScene pScene = CCScene.node();
            pScene.addChild(YebobDemo.Classes.Game.node());
            CCDirector.sharedDirector().pushScene(pScene);
        }

        void submitScoreCallback(CCObject sender)
        {
            //OpenXLive.Features.Leaderboard lb = new OpenXLive.Features.Leaderboard(TweeJump.GameMain.manager.CurrentSession, "xxxxxxxx-xxxx-xxxx-xxxxxxxxxxxxxxxx");
            //lb.SubmitScore(currentScore);
            //lb.SubmitScoreCompleted += new OpenXLive.Features.AsyncEventHandler(lb_SubmitScoreCompleted);
        }

        //void lb_SubmitScoreCompleted(object sender, OpenXLive.AsyncEventArgs e)
        //{
        //    if (e.Result.ReturnValue)
        //    {
        //        MessageBox.Show("Succeed");
        //    }
        //    else
        //    {
        //        MessageBox.Show(e.Result.ErrorMessage);
        //    }
        //}

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

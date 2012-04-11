using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using cocos2d;

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

            CCSprite title = CCSprite.spriteWithFile(@"Images\gameOver");
            title.position = new CCPoint(240, 650);
            addChild(title);

            CCLabelBMFont scoreLabel = CCLabelBMFont.labelWithString(currentScore.ToString(), "Fonts/bitmapFont");
            scoreLabel.scaleX = sx * 1.5f;
            scoreLabel.scaleY = sy * 1.5f;
            addChild(scoreLabel, 5, (int)tags.kScoreLabel);
            scoreLabel.position = new CCPoint(240, 450);

            CCMenuItem button1 = CCMenuItemImage.itemFromNormalImage(@"Images\playAgainButton", @"Images\playAgainButton", this, playAgainCallback);
            //Scale(button1);
            CCMenuItem button2 = CCMenuItemImage.itemFromNormalImage(@"Images\submitScoreButton", @"Images\submitScoreButton", this, submitScoreCallback);
            //Scale(button2);
            CCMenu menu = CCMenu.menuWithItems(button1, button2);

            menu.alignItemsVerticallyWithPadding(15f);
            menu.position = new CCPoint(240, 237);

            this.addChild(menu);

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
            MessageBox.Show("YebobDemo", "submitScoreCallback");
            //OpenXLive.Features.Leaderboard lb = new OpenXLive.Features.Leaderboard(TweeJump.GameMain.manager.CurrentSession, "xxxxxxxx-xxxx-xxxx-xxxxxxxxxxxxxxxx");
            //lb.SubmitScore(currentScore);
            //lb.SubmitScoreCompleted += new OpenXLive.Features.AsyncEventHandler(lb_SubmitScoreCompleted);
        }

    }
}

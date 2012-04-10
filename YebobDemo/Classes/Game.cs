using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using cocos2d;
using System.Diagnostics;
using Microsoft.Devices.Sensors;

namespace YebobDemo.Classes
{
    public class Game : Main
    {
        public CCPoint bird_pos = new CCPoint();
        public ccVertex2F bird_vel = new ccVertex2F();
        public ccVertex2F bird_acc = new ccVertex2F();

        public static bool gameSuspended;
        public bool birdLookingRight;

        public int currentPlatformTag;
        public float currentPlatformY;
        public float currentMaxPlatformStep;
        public int currentBonusPlatformIndex;
        public int currentBonusType;
        public int platformCount;

        public int score;

        public Accelerometer accelerometer;

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

            gameSuspended = true;

            CCSpriteBatchNode spriteManager = (CCSpriteBatchNode)getChildByTag((int)tags.kSpriteManager);

            this.initPlatforms();

            CCSprite bird = CCSprite.spriteWithTexture(spriteManager.Texture, new CCRect(608, 16, 44, 32));
            bird.scale = sx;
            spriteManager.addChild(bird, 4, (int)tags.kBird);

            CCSprite bonus;
            for (int i = 0; i < (int)kbonus.kNumBonuses; i++)
            {
                bonus = CCSprite.spriteWithTexture(spriteManager.Texture, new CCRect(608 + i * 32, 256, 25, 25));
                Scale(bonus);
                spriteManager.addChild(bonus, 4, (int)tags.kBonusStartTag + i);
                bonus.visible = false;
            }

            CCLabelBMFont scoreLabel = CCLabelBMFont.labelWithString("0", "Fonts/bitmapFont");
            Scale(scoreLabel);
            addChild(scoreLabel, 5, (int)tags.kScoreLabel);
            scoreLabel.position = new CCPoint(240, 718);

            this.schedule(step);

            isTouchEnabled = false;
            //isAccelerometerEnabled = true;
            float delta = bird_pos.y - 400;

            this.startGame();

            accelerometer = new Accelerometer();
            accelerometer.CurrentValueChanged += accelerometer_CurrentValueChanged;
            accelerometer.Start();
            return true;
        }

        void accelerometer_CurrentValueChanged(object sender, SensorReadingEventArgs<AccelerometerReading> e)
        {
            if (gameSuspended)
            {
                return;
            }
            float accel_filter = 0.1f;
            bird_vel.x = bird_vel.x * accel_filter + e.SensorReading.Acceleration.X * (1f - accel_filter) * 500f;
        }

        void initPlatforms()
        {
            currentPlatformTag = (int)tags.kPlatformsStartTag;

            while (currentPlatformTag < (int)tags.kPlatformsStartTag + kNumPlatforms)
            {
                this.initPlatform();
                currentPlatformTag++;
            }

            this.resetPlatforms();
        }

        void initPlatform()
        {
            CCRect rect = new CCRect();
            switch (random.Next() % 2)
            {
                case 0: rect = new CCRect(608, 64, 102, 36); break;
                case 1: rect = new CCRect(608, 128, 90, 32); break;
            }

            CCSpriteBatchNode spriteManager = (CCSpriteBatchNode)getChildByTag((int)tags.kSpriteManager);
            CCSprite platform = CCSprite.spriteWithTexture(spriteManager.Texture, rect);
            Scale(platform);
            spriteManager.addChild(platform, 3, currentPlatformTag);
        }



        void startGame()
        {
            score = 0;

            this.resetClouds();
            this.resetPlatforms();
            this.resetBird();
            this.resetBonus();

            gameSuspended = false;
        }

        void resetPlatforms()
        {
            currentPlatformY = -1;
            currentPlatformTag = (int)tags.kPlatformsStartTag;
            currentMaxPlatformStep = 60f;
            currentBonusPlatformIndex = 0;
            currentBonusType = 0;
            platformCount = 0;

            while (currentPlatformTag < (int)tags.kPlatformsStartTag + kNumPlatforms)
            {
                this.resetPlatform();
                currentPlatformTag++;
            }
        }

        void resetPlatform()
        {
            if (currentPlatformY < 0)
            {
                currentPlatformY = 50f;
            }
            else
            {
                currentPlatformY += (random.Next() % (int)(currentMaxPlatformStep - kMinPlatformStep) + kMinPlatformStep) * sy;
                if (currentMaxPlatformStep < kMaxPlatformStep)
                {
                    currentMaxPlatformStep += .5f;
                }
            }

            CCSpriteBatchNode spriteManager = (CCSpriteBatchNode)getChildByTag((int)tags.kSpriteManager);
            CCSprite platform = (CCSprite)spriteManager.getChildByTag(currentPlatformTag);

            //if (random.Next() % 2 == 1)
            //    platform.scaleX = -1.5f;


            float x;
            CCSize size = new CCSize(platform.contentSize.width * sx, platform.contentSize.height * sy);
            if (currentPlatformY == 50f)
            {
                x = 240f;
            }
            else
            {
                x = random.Next() % (480 - (int)size.width) + size.width / 2;
            }

            platform.position = new CCPoint(x, currentPlatformY);
            platformCount++;

            if (platformCount == currentBonusPlatformIndex)
            {
                CCSprite bonus = (CCSprite)spriteManager.getChildByTag((int)tags.kBonusStartTag + currentBonusType);
                bonus.position = new CCPoint(x, currentPlatformY + 30 * sy);
                bonus.visible = true;
            }
        }

        void resetBird()
        {
            CCSpriteBatchNode spriteManager = (CCSpriteBatchNode)getChildByTag((int)tags.kSpriteManager);
            CCSprite bird = (CCSprite)spriteManager.getChildByTag((int)tags.kBird);

            bird_pos.x = 240;
            bird_pos.y = 240;
            bird.position = bird_pos;

            bird_vel.x = 0;
            bird_vel.y = 0;

            bird_acc.x = 0;
            bird_acc.y = -917;

            birdLookingRight = true;
            bird.scaleX = 1.0f;
        }

        void resetBonus()
        {
            CCSpriteBatchNode spriteManager = (CCSpriteBatchNode)getChildByTag((int)tags.kSpriteManager);
            CCSprite bonus = (CCSprite)spriteManager.getChildByTag((int)tags.kBonusStartTag + currentBonusType);
            bonus.visible = false;
            currentBonusPlatformIndex += random.Next() % (kMaxBonusStep - kMinBonusStep) + kMinBonusStep;
            if (score < 10000)
            {
                currentBonusType = 0;
            }
            else if (score < 50000)
            {
                currentBonusType = random.Next() % 2;
            }
            else if (score < 100000)
            {
                currentBonusType = random.Next() % 3;
            }
            else
            {
                currentBonusType = random.Next() % 2 + 2;
            }
        }

        new void step(float dt)
        {
            base.step(dt);

            if (gameSuspended) return;

            CCSpriteBatchNode spriteManager = (CCSpriteBatchNode)getChildByTag((int)tags.kSpriteManager);
            CCSprite bird = (CCSprite)spriteManager.getChildByTag((int)tags.kBird);

            bird_pos.x += bird_vel.x * dt;

            #region ----
            //if (bird_vel.x < -30f && birdLookingRight)
            //{
            //    birdLookingRight = false;
            //    bird.scaleX = -sx;
            //}
            //else if (bird_vel.x > 30f && birdLookingRight)
            //{
            //    birdLookingRight = true;
            //    bird.scaleX = sx;
            //} 
            #endregion

            CCSize bird_size = new CCSize(bird.contentSize.width * sx, bird.contentSize.height * sy);
            float max_x = 480 - bird_size.width / 2;
            float min_x = 0 + bird_size.width / 2;

            if (bird_pos.x > max_x) bird_pos.x = max_x;
            if (bird_pos.x < min_x) bird_pos.x = min_x;

            bird_vel.y += bird_acc.y * dt;
            bird_pos.y += bird_vel.y * dt;

            CCSprite bonus = (CCSprite)spriteManager.getChildByTag((int)tags.kBonusStartTag + currentBonusType);
            if (bonus.visible)
            {
                CCPoint bonus_pos = bonus.position;
                float range = 20f;
                if (bird_pos.x > bonus_pos.x - range &&
                    bird_pos.x < bonus_pos.x + range &&
                    bird_pos.y > bonus_pos.y - range &&
                    bird_pos.y < bonus_pos.y + range)
                {
                    switch (currentBonusType)
                    {
                        case (int)kbonus.kBonus5: score += 5000; break;
                        case (int)kbonus.kBonus10: score += 10000; break;
                        case (int)kbonus.kBonus50: score += 50000; break;
                        case (int)kbonus.kBonus100: score += 100000; break;
                    }

                    string scoreStr = score.ToString();
                    CCLabelBMFont scoreLabel = (CCLabelBMFont)this.getChildByTag((int)tags.kScoreLabel);
                    scoreLabel.setString(scoreStr);
                    var a1 = CCScaleTo.actionWithDuration(.2f, 1.5f, .8f);
                    var a2 = CCScaleTo.actionWithDuration(.2f, 1.0f, 1f);
                    var a3 = CCSequence.actions(a1, a2, a1, a2, a1, a2);
                    scoreLabel.runAction(a3);
                    this.resetBonus();
                }
            }

            int t;

            if (bird_vel.y < 0)
            {
                t = (int)tags.kPlatformsStartTag;
                for (int i = t; i < (int)tags.kPlatformsStartTag + kNumPlatforms; i++)
                {
                    CCSprite platform = (CCSprite)spriteManager.getChildByTag(i);

                    CCSize platform_size = new CCSize(platform.contentSize.width * sx, platform.contentSize.height * sy);
                    CCPoint platform_pos = platform.position;

                    max_x = platform_pos.x - platform_size.width / 2 - 10;
                    min_x = platform_pos.x + platform_size.width / 2 + 10;
                    float min_y = platform_pos.y + (platform_size.height + bird_size.height) / 2 - kPlatformTopPadding;

                    if (bird_pos.x > max_x &&
                        bird_pos.x < min_x &&
                        bird_pos.y > platform_pos.y &&
                        bird_pos.y < min_y)
                    {
                        this.jump();
                    }
                }

                if (bird_pos.y < bird_size.height / 2)
                {
                    this.gameOver();
                }
            }
            else
                if (bird_pos.y > 400)
                {
                    float delta = bird_pos.y - 400;
                    bird_pos.y = 400;

                    currentPlatformY -= delta;

                    t = (int)tags.kCloudsStartTag;
                    for (int i = t; i < t + kNumClouds; i++)
                    {
                        CCSprite cloud = (CCSprite)spriteManager.getChildByTag(i);
                        Scale(cloud);
                        CCPoint pos = cloud.position;
                        pos.y -= delta * cloud.scaleY * 0.8f;
                        if (pos.y < -cloud.contentSize.height * sy / 2)
                        {
                            currentCloudTag = i;
                            this.resetCloud();
                        }
                        else
                        {
                            cloud.position = pos;
                        }
                    }

                    t = (int)tags.kPlatformsStartTag;
                    for (int i = t; i < t + kNumPlatforms; i++)
                    {
                        CCSprite platform = (CCSprite)spriteManager.getChildByTag(i);
                        Scale(platform);
                        CCPoint pos = platform.position;
                        pos = new CCPoint(pos.x, pos.y - delta);
                        if (pos.y < -platform.contentSize.height * sy / 2)
                        {
                            currentPlatformTag = i;
                            this.resetPlatform();
                        }
                        else
                        {
                            platform.position = pos;
                        }
                    }

                    if (bonus.visible)
                    {
                        CCPoint pos = bonus.position;
                        pos.y -= delta;
                        if (pos.y < -bonus.contentSize.height * sy / 2)
                        {
                            this.resetBonus();
                        }
                        else
                        {
                            bonus.position = pos;
                        }
                    }

                    score += (int)delta;
                    string scoreStr = score.ToString();

                    CCLabelBMFont scoreLabel = (CCLabelBMFont)this.getChildByTag((int)tags.kScoreLabel);
                    scoreLabel.setString(scoreStr);
                }

            bird.position = bird_pos;
        }

        void jump()
        {
            bird_vel.y = 583f + Math.Abs(bird_vel.x);
            //bird_vel.y = 53f + Math.Abs(bird_vel.x);
        }

        void gameOver()
        {
            gameSuspended = true;

            GameOver highscores = new GameOver();
            highscores.initWithScore(score);

            CCScene scene = CCScene.node();
            scene.addChild(highscores, 0);
            CCDirector.sharedDirector().replaceScene(scene);
        }
    }
}

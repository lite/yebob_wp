using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using cocos2d;

namespace YebobDemo.Classes
{
    public enum kbonus
    {
        kBonus5 = 0,
        kBonus10,
        kBonus50,
        kBonus100,
        kNumBonuses
    }

    public enum tags
    {
        kSpriteManager = 0,
        kBird,
        kScoreLabel,
        kCloudsStartTag = 100,
        kPlatformsStartTag = 200,
        kBonusStartTag = 300
    }

    public class Main : CCLayer
    {
        public const float sy = 1.666666666666667f;
        public const float sx = 1.5f;
        public static Random random = new Random();

        public int kFPS = 60;

        public int kNumClouds = 12;

        public int kMinPlatformStep = 50;
        public int kMaxPlatformStep = 300;
        public int kNumPlatforms = 10;
        public int kPlatformTopPadding = 10;

        public int kMinBonusStep = 30;
        public int kMaxBonusStep = 50;

        public int currentCloudTag;

        public override bool init()
        {
            if (!base.init())
            {
                return false;
            }

            CCSpriteBatchNode spriteManager = CCSpriteBatchNode.batchNodeWithFile("Images/sprites", 10);
            addChild(spriteManager, -1, (int)tags.kSpriteManager);

            CCSprite background = CCSprite.spriteWithTexture(spriteManager.Texture, new CCRect(0, 0, 320, 480));
            spriteManager.addChild(background);
            Scale(background);
            background.position = new CCPoint(240, 400);

            this.initClouds();

            this.schedule(step);

            return true;
        }

        private void initClouds()
        {
            currentCloudTag = (int)tags.kCloudsStartTag;
            while (currentCloudTag < (int)tags.kCloudsStartTag + kNumClouds)
            {
                this.initCloud();
                currentCloudTag++;
            }

            this.resetClouds();
        }

        private void initCloud()
        {
            CCRect rect = new CCRect();
            switch (random.Next() % 3)
            {
                case 0: rect = new CCRect(336, 16, 256, 108); break;
                case 1: rect = new CCRect(336, 128, 257, 110); break;
                case 2: rect = new CCRect(336, 240, 252, 119); break;
            }

            CCSpriteBatchNode spriteManager = (CCSpriteBatchNode)getChildByTag((int)tags.kSpriteManager);
            CCSprite cloud = CCSprite.spriteWithTexture(spriteManager.Texture, rect);
            Scale(cloud);
            spriteManager.addChild(cloud, 3, currentCloudTag);
        }

        public void resetClouds()
        {
            currentCloudTag = (int)tags.kCloudsStartTag;

            while (currentCloudTag < (int)tags.kCloudsStartTag + kNumClouds)
            {
                this.resetCloud();

                CCSpriteBatchNode spriteManager = (CCSpriteBatchNode)getChildByTag((int)tags.kSpriteManager);
                CCSprite cloud = (CCSprite)spriteManager.getChildByTag(currentCloudTag);
                CCPoint pos = cloud.position;
                pos.y -= 800f;
                cloud.position = pos;
                currentCloudTag++;
            }
        }

        public void resetCloud()
        {
            CCSpriteBatchNode spriteManager = (CCSpriteBatchNode)getChildByTag((int)tags.kSpriteManager);
            CCSprite cloud = (CCSprite)spriteManager.getChildByTag(currentCloudTag);

            float distance = random.Next() % 20 + 5;

            float scale = 5f / distance;
            cloud.scaleY = scale;
            cloud.scaleX = scale;
            if (random.Next() % 2 == 1)
                cloud.scaleX = -cloud.scaleX;

            CCSize size = cloud.contentSize;
            float scaled_width = size.width * scale;
            float x = random.Next() % (480 + (int)scaled_width) - scaled_width / 2;
            float y = random.Next() % (800 - (int)scaled_width) + scaled_width / 2 + 800;

            cloud.position = new CCPoint(x, y);
        }

        public void step(float dt)
        {
            CCSpriteBatchNode spriteManager = (CCSpriteBatchNode)getChildByTag((int)tags.kSpriteManager);

            int t = (int)tags.kCloudsStartTag;
            for (int i = t; i < (int)tags.kCloudsStartTag + kNumClouds; i++)
            {
                CCSprite cloud = (CCSprite)spriteManager.getChildByTag(i);
                CCPoint pos = cloud.position;
                CCSize size = cloud.contentSize;
                pos.x += 0.1f * cloud.scaleY;
                if (pos.x > 480 + size.width / 2)
                    pos.x = -size.width / 2;
                cloud.position = pos;
            }
        }

        public static void Scale(CCNode sprite)
        {
            sprite.scaleY = sy;
            sprite.scaleX = sx;
        }

    }
}

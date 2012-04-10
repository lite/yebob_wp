using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using cocos2d;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace YebobDemo
{
    public class AppDelegate : CCApplication
    {
        public AppDelegate(Game game, GraphicsDeviceManager graphics)
            : base(game, graphics)
        {
            CCApplication.sm_pSharedApplication = this;
        }

        /// <summary>
        /// Implement for initialize OpenGL instance, set source path, etc...
        /// </summary>
        public override bool initInstance()
        {
            return base.initInstance();
        }

        /// <summary>
        ///  Implement CCDirector and CCScene init code here.
        /// </summary>
        /// <returns>
        ///  true  Initialize success, app continue.
        ///  false Initialize failed, app terminate.
        /// </returns>
        public override bool applicationDidFinishLaunching()
        {
            //initialize director
            CCDirector pDirector = CCDirector.sharedDirector();
            pDirector.setOpenGLView();

            //turn on display FPS
            pDirector.DisplayFPS = true;

            pDirector.deviceOrientation = ccDeviceOrientation.CCDeviceOrientationPortrait;

            // set FPS. the default value is 1.0/60 if you don't call this
            pDirector.animationInterval = 1.0 / 60;

            // create a scene. it's an autorelease object
            CCScene pScene = CCScene.node();
            pScene.addChild(YebobDemo.Classes.MainMenu.node());
            //run
            pDirector.runWithScene(pScene);

            return true;
        }

        /// <summary>
        /// The function be called when the application enter background
        /// </summary>
        public override void applicationDidEnterBackground()
        {
            CCDirector.sharedDirector().pause();

            // if you use SimpleAudioEngine, it must be pause
            // SimpleAudioEngine::sharedEngine()->pauseBackgroundMusic();
        }

        /// <summary>
        /// The function be called when the application enter foreground  
        /// </summary>
        public override void applicationWillEnterForeground()
        {
            CCDirector.sharedDirector().resume();

            // if you use SimpleAudioEngine, it must resume here
            // SimpleAudioEngine::sharedEngine()->resumeBackgroundMusic();
        }
    }
}

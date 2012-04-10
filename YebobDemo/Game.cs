using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using Microsoft.Xna.Framework.Media;
using cocos2d;

namespace YebobDemo
{
    public class GameMain : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;

        public GameMain()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            TargetElapsedTime = TimeSpan.FromTicks(333333);

            InactiveSleepTime = TimeSpan.FromSeconds(1);

            //// Create XLive FormManager
            //manager = new XLiveFormManager(this, APISecretKey);
            //manager.OpenSession();

            // Add XLive FormManager in Components
            //Components.Add(manager);

            CCApplication application = new AppDelegate(this, graphics);
            this.Components.Add(application);
        }

        protected override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            Texture2D background = this.Content.Load<Texture2D>(@"images\OpenXLive");
            //manager.Background = background;
            //manager.UIExitingEvent += new EventHandler(manager_UIExitingEvent);
            base.LoadContent();
        }

        void manager_UIExitingEvent(object sender, EventArgs e)
        {
            CCDirector.sharedDirector().runningScene.visible = true;
            CCDirector.sharedDirector().resume();
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
            {
                CCDirector.sharedDirector().pause();
                //MessageBox.Show("Exit ?", "OpenXLive", MessageBoxButtons.OKCancel, Exit);
            }

            base.Update(gameTime);
        }

        //void Exit(DialogResult result)
        //{
        //    if (result == DialogResult.OK)
        //    {
        //        this.Exit();
        //    }
        //    else
        //    {
        //        CCDirector.sharedDirector().resume();
        //    }
        //}
    }
}

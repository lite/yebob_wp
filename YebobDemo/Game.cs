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
using Yebob;

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

            CCApplication application = new AppDelegate(this, graphics);
            this.Components.Add(application);
        }

        protected override void Initialize()
        {
            base.Initialize();
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
            {
                CCDirector.sharedDirector().pause();
                int? ret = MessageBox.Show(
                    "Exit ?", "Do you want to exit?",
                    new string[] { "OK", "Cancel" }, 0, MessageBoxIcon.None, MessageBoxEnd);
                
            }

            base.Update(gameTime);
        }

        void MessageBoxEnd(IAsyncResult result)
        {
            int? dialogResult = Guide.EndShowMessageBox(result);
            if (dialogResult == null)
                dialogResult = -1;

            if (dialogResult == 0)
            {
                this.Exit();
            }
            else
            {
                CCDirector.sharedDirector().resume();
            }
        }
   }
}

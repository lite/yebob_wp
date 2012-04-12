using System;
using System.Windows;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace YebobDemo
{
    public partial class GamePage : PhoneApplicationPage
    {
        ContentManager contentManager;
        GameTimer timer;
        SpriteBatch spriteBatch;

        Texture2D texture;
        Vector2 spritePosition;
        Vector2 spriteSpeed = new Vector2(100.0f, 100.0f);

        // A variety of rectangle colors
        Texture2D redTexture;
        Texture2D greenTexture;
        Texture2D blueTexture;

        // For rendering the XAML onto a texture
        UIElementRenderer elementRenderer;

        public GamePage()
        {
            InitializeComponent();

            // Get the content manager from the application
            contentManager = (Application.Current as App).Content;

            // Create a timer for this page
            timer = new GameTimer();
            //timer.UpdateInterval = TimeSpan.FromTicks(333333);

            // Using TimeSpan.Zero causes the update to happen 
            // at the actual framerate of the device. This makes 
            // animation much smoother. However, this will cause 
            // the speed of the app to vary from device to device 
            // where a fixed UpdateInterval will not.
            timer.UpdateInterval = TimeSpan.Zero;
            timer.Update += OnUpdate;
            timer.Draw += OnDraw;

            // Use the LayoutUpdate event to know when the page layout 
            // has completed so we can create the UIElementRenderer
            LayoutUpdated += new EventHandler(GamePage_LayoutUpdated);
        }


        void GamePage_LayoutUpdated(object sender, EventArgs e)
        {
            // Create the UIElementRenderer to draw the XAML page to a texture.

            // Check for 0 because when we navigate away the LayoutUpdate event
            // is raised but ActualWidth and ActualHeight will be 0 in that case.
            if (ActualWidth > 0 && ActualHeight > 0 && elementRenderer == null)
            {
                elementRenderer = new UIElementRenderer(this, (int)ActualWidth, (int)ActualHeight);
            }
        }
        

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            // Set the sharing mode of the graphics device to turn on XNA rendering
            SharedGraphicsDeviceManager.Current.GraphicsDevice.SetSharingMode(true);

            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(SharedGraphicsDeviceManager.Current.GraphicsDevice);

            // If texture is null, we've never loaded our content.
            if (null == texture)
            {
                redTexture = contentManager.Load<Texture2D>("redRect");
                greenTexture = contentManager.Load<Texture2D>("greenRect");
                blueTexture = contentManager.Load<Texture2D>("blueRect");

                // Start with the red rectangle
                texture = redTexture;
            }

            spritePosition.X = 0;
            spritePosition.Y = 0;

            // Start the timer
            timer.Start();

            base.OnNavigatedTo(e);
        }


        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            // Stop the timer
            timer.Stop();

            // Set the sharing mode of the graphics device to turn off XNA rendering
            SharedGraphicsDeviceManager.Current.GraphicsDevice.SetSharingMode(false);

            base.OnNavigatedFrom(e);
        }


        /// <summary>
        /// Allows the page to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        private void OnUpdate(object sender, GameTimerEventArgs e)
        {
            // Move the sprite around.
            UpdateSprite(e);
        }

        /// <summary>
        /// Moves the rectangle around the screen.
        /// </summary>
        /// <param name="e"></param>
        void UpdateSprite(GameTimerEventArgs e)
        {
            // Move the sprite by speed, scaled by elapsed time.
            spritePosition += spriteSpeed * (float)e.ElapsedTime.TotalSeconds;

            int MinX = 0;
            int MinY = 0;
            int MaxX = SharedGraphicsDeviceManager.Current.GraphicsDevice.Viewport.Width - texture.Width;
            int MaxY = SharedGraphicsDeviceManager.Current.GraphicsDevice.Viewport.Height - texture.Height;

            // Check for bounce.
            if (spritePosition.X > MaxX)
            {
                spriteSpeed.X *= -1;
                spritePosition.X = MaxX;
            }

            else if (spritePosition.X < MinX)
            {
                spriteSpeed.X *= -1;
                spritePosition.X = MinX;
            }

            if (spritePosition.Y > MaxY)
            {
                spriteSpeed.Y *= -1;
                spritePosition.Y = MaxY;
            }
            else if (spritePosition.Y < MinY)
            {
                spriteSpeed.Y *= -1;
                spritePosition.Y = MinY;
            }

        }


        /// <summary>
        /// Allows the page to draw itself.
        /// </summary>
        private void OnDraw(object sender, GameTimerEventArgs e)
        {
            SharedGraphicsDeviceManager.Current.GraphicsDevice.Clear(Color.Black);

            // Render the Silverlight controls using the UIElementRenderer
            elementRenderer.Render();

            // Draw the sprite
            spriteBatch.Begin();

            // Draw the rectangle in its new position
            spriteBatch.Draw(texture, spritePosition, Color.White);

            // Using the texture from the UIElementRenderer, 
            // draw the Silverlight controls to the screen
            spriteBatch.Draw(elementRenderer.Texture, Vector2.Zero, Color.White);

            spriteBatch.End();
        }


        /// <summary>
        /// Toggle the visibility of the StackPanel named "ColorPanel"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ColorPanelToggleButton_Click(object sender, RoutedEventArgs e)
        {
            if (System.Windows.Visibility.Visible == ColorPanel.Visibility)
            {
                ColorPanel.Visibility = System.Windows.Visibility.Collapsed;
            }
            else
            {
                ColorPanel.Visibility = System.Windows.Visibility.Visible;
            }
        }


        /// <summary>
        /// Switches to the red rectangle
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void redButton_Click(object sender, RoutedEventArgs e)
        {
            texture = redTexture;
        }


        /// <summary>
        /// Switches to the green rectangle
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void greenButton_Click(object sender, RoutedEventArgs e)
        {
            texture = greenTexture;
        }


        /// <summary>
        /// Switches to the blue rectangle 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void blueButton_Click(object sender, RoutedEventArgs e)
        {
            texture = blueTexture;
        }
    }
}

using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using Microsoft.Phone.Controls;
using System.Windows.Media;
using System.Windows.Controls.Primitives;

namespace YebobDemo
{
    public partial class GameYebobPage : PhoneApplicationPage
    {
        BitmapImage imageHomeDefault = new BitmapImage(new Uri("res/yb_shortcut_home_default.png", UriKind.Relative));
        BitmapImage imageHomeActive = new BitmapImage(new Uri("res/yb_shortcut_home_active.png", UriKind.Relative));
        BitmapImage imageFriendsDefault = new BitmapImage(new Uri("res/yb_shortcut_friends_default.png", UriKind.Relative));
        BitmapImage imageFriendsActive = new BitmapImage(new Uri("res/yb_shortcut_friends_active.png", UriKind.Relative));
        BitmapImage imageMarketDefault = new BitmapImage(new Uri("res/yb_shortcut_market_default.png", UriKind.Relative));
        BitmapImage imageMarketActive = new BitmapImage(new Uri("res/yb_shortcut_market_active.png", UriKind.Relative));

        ImageBrush imageHighlight = new ImageBrush() { ImageSource = new BitmapImage(new Uri("res/yb_shortcut_highlight.png", UriKind.Relative)) };
           
        public GameYebobPage()
        {
            InitializeComponent();
            
            uncheckButton(buttonHome, buttonHomeImage, imageHomeDefault);
            uncheckButton(buttonFriends, buttonFriendsImage, imageFriendsDefault);
            uncheckButton(buttonMarket, buttonMarketImage, imageMarketDefault);
            
        }

        private void checkButton(ToggleButton button, Image buttonImage, ImageSource imageActive)
        {
            button.IsChecked = true;
            buttonImage.Source = imageActive;
            button.Background = imageHighlight;
        }

        private void uncheckButton(ToggleButton button, Image buttonImage, ImageSource imageDefault)
        {
            button.IsChecked = false;
            buttonImage.Source = imageDefault;
            button.Background = null;
        }

        private void onHome(object sender, RoutedEventArgs e)
        {
            checkButton(buttonHome, buttonHomeImage, imageHomeActive);
            uncheckButton(buttonFriends, buttonFriendsImage, imageFriendsDefault);
            uncheckButton(buttonMarket, buttonMarketImage, imageMarketDefault);

            yebobView.StartPageUri = new Uri("http://www.yebob.com", UriKind.Absolute);
        }

        private void onFriends(object sender, RoutedEventArgs e)
        {
            uncheckButton(buttonHome, buttonHomeImage, imageHomeDefault);
            checkButton(buttonFriends, buttonFriendsImage, imageFriendsActive);
            uncheckButton(buttonMarket, buttonMarketImage, imageMarketDefault);

            yebobView.StartPageUri = new Uri("http://sns.yebob.com", UriKind.Absolute);
        }

        private void onMarket(object sender, RoutedEventArgs e)
        {
            uncheckButton(buttonHome, buttonHomeImage, imageHomeDefault);
            uncheckButton(buttonFriends, buttonFriendsImage, imageFriendsDefault);
            checkButton(buttonMarket, buttonMarketImage, imageMarketActive);

            yebobView.StartPageUri = new Uri("http://open.yebob.com", UriKind.Absolute);
        }
    }
}
using System;
using System.Windows;
using Microsoft.Phone.Controls;

namespace YebobDemo
{
    public partial class SDKDemoPage : PhoneApplicationPage
    {
        // Constructor
        public SDKDemoPage()
        {
            InitializeComponent();
        }

        private void onPlay(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/GamePage.xaml", UriKind.Relative));
        }

        private void onFriends(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/FriendsPage.xaml", UriKind.Relative));
        }
    }
}

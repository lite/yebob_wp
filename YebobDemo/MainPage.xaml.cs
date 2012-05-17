using System;
using System.Windows;
using Microsoft.Phone.Controls;

namespace YebobDemo
{
    public partial class MainPage : PhoneApplicationPage
    {
        // Constructor
        public MainPage()
        {
            InitializeComponent();
        }

        private void onPlay(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/GamePage.xaml", UriKind.Relative));
        }

        private void onYebob(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/GameYebobPage.xaml", UriKind.Relative));
        }

        private void onFriends(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/FriendsPage.xaml", UriKind.Relative));
        }

        private void onSDKDemo(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/SDKDemoPage.xaml", UriKind.Relative));
        }
    }
}

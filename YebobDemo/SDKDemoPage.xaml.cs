using System;
using System.Windows;
using System.Collections.Generic;
using System.Text;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.GamerServices;

namespace YebobDemo
{
    public partial class SDKDemoPage : PhoneApplicationPage
    {
        // Constructor
        public SDKDemoPage()
        {
            InitializeComponent();
        }

        private void onLogin(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/GamePage.xaml", UriKind.Relative));
        }

        private void onMessageBox(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("YebobDemo", "MessageBox clicked.");
        }

        private void onInstall(object sender, RoutedEventArgs e)
        {
            MarketplaceDetailTask marketplaceDetailTask = new MarketplaceDetailTask();

            marketplaceDetailTask.ContentIdentifier = "c14e93aa-27d7-df11-a844-00237de2db9e";
            marketplaceDetailTask.ContentType = MarketplaceContentType.Applications;

            marketplaceDetailTask.Show();

        }

        
    }
}

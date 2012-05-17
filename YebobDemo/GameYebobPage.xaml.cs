using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using Microsoft.Phone.Controls;

namespace YebobDemo
{
    public partial class GameYebobPage : PhoneApplicationPage
    {
        public GameYebobPage()
        {
            InitializeComponent();
        }

        private void onHome(object sender, RoutedEventArgs e)
        {
            button2.IsChecked = false;
            button3.IsChecked = false;
            yebobView.StartPageUri = new Uri("http://alpha.yebob.com", UriKind.Absolute);
        }
    }
}
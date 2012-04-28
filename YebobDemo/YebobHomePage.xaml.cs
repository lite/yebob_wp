using System;
using System.Windows;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using Microsoft.Phone.Controls;

namespace YebobDemo
{
    public partial class YebobHomePage : PhoneApplicationPage
    {
        public YebobHomePage()
        {
            InitializeComponent();

            mainTab.SelectionChanged += new SelectionChangedEventHandler(onSelectionChanged);

            addTabItem("res/home.png");
            addTabItem("res/rankings.png");
            addTabItem("res/baton.png");
        }

        void onSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            WebBrowser webBrowser = ((TabItem)mainTab.SelectedItem).Content as WebBrowser;
            webBrowser.Navigate(new Uri("http://alpha.yebob.com"));
        }

        private void addTabItem(string resUri)
        {
            TabItem item = new TabItem();
            Image img = new Image();
            img.Source = new BitmapImage(new Uri(resUri, UriKind.Relative));
            img.Width = 80;
            item.Header = img;
            item.Content = new WebBrowser();
            mainTab.Items.Add(item);
        }

        //private void addPivotItem(string resUri)
        //{
        //    PivotItem pivotItem = new PivotItem();
        //    Image img = new Image();
        //    img.Source = new BitmapImage(new Uri(resUri, UriKind.Relative));
        //    pivotItem.Header = img;
        //    Grid sta = new Grid();
        //    sta.Children.Add(new WebBrowser());
        //    pivotItem.Content = sta;
        //    mainPivot.Items.Add(pivotItem);          
        //}
    }
}
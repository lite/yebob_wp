using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;

namespace Yebob
{
    public class OrientationHelper
    {
        protected WebBrowser yebobBrowser;
        protected PhoneApplicationPage page;
       
        public OrientationHelper(WebBrowser yebobBrowser, PhoneApplicationPage gapPage)
        {
            this.yebobBrowser = yebobBrowser;
            page = gapPage;

            page.OrientationChanged += new EventHandler<OrientationChangedEventArgs>(page_OrientationChanged);
            yebobBrowser.LoadCompleted += new System.Windows.Navigation.LoadCompletedEventHandler(yebobBrowser_LoadCompleted);


        }

        void yebobBrowser_LoadCompleted(object sender, System.Windows.Navigation.NavigationEventArgs e)
        {
            int i = 0;

            switch (this.page.Orientation)
            {
                case PageOrientation.Portrait: // intentional fall through
                case PageOrientation.PortraitUp:
                    i = 0;
                    break;
                case PageOrientation.PortraitDown:
                    i = 180;
                    break;
                case PageOrientation.Landscape: // intentional fall through
                case PageOrientation.LandscapeLeft:
                    i = -90;
                    break;
                case PageOrientation.LandscapeRight:
                    i = 90;
                    break;
            }
            // Cordova.fireEvent('orientationchange', window);
            string jsCallback = String.Format("window.orientation = {0};", i);

            try
            {
                yebobBrowser.InvokeScript("execScript", jsCallback);
            }
            catch (Exception)
            {
            }
        }

        void page_OrientationChanged(object sender, OrientationChangedEventArgs e)
        {
            int i = 0;

            switch (e.Orientation)
            {
                case PageOrientation.Portrait: // intentional fall through
                case PageOrientation.PortraitUp:
                    i = 0;
                    break;
                case PageOrientation.PortraitDown:
                    i = 180;
                    break;
                case PageOrientation.Landscape: // intentional fall through
                case PageOrientation.LandscapeLeft:
                    i = -90;
                    break;
                case PageOrientation.LandscapeRight:
                    i = 90;
                    break;
            }
            // Cordova.fireEvent('orientationchange', window);
            string jsCallback = String.Format("window.orientation = {0};", i);

            try
            {

                yebobBrowser.InvokeScript("execScript", jsCallback);

                jsCallback = "var evt = document.createEvent('HTMLEvents');";
                jsCallback += "evt.initEvent( 'orientationchange', true, false );";
                jsCallback += "window.dispatchEvent(evt);";
                jsCallback += "if(window.onorientationchange){window.onorientationchange(evt);}";

                yebobBrowser.InvokeScript("execScript", jsCallback);
            }
            catch (Exception)
            {
            }
        }

        public void HandleCommand(string commandStr)
        {

        }
    }
}

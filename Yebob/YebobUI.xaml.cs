using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using System.IO.IsolatedStorage;
using System.Windows.Resources;
using System.Windows.Interop;
using System.IO;
using System.ComponentModel;
using System.Diagnostics;
using System.Text;
using System.Threading;
using Microsoft.Phone.Shell;

namespace Yebob
{
    public partial class YebobUI : UserControl
    {
        private bool IsBrowserInitialized = false;
        private bool OverrideBackButton = false;

        private static string AppRoot = "/app/";


        /// <summary>
        /// Handles native api calls
        /// </summary>
        private NativeExecution nativeExecution;
        protected OrientationHelper orientationHelper;

        public System.Windows.Controls.Grid _LayoutRoot
        {
            get
            {
                return ((System.Windows.Controls.Grid)(this.FindName("LayoutRoot")));
            }
        }

        public WebBrowser Browser
        {
            get
            {
                return YebobBrowser;
            }
        }

        /*
         * Setting StartPageUri only has an effect if called before the view is loaded.
         **/
        protected Uri _startPageUri = null;
        public Uri StartPageUri
        {
            get
            {
                if (_startPageUri == null)
                {
                    // default
                    return new Uri(AppRoot + "www/index.html", UriKind.Relative);
                }
                else
                {
                    return _startPageUri;
                }
            }
            set
            {
                if (!this.IsBrowserInitialized)
                {
                    _startPageUri = value;
                }
            }
        }

        public YebobUI()
        {

            InitializeComponent();

            if (DesignerProperties.IsInDesignTool)
            {
                return;
            }


            StartupMode mode = PhoneApplicationService.Current.StartupMode;

            if (mode == StartupMode.Launch)
            {
                PhoneApplicationService service = PhoneApplicationService.Current;
                service.Activated += new EventHandler<Microsoft.Phone.Shell.ActivatedEventArgs>(AppActivated);
                service.Launching += new EventHandler<LaunchingEventArgs>(AppLaunching);
                service.Deactivated += new EventHandler<DeactivatedEventArgs>(AppDeactivated);
                service.Closing += new EventHandler<ClosingEventArgs>(AppClosing);
            }
            else
            {

            }

            // initializes native execution logic
            this.nativeExecution = new NativeExecution(ref this.YebobBrowser);
        }

        void AppClosing(object sender, ClosingEventArgs e)
        {
            Debug.WriteLine("AppClosing");
        }

        void AppDeactivated(object sender, DeactivatedEventArgs e)
        {
            Debug.WriteLine("AppDeactivated");

            try
            {
                YebobBrowser.InvokeScript("CordovaCommandResult", new string[] { "pause" });
            }
            catch (Exception)
            {
                Debug.WriteLine("Pause event error");
            }
        }

        void AppLaunching(object sender, LaunchingEventArgs e)
        {
            Debug.WriteLine("AppLaunching");
        }

        void AppActivated(object sender, Microsoft.Phone.Shell.ActivatedEventArgs e)
        {
            Debug.WriteLine("AppActivated");
            try
            {
                YebobBrowser.InvokeScript("CordovaCommandResult", new string[] { "resume" });
            }
            catch (Exception)
            {
                Debug.WriteLine("Resume event error");
            }
        }

        void YebobBrowser_Loaded(object sender, RoutedEventArgs e)
        {
            if (DesignerProperties.IsInDesignTool)
            {
                return;
            }

            // prevents refreshing web control to initial state during pages transitions
            if (this.IsBrowserInitialized) return;
            try
            {
                YebobBrowser.Navigate(StartPageUri);
                IsBrowserInitialized = true;
                AttachHardwareButtonHandlers();
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Exception in YebobBrowser_Loaded :: {0}", ex.Message);
            }
        }

        void AttachHardwareButtonHandlers()
        {
            PhoneApplicationFrame frame = Application.Current.RootVisual as PhoneApplicationFrame;
            if (frame != null)
            {
                PhoneApplicationPage page = frame.Content as PhoneApplicationPage;

                if (page != null)
                {
                    page.BackKeyPress += new EventHandler<CancelEventArgs>(page_BackKeyPress);

                    this.orientationHelper = new OrientationHelper(this.YebobBrowser, page);

                }
            }
        }

        void page_BackKeyPress(object sender, CancelEventArgs e)
        {
            if (OverrideBackButton)
            {
                try
                {
                    YebobBrowser.InvokeScript("CordovaCommandResult", new string[] { "backbutton" });
                    e.Cancel = true;
                }
                catch (Exception)
                {

                }
            }
        }

        void YebobBrowser_LoadCompleted(object sender, System.Windows.Navigation.NavigationEventArgs e)
        {
            this.YebobBrowser.Opacity = 1;
        }


        void YebobBrowser_Navigating(object sender, NavigatingEventArgs e)
        {
            Debug.WriteLine("YebobBrowser_Navigating to :: " + e.Uri.ToString());
            // TODO: tell any running plugins to stop doing what they are doing.
            // TODO: check whitelist / blacklist
            // NOTE: Navigation can be cancelled by setting :        e.Cancel = true;
        }

        void YebobBrowser_ScriptNotify(object sender, NotifyEventArgs e)
        {
            string commandStr = e.Value;

            if (commandStr.IndexOf("Orientation") == 0)
            {
                this.orientationHelper.HandleCommand(commandStr);
                return;
            }

            YebobCommandCall commandCallParams = YebobCommandCall.Parse(commandStr);

            if (commandCallParams == null)
            {
                // ERROR
                Debug.WriteLine("ScriptNotify :: " + commandStr);
            }
            else
            {
                this.nativeExecution.ProcessCommand(commandCallParams);
            }
        }

        private void YebobBrowser_Unloaded(object sender, RoutedEventArgs e)
        {

        }

        private void YebobBrowser_NavigationFailed(object sender, System.Windows.Navigation.NavigationFailedEventArgs e)
        {
            Debug.WriteLine("YebobBrowser_NavigationFailed :: " + e.Uri.ToString());
        }

        private void YebobBrowser_Navigated(object sender, System.Windows.Navigation.NavigationEventArgs e)
        {
            Debug.WriteLine("YebobBrowser_Navigated :: " + e.Uri.ToString());
        }


    }
}

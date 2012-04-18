using System;
using System.Net;
using System.Windows;
using System.Collections.Generic;
using System.Text;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Tasks;
using Microsoft.Phone.Reactive;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.GamerServices;

using YebobDemo.Json;
using System.IO;

namespace YebobDemo
{
    public partial class SDKDemoPage : PhoneApplicationPage
    {
        private string APP_SECRET = "1dhjid1n-69og-ek25-jngk-7kraqc3tpmgw";
        private string APP_ID = "sbqf0xpt-d0c9-rsaz-1ujz-vvgljx9qgfsw";
        private string URL_PREFIX = "http://api.yebob.com";
	
        // sina_weibo
        private string SOCIAL_APPCODE = "5a1702c984a84578ada3a56e95c42a5a";
        private string SOCIAL_APPID = "sina_weibo";

        private String token;
        // Constructor
        public SDKDemoPage()
        {
            InitializeComponent();
        }

        private void onLogin(object sender, RoutedEventArgs e)
        {
            respText.Text = "Login clicked";
        }

        public delegate void YebobHandler(JsonValue json);

        private void onAccessToken(object sender, RoutedEventArgs e)
        {
            accessToken(APP_ID, APP_SECRET, new YebobHandler(postAccessToken));
        }

        private void accessToken(String app_id, String secret, YebobHandler handler) {
            String url = String.Format("{0}/access_token?app_id={1}&secret={2}", URL_PREFIX, app_id, secret);
            
            get(url, handler);
	    }

        private void get(String url, YebobHandler handler)
        {
            HttpWebRequest request = WebRequest.CreateHttp(url);
            request.AllowReadStreamBuffering = true;
            var ob = Observable.FromAsyncPattern<WebResponse>(request.BeginGetResponse, request.EndGetResponse);

            Log.d("YebobApiDemo send " + DateTime.Now);
            Observable.Timeout(ob.Invoke(), DateTimeOffset.Now.AddSeconds(10))
                .ObserveOnDispatcher() //include if UI accessed from subscribe
                .Subscribe(response => {
                    Log.d("YebobApiDemo recv " + DateTime.Now);
                    using (var responseStream = response.GetResponseStream())
                    {
                        using (var sr = new StreamReader(responseStream, Encoding.UTF8))
                        {
                            String result = sr.ReadToEnd();
                            handleResponse(result, handler);
                        }
                    }
                }, ex => {
                    Log.e("YebobApiDemo error " + ex.Message);
                    request.Abort(); 
                });
        }

        private void handleResponse(String response, YebobHandler handler)
        {
            if (response == null)
            {
                respText.Text = "sorry, I can't get the data from server.";
                return;
            }
            try
            {
                JsonValue json = JsonValue.Parse(response);
                if (json.ContainsName("ret"))
                {
                    respText.Text = json.GetValue<String>("msg");
                }
                else
                {
                    if (handler != null)
                    {
                        handler(json);
                    }
                }
            }
            catch (Exception e)
            {
                respText.Text = response;
            }
        }
        private void postAccessToken(JsonValue json)
        {
		    respText.Text  = json.GetValue<String>("id");
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

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

        public delegate void YebobHandler(JsonValue json);

        private String token = null;
        private String session = null;
        private String rankingListId = null;
        // Constructor
        public SDKDemoPage()
        {
            InitializeComponent();
        }

        // access_token
        private void onAccessToken(object sender, RoutedEventArgs e)
        {
            accessToken(APP_ID, APP_SECRET, new YebobHandler(postAccessToken));
        }

        private void accessToken(String app_id, String secret, YebobHandler handler)
        {
            String url = String.Format("{0}/access_token?app_id={1}&secret={2}", URL_PREFIX, app_id, secret);

            get(url, handler);
        }

        private void postAccessToken(JsonValue json)
        {
            String token = json.GetValue<String>("id");
            setMessageText("token");
        }

        // login
        private void onLogin(object sender, RoutedEventArgs e)
        {
            login(token, SOCIAL_APPID, SOCIAL_APPCODE, "123",
                "http://alpha.yebob.com/TestYebobListRest/userlogin",
                new YebobHandler(postLogin));
        }

        private void login(String token, String community, String code, String state, String redirect_url, YebobHandler handler)
        {
            String url = String.Format("{0}/login?community={1}&code={2}&state={3}&redirect_url={4}",
                    URL_PREFIX, community, code, state, redirect_url);
            getWithToken(token, url, handler);
        }

        private void postLogin(JsonValue json)
        {
            JsonObject obj = json.GetValue<JsonObject>("session");
            session = obj.GetValue<String>("id");
            setMessageText(session);
        }

        // logout
        private void onLogout(object sender, RoutedEventArgs e)
        {
            logout("token", "session", new YebobHandler(postLogout));
        }

        public void logout(String token, String session, YebobHandler handler)
        {
            String url = String.Format("{0}/logout", URL_PREFIX);

            getWithTokenSession(token, session, url, handler);
        }

        private void postLogout(JsonValue json)
        {
            setMessageText("done");
        }

        // me
        private void onMe(object sender, RoutedEventArgs e)
        {
            me(token, session, new YebobHandler(postMe)); 
        }
		
	    public void me(String token, String session, YebobHandler handler) {
		    String url = String.Format("{0}/me", URL_PREFIX);

		    getWithTokenSession(token, session, url, handler);
	    }
	
	    private void postMe(JsonValue json) {
            String msg = json.GetValue<String>("community");
            setMessageText(msg);
	    }

        // share
        private void onShare(object sender, RoutedEventArgs e)
        {
            share(token, session, "text", null);
        }

	    public void share(String token, String session, String text, YebobHandler handler) {
		    String url = String.Format("{0}/share?text={1}", URL_PREFIX, text);

		    getWithTokenSession(token, session, url, handler);
	    }
	
        // score submit
        private void onScoreSubmit(object sender, RoutedEventArgs e)
        {
            scoreSubmit(token, session, rankingListId, 100, null);
		}
		
	    public void scoreSubmit(String token, String session, String list_id, long score, YebobHandler handler) {
		    String url = String.Format("{0}/score/submit?list_id={1}&score={2}", 
				    URL_PREFIX, list_id, score);

            getWithTokenSession(token, session, url, handler);
	    }

        // ranking lists
        private void onRankingLists(object sender, RoutedEventArgs e)
        {
            rankingLists(token, new YebobHandler(postRankingLists));
        }

	    public void rankingLists(String token, YebobHandler handler) {
		    String url = String.Format("{0}/ranking/lists", URL_PREFIX);

		    getWithToken(token, url, handler);
	    }

	    private void postRankingLists(JsonValue json) 
        {
            JsonArray lists = json.GetValue<JsonArray>("lists");
            JsonObject obj = lists.GetValue<JsonObject>(0);
		    rankingListId = obj.GetValue<String>("id");
            String msg = String.Format("id:{0}, name:{1}", rankingListId, obj.GetValue<String>("name"));
            setMessageText(msg);
	    }

        // ranking tops
        private void onRankingLists(object sender, RoutedEventArgs e)
        {
            int count = 30, start_row = 10;
		    rankingTops(token, rankingListId, count, start_row, "week", "friend", new YebobHandler(postRankingTops));
	    }

	    public void rankingTops(String token, String list_id, int count, int start_row, String time_type, String relation_type, YebobHandler handler) {
		    String url = String.Format("{0}/ranking/tops?list_id={1}&count={2}&start_row={3}&time_type={4}&relation_type={5}", 
				URL_PREFIX, list_id, count, start_row, time_type, relation_type);

		    getWithToken(token, url, handler);
	    }

        private void postRankingTops(JsonValue json) {
	        int total = json.GetValue<int>("total");
            JsonArray items = json.GetValue<JsonArray>("items");
	        JsonObject item = items.GetValue<JsonObject>(0);
	        String msg = String.Format("total:{0}, item[0]: {1}", total, item.GetValue<int>("score"));
            setMessageText(msg);
        }

        // status get
        private void onStatusGet(object sender, RoutedEventArgs e)
        {
            statusGet(token, new YebobHandler(postStatusGet));
	    }

	    public void statusGet(String token, YebobHandler handler) {
		    String url = String.Format("{0}/status/get",URL_PREFIX);
		
		    getWithToken(token, url, handler);
	    }

	    private void postStatusGet(JsonValue json)
        {
		    String msg = String.Format("%d", json.GetValue<int>("expires_in"));
            setMessageText(msg);
	    }

        // status exists
        private void onStatusExists(object sender, RoutedEventArgs e)
        {
            statusExists(token, null);
		}
		
	    public void statusExists(String token, YebobHandler handler) {
		    String url = String.Format("{0}/status/exists",URL_PREFIX);
		
		    getWithToken(token, url, handler);
	    }

        // ################
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

        private void getWithTokenSession(String token, String session, String url, YebobHandler handler)
        {
            HttpWebRequest request = WebRequest.CreateHttp(url);
            request.Headers["access_token"] = token;
            request.Headers["session"] = session;
            get(request, handler);
        }

        private void getWithToken(String token, String url, YebobHandler handler)
        {
            HttpWebRequest request = WebRequest.CreateHttp(url);
            request.Headers["access_token"] = token;
            get(request, handler);
        }

        private void get(String url, YebobHandler handler)
        {
            HttpWebRequest request = WebRequest.CreateHttp(url);
            get(request, handler);
        }

        private void get(HttpWebRequest request, YebobHandler handler)
        {
            request.AllowReadStreamBuffering = true;
            var ob = Observable.FromAsyncPattern<WebResponse>(request.BeginGetResponse, request.EndGetResponse);

            Log.d("YebobApiDemo send " + DateTime.Now);
            Observable.Timeout(ob.Invoke(), DateTimeOffset.Now.AddSeconds(10))
                .ObserveOnDispatcher() //include if UI accessed from subscribe
                .Subscribe(response =>
                {
                    Log.d("YebobApiDemo recv " + DateTime.Now);
                    using (var responseStream = response.GetResponseStream())
                    {
                        using (var sr = new StreamReader(responseStream, Encoding.UTF8))
                        {
                            String result = sr.ReadToEnd();
                            handleResponse(result, handler);
                        }
                    }
                }, ex =>
                {
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

        private void setMessageText(String msg)
        {
            respText.Text = msg;
        }
    }
}

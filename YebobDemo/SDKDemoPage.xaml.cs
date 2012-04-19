using System;
using System.Net;
using System.Windows;
using System.Collections.Generic;
using Microsoft.Phone.Controls;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.GamerServices;

using Yebob;
using Yebob.Json;

namespace YebobDemo
{
    public partial class SDKDemoPage : PhoneApplicationPage
    {
        private string APP_SECRET = "1dhjid1n-69og-ek25-jngk-7kraqc3tpmgw";
        private string APP_ID = "sbqf0xpt-d0c9-rsaz-1ujz-vvgljx9qgfsw";

        // sina_weibo
        private string SOCIAL_APPCODE = "5a1702c984a84578ada3a56e95c42a5a";
        private string SOCIAL_APPID = "sina_weibo";

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
            Api.accessToken(APP_ID, APP_SECRET,
                new YebobHandler(postAccessToken), new ErrorHandler(defultErrorHandler));
        }

        private void postAccessToken(JsonValue json)
        {
            String token = json.GetValue<String>("id");
            setMessageText("token");
        }

        // login
        private void onLogin(object sender, RoutedEventArgs e)
        {
            Api.login(token, SOCIAL_APPID, SOCIAL_APPCODE, "123",
                "http://alpha.yebob.com/TestYebobListRest/userlogin",
                new YebobHandler(postLogin), new ErrorHandler(defultErrorHandler));
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
            Api.logout("token", "session",
                new YebobHandler(postLogout), null);
        }

        private void postLogout(JsonValue json)
        {
            setMessageText("done");
        }

        // me
        private void onMe(object sender, RoutedEventArgs e)
        {
            Api.me(token, session, 
                new YebobHandler(postMe), new ErrorHandler(defultErrorHandler)); 
        }

	    private void postMe(JsonValue json) {
            String msg = json.GetValue<String>("community");
            setMessageText(msg);
	    }

        // share
        private void onShare(object sender, RoutedEventArgs e)
        {
            Api.share(token, session, "text",
                null, new ErrorHandler(defultErrorHandler));
        }

        // score submit
        private void onScoreSubmit(object sender, RoutedEventArgs e)
        {
            Api.scoreSubmit(token, session, rankingListId, 100,
                null, new ErrorHandler(defultErrorHandler));
		}

        // ranking lists
        private void onRankingLists(object sender, RoutedEventArgs e)
        {
            Api.rankingLists(token,
                new YebobHandler(postRankingLists), new ErrorHandler(defultErrorHandler));
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
        private void onRankingTops(object sender, RoutedEventArgs e)
        {
            int count = 30, start_row = 10;
		    Api.rankingTops(token, rankingListId, count, start_row, "week", "friend",
                new YebobHandler(postRankingTops), new ErrorHandler(defultErrorHandler));
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
            Api.statusGet(token,
                new YebobHandler(postStatusGet), new ErrorHandler(defultErrorHandler));
	    }

	    private void postStatusGet(JsonValue json)
        {
		    String msg = String.Format("%d", json.GetValue<int>("expires_in"));
            setMessageText(msg);
	    }

        // status exists
        private void onStatusExists(object sender, RoutedEventArgs e)
        {
            Api.statusExists(token, 
                null, new ErrorHandler(defultErrorHandler));
		}

        // ################
        private void onInstall(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("YebobDemo", "Install clicked.");
            Api.showDetail("c14e93aa-27d7-df11-a844-00237de2db9e");
        }

        private void setMessageText(String msg)
        {
            respText.Text = msg;
        }

        private void defultErrorHandler(ERROR code, string msg)
        {
            String errorText = String.Format("code:{0}, msg: {1}", code, msg);
            setMessageText(errorText);
        }
    }
}

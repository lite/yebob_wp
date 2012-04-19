using System;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;

using Microsoft.Devices;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Tasks;
using Microsoft.Phone.Reactive;

using Yebob.Json;

namespace Yebob
{
    public delegate void YebobHandler(JsonValue json);
    public delegate void ErrorHandler(ERROR code, string msg);

    public enum ERROR : int
    {
        NO_ERROR = 0,
        HTTP_HAS_EXCEPTION = -1,
        RESPONSE_IS_NONE = -2,
        RESPONSE_HAS_ERROR = -3,
        RESPONSE_HAS_EXCEPTION = -4,
    };

    public class Api
    {
        private static string URL_PREFIX = "http://api.yebob.com";

        public static void accessToken(String app_id, String secret, YebobHandler handler, ErrorHandler onError)
        {
            String url = String.Format("{0}/access_token?app_id={1}&secret={2}", URL_PREFIX, app_id, secret);

            get(url, handler, onError);
        }

        public static void login(String token, String community, String code, String state, String redirect_url, YebobHandler handler, ErrorHandler onError)
        {
            String url = String.Format("{0}/login?community={1}&code={2}&state={3}&redirect_url={4}",
                    URL_PREFIX, community, code, state, redirect_url);
            getWithToken(token, url, handler, onError);
        }

        public static void logout(String token, String session, YebobHandler handler, ErrorHandler onError)
        {
            String url = String.Format("{0}/logout", URL_PREFIX);

            getWithTokenSession(token, session, url, handler, onError);
        }

        public static void me(String token, String session, YebobHandler handler, ErrorHandler onError)
        {
            String url = String.Format("{0}/me", URL_PREFIX);

            getWithTokenSession(token, session, url, handler, onError);
        }

        public static void share(String token, String session, String text, YebobHandler handler, ErrorHandler onError)
        {
            String url = String.Format("{0}/share?text={1}", URL_PREFIX, text);

            getWithTokenSession(token, session, url, handler, onError);
        }

        public static void scoreSubmit(String token, String session, String list_id, long score, YebobHandler handler, ErrorHandler onError)
        {
            String url = String.Format("{0}/score/submit?list_id={1}&score={2}",
                    URL_PREFIX, list_id, score);

            getWithTokenSession(token, session, url, handler, onError);
        }

        public static void rankingLists(String token, YebobHandler handler, ErrorHandler onError)
        {
            String url = String.Format("{0}/ranking/lists", URL_PREFIX);

            getWithToken(token, url, handler, onError);
        }

        public static void rankingTops(String token, String list_id, int count, int start_row, String time_type, String relation_type, YebobHandler handler, ErrorHandler onError)
        {
            String url = String.Format("{0}/ranking/tops?list_id={1}&count={2}&start_row={3}&time_type={4}&relation_type={5}",
                URL_PREFIX, list_id, count, start_row, time_type, relation_type);

            getWithToken(token, url, handler, onError);
        }

        public static void statusGet(String token, YebobHandler handler, ErrorHandler onError)
        {
            String url = String.Format("{0}/status/get", URL_PREFIX);

            getWithToken(token, url, handler, onError);
        }

        public static void statusExists(String token, YebobHandler handler, ErrorHandler onError)
        {
            String url = String.Format("{0}/status/exists", URL_PREFIX);

            getWithToken(token, url, handler, onError);
        }

        public static void showDetail(String appId)
        {
            MarketplaceDetailTask marketplaceDetailTask = new MarketplaceDetailTask();

            marketplaceDetailTask.ContentIdentifier = appId;
            marketplaceDetailTask.ContentType = MarketplaceContentType.Applications;

            marketplaceDetailTask.Show();
        }

        private static void getWithTokenSession(String token, String session, String url, YebobHandler handler, ErrorHandler onError)
        {
            HttpWebRequest request = WebRequest.CreateHttp(url);
            request.Headers["access_token"] = token;
            request.Headers["session"] = session;
            get(request, handler, onError);
        }

        private static void getWithToken(String token, String url, YebobHandler handler, ErrorHandler onError)
        {
            HttpWebRequest request = WebRequest.CreateHttp(url);
            request.Headers["access_token"] = token;
            get(request, handler, onError);
        }

        private static void get(String url, YebobHandler handler, ErrorHandler onError)
        {
            HttpWebRequest request = WebRequest.CreateHttp(url);
            get(request, handler, onError);
        }

        private static void get(HttpWebRequest request, YebobHandler handler, ErrorHandler onError)
        {
            request.AllowReadStreamBuffering = true;
            var ob = Observable.FromAsyncPattern<WebResponse>(request.BeginGetResponse, request.EndGetResponse);

            if(onError == null){
                onError = new ErrorHandler(defaultErrorHandler);
            }

            Log.d("Yebob send " + DateTime.Now);
            Observable.Timeout(ob.Invoke(), DateTimeOffset.Now.AddSeconds(10))
                .ObserveOnDispatcher() //include if UI accessed from subscribe
                .Subscribe(response =>
                {
                    Log.d("Yebob recv " + DateTime.Now);
                    using (var responseStream = response.GetResponseStream())
                    {
                        using (var sr = new StreamReader(responseStream, Encoding.UTF8))
                        {
                            String result = sr.ReadToEnd();
                            handleResponse(result, handler, onError);
                        }
                    }
                }, ex =>
                {
                    Log.e("Yebob error " + ex.Message);
                    onError(ERROR.HTTP_HAS_EXCEPTION, "Yebob error " + ex.Message);
                    request.Abort();
                });
        }

        private static void defaultErrorHandler(ERROR code, string msg)
        {
            Log.e("code:{0}, msg: {1}", code, msg);
        }

        private static void handleResponse(String response, YebobHandler handler, ErrorHandler onError)
        {
            if (response == null)
            {
                String msg = "sorry, I can't get the data from server.";
                onError(ERROR.RESPONSE_IS_NONE, msg);
                return;
            }
            try
            {
                JsonValue json = JsonValue.Parse(response);
                if (json.ContainsName("ret"))
                {
                    String msg = json.GetValue<String>("msg");
                    onError(ERROR.RESPONSE_HAS_ERROR, msg);
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
                onError(ERROR.RESPONSE_HAS_EXCEPTION, response);
            }
        }
    }
}
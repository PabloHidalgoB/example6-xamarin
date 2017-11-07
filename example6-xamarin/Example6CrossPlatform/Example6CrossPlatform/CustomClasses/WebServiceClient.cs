using System;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

using Newtonsoft.Json.Linq;
using example6Xamarin;

namespace Example6CrossPlatform.CustomClasses
{
    public class WebServiceClient
    {
        private IRequestInterface requestInterface;
        private static int timeout = 10;
        public String result;
        private String sUrlParameters = "";
        private String sUrlBase;
        private String sUrl;
        private String sRequestMethod;
        public String TAG;

        private string uri = "";
        private string stringJson = "";
        private List<KeyValuePair<string, string>> parameters = new List<KeyValuePair<string, string>>();



        // reply
        public bool success = true;

        public WebServiceClient(IRequestInterface requestInterface, string tag)
        {
            this.requestInterface = requestInterface;
            this.TAG = tag;            

            sRequestMethod = "POST";

            sUrlBase = "http://www.floatrates.com/daily/ils.json";

            Debug.WriteLine(TAG);
        }

        public void getCurrency()
        {
            sRequestMethod = "GET";
            Debug.WriteLine(TAG, "=========> getCurrency");
            //String parameters = "{\"email\":\"" + user.trim() + "\", \"password\":\"" + password.trim() + "\"}";

            sUrl = "";
            sUrlParameters = null;
        }


        public void getCurrencyBase(String currencyType)
        {
            //Log.d(activity.TAG, "=========> getCurrencyBase");
            //String parameters = "{\"email\":\"" + user.trim() + "\", \"password\":\"" + password.trim() + "\"}";

            sUrl = "?base=" + currencyType;
            sUrlParameters = null;
        }

        public void sendToServer()
        {
            new Task(delegate
            {
                try
                {
                    success = false;
                    result = "";
                    string baseAddress = sUrlBase + sUrl;

                    JsonSerializerSettings settings = new JsonSerializerSettings { Converters = new[] { new JSONConverter() } };
                    string json = JsonConvert.SerializeObject(parameters, settings);
                    Debug.WriteLine("------------------------------------------" + "\n" +
                                    "==> baseAddress: " + baseAddress + "\n" +
                                    "==> uri: " + uri + "\n" +
                                    "==> jsonSend: " + stringJson + "\n" +
                                    "==> parameters: " + json + "\n" +
                                    "------------------------------------------");

                    var client = new HttpClient();
                    client.BaseAddress = new Uri(baseAddress);
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    client.Timeout = TimeSpan.FromSeconds(timeout);

                    var content = new FormUrlEncodedContent(parameters);
                    var response = client.PostAsync(uri, content);
                    result = response.Result.Content.ReadAsStringAsync().Result;

                    if (response.Result.IsSuccessStatusCode)
                    {
                        success = true;
                    }
                }
                catch (Exception ex)
                {
                    success = false;
                    Debug.WriteLine("------------------------------------------" + "\n" +
                                    "==> error: " + ex.Message + "\n" +
                                    "==> source: " + ex.Source + "\n" +
                                    "==> helpLink: " + ex.HelpLink + "\n" +
                                    "==> stack: " + ex.StackTrace);
                }
                finally
                {
                    if (success == false)
                        result = "";

                    Debug.WriteLine("==> success: " + success + "\n" +
                                    "==> result: " + result + "\n" +
                                    "------------------------------------------");

                    requestInterface.ResultRequest(this);
                }

            }).Start();
        }
    }
}
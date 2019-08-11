using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json.Linq;
using System.Net.Http;
using System.Threading;
using RestSharp;

namespace FPV_Video_Manager.Reporting
{
    class AppStartup
    {
        public static string AppDataRoot = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        public static string AppRoot = $@"{AppDataRoot}\FPVVideoManager";
        public static string ClientTokenFile = $@"{AppRoot}\tokenfile.json";

        private static readonly HttpClient client = new HttpClient();
        public void Execute()
        {
            new Thread(DoWork).Start();
        }

        private async void DoWork()
        {
            try
            {
                var values = new Dictionary<string, string>
                {
                   { "client_token", Get_Client_Token() }
                };

                string url = "https://fpvfilemanager.azurewebsites.net/api/appstartup";

                MakeRequest<string>(values);

            }
            catch(Exception e)
            {
            }
        }

        private void MakeRequest<T>(Dictionary<string, string> postParams = null)
        {
            var client = new RestClient("https://fpvfilemanager.azurewebsites.net/api/appstartup");
            var request = new RestRequest(Method.POST);
            request.AddHeader("Content-Type", "application/json");
            request.AddParameter("undefined", JObject.FromObject(postParams).ToString(), ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);
        }

        private string Get_Client_Token()
        {
            string client_token = "nuked";
            try
            {
                if (!Directory.Exists(AppRoot))
                    Directory.CreateDirectory(AppRoot);

                if (!File.Exists(ClientTokenFile))
                {
                    File.WriteAllText(ClientTokenFile, "{\"client_token\":\"" + Guid.NewGuid() + "\"}");
                }
                string returnedText = File.ReadAllText(ClientTokenFile);

                client_token = JObject.Parse(returnedText)["client_token"].ToString();
            }
            catch(Exception e)
            {
            }
            return client_token;
        }
    }
}

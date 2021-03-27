using System;
using System.Diagnostics.Tracing;
using System.Net;
using System.IO;
using System.ComponentModel;
using System.Configuration;
using System.Collections.Specialized;
using System.Xml;
using System.Web.UI;
using System.Web.Script.Serialization;
using System.Xml.Serialization;
using System.Runtime.Serialization;
using System.Runtime.InteropServices;
using System.Reflection;
using System.Reflection.Emit;
using System.Diagnostics;

namespace App
{
    public class MercureClient
    {
        private static WebClient webClient = new WebClient();
        private static JavaScriptSerializer serializer = new JavaScriptSerializer();
        private static KeyIssuer keyIssuer = new KeyIssuer();
        private AppConfig config = App.ConfigParser.getConfig();

        public void run()
        {
            Console.WriteLine("Клиент запущен: " + config.MercureURL);

            using (Stream stream = webClient.OpenRead(config.MercureURL))
            {
                using (StreamReader reader = new StreamReader(stream))
                {
                    string line = null;
                    while (null != (line = reader.ReadLine()))
                    {
                        var firstSeparator = line.IndexOf(":");

                        if (!(firstSeparator > 1)) {
                           continue;
                        }

                        var firstPart = line.Substring(0, firstSeparator);

                        if (firstPart != "data") {
                           continue;
                        }

                        string secondPart = line.Substring(firstSeparator + 1);
                        string jsonContent = secondPart.Trim();
                        Console.WriteLine("Получен новый ключ: " + jsonContent);
                        DoorKey doorKey = serializer.Deserialize<DoorKey>(jsonContent);
                        Console.WriteLine(doorKey);

                        keyIssuer.issue(doorKey);
                    }
                }
            }
        }

        public void keyConfirm(DoorKey key)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string responseText = "";
            string CallbackUrl = key.CallbackUrl;

            try
            {
                string serializedResult = serializer.Serialize(key);

                WebClient client = new WebClient();
                client.Headers.Add("Content-Type", "application/json");
                client.Headers.Add("X-AUTH-TOKEN", config.Token);

                client.Encoding = System.Text.Encoding.UTF8;
                string reply = client.UploadString(CallbackUrl, "PUT", serializedResult);
            }
            catch (WebException exception)
            {
              var responseStream = exception.Response?.GetResponseStream();
              if (responseStream != null)
              {
                  using (var reader = new StreamReader(responseStream))
                  {
                     responseText = reader.ReadToEnd();
                  }
              }
              Console.WriteLine("Ошибка при отправке запроса на " + key.CallbackUrl + ": " + responseText);
            }
            catch (Exception err)
            {
                Console.WriteLine("Ошибка при отправке запроса на " + key.CallbackUrl + ": " + err.Message);
            }
        }
    }
}
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
    public class CallbackSender
    {
        private static WebClient webClient = new WebClient();
        private static JavaScriptSerializer serializer = new JavaScriptSerializer();
        private AppConfig config = App.ConfigParser.getConfig();

        public void send(DoorKey doorKey)
        {
            WebClient client = new WebClient();
            client.Headers.Add("Content-Type", "application/json");
            client.Headers.Add("X-AUTH-TOKEN", config.Token);
            client.Encoding = System.Text.Encoding.UTF8;

            string responseText = "";

            Console.WriteLine("Отправка результата на сервер: " + doorKey.CallbackUrl);

            try
            {
                string doorKeySerialized = serializer.Serialize(doorKey);
                Console.WriteLine(doorKeySerialized);

                string response = client.UploadString(doorKey.CallbackUrl, "PUT", doorKeySerialized);
                Console.WriteLine("Ответ сервера: " + response);

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
              Console.WriteLine("Ошибка при отправке запроса на " + doorKey.CallbackUrl + ": " + responseText);
            }
            catch (Exception err)
            {
                Console.WriteLine("Ошибка при отправке запроса на " + doorKey.CallbackUrl + ": " + err.Message);
            }
        }
    }
}
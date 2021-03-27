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
    [Serializable]
    public class AppConfig
    {
        public string Token { get; set; }
        public string MercureURL { get; set; }
        public string ServerURL { get; set; }

        public AppConfig()
        {
        }

        public AppConfig(string token, string mercureUrl, string serverUrl)
        {
            Token = token;
            MercureURL = mercureUrl;
            ServerURL = serverUrl;
        }
    }
}

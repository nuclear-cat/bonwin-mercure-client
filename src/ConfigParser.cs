using System;
using System.IO;
using System.Xml.Serialization;
using System.Reflection;

namespace App
{
    public class ConfigParser
    {
        public static AppConfig getConfig()
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(AppConfig));

            string buildDir = new FileInfo(Assembly.GetEntryAssembly().Location).Directory.ToString();
            string configPath = Path.Combine(new string[] {buildDir, "config.xml"});

            if (!File.Exists(configPath))
            {
                Console.WriteLine("Ошибка. Создайте файл конфигруации " + configPath);
                Console.ReadLine();
            }

            using (FileStream fs = new FileStream(configPath, FileMode.OpenOrCreate))
            {
                AppConfig config = (AppConfig)xmlSerializer.Deserialize(fs);
                return config;
            }
        }
    }
}
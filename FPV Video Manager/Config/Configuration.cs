using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json.Linq;


namespace FPV_Video_Manager.Config
{
    public class Configuration
    {
        private static JObject ConfigFile;
        private static GlobalConfig _Config;
        public GlobalConfig Config { get { return _Config; } set { _Config = value; } }
        private static string AppDataRoot = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        private static string AppConfigDirectory = $@"{AppDataRoot}\FPVVideoManager";
        private static string AppConfigFile = $@"{AppConfigDirectory}\AppConfig.json";

        public void LoadFromConfig()
        {
            int attempt = 0;
            bool exists = false;
            do
            {
                exists = false;
                try
                {
                    if (File.Exists($@"{AppConfigFile}"))
                    {
                        exists = true;
                        ConfigFile = JObject.Parse(File.ReadAllText($@"{AppConfigFile}"));
                    }
                    else
                        ConfigFile = null;
                }
                catch
                {
                    ConfigFile = null;
                }
                attempt++;
            } while (ConfigFile == null && attempt < 5 && exists);

            if (ConfigFile == null)
                CreateBaseConfig();

            Config = ConfigFile.ToObject<GlobalConfig>();
        }

        public void save()
        {
            try
            {
                ConfigFile = JObject.FromObject(_Config);
                File.WriteAllText($@"{AppConfigFile}", ConfigFile.ToString());
            }
            catch
            {
            }
        }

        public void CreateBaseConfig()
        {
            File.Create($@"{AppConfigFile}").Close();
            JObject JO = JObject.FromObject(new GlobalConfig());
            File.WriteAllText($@"{AppConfigFile}", JO.ToString());
            ConfigFile = JO;
        }

        
    }
    public class GlobalConfig
    {
        public string version = "1";
        public bool speakStages = true;
    }
}

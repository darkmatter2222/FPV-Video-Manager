using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json.Linq;

namespace InterlacingLayer
{
    public class InterlacingConfiguration
    {
        private static JObject ConfigFile;
        private static Records _Config;
        public Records Config { get { return _Config; } set { _Config = value; } }
        private static string AppDataRoot = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        private static string AppConfigDirectory = $@"{AppDataRoot}\FPVVideoManager";
        private static string AppConfigFile = $@"{AppConfigDirectory}\EngineConfig.json";

        public void SaveRecord(RecordConfig recordConfig)
        {
            bool recordUpdated = false;

            for(int x=0;x<Config.recordConfigs.Count;x++)
            {
                if (recordConfig.record_id.Equals(Config.recordConfigs[x].record_id))
                {
                    Config.recordConfigs[x] = recordConfig;
                    recordUpdated = true;
                    break;
                }
            }

            if (!recordUpdated)
            {
                Config.recordConfigs.Add(recordConfig);
            }
            // perform save
            // TODO
        }

        public List<RecordConfig> GetRecords()
        {
            return Config.recordConfigs;
        }
    }

    public class Records
    {
        public List<RecordConfig> recordConfigs = new List<RecordConfig>();
    }

    public class RecordConfig
    {
        public DateTime driveMountTime = DateTime.Now;
        public string record_id = "";
        public string source = "";
        public string destination = "";
        public string sourceID = "Pending Save...";
        public string targetTimeZone = "System Time";
        public string fileNameing = "Preserve File Name";
        public string targetPrefix = "";
        public string targetSuffix = "";
        public bool audiableNotification = false;
        public bool autoCompression = false;
        public string destinationTargetFormat = "MM-dd-yy H-mm-ss fffffff"; //https://docs.microsoft.com/en-us/dotnet/standard/base-types/custom-date-and-time-format-strings
    }
}

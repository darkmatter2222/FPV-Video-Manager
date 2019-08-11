﻿using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json.Linq;
using System.IO;

namespace InterlacingLayer
{
    public class InterlacingConfiguration
    {
        
        private static JObject ConfigFile;
        private static Records _Config;
        public Records Config { get { return _Config; } set { _Config = value; } }
        private static string AppDataRoot = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        private static string AppConfigDirectory = $@"{AppDataRoot}\FPVVideoManager";
        private static string AppConfigFile = $@"{AppConfigDirectory}\EngineConfigV2.json";

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
            save();

        }

        public void save()
        {
            // check to see if source has the config file, if not create it



            File.WriteAllText($@"{AppConfigFile}", JObject.FromObject(Config).ToString());
        }

        public List<RecordConfig> GetRecords()
        {
            return Config.recordConfigs;
        }

        public void LoadRecords()
        {
            if (!Directory.Exists(AppConfigDirectory))
                Directory.CreateDirectory(AppConfigDirectory);

            if (!File.Exists(AppConfigFile))
            {
                File.Create(AppConfigFile).Close();
                File.WriteAllText(AppConfigFile, JObject.FromObject(new Records()).ToString());
            }
            var configText = File.ReadAllText(AppConfigFile);
            if (string.IsNullOrEmpty(configText))
            {
                File.WriteAllText(AppConfigFile, JObject.FromObject(new Records()).ToString());
                configText = File.ReadAllText(AppConfigFile);
            }
            
            Config = JObject.Parse(configText).ToObject<Records>();
        }


    }

    public class Records
    {
        public List<RecordConfig> recordConfigs = new List<RecordConfig>();
    }

    public class RecordConfig
    {
        public DateTime driveMountTime = DateTime.Now;
        public string record_id = Guid.NewGuid().ToString();
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

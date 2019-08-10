using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json.Linq;

namespace InterlacingLayer
{
    class InterlacingConfiguration
    {
        private static JObject ConfigFile;
        private static EngineConfig _Config;
        public EngineConfig Config { get { return _Config; } set { _Config = value; } }
        private static string AppDataRoot = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        private static string AppConfigDirectory = $@"{AppDataRoot}\FPVVideoManager";
        private static string AppConfigFile = $@"{AppConfigDirectory}\EngineConfig.json";
    }

    class EngineConfig
    {
    }
}

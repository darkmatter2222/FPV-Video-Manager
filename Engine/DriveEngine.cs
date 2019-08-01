using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.IO;
using Newtonsoft.Json.Linq;

namespace Engine
{
   
    public class DriveEngine
    {
        #region Drive Lists
        private static List<DriveInformation> _knownConnectedDrives = new List<DriveInformation>();
        public List<DriveInformation> knownConnectedDrives
        {
            get { return _knownConnectedDrives; }
            set { _knownConnectedDrives = value; }
        }

        private static event TickHandler _DriveChange;

        public TickHandler DriveChange
        {
            get { return _DriveChange; }
            set { _DriveChange = value; }
        }

        public delegate void TickHandler(List<DriveInformation> _LDI, EventArgs e);

        public void removeFromKnownConnectedDrive(DriveInformation DI)
        {
            knownConnectedDrives.Remove(DI);
            DriveChange(_knownConnectedDrives, null);
        }

        public void addToKnownConnectedDrive(DriveInformation DI)
        {
            DI.LoadFromConfig();

            knownConnectedDrives.Add(DI);
            DriveChange(_knownConnectedDrives, null);
        }

        private static List<DriveInformation> _disconnectingDrives = new List<DriveInformation>();
        public List<DriveInformation> disconnectingDrives
        {
            get { return _disconnectingDrives; }
            set { _disconnectingDrives = value; }
        }

        private static List<DriveInformation> _connectingDrives = new List<DriveInformation>();
        public List<DriveInformation> connectingDrives
        {
            get { return _connectingDrives; }
            set { _connectingDrives = value; }
        }
        #endregion

        public static bool EngineRunning = false;

        Thread T;


        public DriveEngine()
        {

        }

        public void InitializeEngine()
        {
            EngineRunning = true;
            // start engine
            if (T == null)
            {
                T = new Thread(TickEngine);
                T.Start();
            }
        }

        public void TerminateEngine()
        {
            EngineRunning = false;
        }

        public void ReloadAll()
        {
            _knownConnectedDrives = new List<DriveInformation>();
            DriveChange(_knownConnectedDrives, null);
        }

        public void TickEngine()
        {
            while (EngineRunning)
            {
                try
                {
                    // Get all connected Drives
                    DriveInfo[] allDrives = DriveInfo.GetDrives();

                    // Find all New Drives
                    foreach (var drive in allDrives)
                    {
                        bool driveFound = false;
                        foreach (var knownDrive in knownConnectedDrives)
                        {
                            if (drive.Name.Equals(knownDrive.Name))
                            {
                                driveFound = true;
                                break;
                            }
                        }

                        if (!driveFound)
                        {
                            connectingDrives.Add(new DriveInformation() { Name = drive.Name });
                        }
                    }

                    // Find all Missing Drives
                    foreach (var knownDrive in knownConnectedDrives)
                    {
                        bool driveFound = false;
                        foreach (var drive in allDrives)
                        {
                            if (knownDrive.Name.Equals(drive.Name))
                            {
                                driveFound = true;
                                break;
                            }
                        }
                        if (!driveFound)
                        {
                            disconnectingDrives.Add(new DriveInformation() { Name = knownDrive.Name });
                        }
                    }

                    // Apply Changes
                    if (connectingDrives.Count > 0)
                    {
                        foreach (var drive in connectingDrives)
                        {
                            addToKnownConnectedDrive(drive);
                        }
                        connectingDrives = new List<DriveInformation>();
                    }

                    if (disconnectingDrives.Count > 0)
                    {
                        var removeTarget = new DriveInformation();
                        foreach (var drive in disconnectingDrives)
                        {
                            foreach (var knownDrive in knownConnectedDrives)
                            {
                                if (drive.Name.Equals(knownDrive.Name))
                                {
                                    removeTarget = knownDrive;
                                }
                            }
                        }
                        removeFromKnownConnectedDrive(removeTarget);
                        disconnectingDrives = new List<DriveInformation>();
                    }

                    //sleep
                    Thread.Sleep(100);
                }
                catch
                {

                }
            }
        }
    }

    public class DriveInformation
    {
        public string Name = "";
        public DateTime driveMountTime;
        public string offloadFolderName = "";

        private JObject ConfigFile;

        public void LoadFromConfig()
        {
            driveMountTime = DateTime.UtcNow;
            offloadFolderName = $@"{driveMountTime.ToString().Replace("/","-").Replace("\\","-").Replace(":","-")}";
            try
            {
                if (File.Exists($@"{Name}\FPVVideoManager.json"))
                    ConfigFile = JObject.Parse(File.ReadAllText($@"{Name}\FPVVideoManager.json"));
                else
                    ConfigFile = null;
            }
            catch
            {
                ConfigFile = null;
            }
        }

        public bool isMonitoring
        {
            get
            {
                bool Monitoring = false;
                if (ConfigFile != null && ConfigFile["Monitoring"].ToString().Equals("True"))
                {
                    Monitoring = true;
                }
                return Monitoring;
            }
            set
            {
                if (ConfigFile == null)
                {
                    CreateBaseConfig();
                }
                if (value)
                {
                    ConfigFile["Monitoring"] = "True";
                }
                else
                {
                    ConfigFile["Monitoring"] = "False";
                }
                save();
            }
        }

        public string source
        {
            get
            {
                return ConfigFile["Source"].ToString();
            }
            set
            {
                ConfigFile["Source"] = value;
            }
        }

        public string destination
        {
            get
            {
                return ConfigFile["Destination"].ToString();
            }
            set
            {
                ConfigFile["Destination"] = value;
            }
        }

        public void save()
        {
            File.WriteAllText($@"{Name}\FPVVideoManager.json", ConfigFile.ToString());
        }

        public void CreateBaseConfig()
        {
            if (ConfigFile == null)
            {
                File.Create($@"{Name}\FPVVideoManager.json").Close();
                JObject JO = JObject.Parse("{\"Version\": \"1\",\"Monitoring\": \"False\",\"Source\":\"\",\"Destination\":\"\",\"DeleteAfterMobe\":\"False\"}");
                File.WriteAllText($@"{Name}\FPVVideoManager.json", JO.ToString());
                ConfigFile = JO;
            }
        }
    }
}

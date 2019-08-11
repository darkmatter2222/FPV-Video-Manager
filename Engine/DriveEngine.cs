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
        public string sourceID;

        public void LoadFromConfig()
        {
            driveMountTime = DateTime.UtcNow;
            try
            {
                if (File.Exists($@"{Name}\FPVVideoManagerV2.json"))
                    sourceID = JObject.Parse(File.ReadAllText($@"{Name}\FPVVideoManagerV2.json"))["sourceID"].ToString();
                else
                    sourceID = null;
            }
            catch
            {
                sourceID = null;
            }
        }

        public void save()
        {
            File.WriteAllText($@"{Name}\FPVVideoManagerV2.json", new InterlacingLayer.InterlacingConfiguration().Config.ToString());
        }

        public void CreateBaseConfig(string _sourceID)
        {
            File.Create($@"{Name}\FPVVideoManagerV2.json").Close();
            JObject JO = JObject.FromObject("{\"sourceID\":\"" + _sourceID + "\" }");
            File.WriteAllText($@"{Name}\FPVVideoManagerV2.json", JO.ToString());
            sourceID = _sourceID;
        }
    }
}

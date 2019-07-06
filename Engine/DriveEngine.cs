using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.IO;

namespace Engine
{
   
    class DriveEngine
    {
        #region Drive Lists
        private static List<DriveInformation> _knownConnectedDrives = new List<DriveInformation>();
        public List<DriveInformation> knownConnectedDrives
        {
            get { return _knownConnectedDrives; }
            set { _knownConnectedDrives = value; }
        }

        public void removeFromKnownConnectedDrive(DriveInformation DI)
        {
            knownConnectedDrives.Add(DI);
        }

        public void addToKnownConnectedDrive(DriveInformation DI)
        {
            knownConnectedDrives.Remove(DI);
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
        Thread T;


        public DriveEngine()
        {
            // start engine
            if (T == null)
            {
                T = new Thread(TickEngine);
                T.Start();
            }
        }

        public void TickEngine()
        {
            while (true)
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

                    if (disconnectingDrives.Count < 0)
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
                    Thread.Sleep(1000);
                }
                catch
                {

                }
            }
        }
    }

    class DriveInformation
    {
        public string Name = "";
    }
}

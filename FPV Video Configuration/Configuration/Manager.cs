using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace FPV_Video_Configuration.Configuration
{
    public class Manager
    {
        private readonly static string appDataRoot = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        private readonly static string fPVVideoDirectory = @"FPV Video";
        private readonly static string managerDirectory = @"Manager";
        private readonly static string migratorDirectory = @"Migrator";
        private readonly static string versionDirectory = @"V1";
        private readonly static string fPVVideoRoot = $@"{appDataRoot}\{fPVVideoDirectory}\{versionDirectory}";
        private readonly static string fPVVideoManagerRoot = $@"{appDataRoot}\{fPVVideoDirectory}\{versionDirectory}\{managerDirectory}";
        private readonly static string fPVVideoMigratorRoot = $@"{appDataRoot}\{fPVVideoDirectory}\{versionDirectory}\{migratorDirectory}";

        public Manager()
        {
            if (!Directory.Exists(fPVVideoRoot))
                Directory.CreateDirectory(fPVVideoRoot);

            if (!Directory.Exists(fPVVideoManagerRoot))
                Directory.CreateDirectory(fPVVideoManagerRoot);

            if (!Directory.Exists(fPVVideoMigratorRoot))
                Directory.CreateDirectory(fPVVideoMigratorRoot);
        }
    }
}

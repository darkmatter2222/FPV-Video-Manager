using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FPV_Video_Manager
{
    class GlobalVariables
    {
        private static MainWindowV2 _MWV2 = null;

        public MainWindowV2 MWV2
        {
            get { return _MWV2; }
            set { _MWV2 = value; }
        }
    }
}

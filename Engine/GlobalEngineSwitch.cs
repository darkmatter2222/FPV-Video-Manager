using System;
using System.Collections.Generic;
using System.Text;

namespace Engine
{
    public class GlobalEngineSwitch
    {
        private static bool _AllEnginesRunning = true;
        public bool AllEnginesRunning
        {
            get { return _AllEnginesRunning; }
            set { _AllEnginesRunning = value; }
        }
    }
}

using BlyadleField.Overlay;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlyadleField
{
    class BlyadleField
    {
        public static string GameName { get { return "Battlefield 1 Open Beta"; } }
        public static string ProcessName { get { return "bf1"; } }
        public static bool IsAttached { get; set; }
        public static Process Process { get; set; }
        public static OverlayWindow Overlay { get; set; }
        public static MemorySystem.MemoryScanner Memory { get; set; }
    }
}

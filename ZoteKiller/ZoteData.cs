using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZoteKiller
{
    public class ZoteData
    {
        public bool KilledZote { get; set; } = false;
        public float DeadX { get; set; } = 0;
        public float DeadY { get; set; } = 0;
        public string DeadScene { get; set; } = "";
        public bool GrilFan { get; set; } = false;
    }
}

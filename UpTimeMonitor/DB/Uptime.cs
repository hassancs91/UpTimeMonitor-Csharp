using System;
using System.Collections.Generic;

namespace UpTimeMonitor
{
    public partial class Uptime
    {
        public int Scanid { get; set; }
        public int? Websiteid { get; set; }
        public DateTime? Time { get; set; }
        public bool? Status { get; set; }
        public int ResponseTime { get; set; }
    }
}

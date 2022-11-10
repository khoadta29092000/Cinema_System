using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CinemaSystem.Models
{
    public partial class SendEmail
    {
        public string to { get; set; }
        public string subject { get; set; }

        public string film { get; set; }
        public string time { get; set; }
        public string room { get; set; }
        public string cinema { get; set; }
        public string date { get; set; }
        public string startTime { get; set; }
        public string Total { get; set; }
        public string image { get; set; }
        public string service  { get; set; }
        public string ticked { get; set; }
    }
}

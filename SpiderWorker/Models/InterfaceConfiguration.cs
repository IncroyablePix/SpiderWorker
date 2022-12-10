using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpiderWorker.Models
{
    [Serializable]
    public record IPConfiguration
    {
        public event EventHandler? Changed;
        public string IP { get; set; } 
        public string SubnetMask { get; set; }
        public string Gateway { get; set; }
        public string PreferredDNS { get; set; }
        public string AlternateDNS { get; set; }
        public bool IsDHCP { get; set; }
        public bool AutoDNS { get; set; }
    }

    [Serializable]
    public class InterfaceConfiguration
    {
        public IPConfiguration IPv4 { get; init; }
    }
}

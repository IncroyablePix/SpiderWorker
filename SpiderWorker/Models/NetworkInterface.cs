using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpiderWorker.Models
{
    public class NetworkInterface
    {
        public NetworkInterface(string name, string ip, string subnetMask, string gateway, string dns1, string dns2, bool isDHCP)
        {
            Name = name;
            IP = ip;
            SubnetMask = subnetMask;
            Gateway = gateway;
            PreferredDNS = dns1;
            AlternateDNS = dns2;
            IsDHCP = isDHCP;
            IsApplied = true;
        }
        
        public string Name { get; set; }
        public string IP { get; set; }
        public string SubnetMask { get; set; }
        public string Gateway { get; set; }
        public string PreferredDNS { get; set; }
        public string AlternateDNS { get; set; }
        public bool IsDHCP { get; set; }
        public bool IsApplied { get; set; }
    }
}

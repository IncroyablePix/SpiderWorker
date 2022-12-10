using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tmds.DBus;

namespace SpiderWorker.Models
{    
    public class NetworkInterface
    {
        public NetworkInterface(string name, string ip, string subnetMask, string gateway, string dns1, string dns2, bool isDHCP, bool autoDns)
        {
            Name = name;
            Configuration = new InterfaceConfiguration
            {
                IPv4 = new IPConfiguration
                {
                    IP = ip,
                    SubnetMask = subnetMask,
                    Gateway = gateway,
                    PreferredDNS = dns1,
                    AlternateDNS = dns2,
                    IsDHCP = isDHCP,
                    AutoDNS = autoDns
                }
            };

            Configuration.IPv4.Changed += (s, e) => IsApplied = false;
            
            IsApplied = true;
        }
        
        public string Name { get; set; }
        public InterfaceConfiguration Configuration { get; init; }
        public bool IsApplied { get; set; }
    }
}

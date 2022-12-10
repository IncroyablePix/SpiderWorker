using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
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

        public void Copy(IPConfiguration other)
        {
            IP = other.IP;
            SubnetMask = other.SubnetMask;
            Gateway = other.Gateway;
            PreferredDNS = other.PreferredDNS;
            AlternateDNS = other.AlternateDNS;
            IsDHCP = other.IsDHCP;
            AutoDNS = other.AutoDNS;
        }
    }

    [Serializable]
    public class InterfaceConfiguration
    {
        public string Name { get; set; } = string.Empty;
        public IPConfiguration? IPv4 { get; init; }

        public void Copy(InterfaceConfiguration other)
        {
            Name = other.Name;
            IPv4?.Copy(other.IPv4!);
        }

        [JsonIgnore]
        public string FullDescription 
        {
            get
            {
                string ToOnOff(bool v)
                {
                    return v ? "On" : "Off";
                }
                
                var sb = new StringBuilder();

                if (IPv4 != null)
                {
                    sb.AppendLine("IPv4:");
                    sb.AppendLine($"IP: {IPv4.IP}");
                    sb.AppendLine($"Subnet Mask: {IPv4.SubnetMask}");
                    sb.AppendLine($"Gateway: {IPv4.Gateway}");
                    sb.AppendLine($"Preferred DNS: {IPv4.PreferredDNS}");
                    sb.AppendLine($"Alternate DNS: {IPv4.AlternateDNS}");
                    sb.AppendLine($"DHCP: {ToOnOff(IPv4.IsDHCP)}");
                    sb.AppendLine($"DNS through DHCP: {ToOnOff(IPv4.AutoDNS)}");
                }

                return sb.ToString();
            }
        }

        public string GetDefaultName()
        {
            if (IPv4 != null)
            {
                var ip = IPv4.IP;
                var subnetMask = IPv4.SubnetMask;
                var cidr = Convert.ToInt32(subnetMask.Split('.').Select(byte.Parse).Aggregate(0, (a, b) => a + Convert.ToString(b, 2).Count(c => c == '1')));

                return $"{ip}/{cidr}";
            }

            return string.Empty;            
        }
    }
}

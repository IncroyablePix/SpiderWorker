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
        private string _ip;
        private string _subnetMask;
        private string _gateway;
        private string _preferredDNS;
        private string _alternateDNS;
        private bool _isDHCP;
        private bool _autoDNS;
        public string IP { get => _ip; set { _ip = value; Changed?.Invoke(this, null); } }
        public string SubnetMask { get => _subnetMask; set { _subnetMask = value; Changed?.Invoke(this, null); } }
        public string Gateway { get => _gateway; set { _gateway = value; Changed?.Invoke(this, null); } }
        public string PreferredDNS { get => _preferredDNS; set { _preferredDNS = value; Changed?.Invoke(this, null); } }
        public string AlternateDNS { get => _alternateDNS; set { _alternateDNS = value; Changed?.Invoke(this, null); } }
        public bool IsDHCP { get => _isDHCP; set { _isDHCP = value; Changed?.Invoke(this, null); } }
        public bool AutoDNS { get => _autoDNS; set { _autoDNS = value; Changed?.Invoke(this, null); } }
        
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

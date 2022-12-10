using SpiderWorker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.NetworkInformation;
using SpiderInterface = SpiderWorker.Models.NetworkInterface;
using NetworkInterface = System.Net.NetworkInformation.NetworkInterface;
using System.Management;
using System.Diagnostics;

namespace SpiderWorker.Services.IPConfig
{
    public class WindowsIPConfigurator : IIPConfigurator
    {
        IEnumerable<SpiderInterface> IIPConfigurator.GetNetworkInterfaces()
        {
            var interfaces = NetworkInterface.GetAllNetworkInterfaces();
            return interfaces.Select(FromWin32NetworkInterface);
        }

        private SpiderInterface FromWin32NetworkInterface(NetworkInterface itf)
        {
            var ipProps = itf.GetIPProperties();
            var ip = ipProps.UnicastAddresses.FirstOrDefault(a => a.Address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork);
            var subnetMask = ipProps.UnicastAddresses.FirstOrDefault(a => a.Address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork);
            var gateway = ipProps.GatewayAddresses.FirstOrDefault(a => a.Address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork);

            // Get preferred and alternate DNS
            var preferredDNS = ipProps.DnsAddresses.FirstOrDefault(a => a.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork);
            var alternateDNS = ipProps.DnsAddresses.LastOrDefault(a => a.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork);

            // Are DNS servers provided by DHCP ?
            var autoDNS = false;
            var key = Microsoft.Win32.Registry.LocalMachine.OpenSubKey(@"SYSTEM\CurrentControlSet\Services\Tcpip\Parameters\Interfaces\" + itf.Id);
            if (key != null)
            {
                var nameServer = key.GetValue("NameServer");
                if (nameServer != null)
                {
                    autoDNS = string.IsNullOrEmpty(nameServer.ToString());
                }
            }

            var isDHCP = ipProps.GetIPv4Properties().IsDhcpEnabled;

            return new SpiderInterface(itf.Name, ip.Address.ToString() ?? string.Empty, subnetMask?.IPv4Mask?.ToString() ?? string.Empty, gateway?.Address?.ToString() ?? string.Empty, preferredDNS?.ToString() ?? string.Empty, alternateDNS?.ToString() ?? string.Empty, isDHCP, autoDNS);
        }

        public bool ApplyConfiguration(SpiderInterface networkInterface)
        {
            if (networkInterface.IsApplied)
                return true;
            
            var itf = NetworkInterface.GetAllNetworkInterfaces().FirstOrDefault(i => i.Name == networkInterface.Name);
            if (itf == null)
                return false;

            if (!SetIP(networkInterface))
                return false;
            
            if(!SetDNS(networkInterface))
                return false;

            return true;
        }

        private bool SetIP(SpiderInterface netIf)
        {
            var p = new Process();
            if (netIf.Configuration.IPv4.IsDHCP)
                p.StartInfo = new ProcessStartInfo("netsh", $"interface ip set address \"{netIf.Name}\" dhcp");
            else
                p.StartInfo = new ProcessStartInfo("netsh", $"interface ip set address \"{netIf.Name}\" static {netIf.Configuration.IPv4.IP} {netIf.Configuration.IPv4.SubnetMask} {netIf.Configuration.IPv4.Gateway}");
            
            p.StartInfo.CreateNoWindow = true;
            p.Start();
            p.WaitForExit();

            return p.ExitCode == 0;
        }

        public bool SetDNS(SpiderInterface netIf)
        {
            var p = new Process();
            if(netIf.Configuration.IPv4.AutoDNS)
                p.StartInfo = new ProcessStartInfo("netsh", $"interface ip set dns \"{netIf.Name}\" dhcp");
            else
                p.StartInfo = new ProcessStartInfo("netsh", $"interface ip set dns \"{netIf.Name}\" static {netIf.Configuration.IPv4.PreferredDNS} {netIf.Configuration.IPv4.AlternateDNS}");
            
            p.StartInfo.CreateNoWindow = true;
            p.Start();
            p.WaitForExit();

            return p.ExitCode == 0;
        }
    }
}

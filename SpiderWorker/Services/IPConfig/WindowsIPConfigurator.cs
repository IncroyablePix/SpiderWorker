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
            
            var dns = ipProps.DnsAddresses.FirstOrDefault(a => a.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork);
            var dns2 = ipProps.DnsAddresses.FirstOrDefault(a => a.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork);

            var isDHCP = ipProps.GetIPv4Properties().IsDhcpEnabled;

            return new SpiderInterface(itf.Name, ip.Address.ToString() ?? string.Empty, subnetMask?.IPv4Mask?.ToString() ?? string.Empty, gateway?.Address?.ToString() ?? string.Empty, dns?.ToString() ?? string.Empty, dns2?.ToString() ?? string.Empty, isDHCP);
        }

        public bool ApplyConfiguration(SpiderInterface networkInterface)
        {
            var itf = NetworkInterface.GetAllNetworkInterfaces().FirstOrDefault(i => i.Name == networkInterface.Name);
            if (itf == null)
                return false;

            var ipProps = itf.GetIPProperties();
            if (networkInterface.IsDHCP)
                SetIP(networkInterface.IP, networkInterface.SubnetMask, networkInterface.Gateway);

            // Change interface properties
            var ip = ipProps.UnicastAddresses.FirstOrDefault(a => a.Address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork);

            ip.Address.Address = System.Net.IPAddress.Parse(networkInterface.IP).Address;
            ip.IPv4Mask.Address = System.Net.IPAddress.Parse(networkInterface.SubnetMask).Address;

            return true;
        }

        private void SetIP(string ipAddress, string subnetMask, string gateway)
        {
            using (var networkConfigMng = new ManagementClass("Win32_NetworkAdapterConfiguration"))
            {
                using (var networkConfigs = networkConfigMng.GetInstances())
                {
                    foreach (var managementObject in networkConfigs.Cast<ManagementObject>().Where(managementObject => (bool)managementObject["IPEnabled"]))
                    {
                        using (var newIP = managementObject.GetMethodParameters("EnableStatic"))
                        {
                            // Set new IP address and subnet if needed
                            if ((!String.IsNullOrEmpty(ipAddress)) || (!String.IsNullOrEmpty(subnetMask)))
                            {
                                if (!String.IsNullOrEmpty(ipAddress))
                                {
                                    newIP["IPAddress"] = new[] { ipAddress };
                                }

                                if (!String.IsNullOrEmpty(subnetMask))
                                {
                                    newIP["SubnetMask"] = new[] { subnetMask };
                                }

                                managementObject.InvokeMethod("EnableStatic", newIP, null);
                            }

                            // Set mew gateway if needed
                            if (!String.IsNullOrEmpty(gateway))
                            {
                                using (var newGateway = managementObject.GetMethodParameters("SetGateways"))
                                {
                                    newGateway["DefaultIPGateway"] = new[] { gateway };
                                    newGateway["GatewayCostMetric"] = new[] { 1 };
                                    managementObject.InvokeMethod("SetGateways", newGateway, null);
                                }
                            }
                        }
                    }
                }
            }
        }

        public void SetDNS(string NIC, string priWINS, string secWINS)
        {
            ManagementClass objMC = new ManagementClass("Win32_NetworkAdapterConfiguration");
            ManagementObjectCollection objMOC = objMC.GetInstances();

            foreach (ManagementObject objMO in objMOC)
            {
                if ((bool)objMO["IPEnabled"])
                {
                    if (objMO["Caption"].Equals(NIC))
                    {
                        ManagementBaseObject setWINS;
                        ManagementBaseObject wins =
                        objMO.GetMethodParameters("SetWINSServer");
                        wins.SetPropertyValue("WINSPrimaryServer", priWINS);
                        wins.SetPropertyValue("WINSSecondaryServer", secWINS);

                        setWINS = objMO.InvokeMethod("SetWINSServer", wins, null);
                    }
                }
            }
        }
    }
}

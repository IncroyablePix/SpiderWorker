using SpiderWorker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using NetworkInterface = SpiderWorker.Models.NetworkInterface;

namespace SpiderWorker.Services.IPConfig
{
    public interface IIPConfigurator
    {
        IEnumerable<NetworkInterface> GetNetworkInterfaces();
        bool ApplyConfiguration(NetworkInterface networkInterface);
    }
}

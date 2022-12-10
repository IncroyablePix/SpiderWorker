using SpiderWorker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpiderWorker.Services.IPConfig
{
    public interface IIPConfigurator
    {
        IEnumerable<NetworkInterface> GetNetworkInterfaces();
        bool ApplyConfiguration(NetworkInterface networkInterface);
    }
}

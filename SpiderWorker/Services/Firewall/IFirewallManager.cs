using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpiderWorker.Services.Firewall
{
    public interface IFirewallManager
    {
        bool ToggleFirewall(bool enable);
        bool IsFirewallEnabled();
    }
}

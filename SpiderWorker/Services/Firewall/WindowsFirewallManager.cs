using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpiderWorker.Services.Firewall
{
    public class WindowsFirewallManager : IFirewallManager
    {
        public bool ToggleFirewall(bool enable)
        {
            var p = new Process();
            if(enable)
                p.StartInfo = new ProcessStartInfo("netsh", "advfirewall set allprofiles state on");
            else
                p.StartInfo = new ProcessStartInfo("netsh", "advfirewall set allprofiles state off");

            p.StartInfo.CreateNoWindow = true;
            p.StartInfo.RedirectStandardOutput = true;
            p.Start();
            p.WaitForExit();

            var output = p.StandardOutput.ReadToEnd();

            return p.ExitCode == 0;
        }

        public bool IsFirewallEnabled()
        {
            var p = new Process();
            p.StartInfo = new ProcessStartInfo("netsh", "advfirewall show currentprofile");
            p.StartInfo.CreateNoWindow = true;
            p.StartInfo.RedirectStandardOutput = true;
            p.Start();
            p.WaitForExit();

            var output = p.StandardOutput.ReadToEnd();

            return output.Contains("ON");
        }
    }
}

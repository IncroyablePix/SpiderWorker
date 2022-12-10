using SpiderWorker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace SpiderWorker.Services
{
    public interface IDnsManagerService : IEnumerable<DNSEntry>
    {
        public bool AddEntry(DNSEntry dnsEntry);
        public bool RemoveEntry(DNSEntry dnsEntry);
        public void Update();
        public static IDnsManagerService operator+(IDnsManagerService dnsManager, DNSEntry dnsEntry)
        {
            dnsManager.AddEntry(dnsEntry);
            return dnsManager;
        }
        
        public static IDnsManagerService operator -(IDnsManagerService dnsManager, DNSEntry dnsEntry)
        {
            dnsManager.RemoveEntry(dnsEntry);
            return dnsManager;
        }
    }
}

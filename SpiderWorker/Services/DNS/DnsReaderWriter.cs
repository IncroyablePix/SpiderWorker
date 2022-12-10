using SpiderWorker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SpiderWorker.Services
{
    public abstract class DnsReaderWriter
    {
        public abstract void Write(IEnumerable<DNSEntry> entries);
        public abstract IEnumerable<DNSEntry> Read();
        public bool TryReadEntry(string line, out DNSEntry? entry)
        {
            var (Success, Groups) = Matches(line);

            if (Success)
            {
                entry = new DNSEntry(Groups[2].Value, Groups[1].Value);
                return true;
            }

            entry = null;
            return false;
        }

        protected (bool Success, GroupCollection Groups) Matches(string line)
        {
            
            var lineRx = new Regex(@"^(\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3})\s+((([a-zA-Z0-9][0-9a-zA-Z\-_]+)*\.)+[a-zA-Z]{2,})\s*#\s*(Managed by SpiderWorker)\s*$");
            var match = lineRx.Match(line);

            return (match.Success, match.Groups);
        }
    }
}

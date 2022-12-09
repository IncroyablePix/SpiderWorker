using SpiderWorker.Models;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpiderWorker.Services
{
    public class WindowsDnsReaderWriter : DnsReaderWriter
    {
        private const string WindowsHostsFile = @"C:\Windows\System32\drivers\etc\hosts";
        public override void Write(IEnumerable<DNSEntry> entries)
        {
            var lines = System.IO.File.ReadAllLines(WindowsHostsFile).Where(l => !Matches(l).Success).ToList();

            var sb = new StringBuilder();
            
            foreach(var line in lines) 
                sb.AppendLine(line);

            foreach (var entry in entries) 
                sb.AppendLine(entry.ToString());

            System.IO.File.WriteAllText(WindowsHostsFile, sb.ToString());
        }

        public override IEnumerable<DNSEntry> Read()
        {
            var entries = new List<DNSEntry>();
            var lines = System.IO.File.ReadAllLines(WindowsHostsFile);
            foreach(var line in lines)
                if (TryReadEntry(line, out var entry))
                    entries.Add(entry);

            return entries;
        }
    }
}

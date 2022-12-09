using SpiderWorker.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpiderWorker.Services
{
    public class DnsManagerService : IDnsManagerService
    {
        private HashSet<DNSEntry> _entries = new();
        private DnsReaderWriter ReaderWriter { get; set; }

        public DnsManagerService(DnsReaderWriter readerWriter)
        {
            _entries = readerWriter.Read().ToHashSet();
            ReaderWriter = readerWriter;
        }

        public IEnumerator<DNSEntry> GetEnumerator() => _entries.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public bool RemoveEntry(DNSEntry dnsEntry)
        {
            if (_entries.Contains(dnsEntry))
            {
                _entries.Remove(dnsEntry);
                return true;
            }
            return false;
        }

        public bool AddEntry(DNSEntry dnsEntry)
        {
            if (!_entries.Contains(dnsEntry))
            {
                _entries.Add(dnsEntry);
                return true;
            }
            return false;
        }

        public void Update()
        {
            ReaderWriter.Write(_entries);
        }
    }
}

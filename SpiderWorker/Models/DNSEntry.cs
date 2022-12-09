using System.Text.RegularExpressions;

namespace SpiderWorker.Models
{
    public class DNSEntry
    {
        public const string SpiderTag = "# Managed by SpiderWorker";

        public DNSEntry(string domain, string ip)
        {
            HostName = domain;
            IPAddress = ip;
        }
        
        public string IPAddress { get; set; }
        public string HostName { get; set; }

        public override string ToString() => $"{IPAddress} {HostName} {SpiderTag}\r\n";
        public override int GetHashCode() => IPAddress.GetHashCode() ^ HostName.GetHashCode();
        public override bool Equals(object? obj)
        {
            if (obj == null)
                return false;

            var other = obj as DNSEntry;
            if (other is null)
                return false;

            return HostName == other.HostName && IPAddress.Equals(other.IPAddress);
        }

        public static bool operator ==(DNSEntry a, DNSEntry b)
            => a?.Equals(b) ?? false;

        public static bool operator !=(DNSEntry a, DNSEntry b)
            => !(a?.Equals(b) ?? false);
    }
}

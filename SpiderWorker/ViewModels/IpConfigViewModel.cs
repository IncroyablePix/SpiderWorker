using ReactiveUI;
using SpiderWorker.Models;
using SpiderWorker.Services.IPConfig;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reactive;
using System.Text;
using System.Threading.Tasks;

namespace SpiderWorker.ViewModels
{
    public class IpConfigViewModel : ReactiveObject, IRoutableViewModel, INotifyPropertyChanged
    {
        private NetworkInterface _currentNetworkInterface;
        private IIPConfigurator _ipConfigurator;
        public string? UrlPathSegment => Guid.NewGuid().ToString().Substring(0, 5);
        public List<NetworkInterface> NetworkInterfaces { get; }

        public ReactiveCommand<Unit, Unit> ApplyIpConfigCommand { get; }

        //---
        public string IPv4Address { get => _currentNetworkInterface.IP; set => _currentNetworkInterface.IP = value; }
        public string SubnetMask { get => _currentNetworkInterface.SubnetMask; set => _currentNetworkInterface.SubnetMask = value; }
        public string DefaultGateway { get => _currentNetworkInterface.Gateway; set => _currentNetworkInterface.Gateway = value; }
        public string PreferredDNS { get => _currentNetworkInterface.PreferredDNS; set => _currentNetworkInterface.PreferredDNS = value; }
        public string AlternateDNS { get => _currentNetworkInterface.AlternateDNS; set => _currentNetworkInterface.AlternateDNS = value; }
        public bool IsDhcpEnabled 
        { 
            get => _currentNetworkInterface.IsDHCP; 
            set
            {
                _currentNetworkInterface.IsDHCP = value;
                OnPropertyChanged(nameof(IsDhcpDisabled));
            }
        }
        public bool IsDhcpDisabled => !IsDhcpEnabled;
        public NetworkInterface SelectedNetworkInterface
        {
            get => _currentNetworkInterface;
            set
            {
                _currentNetworkInterface = value;
                OnPropertyChanged(nameof(IPv4Address));
                OnPropertyChanged(nameof(SubnetMask));
                OnPropertyChanged(nameof(DefaultGateway));
                OnPropertyChanged(nameof(PreferredDNS));
                OnPropertyChanged(nameof(AlternateDNS));
                OnPropertyChanged(nameof(IsDhcpEnabled));
                OnPropertyChanged(nameof(IsDhcpDisabled));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public IScreen HostScreen { get; }

        public IpConfigViewModel(IScreen screen, IIPConfigurator ipConfigurator)
        {
            HostScreen = screen;
            NetworkInterfaces = ipConfigurator.GetNetworkInterfaces().ToList();
            _ipConfigurator = ipConfigurator;
            _currentNetworkInterface = NetworkInterfaces.FirstOrDefault();
            ApplyIpConfigCommand = ReactiveCommand.Create(ApplyIpConfig);
        }

        private void OnPropertyChanged(string propertyName) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        private void ApplyIpConfig()
        {
            _ipConfigurator.ApplyConfiguration(_currentNetworkInterface);
        }
    }
}

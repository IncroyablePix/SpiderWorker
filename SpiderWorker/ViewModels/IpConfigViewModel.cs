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
        private IConfigurationsProvider _configurationsProvider;
        public string? UrlPathSegment => Guid.NewGuid().ToString().Substring(0, 5);
        public List<NetworkInterface> NetworkInterfaces { get; }

        public ReactiveCommand<Unit, Unit> ApplyIpConfigCommand { get; }

        //---
        public string IPv4Address { get => _currentNetworkInterface.Configuration.IPv4.IP; set => _currentNetworkInterface.Configuration.IPv4.IP = value; }
        public string SubnetMask { get => _currentNetworkInterface.Configuration.IPv4.SubnetMask; set => _currentNetworkInterface.Configuration.IPv4.SubnetMask = value; }
        public string DefaultGateway { get => _currentNetworkInterface.Configuration.IPv4.Gateway; set => _currentNetworkInterface.Configuration.IPv4.Gateway = value; }
        public string PreferredDNS { get => _currentNetworkInterface.Configuration.IPv4.PreferredDNS; set => _currentNetworkInterface.Configuration.IPv4.PreferredDNS = value; }
        public string AlternateDNS { get => _currentNetworkInterface.Configuration.IPv4.AlternateDNS; set => _currentNetworkInterface.Configuration.IPv4.AlternateDNS = value; }
        public bool IsDhcpEnabled 
        { 
            get => _currentNetworkInterface.Configuration.IPv4.IsDHCP; 
            set
            {
                _currentNetworkInterface.Configuration.IPv4.IsDHCP = value;
                if (!value)
                {
                    IsDnsDhcpEnabled = false;
                    OnPropertyChanged(nameof(IsDnsDhcpEnabled));
                    OnPropertyChanged(nameof(IsDnsDhcpDisabled));
                }

                OnPropertyChanged(nameof(IsDhcpDisabled));
                OnPropertyChanged(nameof(CanEnableDnsDhcp));
            }
        }

        public bool CanEnableDnsDhcp => IsDhcpEnabled;
        public bool IsDhcpDisabled => !IsDhcpEnabled;

        public bool IsDnsDhcpEnabled
        {
            get => _currentNetworkInterface.Configuration.IPv4.AutoDNS;
            set
            {
                _currentNetworkInterface.Configuration.IPv4.AutoDNS = value;
                OnPropertyChanged(nameof(IsDnsDhcpDisabled));
            }
        }
        public bool IsDnsDhcpDisabled => !IsDnsDhcpEnabled;
        
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
                OnPropertyChanged(nameof(IsDnsDhcpEnabled));
                OnPropertyChanged(nameof(IsDnsDhcpDisabled));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public IScreen HostScreen { get; }

        public IpConfigViewModel(IScreen screen, IIPConfigurator ipConfigurator, IConfigurationsProvider configurationsProvider)
        {
            HostScreen = screen;
            NetworkInterfaces = ipConfigurator.GetNetworkInterfaces().ToList();
            _ipConfigurator = ipConfigurator;
            _configurationsProvider = configurationsProvider;
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

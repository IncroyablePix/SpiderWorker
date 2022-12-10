using ReactiveUI;
using SpiderWorker.Models;
using SpiderWorker.Services.IPConfig;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        public ObservableCollection<InterfaceConfiguration> InterfacesConfigurations { get; }

        public ReactiveCommand<Unit, Unit> ApplyIpConfigCommand { get; }
        public ReactiveCommand<Unit, Unit> AddCurrentConfigurationCommand { get; }
        public ReactiveCommand<InterfaceConfiguration, Unit> DeleteConfigurationCommand { get; }
        public ReactiveCommand<InterfaceConfiguration, Unit> ApplyConfigurationCommand { get; }

        //---
        public string IPv4Address { get => _currentNetworkInterface.Configuration?.IPv4?.IP ?? string.Empty; set => _currentNetworkInterface.Configuration.IPv4.IP = value; }
        public string SubnetMask { get => _currentNetworkInterface.Configuration?.IPv4?.SubnetMask ?? string.Empty; set => _currentNetworkInterface.Configuration.IPv4.SubnetMask = value; }
        public string DefaultGateway { get => _currentNetworkInterface.Configuration?.IPv4?.Gateway ?? string.Empty; set => _currentNetworkInterface.Configuration.IPv4.Gateway = value; }
        public string PreferredDNS { get => _currentNetworkInterface.Configuration?.IPv4?.PreferredDNS ?? string.Empty; set => _currentNetworkInterface.Configuration.IPv4.PreferredDNS = value; }
        public string AlternateDNS { get => _currentNetworkInterface.Configuration?.IPv4.AlternateDNS ?? string.Empty; set => _currentNetworkInterface.Configuration.IPv4.AlternateDNS = value; }
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
            InterfacesConfigurations = new ObservableCollection<InterfaceConfiguration>(configurationsProvider.ReadConfigurations().ToList());
            _ipConfigurator = ipConfigurator;
            _configurationsProvider = configurationsProvider;
            _currentNetworkInterface = NetworkInterfaces.FirstOrDefault();
            ApplyIpConfigCommand = ReactiveCommand.Create(ApplyIpConfig);
            AddCurrentConfigurationCommand = ReactiveCommand.Create(AddCurrentConfiguration);
            DeleteConfigurationCommand = ReactiveCommand.Create<InterfaceConfiguration>(DeleteConfiguration);
            ApplyConfigurationCommand = ReactiveCommand.Create<InterfaceConfiguration>(ApplyConfiguration);
        }

        private void OnPropertyChanged(string propertyName) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        private void ApplyIpConfig()
        {
            _ipConfigurator.ApplyConfiguration(_currentNetworkInterface);
        }

        private void AddCurrentConfiguration()
        {
            var ifConfig = new InterfaceConfiguration
            {
                IPv4 = new IPConfiguration
                {
                    IP = IPv4Address,
                    SubnetMask = SubnetMask,
                    Gateway = DefaultGateway,
                    PreferredDNS = PreferredDNS,
                    AlternateDNS = AlternateDNS,
                    IsDHCP = IsDhcpEnabled,
                    AutoDNS = IsDnsDhcpEnabled
                }
            };

            ifConfig.Name = ifConfig.GetDefaultName();

            InterfacesConfigurations.Add(ifConfig);
            _configurationsProvider.WriteConfigurations(InterfacesConfigurations);
            OnPropertyChanged(nameof(InterfacesConfigurations));
        }

        private void ApplyConfiguration(InterfaceConfiguration ifConfig)
        {
            _currentNetworkInterface.Configuration.Copy(ifConfig);
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

        private void DeleteConfiguration(InterfaceConfiguration ifConfig)
        {
            InterfacesConfigurations.Remove(ifConfig);
            _configurationsProvider.WriteConfigurations(InterfacesConfigurations);
            OnPropertyChanged(nameof(InterfacesConfigurations));
        }
    }
}

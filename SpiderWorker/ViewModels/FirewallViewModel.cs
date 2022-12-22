using ReactiveUI;
using SpiderWorker.Models;
using SpiderWorker.Services;
using SpiderWorker.Services.Firewall;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpiderWorker.ViewModels
{
    public class FirewallViewModel : ReactiveObject, IRoutableViewModel, INotifyPropertyChanged
    {
        public string? UrlPathSegment => Guid.NewGuid().ToString().Substring(0, 5);

        public bool IsFirewallEnabled 
        {
            get
            {
                return FirewallManager.IsFirewallEnabled();
            }

            set
            {
                FirewallManager.ToggleFirewall(value);
                OnPropertyChanged(nameof(IsFirewallEnabled));
            }
        }

        public IScreen HostScreen { get; }
        private IFirewallManager FirewallManager { get; set; }
        public FirewallViewModel(IScreen screen, IFirewallManager firewallManager)
        {
            HostScreen = screen;
            FirewallManager = firewallManager;
        }
        
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}

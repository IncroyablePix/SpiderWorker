using Avalonia.Controls;
using Avalonia.Input;
using DynamicData;
using ReactiveUI;
using SpiderWorker.Models;
using SpiderWorker.Services;
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
    public class DnsViewModel : ReactiveObject, IRoutableViewModel, INotifyPropertyChanged
    {
        public string? UrlPathSegment => Guid.NewGuid().ToString().Substring(0, 5);

        public IScreen HostScreen { get; }
        public IDnsManagerService DnsManagerService { get; set; }
        public ObservableCollection<DNSEntry> Entries { get; private set; }
        public ReactiveCommand<Unit, Unit> AddNewEntryCommand { get; }
        public ReactiveCommand<DNSEntry, Unit> DeleteEntryCommand { get; }
        public ReactiveCommand<Unit, Unit> SaveCommand { get; }
        public ReactiveCommand<SelectionChangedEventArgs, Unit> SelectionChangedCommand;

        public DnsViewModel(IScreen screen, IDnsManagerService dnsManagerService)
        {
            HostScreen = screen;
            DnsManagerService = dnsManagerService;
            Entries = new ObservableCollection<DNSEntry>(DnsManagerService);
            AddNewEntryCommand = ReactiveCommand.Create(AddEntry);
            DeleteEntryCommand = ReactiveCommand.Create<DNSEntry>(DeleteEntry);
            SaveCommand = ReactiveCommand.Create(Save);
            SelectionChangedCommand = ReactiveCommand.Create<SelectionChangedEventArgs>(SelectionChanged);
        }

        private void SelectionChanged(SelectionChangedEventArgs e)
        {
            Console.WriteLine();
        }

        private void AddEntry()
        {
            var entry = new DNSEntry("localhost", "127.0.0.1");
                        
            if (DnsManagerService.AddEntry(entry))
                Entries.Add(entry);
        }

        private void DeleteEntry(DNSEntry entry)
        {
            if (DnsManagerService.RemoveEntry(entry))
                Entries.Remove(entry);
        }

        private void Save()
        {
            DnsManagerService.Update();
            Entries.Clear();
            Entries.AddRange(DnsManagerService);
            
        }
    }
}

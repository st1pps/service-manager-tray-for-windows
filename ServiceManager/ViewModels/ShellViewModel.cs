using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.ServiceProcess;
using System.Windows;
using System.Windows.Data;
using Caliburn.Micro;
using ServiceManager.Events;
using ServiceManager.Tray;
using ServiceManager.Util;

namespace ServiceManager.ViewModels
{
    public sealed class ShellViewModel : Conductor<ServiceViewModel>.Collection.OneActive, INotifyIconShell, IHandle<ShutdownRequest>
    {
        private bool _showFavoritesOnly;
        private string _searchText;
        private readonly IAppSettings _settings;

        public ShellViewModel(IAppSettings settings)
        {
            DisplayName = "Service Manager";
            _settings = settings;
            Activated += (s,e) => RefreshServices();
        }


        public void RefreshServices()
        {
            var services = ServiceController.GetServices();
            foreach (var s in services)
            {
                Items.Add(new ServiceViewModel(s, _settings));
            }

            ServiceView = CollectionViewSource.GetDefaultView(Items);
        }

        protected override ServiceViewModel DetermineNextItemToActivate(IList<ServiceViewModel> list, int lastIndex)
        {
            if (ServiceView.IsEmpty)
            {
                return null;
            }

            var newIndex = lastIndex + 1;
            if (newIndex >= list.Count)
            {
                newIndex = newIndex - 2;
            }

            return list[newIndex];
        }

        public ICollectionView ServiceView { get; private set; }

        public string SearchText
        {
            get => _searchText;
            set
            {
                _searchText = value.Trim();
                RefreshFilter();
            }
        }

        public void OpenWindowsServiceManager()
        {
            Process.Start("services.msc");
        }

        public void OpenTaskManager()
        {
            Process.Start("taskmgr.exe");
        }

        public void Exit()
        {
            Application.Current.Shutdown(0);
        }

        public bool ShowFavoritesOnly
        {
            get => _showFavoritesOnly;
            set
            {
                _showFavoritesOnly = value;
                RefreshFilter();
            }
        }

        private void RefreshFilter()
        {
            if (!_showFavoritesOnly && string.IsNullOrWhiteSpace(SearchText))
            {
                ServiceView.Filter = null;
                return;
            }
            
            if (_showFavoritesOnly)
            {
                ServiceView.Filter = f =>
                {
                    if (!(f is ServiceViewModel vm)) return false;
                    if (!vm.Favorite) return false;
                    return string.IsNullOrWhiteSpace(SearchText) || vm.DisplayName.ContainsIgnoreCase(SearchText);
                };
            }
            else
            {
                ServiceView.Filter = f =>
                {
                    if (!(f is ServiceViewModel vm)) return false;
                    return vm.DisplayName.ContainsIgnoreCase(SearchText);
                };
            }

            NotifyOfPropertyChange(nameof(ServiceView));
        }

        public void Handle(ShutdownRequest message) => Exit();
    }
}

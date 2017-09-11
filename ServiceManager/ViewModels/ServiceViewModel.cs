using System;
using System.Management;
using Caliburn.Micro;
using System.ServiceProcess;
using ServiceManager.Util;

namespace ServiceManager.ViewModels
{
    public class ServiceViewModel : PropertyChangedBase
    {
        private readonly ServiceController _serviceController;
        private readonly IAppSettings _settings;
        private bool _favorite;
        private string _description;

        public ServiceViewModel(ServiceController serviceController, IAppSettings settings)
        {
            _serviceController = serviceController;
            _settings = settings;
            Description = GetDescription();
        }

        public bool Favorite
        {
            get => _favorite;
            set { _favorite = value; NotifyOfPropertyChange(); }
        }

        public string Description
        {
            get => _description;
            private set
            {
                _description = value;
                NotifyOfPropertyChange();
            }
        }

        public ServiceControllerStatus Status => _serviceController.Status;

        public string DisplayName => _serviceController.DisplayName;

        public void StartStop()
        {
            ServiceControllerStatus waitFor;
            switch (_serviceController.Status)
            {
                case ServiceControllerStatus.Stopped:
                    _serviceController.Start();
                    waitFor = ServiceControllerStatus.Running;
                    break;
                default:
                    _serviceController.Stop();
                    waitFor = ServiceControllerStatus.Stopped;
                    break;
            }
            _serviceController.WaitForStatus(waitFor, TimeSpan.FromSeconds(10));
            _serviceController.Refresh();
            NotifyOfPropertyChange(nameof(Status));
        }
        
        public bool CanStartStop
        {
            get
            {
                switch (_serviceController.Status)
                {
                    case ServiceControllerStatus.Running when _serviceController.CanStop:
                        return true;
                    case ServiceControllerStatus.Stopped:
                        return true;
                }

                return false;
            }
        }

        private string GetDescription()
        {
            var wmiService = new ManagementObject($"Win32_Service.Name='{_serviceController.ServiceName}'");
            wmiService.Get();
            var d = wmiService["Description"]?.ToString() ?? "";
            if (d == DisplayName) d = "";
            return d;
        }
    }
}

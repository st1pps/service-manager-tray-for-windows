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
        private string _status;
        private bool _isRunning;

        public ServiceViewModel(ServiceController serviceController, IAppSettings settings)
        {
            _serviceController = serviceController;
            _settings = settings;
            Description = GetDescription();
            Status = _serviceController.Status.ToString();
            IsRunning = Status == "Running";
        }

        public bool IsRunning
        {
            get => _isRunning;
            set
            {
                _isRunning = value;
                NotifyOfPropertyChange();
            }
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

        public string Status
        {
            get => _status;
            set
            {
                _status = value;
                NotifyOfPropertyChange();
            }
        }

        public string DisplayName => _serviceController.DisplayName;

        public void StartStop()
        {
            ServiceControllerStatus waitFor;
            bool setIsRunningTo;
            switch (_serviceController.Status)
            {
                case ServiceControllerStatus.Stopped:
                    _serviceController.Start();
                    waitFor = ServiceControllerStatus.Running;
                    setIsRunningTo = true;
                    break;
                default:
                    _serviceController.Stop();
                    waitFor = ServiceControllerStatus.Stopped;
                    setIsRunningTo = false;
                    break;
            }
            _serviceController.WaitForStatus(waitFor, TimeSpan.FromSeconds(10));
            _serviceController.Refresh();
            _isRunning = setIsRunningTo;
            Status = _serviceController.Status.ToString();
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

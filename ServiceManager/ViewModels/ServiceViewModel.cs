using System.Management;
using Caliburn.Micro;
using System.ServiceProcess;

namespace ServiceManager.ViewModels
{
    public class ServiceViewModel : PropertyChangedBase
    {
        private readonly ServiceController _serviceController;
        private bool _favorite;
        private string _description;

        public ServiceViewModel(ServiceController serviceController)
        {
            _serviceController = serviceController;
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

        public string DisplayName => _serviceController.DisplayName;

        private string GetDescription()
        {
            var wmiService = new ManagementObject($"Win32_Service.Name='{_serviceController.ServiceName}'");
            wmiService.Get();
            return wmiService["Description"]?.ToString() ?? "";
        }
    }
}

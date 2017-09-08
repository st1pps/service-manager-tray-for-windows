using System.Diagnostics;
using System.ServiceProcess;
using System.Windows;
using Caliburn.Micro;

namespace ServiceManager.ViewModels
{
    public sealed class ShellViewModel : Conductor<ServiceViewModel>.Collection.OneActive
    {
        public ShellViewModel()
        {
            DisplayName = "Service Manager";
            Activated += (s,e) => RefreshServices();
        }


        public void RefreshServices()
        {
            var services = ServiceController.GetServices();
            foreach (var s in services)
            {
                Items.Add(new ServiceViewModel(s));
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
        
    }
}

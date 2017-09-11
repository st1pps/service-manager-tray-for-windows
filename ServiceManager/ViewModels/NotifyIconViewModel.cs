using System.Windows.Input;
using Caliburn.Micro;
using ServiceManager.Tray;
using ServiceManager.Util;

namespace ServiceManager.ViewModels
{
    public class NotifyIconViewModel : Conductor<NotifyIconMenuEntryViewModel>.Collection.AllActive
    {
        private readonly IWindowManager _windowManager;
        private readonly INotifyIconShell _shell;

        public NotifyIconViewModel(IWindowManager windowManager, INotifyIconShell shell)
        {
            _windowManager = windowManager;
            _shell = shell;
        }

        public ICommand DoubleClickCommand => new DelegateCommand(o =>
        {
            _windowManager.ShowWindow(_shell);
        });
    }
}

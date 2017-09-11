using Caliburn.Micro;
using ServiceManager.Events;

namespace ServiceManager.Tray
{
    public interface INotifyIconService : IHandle<RegisterNotifyIconMenuEntry>,
        IHandle<UiMessage>,
        IHandle<UnregisterNotifyIconMenuEntry>
    {
        void Initialize();
    }
}

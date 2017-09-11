namespace ServiceManager.Events
{
    public class UnregisterNotifyIconMenuEntry : IEvent
    {
        public UnregisterNotifyIconMenuEntry(string menuTitle)
        {
            MenuTitle = menuTitle;
        }

        public string MenuTitle { get; }
    }
}

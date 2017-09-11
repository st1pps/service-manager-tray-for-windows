namespace ServiceManager.Events
{
    public class RegisterNotifyIconMenuEntry : IEvent
    {
        public RegisterNotifyIconMenuEntry(object eventToTrigger, string menuTitle)
        {
            EventToTrigger = eventToTrigger;
            MenuTitle = menuTitle;
        }

        public object EventToTrigger { get; }

        public string MenuTitle { get; }
    }
}

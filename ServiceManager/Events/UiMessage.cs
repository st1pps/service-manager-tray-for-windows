namespace ServiceManager.Events
{
    public class UiMessage : IEvent
    {
        public UiMessage(string message, Severity severy)
        {
            Message = message;
            MessageSeverity = severy;
        }

        public enum Severity
        {
            Info,

            Warn,

            Error
        }

        public string Message { get; }

        public Severity MessageSeverity { get; }
    }
}
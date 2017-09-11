using Caliburn.Micro;

namespace ServiceManager.ViewModels
{
    public class UiMessageViewModel : Screen
    {
        private string _title;
        private string _message;

        public string Title
        {
            get => _title;

            set
            {
                _title = value;
                NotifyOfPropertyChange(() => Title);
            }
        }

        public string Message
        {
            get => _message;

            set
            {
                _message = value;
                NotifyOfPropertyChange(() => Message);
            }
        }
    }
}

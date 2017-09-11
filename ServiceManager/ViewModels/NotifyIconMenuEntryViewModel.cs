using System;
using System.Windows.Input;
using Caliburn.Micro;
using Serilog;
using ServiceManager.Events;
using ServiceManager.Tray;
using ServiceManager.Util;

namespace ServiceManager.ViewModels
{
    public class NotifyIconMenuEntryViewModel : PropertyChangedBase
    {
        private readonly object _eventInstance;

        private readonly IEventAggregator _events;

        private string _text;

        private readonly ILogger _log = Log.ForContext<NotifyIconMenuEntryViewModel>();

        public NotifyIconMenuEntryViewModel(string text, IEventAggregator events, object eventInstance)
        {
            _events = events;
            _eventInstance = eventInstance;
            Text = text;
        }

        public string Text
        {
            get => _text;

            set
            {
                _text = value;
                NotifyOfPropertyChange(nameof(Text));
            }
        }

        public ICommand Command => new DelegateCommand(o =>
        {
            try
            {
                _events.PublishOnUIThread(_eventInstance);
            }
            catch (Exception ex)
            {
                _log.Error("Error on executing notifyicon {text} menu command", ex, _text);
                _events.PublishOnUIThread(new UiMessage("Fehler beim Ausführen des Befehls", UiMessage.Severity.Error));
            }
        });
    }
}
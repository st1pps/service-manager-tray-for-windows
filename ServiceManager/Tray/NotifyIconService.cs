// <copyright file="NotifyIconService.cs" company="Pharmalink Extracts Europe GmbH">
// Copyright (c) Pharmalink Extracts Europe GmbH. All rights reserved.
// </copyright>

using System;
using System.Linq;
using System.Windows;
using System.Windows.Threading;
using Caliburn.Micro;
using Hardcodet.Wpf.TaskbarNotification;
using Serilog;
using ServiceManager.Events;
using ServiceManager.ViewModels;

namespace ServiceManager.Tray
{
    public class NotifyIconService : DispatcherObject, INotifyIconService
    {
        private readonly IEventAggregator _events;

        private readonly IWindowManager _mainWindowManager;

        private readonly INotifyIconShell _shell;

        private TaskbarIcon _icon;

        private NotifyIconViewModel _iconViewModel;

        private readonly ILogger _log = Log.ForContext<NotifyIconService>();

        public NotifyIconService(IEventAggregator events, IWindowManager mainWindowManager, INotifyIconShell shell)
        {
            _events = events;
            _mainWindowManager = mainWindowManager;
            _events.Subscribe(this);
            _shell = shell;
            _log.Debug("NotifyIconService constructed.");
        }

        public bool IsInitialized { get; private set; }

        public string ServiceName => nameof(INotifyIconService);

        public void Initialize()
        {
            if (!IsInitialized)
            {
                _icon = Application.Current.Resources["NotifyIcon"] as TaskbarIcon;

                if (_icon == null)
                {
                    _log.Error("Unable to create TaskbarIcon instance");
                    throw new NullReferenceException("TaskbarIcon resource is null");
                }

                _iconViewModel = new NotifyIconViewModel(_mainWindowManager, _shell);
                _icon.DataContext = _iconViewModel;
                RegisterStandardTaskbarMenuEntries();
                IsInitialized = true;
            }
        }

        public void Handle(RegisterNotifyIconMenuEntry message)
        {
            _iconViewModel.Items.Add(new NotifyIconMenuEntryViewModel(message.MenuTitle, _events, message.EventToTrigger));
        }

        public void Handle(UiMessage message)
        {
            string title;
            BalloonIcon balloonIcon;
            switch (message.MessageSeverity)
            {
                case UiMessage.Severity.Info:
                    title = "Info";
                    balloonIcon = BalloonIcon.Info;
                    break;
                case UiMessage.Severity.Warn:
                    title = "Warning";
                    balloonIcon = BalloonIcon.Warning;
                    break;
                case UiMessage.Severity.Error:
                    title = "ERROR";
                    balloonIcon = BalloonIcon.Error;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            Dispatcher.Invoke(() => { _icon.ShowBalloonTip(title, message.Message, balloonIcon); });
        }

        public void Handle(UnregisterNotifyIconMenuEntry message)
        {
            var toremove = _iconViewModel.Items.FirstOrDefault(e => e.Text == message.MenuTitle);
            _iconViewModel.Items.Remove(toremove);
        }

        private void RegisterStandardTaskbarMenuEntries()
        {
            _iconViewModel.Items.Add(new NotifyIconMenuEntryViewModel("Exit", _events, new ShutdownRequest()));
        }
    }
}
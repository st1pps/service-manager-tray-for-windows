using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using Autofac;
using Caliburn.Micro;
using ServiceManager.ViewModels;
using IContainer = Autofac.IContainer;

namespace ServiceManager
{
    public class AppBootstrapper : BootstrapperBase
    {
        private IContainer _container;

        public AppBootstrapper()
        {
            Initialize();
        }

        protected override void Configure()
        {
            var builder = new ContainerBuilder();

            // Register ViewModels
            builder.RegisterAssemblyTypes(AssemblySource.Instance.ToArray())
                .Where(type => type.Name.EndsWith("ViewModel"))
                .Where(type => (!string.IsNullOrEmpty(type.Namespace)) && type.Namespace.EndsWith("ViewModels"))
                .Where(type => type.GetInterface(nameof(INotifyPropertyChanged), false) != null)
                .AsSelf()
                .InstancePerDependency();

            // Register Views
            builder.RegisterAssemblyTypes(AssemblySource.Instance.ToArray())
                .Where(type => type.Name.EndsWith("View"))
                .Where(type => !string.IsNullOrEmpty(type.Namespace) && type.Namespace.EndsWith("Views"))
                .AsSelf()
                .InstancePerDependency();

            builder.RegisterInstance<IWindowManager>(new WindowManager());
            builder.RegisterInstance<IEventAggregator>(new EventAggregator());
            

            _container = builder.Build();
        }

        protected override object GetInstance(Type service, string key)
        {
            object instance;
            if (string.IsNullOrWhiteSpace(key))
            {
                if (_container.TryResolve(service, out instance))
                    return instance;
            }
            else
            {
                if (_container.TryResolveNamed(key, service, out instance))
                    return instance;
            }
            throw new Exception($"Could not locate any instances of contract {key ?? service.Name}.");
        }

        protected override IEnumerable<object> GetAllInstances(Type service)
        {
            return _container.Resolve(typeof(IEnumerable<>).MakeGenericType(service)) as IEnumerable<object>;
        }

        protected override void BuildUp(object instance)
        {
            _container.InjectProperties(instance);
        }

        protected override void OnStartup(object sender, StartupEventArgs e)
        {
            DisplayRootViewFor<ShellViewModel>();
        }
    }
}

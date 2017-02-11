using System;
using System.Collections.Generic;
using System.Windows;
using Bootstrap.Ninject;
using Caliburn.Micro;
using Ninject;
using Scrubber.Helpers;
using Scrubber.Model.Maintenance.Shell.ViewModels;
using Scrubber.Workers;

namespace Scrubber.Model.Bootstrapper
{
    public class AppBootstrapper : BootstrapperBase
    {
        private IKernel _kernel;

        public AppBootstrapper()
        {
            Initialize();
        }

        protected override void OnExit(object sender, EventArgs e)
        {
            _kernel.Dispose();
            base.OnExit(sender, e);
        }

        protected override object GetInstance(Type service, string key)
        {
            if (service == null)
                throw new ArgumentNullException(nameof(service));

            return _kernel.Get(service);
        }

        protected override IEnumerable<object> GetAllInstances(Type service)
        {
            return _kernel.GetAll(service);
        }

        protected override void BuildUp(object instance)
        {
            _kernel.Inject(instance);
        }

        protected override void OnStartup(object sender, StartupEventArgs e)
        {
            _kernel = new StandardKernel();

            _kernel.Bind<IWindowManager>().To<WindowManager>().InSingletonScope();
            _kernel.Bind<IEventAggregator>().To<EventAggregator>().InSingletonScope();
            _kernel.Bind<IShell>().To<ShellViewModel>().InSingletonScope();

            Bootstrap.Bootstrapper
                .Including.Assembly(typeof(AppBootstrapper).Assembly)
                .AndAssembly(typeof(Soap).Assembly)
                .With.Ninject().WithContainer(_kernel).Start();

            DisplayRootViewFor<IShell>();
        }
    }
}
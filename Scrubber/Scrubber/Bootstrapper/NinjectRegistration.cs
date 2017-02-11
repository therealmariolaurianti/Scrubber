using Bootstrap.Ninject;
using Ninject;
using Ninject.Extensions.Conventions;
using NLog;
using Scrubber.Factories;

namespace Scrubber.Bootstrapper
{
    public class NinjectRegistration : INinjectRegistration
    {
        public void Register(IKernel container)
        {
            container.Bind(x => x.FromThisAssembly()
                .SelectAllInterfaces().InheritedFrom<IFactory>()
                .BindToFactory());

            var logger = LogManager.GetLogger("Log");
            container.Bind<Logger>().ToConstant(logger).InSingletonScope();
        }
    }
}
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
            var logger = LogManager.GetLogger("Log");
            container.Bind<Logger>().ToConstant(logger).InSingletonScope();

            container.Bind(x => x.FromThisAssembly()
                .SelectAllInterfaces().InheritedFrom<IFactory>()
                .BindToFactory());
        }
    }
}
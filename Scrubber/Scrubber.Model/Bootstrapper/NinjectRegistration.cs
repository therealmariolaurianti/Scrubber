using Bootstrap.Ninject;
using Ninject;
using Ninject.Extensions.Conventions;
using Scrubber.Factories;

namespace Scrubber.Model.Bootstrapper
{
    public class NinjectRegistration : INinjectRegistration
    {
        public void Register(IKernel container)
        {
            container.Bind<UserSettings>().ToSelf().InSingletonScope();

            container.Bind(x => x.FromThisAssembly()
              .SelectAllInterfaces().InheritedFrom<IFactory>()
              .BindToFactory());
        }
    }
}
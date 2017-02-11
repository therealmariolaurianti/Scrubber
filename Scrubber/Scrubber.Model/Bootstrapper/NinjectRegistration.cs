using Bootstrap.Ninject;
using Ninject;

namespace Scrubber.Model.Bootstrapper
{
    public class NinjectRegistration : INinjectRegistration
    {
        public void Register(IKernel container)
        {
            container.Bind<UserSettings>().ToSelf().InSingletonScope();
        }
    }
}
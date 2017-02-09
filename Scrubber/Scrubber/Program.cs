using System.Windows.Forms;
using Bootstrap.Ninject;
using Ninject;
using Scrubber.Helpers;
using Scrubber.Workers;
using Bootstrapper = Bootstrap.Bootstrapper;

namespace Scrubber
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Bootstrapper.With.Ninject().Start();
            var container = (IKernel) Bootstrapper.Container;

            container.Bind<Options>().ToSelf().InSingletonScope();
            var bathtub = container.Get<Bathtub>();

            bathtub.Fill();
            bathtub.Rinse();
            var result = bathtub.Drain();
            MessageBox.Show(result.Success
                ? $"Operation Completed. {result.ResultValue.Count} Cleaned."
                : "Operation Failed.");
        }
    }
}
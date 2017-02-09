using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Bootstrap;
using Bootstrap.Ninject;
using Ninject;
using Ninject.Parameters;

namespace Scrubber
{
    class Program
    {
        static void Main(string[] args)
        {
            Bootstrapper.With.Ninject().Start();
            IKernel container = (IKernel)Bootstrapper.Container;

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

    public class Options
    {
    }

    public class Bathtub
    {
    }
}

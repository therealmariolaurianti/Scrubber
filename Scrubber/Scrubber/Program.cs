using System.Collections.Generic;
using System.Windows.Forms;
using Bootstrap;
using Bootstrap.Ninject;
using CommandLine;
using Ninject;
using Scrubber.Helpers;
using Scrubber.Interfaces;
using Scrubber.Objects;
using Scrubber.Workers;

namespace Scrubber
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var parsedOptions = Parser.Default.ParseArguments<Options>(args);

            Bootstrapper.With.Ninject().Start();
            var container = (IKernel) Bootstrapper.Container;

            parsedOptions.MapResult(options =>
            {
                container.Bind<IOptions>().ToConstant(options).InSingletonScope();
                var bathtub = container.Get<Bathtub>();

                bathtub.Fill();
                bathtub.Rinse();

                var result = bathtub.Drain();
                bathtub.FinishScrub(result);

                return 0;
            }, error => -1);
        }
    }
}
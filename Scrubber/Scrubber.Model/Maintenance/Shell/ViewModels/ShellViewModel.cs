using Caliburn.Micro;
using Scrubber.Helpers;
using Scrubber.Workers;

namespace Scrubber.Model.Maintenance.Shell.ViewModels
{
    public class ShellViewModel : PropertyChangedBase, IShell
    {
        public void RunScrubber()
        {
            var bathtub = IoC.Get<Bathtub>();

            bathtub.Fill();
            bathtub.Rinse();

            var result = bathtub.Drain();
            result.DisplayResult();
        }
    }

    public interface IShell
    {
    }
}
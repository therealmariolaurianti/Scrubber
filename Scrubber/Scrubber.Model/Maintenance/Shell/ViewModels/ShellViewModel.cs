using Caliburn.Micro;
using Scrubber.Factories;
using Scrubber.Helpers;

namespace Scrubber.Model.Maintenance.Shell.ViewModels
{
    public class ShellViewModel : PropertyChangedBase, IShell
    {
        private readonly IBathtubFactory _bathtubFactory;

        public ShellViewModel(IBathtubFactory bathtubFactory)
        {
            _bathtubFactory = bathtubFactory;
        }

        public void RunScrubber()
        {
            var bathtub = _bathtubFactory.Create(FolderPath);

            bathtub.Fill();
            bathtub.Rinse();

            var result = bathtub.Drain();
            result.DisplayResult();
        }

        public string FolderPath { get; set; }
    }

    public interface IShell
    {
    }
}
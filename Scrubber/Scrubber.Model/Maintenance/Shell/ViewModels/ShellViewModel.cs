using System.Windows;
using System.Windows.Input;
using Caliburn.Micro;
using Scrubber.Factories;
using Scrubber.Helpers;

namespace Scrubber.Model.Maintenance.Shell.ViewModels
{
    public class ShellViewModel : PropertyChangedBase, IShell
    {
        private readonly IBathtubFactory _bathtubFactory;
        private string _folderPath;

        public ShellViewModel(IBathtubFactory bathtubFactory)
        {
            _bathtubFactory = bathtubFactory;
        }

        public string FolderPath
        {
            get { return _folderPath; }
            set
            {
                if (value == _folderPath) return;
                _folderPath = value;
                NotifyOfPropertyChange();
            }
        }

        public ICommand ScrubCommand => new DelegateCommand(RunScrubber);

        public void RunScrubber()
        {
            if (string.IsNullOrEmpty(FolderPath))
            {
                MessageBox.Show("Enter path.");
                return;
            }

            var bathtub = _bathtubFactory.Create(FolderPath);

            bathtub.Fill();
            bathtub.Rinse();

            var result = bathtub.Drain();
            result.DisplayResult();
        }
    }
}
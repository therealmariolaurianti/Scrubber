using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Caliburn.Micro;
using Scrubber.Factories;
using Scrubber.Helpers;

namespace Scrubber.Model.Maintenance.Shell.ViewModels
{
    public class ShellViewModel : Screen, IShell
    {
        private readonly IBathtubFactory _bathtubFactory;
        private readonly UserSettings _userSettings;
        private string _folderPath;
        private bool _isLoading;
        private bool _clearComments;

        public ShellViewModel(IBathtubFactory bathtubFactory, UserSettings userSettings)
        {
            _bathtubFactory = bathtubFactory;
            _userSettings = userSettings;

            _userSettings.Load(this);
        }

        public ICommand ScrubCommand => new DelegateCommand(RunScrubber);

        protected override void OnActivate()
        {
            DisplayName = "Scrubber";

            base.OnActivate();
        }

        public void RunScrubber()
        {
            if (string.IsNullOrEmpty(FolderPath))
            {
                MessageBox.Show("Enter path.");
                return;
            }
            Task.Run(() =>
                {
                    IsLoading = true;

                    var bathtub = _bathtubFactory.Create(FolderPath, ClearComments);

                    bathtub.Fill();
                    bathtub.Rinse();

                    var result = bathtub.Drain();
                    result.DisplayResult();
                }).GetAwaiter()
                .OnCompleted(() => { IsLoading = false; });
        }

        public bool ClearComments
        {
            get { return _clearComments; }
            set
            {
                if (value == _clearComments) return;
                _clearComments = value;
                NotifyOfPropertyChange();

                _userSettings?.SaveSingle(nameof(ClearComments), value);
            }
        }

        public void Close()
        {
            TryClose();
        }

        public string FolderPath
        {
            get { return _folderPath; }
            set
            {
                if (value == _folderPath) return;
                _folderPath = value;
                NotifyOfPropertyChange();

                _userSettings?.SaveSingle(nameof(FolderPath), value);
            }
        }

        public bool IsLoading
        {
            get { return _isLoading; }
            set
            {
                if (value == _isLoading) return;
                _isLoading = value;
                NotifyOfPropertyChange();
            }
        }
    }
}
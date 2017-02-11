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
        private bool _columnThenRow;
        private string _folderPath;
        private bool _isLoading;
        private bool _rowThenColumn;
        private readonly UserSettings _userSettings;

        public ShellViewModel(IBathtubFactory bathtubFactory, UserSettings userSettings)
        {
            _bathtubFactory = bathtubFactory;
            _userSettings = userSettings;

            _userSettings.Load(this);
        }

        public bool ColumnThenRow
        {
            get { return _columnThenRow; }
            set
            {
                if (value == _columnThenRow) return;
                _columnThenRow = value;
                NotifyOfPropertyChange();
            }
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

        public bool RowThenColumn
        {
            get { return _rowThenColumn; }
            set
            {
                if (value == _rowThenColumn) return;
                _rowThenColumn = value;
                NotifyOfPropertyChange();
            }
        }


        public ICommand ScrubCommand => new DelegateCommand(RunScrubber);

        protected override void OnActivate()
        {
            base.OnActivate();
            RowThenColumn = true;
            DisplayName = "Scrubber";
        }

        public void ColumnThenRowChecked()
        {
            if (RowThenColumn)
                RowThenColumn = false;

            ColumnThenRow = true;
        }

        public void RowThenColumnChecked()
        {
            if (ColumnThenRow)
                ColumnThenRow = false;

            RowThenColumn = true;
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

                    var bathtub = _bathtubFactory.Create(FolderPath);

                    bathtub.Fill();
                    bathtub.Rinse();

                    var result = bathtub.Drain();
                    result.DisplayResult();
                }).GetAwaiter()
                .OnCompleted(() => { IsLoading = false; });
        }

        public void Close()
        {
            TryClose();
        }
    }
}
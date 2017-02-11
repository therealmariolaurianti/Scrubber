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
        private string _folderPath;
        private bool _columnThenRow;
        private bool _rowThenColumn;
        private bool _isLoading;

        public ShellViewModel(IBathtubFactory bathtubFactory)
        {
            _bathtubFactory = bathtubFactory;
        }

        protected override void OnActivate()
        {
            base.OnActivate();
            RowThenColumn = true;
            DisplayName = "Scrubber";
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


        public ICommand ScrubCommand => new DelegateCommand(RunScrubber);

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

            IsLoading = true;
            Task.Run(() =>
            {
                var bathtub = _bathtubFactory.Create(FolderPath);

                bathtub.Fill();
                bathtub.Rinse();

                var result = bathtub.Drain();
                result.DisplayResult();
            }).GetAwaiter()
              .OnCompleted(() => { IsLoading = false; });
        }
    }
}
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Caliburn.Micro;
using Scrubber.Factories;
using Scrubber.Helpers;
using Scrubber.Maintenance;
using Scrubber.Model.Factories;
using Scrubber.Model.Maintenance.InputAttributes.ViewModels;
using Scrubber.Objects;

namespace Scrubber.Model.Maintenance.Shell.ViewModels
{
    public class ShellViewModel : ViewModel
    {
        private readonly IBathtubFactory _bathtubFactory;
        private readonly IInputAttributeViewModelFactory _inputAttributeViewModelFactory;
        private readonly IResultViewModelFactory _resultViewModelFactory;
        private readonly UserSettings _userSettings;
        private readonly IWindowManager _windowManager;
        private bool _clearComments;
        private string _folderPath;
        private ObservableCollection<InputAttributeViewModel> _inputAttributeViewModels;
        private bool _isLoading;

        public ShellViewModel(IBathtubFactory bathtubFactory, UserSettings userSettings,
            IWindowManager windowManager, IResultViewModelFactory resultViewModelFactory,
            IInputAttributeViewModelFactory inputAttributeViewModelFactory)
        {
            _bathtubFactory = bathtubFactory;
            _userSettings = userSettings;
            _windowManager = windowManager;
            _resultViewModelFactory = resultViewModelFactory;
            _inputAttributeViewModelFactory = inputAttributeViewModelFactory;

            _userSettings.Load(this);
        }

        private Result<Dictionary<bool, List<DirtyFile>>> CleaningResults { get; set; }

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

        //public ICollection<InputAttribute> InputAttributes => InputAttributeViewModels.Select(
        //    inputAttributeViewModel => inputAttributeViewModel.Item).ToList();

        public ObservableCollection<InputAttributeViewModel> InputAttributeViewModels
        {
            get { return _inputAttributeViewModels; }
            set
            {
                if (Equals(value, _inputAttributeViewModels)) return;
                _inputAttributeViewModels = value;
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

        protected override void OnActivate()
        {
            DisplayName = "Scrubber";
            InputAttributeViewModels = new ObservableCollection<InputAttributeViewModel>();
            base.OnActivate();
        }

        public void AddAttribute()
        {
            var attribute = _inputAttributeViewModelFactory.Create();
            InputAttributeViewModels.Add(attribute);
        }

        public void RemoveAttribute(object sender, RoutedEventArgs e)
        {
            if (!(sender is Button button))
                return;

            var attribute = button.DataContext as InputAttributeViewModel;
            if(attribute == null)
                return;

            var viewModel = InputAttributeViewModels.SingleOrDefault(vm => vm.Id == attribute.Id);
            InputAttributeViewModels.Remove(viewModel);
        }

        public void OpenFileExplorer()
        {
            if (string.IsNullOrEmpty(FolderPath))
            {
                Process.Start("explorer.exe", "-p");
            }
            else
            {
                var info = new ProcessStartInfo
                {
                    Arguments = "/select, \"" + FolderPath + "\"",
                    FileName = "explorer.exe"
                };

                Process.Start(info);
            }
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

                    //var bathtubOptions = new BathtubOptions(FolderPath, ClearComments, InputAttributes);
                    //var bathtub = _bathtubFactory.Create(bathtubOptions);
                    //
                    //bathtub.Fill();
                    //bathtub.Rinse();
                    //
                    //CleaningResults = bathtub.Drain();
                }).GetAwaiter()
                .OnCompleted(() =>
                {
                    var resultViewModel = _resultViewModelFactory.Create(CleaningResults);
                    _windowManager.ShowDialog(resultViewModel);

                    IsLoading = false;
                });
        }
    }
}
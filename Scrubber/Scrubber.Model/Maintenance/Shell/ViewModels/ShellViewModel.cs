using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Caliburn.Micro;
using Ninject;
using Scrubber.Factories;
using Scrubber.Helpers;
using Scrubber.Maintenance;
using Scrubber.Model.Factories;
using Scrubber.Model.Maintenance.InputAttributes.ViewModels;
using Scrubber.Objects;
using Scrubber.Options;

namespace Scrubber.Model.Maintenance.Shell.ViewModels
{
    public class ShellViewModel : ViewModel
    {
        private readonly IBathtubFactory _bathtubFactory;
        private readonly IErrorViewModelFactory _errorViewModelFactory;
        private readonly IInputAttributeViewModelFactory _inputAttributeViewModelFactory;
        private readonly IResultViewModelFactory _resultViewModelFactory;
        private readonly UserSettings _userSettings;
        private readonly IWindowManager _windowManager;
        private ObservableCollection<InputAttributeViewModel> _additionalInputAttributeViewModels;
        private bool _clearComments;
        private string _folderPath;
        private bool _isLoading;
        private ObservableCollection<InputAttributeViewModel> _removalInputAttributeViewModels;

        public ShellViewModel(IBathtubFactory bathtubFactory, UserSettings userSettings,
            IWindowManager windowManager, IResultViewModelFactory resultViewModelFactory,
            IInputAttributeViewModelFactory inputAttributeViewModelFactory, IErrorViewModelFactory errorViewModelFactory)
        {
            _bathtubFactory = bathtubFactory;
            _userSettings = userSettings;
            _windowManager = windowManager;
            _resultViewModelFactory = resultViewModelFactory;
            _inputAttributeViewModelFactory = inputAttributeViewModelFactory;
            _errorViewModelFactory = errorViewModelFactory;

            _userSettings.Load(this);
        }

        public ICollection<InputAttribute> AdditionalInputAttributes => AdditionalInputAttributeViewModels.Select(
            inputAttributeViewModel => inputAttributeViewModel.Item).ToList();

        public ObservableCollection<InputAttributeViewModel> AdditionalInputAttributeViewModels
        {
            get { return _additionalInputAttributeViewModels; }
            set
            {
                if (Equals(value, _additionalInputAttributeViewModels)) return;
                _additionalInputAttributeViewModels = value;
                NotifyOfPropertyChange();
            }
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

        public ICollection<InputAttribute> RemovalInputAttributes => RemovalInputAttributeViewModels.Select(
            attributeViewModel => attributeViewModel.Item).ToList();

        public ObservableCollection<InputAttributeViewModel> RemovalInputAttributeViewModels
        {
            get { return _removalInputAttributeViewModels; }
            set
            {
                if (Equals(value, _removalInputAttributeViewModels)) return;
                _removalInputAttributeViewModels = value;
                NotifyOfPropertyChange();
            }
        }

        public ICommand ScrubCommand => new DelegateCommand(RunScrubber);


        protected override void OnActivate()
        {
            DisplayName = "Scrubber";
            AdditionalInputAttributeViewModels = new ObservableCollection<InputAttributeViewModel>();
            RemovalInputAttributeViewModels = new ObservableCollection<InputAttributeViewModel>();
            base.OnActivate();
        }

        public void AddAttribute()
        {
            AdditionalInputAttributeViewModels.Add(_inputAttributeViewModelFactory.Create());
        }

        public void AddRemoveAttribute()
        {
            RemovalInputAttributeViewModels.Add(_inputAttributeViewModelFactory.Create());
        }

        public void RemoveRemovalAttribute(object sender, RoutedEventArgs e)
        {
            if (!(sender is Button button))
                return;

            var attribute = button.DataContext as InputAttributeViewModel;
            if (attribute == null)
                return;

            var viewModel = RemovalInputAttributeViewModels.SingleOrDefault(vm => vm.Id == attribute.Id);
            RemovalInputAttributeViewModels.Remove(viewModel);
        }

        public void RemoveAttribute(object sender, RoutedEventArgs e)
        {
            if (!(sender is Button button))
                return;

            var attribute = button.DataContext as InputAttributeViewModel;
            if (attribute == null)
                return;

            var viewModel = AdditionalInputAttributeViewModels.SingleOrDefault(vm => vm.Id == attribute.Id);
            AdditionalInputAttributeViewModels.Remove(viewModel);
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
                IsLoading = true;
                _windowManager.ShowDialog(_errorViewModelFactory.Create("Please Enter a Path."));
                IsLoading = false;
                return;
            }
            Task.Run(() =>
                {
                    IsLoading = true;

                    var bathtubOptions = new ScrubberOptions(FolderPath, ClearComments, AdditionalInputAttributes,
                        RemovalInputAttributes);

                    var bathtub = _bathtubFactory.Create(bathtubOptions);

                    bathtub.Fill();
                    bathtub.Rinse();

                    CleaningResults = bathtub.Drain();
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
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Caliburn.Micro;
using Scrubber.Enums;
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
        private ObservableCollection<InputAttributeViewModel> _inputAttributeViewModels;
        private bool _clearComments;
        private string _path;
        private bool _formatFiles;
        private bool _isLoading;
        private ObservableCollection<InputAttributeViewModel> _removeInputAttributeViewModels;
        private FolderOrFile _folderOrFile;

        public ShellViewModel(
            IBathtubFactory bathtubFactory, 
            UserSettings userSettings,
            IWindowManager windowManager, 
            IResultViewModelFactory resultViewModelFactory,
            IInputAttributeViewModelFactory inputAttributeViewModelFactory,
            IErrorViewModelFactory errorViewModelFactory)
        {
            _bathtubFactory = bathtubFactory;
            _userSettings = userSettings;
            _windowManager = windowManager;
            _resultViewModelFactory = resultViewModelFactory;
            _inputAttributeViewModelFactory = inputAttributeViewModelFactory;
            _errorViewModelFactory = errorViewModelFactory;
            
            _userSettings.Load(this);
        }

        public ICollection<InputAttribute> InputAttributes => InputAttributeViewModels?.Select(
            inputAttributeViewModel => inputAttributeViewModel.Item).ToList();

        public ObservableCollection<InputAttributeViewModel> InputAttributeViewModels
        {
            get => _inputAttributeViewModels;
            set
            {
                if (Equals(value, _inputAttributeViewModels)) return;
                _inputAttributeViewModels = value;
                NotifyOfPropertyChange();
            }
        }

        private Result<Dictionary<bool, List<DirtyFile>>> CleaningResults { get; set; }

        public bool ClearComments
        {
            get => _clearComments;
            set
            {
                if (value == _clearComments) return;
                _clearComments = value;
                NotifyOfPropertyChange();

                _userSettings?.SaveSingle(nameof(ClearComments), value);
            }
        }

        public string Path
        {
            get => _path.Trim();
            set
            {
                if (value == _path) return;
                _path = value;
                NotifyOfPropertyChange();

                _userSettings?.SaveSingle(nameof(Path), value);
            }
        }

        public bool FormatFiles
        {
            get => _formatFiles;
            set
            {
                if (value == _formatFiles) return;
                _formatFiles = value;
                NotifyOfPropertyChange();

                _userSettings?.SaveSingle(nameof(FormatFiles), value);
            }
        }

        public bool IsLoading
        {
            get => _isLoading;
            set
            {
                if (value == _isLoading) return;
                _isLoading = value;
                NotifyOfPropertyChange();
            }
        }

        public ICollection<InputAttribute> RemoveInputAttributes => RemoveInputAttributeViewModels?.Select(
            attributeViewModel => attributeViewModel.Item).ToList();

        public ObservableCollection<InputAttributeViewModel> RemoveInputAttributeViewModels
        {
            get => _removeInputAttributeViewModels;
            set
            {
                if (Equals(value, _removeInputAttributeViewModels)) return;
                _removeInputAttributeViewModels = value;
                NotifyOfPropertyChange();
            }
        }

        public ICommand ScrubCommand => new DelegateCommand(RunScrubber);

        public DelegateCommand SelectFolderPathCommand => new DelegateCommand(SelectFolderPath);

        public FolderOrFile FolderOrFile
        {
            get => _folderOrFile;
            set
            {
                if (value == _folderOrFile) return;
                _folderOrFile = value;
                NotifyOfPropertyChange();
                NotifyOfPropertyChange(nameof(FolderPathLabel));

                _userSettings?.SaveSingle(nameof(FolderOrFile), value);
            }
        }

        public string FolderPathLabel => FolderOrFile == FolderOrFile.Folder ? "Folder Path" : "File Path";

        private static void SelectFolderPath()
        {
            var textBox = VisualHelper.FindChild<TextBox>(Application.Current.MainWindow, "FolderPath");
            Keyboard.Focus(textBox);
            textBox.SelectAll();
        }

        protected override void OnActivate()
        {
            DisplayName = "Scrubber";
            InputAttributeViewModels = new ObservableCollection<InputAttributeViewModel>();
            RemoveInputAttributeViewModels = new ObservableCollection<InputAttributeViewModel>();
            base.OnActivate();
        }

        public void AddAttribute()
        {
            InputAttributeViewModels.Add(_inputAttributeViewModelFactory.Create());
        }

        public void AddRemoveAttribute()
        {
            RemoveInputAttributeViewModels.Add(_inputAttributeViewModelFactory.Create());
        }

        public void RemoveRemovalAttribute(object sender, RoutedEventArgs e)
        {
            if (!(sender is Button button))
                return;

            var attribute = button.DataContext as InputAttributeViewModel;
            if (attribute == null)
                return;

            var viewModel = RemoveInputAttributeViewModels.SingleOrDefault(vm => vm.Id == attribute.Id);
            RemoveInputAttributeViewModels.Remove(viewModel);
        }

        public void RemoveAttribute(object sender, RoutedEventArgs e)
        {
            if (!(sender is Button button))
                return;

            var attribute = button.DataContext as InputAttributeViewModel;
            if (attribute == null)
                return;

            var viewModel = InputAttributeViewModels.SingleOrDefault(vm => vm.Id == attribute.Id);
            InputAttributeViewModels.Remove(viewModel);
        }

        public void OpenFileExplorer()
        {
            if (string.IsNullOrEmpty(Path))
            {
                Process.Start("explorer.exe", "-p");
            }
            else
            {
                var info = new ProcessStartInfo
                {
                    Arguments = "/select, \"" + Path + "\"",
                    FileName = "explorer.exe"
                };

                Process.Start(info);
            }
        }

        private bool CanRunScrubber()
        {
            switch (FolderOrFile)
            {
                case FolderOrFile.Folder:
                    if (Directory.Exists(Path))
                        return true;
                    break;
                case FolderOrFile.File:
                    if (File.Exists(Path))
                        return true;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            IsLoading = true;
            _windowManager.ShowDialog(_errorViewModelFactory.Create($"The {FolderPathLabel} is invaild."));
            IsLoading = false;
            return false;
        }

        public void RunScrubber()
        {
            if (!CanRunScrubber())
                return;

            Task.Run(() => BeginClean()).GetAwaiter().OnCompleted(CompleteClean);
        }

        private void CompleteClean()
        {
            var resultViewModel = _resultViewModelFactory.Create(CleaningResults);
            _windowManager.ShowDialog(resultViewModel);

            IsLoading = false;
        }

        private void BeginClean()
        {
            IsLoading = true;

            var bathtubOptions = new ScrubberOptions(Path, ClearComments, FormatFiles, InputAttributes,
                RemoveInputAttributes, FolderOrFile);

            var bathtub = _bathtubFactory.Create(bathtubOptions);
            bathtub.FillAndRinse();

            CleaningResults = bathtub.Drain();
        }
    }
}
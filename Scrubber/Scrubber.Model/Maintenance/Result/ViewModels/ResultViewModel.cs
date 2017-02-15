using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using Caliburn.Micro;
using Scrubber.Helpers;
using Scrubber.Maintenance;
using Scrubber.Model.Factories;
using Scrubber.Objects;

namespace Scrubber.Model.Maintenance.Result.ViewModels
{
    public class ResultViewModel : ViewModel
    {
        public Result<Dictionary<bool, List<DirtyFile>>> Result { get; }
        private readonly IFileViewModelFactory _fileViewModelFactory;
        private readonly IWindowManager _windowManager;

        public ResultViewModel(
            Result<Dictionary<bool, List<DirtyFile>>> result, 
            IFileViewModelFactory fileViewModelFactory, 
            IWindowManager windowManager)
        {
            Result = result;
            _fileViewModelFactory = fileViewModelFactory;
            _windowManager = windowManager;
        }

        protected override void OnActivate()
        {
            DisplayName = "Results";
            base.OnActivate();
        }

        public void ViewFiles()
        {
            var files = Result.ResultValue.SelectMany(result => result.Value).ToList();
            var fileViewModel = _fileViewModelFactory.Create(files.OrderBy(x => x.IsClean).ToList());

            _windowManager.ShowDialog(fileViewModel);
        }

        public string ResultString
        {
            get
            {
                var cleaned = Result.ResultValue.Any(r => r.Key)
                    ? Result.ResultValue[true].Count
                    : 0;

                if (Result.Success)
                    return $"Operation Completed. {cleaned} Cleaned. 0 Failed.";

                var dirty = Result.ResultValue[false].Count;

                return $"Operation Completed With Errors. {cleaned} Cleaned. {dirty} Failed.";
            }
        }

        public ICommand ViewFilesCommand => new DelegateCommand(ViewFiles);
    }
}
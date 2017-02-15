using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows.Input;
using Caliburn.Micro;
using Scrubber.Helpers;
using Scrubber.Maintenance;
using Scrubber.Objects;

namespace Scrubber.Model.Maintenance.Files.ViewModels
{
    public class FileViewModel : ViewModel
    {
        public List<DirtyFile> Files { get; }
        public DirtyFile SelectedFile { get; set; }

        public ICommand CloseCommand => new DelegateCommand(Close);

        public FileViewModel(List<DirtyFile> files)
        {
            Files = files.OrderBy(x => x.IsClean).ToList();
        }

        public void OpenFile()
        {
            if (SelectedFile != null)
                Process.Start(SelectedFile.FilePath);
        }
    }
}
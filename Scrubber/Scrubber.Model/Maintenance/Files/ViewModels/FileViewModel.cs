using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Input;
using Caliburn.Micro;
using Scrubber.Helpers;
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
            Files = files;
        }

        public void OpenFile()
        {
            if (SelectedFile != null)
                Process.Start(SelectedFile.FilePath);
        }
    }
}
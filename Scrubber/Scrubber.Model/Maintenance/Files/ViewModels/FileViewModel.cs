using System.Collections.Generic;
using System.Diagnostics;
using Caliburn.Micro;
using Scrubber.Objects;

namespace Scrubber.Model.Maintenance.Files.ViewModels
{
    public class FileViewModel : Screen
    {
        public List<DirtyFile> Files { get; }
        public DirtyFile SelectedFile { get; set; }

        public FileViewModel(List<DirtyFile> files)
        {
            Files = files;
        }

        public void OpenFile()
        {
            if (SelectedFile != null)
                Process.Start(SelectedFile.FilePath);
        }

        public void Close()
        {
            TryClose();
        }
    }
}
using System.Collections.Generic;
using Caliburn.Micro;
using Scrubber.Objects;

namespace Scrubber.Model.Maintenance.Files.ViewModels
{
    public class FileViewModel : Screen
    {
        public List<DirtyFile> Files { get; }

        public FileViewModel(List<DirtyFile> files)
        {
            Files = files;
        }

        public void Close()
        {
            TryClose();
        }
    }
}
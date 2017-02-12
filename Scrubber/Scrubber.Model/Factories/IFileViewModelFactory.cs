using System.Collections.Generic;
using Scrubber.Model.Maintenance.Files.ViewModels;
using Scrubber.Objects;

namespace Scrubber.Model.Factories
{
    public interface IFileViewModelFactory : IFactory
    {
        FileViewModel Create(List<DirtyFile> files);
    }
}
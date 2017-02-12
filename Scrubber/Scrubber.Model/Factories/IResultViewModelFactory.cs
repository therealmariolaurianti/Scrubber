using System.Collections.Generic;
using Scrubber.Helpers;
using Scrubber.Model.Maintenance.Result.ViewModels;
using Scrubber.Objects;

namespace Scrubber.Model.Factories
{
    public interface IResultViewModelFactory : IFactory
    {
        ResultViewModel Create(Result<Dictionary<bool, List<DirtyFile>>> result);
    }
}
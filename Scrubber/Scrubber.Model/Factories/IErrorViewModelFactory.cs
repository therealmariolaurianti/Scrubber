using Scrubber.Factories;
using Scrubber.Model.Maintenance.Error.ViewModels;

namespace Scrubber.Model.Factories
{
    public interface IErrorViewModelFactory : IFactory
    {
        ErrorViewModel Create(string errorMessage);
    }
}
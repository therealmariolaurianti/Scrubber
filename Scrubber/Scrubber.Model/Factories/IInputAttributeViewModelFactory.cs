using Scrubber.Factories;
using Scrubber.Model.Maintenance.InputAttributes.ViewModels;

namespace Scrubber.Model.Factories
{
    public interface IInputAttributeViewModelFactory : IFactory
    {
        InputAttributeViewModel Create();
    }
}
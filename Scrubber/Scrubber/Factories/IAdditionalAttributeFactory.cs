using Scrubber.Enums;
using Scrubber.Model.Factories;
using Scrubber.Objects;

namespace Scrubber.Factories
{
    public interface IAdditionalAttributeFactory : IFactory
    {
        AdditionalAttribute Create(CommonAttributes attributeName, object value);
    }
}
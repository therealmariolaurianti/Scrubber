using Scrubber.Enums;
using Scrubber.Objects;

namespace Scrubber.Factories
{
    public interface IAdditionalAttributeFactory : IFactory
    {
        AdditionalAttribute Create(CommonAttributes attributeName, object value);
    }
}
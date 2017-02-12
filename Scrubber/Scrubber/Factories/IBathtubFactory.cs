using Scrubber.Enums;
using Scrubber.Objects;
using Scrubber.Workers;

namespace Scrubber.Factories
{
    public interface IBathtubFactory : IFactory
    {
        Bathtub Create(string folderPath, bool clearComments);
    }

    public interface IAdditionalAttributeFactory : IFactory
    {
        AdditionalAttribute Create(CommonAttributes attributeName, object value);
    }
}
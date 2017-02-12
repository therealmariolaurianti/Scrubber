using Scrubber.Workers;

namespace Scrubber.Model.Factories
{
    public interface IBathtubFactory : IFactory
    {
        Bathtub Create(string folderPath, bool clearComments);
    }
}
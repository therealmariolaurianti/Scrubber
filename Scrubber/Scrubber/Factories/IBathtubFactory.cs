using Scrubber.Workers;

namespace Scrubber.Factories
{
    public interface IBathtubFactory : IFactory
    {
        Bathtub Create(string folderPath);
    }
}
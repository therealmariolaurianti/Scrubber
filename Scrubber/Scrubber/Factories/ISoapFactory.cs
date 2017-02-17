using Scrubber.Objects;
using Scrubber.Workers;

namespace Scrubber.Factories
{
    public interface ISoapFactory : IFactory
    {
        Soap Create(IScrubberOptions options);
    }
}
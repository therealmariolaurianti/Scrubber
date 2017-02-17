using Scrubber.Objects;
using Scrubber.Options;
using Scrubber.Workers;

namespace Scrubber.Factories
{
    public interface ISoapFactory : IFactory
    {
        Soap Create(IScrubberOptions options);
    }
}
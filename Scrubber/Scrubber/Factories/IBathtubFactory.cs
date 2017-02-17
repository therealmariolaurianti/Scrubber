using Scrubber.Objects;
using Scrubber.Options;
using Scrubber.Workers;

namespace Scrubber.Factories
{
    public interface IBathtubFactory : IFactory
    {
        Bathtub Create(IScrubberOptions bathtubOptions);
    }
}
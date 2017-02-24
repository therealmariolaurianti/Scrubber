namespace Scrubber.Objects
{
    public class AttributeValue
    {
        public AttributeValue(string name)
        {
            Name = name.EndsWith("String") ? name.Replace("String", string.Empty) : name;
        }

        public string Name { get; }
    }
}
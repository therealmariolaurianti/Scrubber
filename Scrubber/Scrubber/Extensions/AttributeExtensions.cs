using System.Xml;
using Scrubber.Enums;

namespace Scrubber.Extensions
{
    public static class AttributeExtensions
    {
        public static void RemoveExistingAttribute(this XmlNode node, string attributeName)
        {
            var attribute = node.Attributes?[attributeName];
            if (attribute != null)
                node.Attributes.Remove(attribute);
        }
    }
}
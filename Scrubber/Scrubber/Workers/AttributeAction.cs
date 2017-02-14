using System.Xml;
using Scrubber.Enums;
using Scrubber.Extensions;
using Scrubber.Helpers;
using Scrubber.Objects;

namespace Scrubber.Workers
{
    public class AttributeAction
    {
        public void AddToNode(XmlNode node, XmlDocument xDoc, InputAttribute inputAttribute)
        {
            if (node.Attributes != null && node.Attributes.Count > 0)
                node.RemoveExistingAttribute(inputAttribute.Name);

            var attribute = xDoc.CreateAttribute(inputAttribute.Name);

            if (inputAttribute.IsDesignTimeAttribute)
                attribute.Prefix = inputAttribute.NamespaceXmnlsCharacter;

            attribute.Value = inputAttribute.Value.ToString();

            node.Attributes?.Append(attribute);
        }
    }
}
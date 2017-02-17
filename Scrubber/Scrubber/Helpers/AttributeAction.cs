using System.Xml;
using Scrubber.Extensions;
using Scrubber.Objects;

namespace Scrubber.Helpers
{
    public class AttributeAction
    {
        public void Add(XmlNode node, XmlDocument xDoc, InputAttribute inputAttribute)
        {
            if (node.Name != inputAttribute.ControlName)
                return;

            if (node.Attributes != null && node.Attributes.Count > 0)
                node.RemoveExistingAttribute(inputAttribute.AttributeName);

            var attribute = xDoc.CreateAttribute(inputAttribute.AttributeName);

            if (inputAttribute.IsDesignTimeAttribute)
                attribute.Prefix = inputAttribute.NamespaceXmnlsCharacter;

            attribute.Value = inputAttribute.AttributeValue.ToString();

            node.Attributes?.Append(attribute);
        }

        public void Remove(XmlNode node, InputAttribute existingAttribute)
        {
            if (node.Name != existingAttribute.ControlName)
                return;

            node.RemoveExistingAttribute(existingAttribute.AttributeName);
        }
    }
}
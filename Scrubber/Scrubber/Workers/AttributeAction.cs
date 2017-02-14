using System.Xml;
using Scrubber.Extensions;
using Scrubber.Objects;

namespace Scrubber.Workers
{
    public class AttributeAction
    {
        public void AddToNode(XmlNode node, XmlDocument xDoc, InputAttribute inputAttribute)
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
    }
}
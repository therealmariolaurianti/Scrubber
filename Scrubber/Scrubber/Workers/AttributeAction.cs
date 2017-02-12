using System.Xml;
using Scrubber.Enums;
using Scrubber.Extensions;
using Scrubber.Helpers;
using Scrubber.Objects;

namespace Scrubber.Workers
{
    public class AttributeAction
    {
        public void AddToNode(XmlNode node, XmlDocument xDoc, AdditionalAttribute additionalAttribute)
        {
            if (node.Attributes != null && node.Attributes.Count > 0)
                node.RemoveExistingAttribute(additionalAttribute.Name);

            var attribute = xDoc.CreateAttribute(additionalAttribute.Name);

            if (additionalAttribute.IsDesignTimeAttribute)
                attribute.Prefix = additionalAttribute.NamespaceXmnlsCharacter;

            attribute.Value = additionalAttribute.Value.ToString();

            node.Attributes?.Append(attribute);
        }

        public void AddPaddingToGroupBox(XmlNode node, XmlDocument xDoc)
        {
            if (node.Name.EnumEquals(CommonControls.GroupBox))
                AddToNode(node, xDoc, new AdditionalAttribute(CommonAttributes.Padding, 0));
        }

        public void RemovePaddingFromGroupBox(XmlNode node)
        {
            if (!node.Name.EnumEquals(CommonControls.GroupBox))
                return;

            node.RemoveExistingAttribute(CommonAttributes.Padding);
        }

        public void AddFontSizeToTabItems(XmlNode node, XmlDocument xDoc)
        {
            if (node.Name.Contains(CommonControls.TabItemExt) ||
                node.Name.Contains(CommonControls.TabControlExt))
                AddToNode(node, xDoc, new AdditionalAttribute(CommonAttributes.FontSize, 12));
        }

        public void RemoveFontSizeFromTabItems(XmlNode node)
        {
            if (!node.Name.Contains(CommonControls.TabItemExt) &&
                node.Name.Contains(CommonControls.TabControlExt))
                return;

            node.RemoveExistingAttribute(CommonAttributes.FontSize);
        }
    }
}
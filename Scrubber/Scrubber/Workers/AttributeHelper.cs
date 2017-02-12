using System.Collections.Generic;
using System.Linq;
using System.Xml;
using Scrubber.Enums;
using Scrubber.Helpers;
using Scrubber.Objects;

namespace Scrubber.Workers
{
    public class AttributeHelper
    {
        public void AddAttributeToNode(XmlNode node, XmlDocument xDoc, AdditionalAttribute additionalAttribute)
        {
            if (node.Attributes != null && node.Attributes.Count > 0)
                node.RemoveExistingAttribute(additionalAttribute.Name);

            var attribute = xDoc.CreateAttribute(additionalAttribute.Name);

            if (additionalAttribute.IsDesignTimeAttribute)
                attribute.Prefix = additionalAttribute.NamespaceXmnlsCharacter;

            attribute.Value = additionalAttribute.Value.ToString();

            node.Attributes?.Append(attribute);
        }

        public void ClearComments(XmlNode node, bool clearComments)
        {
            if (!clearComments)
                return;

            if (node.NodeType == XmlNodeType.Comment)
                node.ParentNode?.RemoveChild(node);
        }

        public void RebuildDefaultAttributes(XmlNode node, XmlDocument xDoc,
            List<XmlAttribute> xmlAttributes)
        {
            foreach (var xmlAttribute in xmlAttributes)
                node.Attributes?.Append(xmlAttribute);
        }

        private void DisableTabStopForContainers(XmlNode node, XmlDocument xDoc)
        {
            if (Containers.ExactContainers.Any(node.Name.Equals))
                AddAttributeToNode(node, xDoc, new AdditionalAttribute(CommonAttributes.IsTabStop, false));
        }

        public void InitialClean(XmlNode node, XmlDocument xDoc)
        {
            DisableTabStopForContainers(node, xDoc);

            //TEMP BECUASE OF SYNCFUSION BULLSHIT
            AddFontSizeToTabItems(node, xDoc);
            AddPaddingToGroupBox(node, xDoc);

            //RemoveFontSizeFromTabItems(node);
            //RemovePaddingFromGroupBox(node);
        }

        private void AddPaddingToGroupBox(XmlNode node, XmlDocument xDoc)
        {
            if (node.Name.EnumEquals(CommonControls.GroupBox))
                AddAttributeToNode(node, xDoc, new AdditionalAttribute(CommonAttributes.Padding, 0));
        }

        private void RemovePaddingFromGroupBox(XmlNode node)
        {
            if (!node.Name.EnumEquals(CommonControls.GroupBox))
                return;

            node.RemoveExistingAttribute(CommonAttributes.Padding);
        }

        private void AddFontSizeToTabItems(XmlNode node, XmlDocument xDoc)
        {
            if (node.Name.Contains(CommonControls.TabItemExt) ||
                node.Name.Contains(CommonControls.TabControlExt))
                AddAttributeToNode(node, xDoc, new AdditionalAttribute(CommonAttributes.FontSize, 12));
        }

        private void RemoveFontSizeFromTabItems(XmlNode node)
        {
            if (!node.Name.Contains(CommonControls.TabItemExt) &&
                node.Name.Contains(CommonControls.TabControlExt))
                return;

            node.RemoveExistingAttribute(CommonAttributes.FontSize);
        }
    }
}
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using Scrubber.Helpers;
using Scrubber.Interfaces;
using Scrubber.Objects;

namespace Scrubber.Workers
{
    public class AttributeHelper
    {
        private readonly List<string> _containers = new List<string>
        {
            "GroupBox",
            "syncfusion:TabControlExt",
            "Button",
            "syncfusion:TabItemExt"
        };

        private readonly IOptions _options;

        public AttributeHelper(IOptions options)
        {
            _options = options;
        }

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

        private void CleanComments(XmlNode node)
        {
            if (!_options.ClearComments)
                return;

            if (node.NodeType == XmlNodeType.Comment)
                node.ParentNode?.RemoveChild(node);
        }

        public void RebuildDefaultAttributes(XmlNode node, XmlDocument xDoc, List<XmlAttribute> xmlAttributes)
        {
            foreach (var xmlAttribute in xmlAttributes)
                node.Attributes?.Append(xmlAttribute);
        }

        private void DisableTabStopForContainers(XmlNode node, XmlDocument xDoc)
        {
            if (_containers.Any(node.Name.Equals))
                AddAttributeToNode(node, xDoc, new AdditionalAttribute("IsTabStop", false));
        }

        public void InitialClean(XmlNode node, XmlDocument xDoc)
        {
            CleanComments(node);
            DisableTabStopForContainers(node, xDoc);

            //TEMP BECUASE OF SYNCFUSION BULLSHIT
            AddFontSizeToTabItems(node, xDoc);
            AddPaddingToGroupBox(node, xDoc);

            //RemoveFontSizeFromTabItems(node);
            //RemovePaddingFromGroupBox(node);
        }

        private void AddPaddingToGroupBox(XmlNode node, XmlDocument xDoc)
        {
            if (node.Name.Equals("GroupBox"))
                AddAttributeToNode(node, xDoc, new AdditionalAttribute("Padding", 0));
        }

        private void RemovePaddingFromGroupBox(XmlNode node)
        {
            if (!node.Name.Equals("GroupBox"))
                return;

            node.RemoveExistingAttribute("Padding");
        }

        private void AddFontSizeToTabItems(XmlNode node, XmlDocument xDoc)
        {
            if (node.Name.Contains("TabItemExt") || node.Name.Contains("TabControlExt"))
                AddAttributeToNode(node, xDoc, new AdditionalAttribute("FontSize", 12));
        }

        private void RemoveFontSizeFromTabItems(XmlNode node)
        {
            if (!node.Name.Contains("TabItemExt") && node.Name.Contains("TabControlExt"))
                return;

            node.RemoveExistingAttribute("FontSize");
        }
    }
}
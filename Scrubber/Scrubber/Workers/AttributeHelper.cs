using System.Collections.Generic;
using System.Linq;
using System.Xml;
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

        public void AddAttributeToNode(XmlNode node, XmlDocument xDoc, AdditionalAttribute additionalAttribute)
        {
            if (node.Attributes != null && node.Attributes.Count > 0)
            {
                var existing = node.Attributes.GetNamedItem(additionalAttribute.Name);

                if (existing != null)
                    node.Attributes.Remove(node.Attributes[additionalAttribute.Name]);
            }

            var attribute = xDoc.CreateAttribute(additionalAttribute.Name);
            attribute.Value = additionalAttribute.Value.ToString();

            node.Attributes?.Append(attribute);
        }

        private void CleanComments(XmlNode node)
        {
            if (node.NodeType == XmlNodeType.Comment)
                node.ParentNode?.RemoveChild(node);
        }

        public void RebuildDefaultAttributes(XmlNode node, XmlDocument xDoc, List<AdditionalAttribute> nodeAttributes)
        {
            foreach (var additionalAttribute in nodeAttributes)
                AddAttributeToNode(node, xDoc, additionalAttribute);
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
        }
    }
}
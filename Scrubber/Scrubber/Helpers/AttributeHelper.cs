using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Xml;
using Scrubber.Objects;
using Scrubber.Options;

namespace Scrubber.Helpers
{
    public class AttributeHelper
    {
        private readonly AttributeAction _attributeAction;

        public AttributeHelper(AttributeAction attributeAction)
        {
            _attributeAction = attributeAction;
        }

        public void GoToCleaners(XmlNode node, XmlDocument xDoc, IScrubberOptions options)
        {
            ClearComments(node, options.ClearComments);
            AddInputAttribute(node, xDoc, options.InputAttributes);
            RemoveAttributes(node, options.ExistingAttributes);
            //SwapControl(node, xDoc, "TextBox", "syncfusion:DoubleTextBox");
        }

        private static void ClearComments(XmlNode node, bool clearComments)
        {
            if (!clearComments)
                return;

            if (node.NodeType == XmlNodeType.Comment)
                node.ParentNode?.RemoveChild(node);
        }

        private void AddInputAttribute(XmlNode node, XmlDocument xDoc,
            ICollection<InputAttribute> inputAttributes)
        {
            if (inputAttributes == null)
                return;

            _attributeAction.AddMany(node, xDoc, inputAttributes);
        }

        private void RemoveAttributes(XmlNode node,
            ICollection<InputAttribute> existingAttributes)
        {
            if (existingAttributes == null)
                return;

            _attributeAction.RemoveMany(node, existingAttributes);
        }
        
        private void SwapControl(XmlNode node, XmlDocument xDoc, string oldControlName, string newControl)
        {
            if (node.LocalName != oldControlName)
                return;

            var newNode = xDoc.CreateNode(XmlNodeType.Element, newControl, "http://schemas.syncfusion.com/wpf");

            var nodes = node.ChildNodes.Cast<XmlNode>().ToList();
            nodes.ForEach(xmlNode => newNode.AppendChild(xmlNode));

            var nodeAttributes = node.Attributes;
            if (nodeAttributes != null)
            {
                var attributes = new List<XmlAttribute>(nodeAttributes.Cast<XmlAttribute>());

                var textAttribute = attributes.SingleOrDefault(a => a.Name == "Text");
                if(textAttribute == null)
                    return;
                
                var equalSignIndex = textAttribute.Value.IndexOf('=');
                var commaIndex = textAttribute.Value.IndexOf(',') - 1;
                if (commaIndex < 0)
                    return;
                
                var propertyName = textAttribute.Value.Substring(equalSignIndex + 1, commaIndex - equalSignIndex);

                attributes.Remove(textAttribute);
                
                var nameAttribute = xDoc.CreateAttribute("Name");
                nameAttribute.Value = propertyName;
                attributes.Add(nameAttribute);

                attributes.ForEach(attr => newNode.Attributes?.Append(attr));
            }

            var parentNode = node.ParentNode;
            parentNode?.ReplaceChild(newNode, node);
        }
    }
}
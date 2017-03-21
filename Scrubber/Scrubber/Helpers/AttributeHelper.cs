using System.Collections.Generic;
using System.Linq;
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

        private void ClearComments(XmlNode node, bool clearComments)
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

        private void AddInputAttribute(XmlNode node, XmlDocument xDoc,
            ICollection<InputAttribute> inputAttributes)
        {
            if (inputAttributes == null)
                return;

            foreach (var inputAttribute in inputAttributes)
                _attributeAction.Add(node, xDoc, inputAttribute);
        }

        private void RemoveExistingAttributes(XmlNode node,
            ICollection<InputAttribute> existingAttributes)
        {
            if (existingAttributes == null)
                return;

            foreach (var existingAttribute in existingAttributes)
                _attributeAction.Remove(node, existingAttribute);

            _attributeAction.Remove(node, new InputAttribute("autoCompleteControl:AutoCompleteControl", "Margin"));
        }

        private void SwapControls(List<XmlNode> nodes, XmlDocument xDoc, string oldControlName, string newControl)
        {
            foreach (var node in nodes)
                SwapControl(node, xDoc, oldControlName, newControl);
        }

        private void SwapControl(XmlNode node, XmlDocument xDoc, string oldControlName, string newControl)
        {
            if (node.LocalName != oldControlName)
                return;

            var tabControl = xDoc.CreateNode(XmlNodeType.Element, newControl,
                "http://schemas.microsoft.com/winfx/2006/xaml/presentation");

            var nodes = node.ChildNodes.Cast<XmlNode>().ToList();
            nodes.ForEach(xmlNode => tabControl.AppendChild(xmlNode));

            var nodeAttributes = node.Attributes;
            if (nodeAttributes != null)
            {
                //TODO finish getfieldsbycontroltype
                var newControlAttributes = ControlHelper.GetFieldsByControlType(newControl).Select(x => x.Name).ToList();
                var attributes = new List<XmlAttribute>(nodeAttributes.Cast<XmlAttribute>()
                    .Where(attr => newControlAttributes.Contains(attr.Name)));

                attributes.ForEach(attr => tabControl.Attributes?.Append(attr));
            }

            var parentNode = node.ParentNode;
            parentNode?.ReplaceChild(tabControl, node);
        }

        public void GoToCleaner(XmlNode node, XmlDocument xDoc, IScrubberOptions options)
        {
            ClearComments(node, options.ClearComments);
            AddInputAttribute(node, xDoc, options.InputAttributes);
            RemoveExistingAttributes(node, options.ExistingAttributes);
            SwapControls(Enumerable.Empty<XmlNode>().ToList(), xDoc, string.Empty, string.Empty);
        }
    }
}   
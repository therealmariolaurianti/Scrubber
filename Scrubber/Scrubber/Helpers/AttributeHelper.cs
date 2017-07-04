using System.Collections.Generic;
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
    }
}
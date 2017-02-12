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
        private readonly AttributeAction _attributeAction;

        public AttributeHelper(AttributeAction attributeAction)
        {
            _attributeAction = attributeAction;
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
                _attributeAction.AddAttributeToNode(node, xDoc, new AdditionalAttribute(CommonAttributes.IsTabStop, false));
        }

        public void InitialClean(XmlNode node, XmlDocument xDoc)
        {
            DisableTabStopForContainers(node, xDoc);

            //TEMP BECUASE OF SYNCFUSION BULLSHIT
            _attributeAction.AddFontSizeToTabItems(node, xDoc);
            _attributeAction.AddPaddingToGroupBox(node, xDoc);

            //_attributeAction.RemoveFontSizeFromTabItems(node);
            //_attributeAction.RemovePaddingFromGroupBox(node);
        }
    }
}
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using Castle.Core.Internal;
using Scrubber.Extensions;
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

        private static void ClearComments(XmlNode node, bool clearComments)
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

            _attributeAction.AddMany(node, xDoc, inputAttributes);
        }

        private void RemoveAttributes(XmlNode node,
            ICollection<InputAttribute> existingAttributes)
        {
            if (existingAttributes == null)
                return;

            _attributeAction.RemoveMany(node, existingAttributes);
        }
        
        public void GoToCleaners(XmlNode node, XmlDocument xDoc, IScrubberOptions options)
        {
            ClearComments(node, options.ClearComments);
            AddInputAttribute(node, xDoc, options.InputAttributes);
            RemoveAttributes(node, options.ExistingAttributes);
        }

        //public List<XmlNode> OrderNodes(List<XmlNode> nodes)
        //{
        //    var rowCount = 0;
        //    var columnCount = 0;
        //    var nodesWithoutAssociatedControl = new List<GridXmlNode>();
        //    var orderedNodes = new List<XmlNode>();

        //    var nodesWithAttributes = nodes
        //        .Where(n => n.Attributes?["Grid.Column"] != null
        //                    && n.Attributes["Grid.Row"] != null).ToList();

        //    var maxColumns = nodesWithAttributes.FindMaxGridValue("Grid.Column");
        //    var maxRows = nodesWithAttributes.FindMaxGridValue("Grid.Row");

        //    while (!nodesWithAttributes.IsNullOrEmpty())
        //        foreach (var xmlNode in nodesWithAttributes.ToList())
        //        {
        //            var columnValue = xmlNode.GetAttributeValue("Grid.Column");
        //            var rowValue = xmlNode.GetAttributeValue("Grid.Row");

        //            if (!xmlNode.HasAssociatedControl(nodesWithAttributes, columnValue, rowValue))
        //            {
        //                nodesWithoutAssociatedControl.Add(new GridXmlNode(xmlNode));
        //                nodesWithAttributes.Remove(xmlNode);
        //                continue;
        //            }

        //            if (maxRows < rowCount)
        //                continue;
        //            if (columnValue != columnCount)
        //                continue;
        //            if (rowValue != rowCount)
        //                continue;

        //            var nodesWithoutExplicitDeclaration = new List<XmlNode>();
        //            if (nodesWithoutAssociatedControl.Any(nwac => nwac.Row == rowValue))
        //                nodesWithoutExplicitDeclaration =
        //                    nodesWithoutAssociatedControl.Where(nwac => nwac.Row == rowValue)
        //                        .Select(n => n.XmlNode)
        //                        .ToList();

        //            orderedNodes.AddRange(nodesWithoutExplicitDeclaration);
        //            orderedNodes.Add(xmlNode);
        //            nodesWithAttributes.Remove(xmlNode);

        //            if (maxColumns > columnCount)
        //            {
        //                columnCount++;
        //            }
        //            else
        //            {
        //                rowCount++;
        //                columnCount = 0;
        //            }
        //        }

        //    var unorderedNodes = nodes.Where(n => !orderedNodes.Contains(n)).ToList();
        //    unorderedNodes.AddRange(orderedNodes);

        //    return unorderedNodes;
        //}
    }
}   
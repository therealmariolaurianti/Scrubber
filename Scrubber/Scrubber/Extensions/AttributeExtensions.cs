using System.Collections.Generic;
using System.Xml;

namespace Scrubber.Extensions
{
    public static class AttributeExtensions
    {
        public static bool HasAssociatedControl(this XmlNode node, List<XmlNode> nodes,
            int columnValue, int rowValue)
        {
            foreach (var xmlNode in nodes)
            {
                var xmlNodeColumnValue = xmlNode.GetAttributeValue("Grid.Column");
                var xmlNodeRowValue = xmlNode.GetAttributeValue("Grid.Row");

                return xmlNodeColumnValue == columnValue + 1 &&
                       xmlNodeRowValue == rowValue;
            }

            return false;
        }

        public static int GetAttributeValue(this XmlNode node, string attributeName)
        {
            var attributes = node.Attributes;
            var attribute = attributes?[attributeName];
            if (attribute?.Value == null)
                return -1;

            var attributeValue = int.Parse(attribute?.Value);
            return attributeValue;
        }

        public static void RemoveExistingAttribute(this XmlNode node, string attributeName)
        {
            var attribute = node.Attributes?[attributeName];
            if (attribute != null)
                node.Attributes.Remove(attribute);
        }

        public static int FindMaxValue(this List<XmlNode> nodes, string attribute)
        {
            var maxColumn = 0;
            foreach (var orderedNode in nodes)
            {
                var gridColumn = orderedNode.Attributes?[attribute];
                if (gridColumn == null)
                    continue;

                var column = int.Parse(gridColumn.Value);
                if (column > maxColumn)
                    maxColumn = column;
            }
            return maxColumn;
        }
    }
}
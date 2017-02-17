using System.Collections.Generic;
using System.Xml;
using Scrubber.Enums;

namespace Scrubber.Extensions
{
    public static class AttributeExtensions
    {
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
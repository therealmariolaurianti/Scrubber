using System.Xml;
using Scrubber.Extensions;

namespace Scrubber.Objects
{
    public class GridXmlNode
    {
        public GridXmlNode(XmlNode xmlNode)
        {
            XmlNode = xmlNode;
        }
        public XmlNode XmlNode { get; set; }
        public int Column => XmlNode.GetAttributeValue(GridProperties.Column);
        public int Row => XmlNode.GetAttributeValue(GridProperties.Row);
    }
}
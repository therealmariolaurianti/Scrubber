using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using NLog;
using Scrubber.Extensions;
using Scrubber.Helpers;
using Scrubber.Objects;
using Scrubber.Options;

namespace Scrubber.Workers
{
    public class Soap
    {
        private readonly AttributeHelper _attributeHelper;
        private readonly Logger _logger;
        private readonly IScrubberOptions _options;

        public Soap(Logger logger, AttributeHelper attributeHelper, IScrubberOptions options)
        {
            _logger = logger;
            _attributeHelper = attributeHelper;
            _options = options;
        }

        private static int AttributeCountTolerance => 1;
        private static string IndentString => "\t";

        //Entry
        public void Scrub(DirtyFile dirtyFile)
        {
            Run(dirtyFile);
        }

        private void ProcessNodes(string filePath)
        {
            var xDoc = new XmlDocument();
            xDoc.Load(filePath);

            var root = xDoc.DocumentElement;
            if (root != null)
                foreach (XmlNode rootChildNode in root.ChildNodes)
                    ProcessChildNode(rootChildNode, xDoc);

            xDoc.Save(filePath);
        }

        private void ProcessChildNode(XmlNode node, XmlDocument xDoc)
        {
            var childNodes = node.ChildNodes.OfType<XmlNode>().ToList();
            foreach (var childNode in childNodes)
                ProcessChildNode(childNode, xDoc);

            _attributeHelper.GoToCleaners(node, xDoc, _options);

            if (node.Name.Equals(Grid))
                ProcessGrid(node, xDoc);
        }

        private void ProcessGrid(XmlNode node, XmlDocument xDoc)
        {
            if (node?.Attributes == null)
                return;

            var nodes = node.ChildNodes.Cast<XmlNode>().ToList();
            if (!nodes.Any())
                return;

            var orderedNodes = OrderNodesByColumn(nodes).ToList();
            //var orderedNodes = OrderNodesByRow(nodes).ToList();

            foreach (var orderedNode in orderedNodes.Where(on => on.HasChildNodes))
                ProcessChildNode(orderedNode, xDoc);

            foreach (var orderedNode in orderedNodes)
                node.AppendChild(orderedNode);
        }

        private IEnumerable<XmlNode> OrderNodesByColumn(IReadOnlyCollection<XmlNode> nodes)
        {
            var rows = GetAttributeValues(nodes, GridProperties.Column).ToList();

            for (var nodeIndex = 0; nodeIndex <= rows.Max(); nodeIndex++)
            {
                var index = nodeIndex;
                var controlsInRow = nodes.Where(node => node.GetAttributeValue(GridProperties.Column) == index);
                var rowControls = controlsInRow.OrderBy(control => control.GetAttributeValue(GridProperties.Row)).ToList();

                foreach (var rowControl in rowControls)
                    yield return rowControl;
            }
        }

        private IEnumerable<XmlNode> OrderNodesByRow(IReadOnlyCollection<XmlNode> nodes)
        {
            var rows = GetAttributeValues(nodes, GridProperties.Row).ToList();

            for (var nodeIndex = 0; nodeIndex <= rows.Max(); nodeIndex++)
            {
                var index = nodeIndex;
                var controlsInRow = nodes.Where(node => node.GetAttributeValue(GridProperties.Row) == index);
                var rowControls = controlsInRow.OrderBy(control => control.GetAttributeValue(GridProperties.Column)).ToList();

                foreach (var rowControl in rowControls)
                    yield return rowControl;
            }
        }

        public IEnumerable<int> GetAttributeValues(IEnumerable<XmlNode> nodes, string attributeName)
        {
            foreach (var node in nodes)
            {
                var attributes = node.Attributes;
                var attribute = attributes?[attributeName];

                if (attribute?.Value == null)
                    yield return -1;
                if (attribute?.Value == null)
                    continue;

                var attributeValue = int.Parse(attribute?.Value);
                yield return attributeValue;
            }
        }

        private static string Grid => "Grid";

        private void Run(DirtyFile dirtyFile)
        {
            try
            {
                ProcessNodes(dirtyFile.FilePath);

                if(_options.FormatFiles)
                    Format(dirtyFile);
            }
            catch (Exception exception)
            {
                dirtyFile.IsClean = false;
                _logger.Error($"FileName: {dirtyFile.FileName}, Exception: {exception.Message}");
            }
        }

        private static void Format(DirtyFile dirtyFile)
        {
            var path = dirtyFile.FilePath;
            var fileContent = File.ReadAllText(path);

            var stream = new MemoryStream(fileContent.Length);

            var writer = new StreamWriter(stream);
            writer.Write(fileContent);
            writer.Flush();

            stream.Seek(0L, SeekOrigin.Begin);
            var streamReader = new StreamReader(stream);

            var xmlReader = XmlReader.Create(streamReader.BaseStream);
            xmlReader.Read();

            var cleanedFile = "";
            while (!xmlReader.EOF)
            {
                string str2;
                int num;
                int num2;
                int num3;
                int num4;
                switch (xmlReader.NodeType)
                {
                    case XmlNodeType.Comment:
                    {
                        str2 = "";
                        num4 = 0;
                        goto Label_0465;
                    }

                    case XmlNodeType.Whitespace:
                    {
                        xmlReader.Read();
                        continue;
                    }
                    case XmlNodeType.EndElement:
                    {
                        str2 = "";
                        num2 = 0;
                        goto Label_039D;
                    }

                    case XmlNodeType.Element:
                    {
                        str2 = "";
                        num = 0;
                        goto Label_015F;
                    }

                    case XmlNodeType.Text:
                    {
                        var str7 =
                            xmlReader.Value.Replace("&", "&amp;")
                                .Replace("<", "&lt;")
                                .Replace(">", "&gt;")
                                .Replace("\"", "&quot;");
                        cleanedFile = cleanedFile + str7;
                        xmlReader.Read();
                        continue;
                    }
                    case XmlNodeType.ProcessingInstruction:
                    {
                        str2 = "";
                        num3 = 0;
                        goto Label_040D;
                    }

                    default:
                        goto Label_04CD;
                }

                Label_014A:
                str2 = str2 + IndentString;
                num++;
                Label_015F:
                if (num < xmlReader.Depth)
                    goto Label_014A;
                var name = xmlReader.Name;
                var str4 = cleanedFile;
                string[] textArray1 = {str4, "\r\n", str2, "<", xmlReader.Name};
                cleanedFile = string.Concat(textArray1);
                var isEmptyElement = xmlReader.IsEmptyElement;
                if (xmlReader.HasAttributes)
                {
                    var list = new List<AttributeValuePair>(xmlReader.AttributeCount);
                    for (var i = 0; i < xmlReader.AttributeCount; i++)
                    {
                        xmlReader.MoveToAttribute(i);
                        if (!AttributeValuePair.IsCommonDefault(name, xmlReader.Name, xmlReader.Value))
                            list.Add(new AttributeValuePair(name, xmlReader.Name, xmlReader.Value));
                    }
                    list.Sort();
                    str2 = "";
                    var str8 = "";
                    var depth = xmlReader.Depth;
                    for (var j = 0; j < depth; j++)
                        str2 = str2 + IndentString;
                    foreach (var pair in list)
                    {
                        var str9 = cleanedFile;
                        if (list.Count > AttributeCountTolerance && !AttributeValuePair.ForceNoLineBreaks(name))
                        {
                            string[] textArray2 = {str9, "\r\n", str2, str8, pair.Name, "=\"", pair.Value, "\""};
                            cleanedFile = string.Concat(textArray2);
                        }
                        else
                        {
                            string[] textArray3 = {str9, " ", pair.Name, "=\"", pair.Value, "\""};
                            cleanedFile = string.Concat(textArray3);
                        }
                    }
                }
                if (isEmptyElement)
                    cleanedFile = cleanedFile + "/";
                cleanedFile = cleanedFile + ">";
                xmlReader.Read();
                continue;
                Label_0388:
                str2 = str2 + IndentString;
                num2++;
                Label_039D:
                if (num2 < xmlReader.Depth)
                    goto Label_0388;
                var str5 = cleanedFile;
                string[] textArray4 = {str5, "\r\n", str2, "</", xmlReader.Name, ">"};
                cleanedFile = string.Concat(textArray4);
                xmlReader.Read();
                continue;
                Label_03F8:
                str2 = str2 + "    ";
                num3++;
                Label_040D:
                if (num3 < xmlReader.Depth)
                    goto Label_03F8;
                var str6 = cleanedFile;
                string[] textArray5 = {str6, "\r\n", str2, "<?Mapping ", xmlReader.Value, " ?>"};
                cleanedFile = string.Concat(textArray5);
                xmlReader.Read();
                continue;
                Label_0465:
                if (num4 < xmlReader.Depth)
                {
                    str2 = str2 + IndentString;
                    num4++;
                }
                string[] textArray6 = {cleanedFile, "\r\n", str2, "<!--", xmlReader.Value, "-->"};
                cleanedFile = string.Concat(textArray6);
                xmlReader.Read();
                continue;
                Label_04CD:
                xmlReader.Read();
            }

            xmlReader.Close();

            File.WriteAllText(dirtyFile.FilePath, cleanedFile);
            dirtyFile.IsClean = true;
        }
    }
}
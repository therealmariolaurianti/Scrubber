using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using NLog;
using Scrubber.Helpers;
using Scrubber.Objects;

namespace Scrubber.Workers
{
    public class Soap
    {
        private readonly Logger _logger;

        public Soap(Logger logger)
        {
            _logger = logger;
        }

        private static readonly List<string> IncludeList = new List<string>
        {
            "TextBox",
            "TypeEntityComboBox",
            "CheckBox",
            "UpDown",
            "Button",
            "RadioButton",
            "DoubleTextBox",
            "DateTimeEdit",
            "AutoCompleteControl"
        };

        private static int AttributeCountTolerance => 1;
        private static string IndentString => "\t";
        private int _counter = 1;
        
        private void AddTabAttribute(string filePath)
        {
            var xDoc = new XmlDocument();
            xDoc.Load(filePath);

            var root = xDoc.DocumentElement;
            if (root != null)
            {
                foreach (XmlNode rootChildNode in root.ChildNodes)
                {
                    ProcessChildNode(rootChildNode, xDoc);
                }
            }

            xDoc.Save(filePath);
        }

        private void ProcessChildNode(XmlNode node, XmlDocument xDoc)
        {
            foreach (XmlNode childNode in node.ChildNodes)
            {
                ProcessChildNode(childNode, xDoc);
            }

            if (!IncludeList.Any(node.Name.Contains))
                return;

            if (node.Attributes != null)
            {
                var existingTabIndexAttribute = node.Attributes["TabIndex"];

                if (existingTabIndexAttribute.Specified)
                    node.Attributes.Remove(existingTabIndexAttribute);
            }

            var tabIndexAttribute = xDoc.CreateAttribute("TabIndex");
            tabIndexAttribute.Value = _counter.ToString();

            node.Attributes?.Append(tabIndexAttribute);
            _counter++;
        }

        public void Scrub(DirtyFile dirtyFile)
        {
            FormatAndOrder(dirtyFile);
        }


        private void FormatAndOrder(DirtyFile dirtyFile)
        {
            try
            {
                AddTabAttribute(dirtyFile.FilePath);

                var reader2 = StartXmlReader(dirtyFile.FilePath);
                reader2.Read();
                var cleanedFile = "";
                while (!reader2.EOF)
                {
                    string str2;
                    int num;
                    int num2;
                    int num3;
                    int num4;
                    switch (reader2.NodeType)
                    {
                        case XmlNodeType.Comment:
                        {
                            str2 = "";
                            num4 = 0;
                            goto Label_0465;
                        }

                        case XmlNodeType.Whitespace:
                        {
                            reader2.Read();
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
                                reader2.Value.Replace("&", "&amp;")
                                    .Replace("<", "&lt;")
                                    .Replace(">", "&gt;")
                                    .Replace("\"", "&quot;");
                            cleanedFile = cleanedFile + str7;
                            reader2.Read();
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
                    if (num < reader2.Depth)
                        goto Label_014A;
                    var name = reader2.Name;
                    var str4 = cleanedFile;
                    string[] textArray1 = {str4, "\r\n", str2, "<", reader2.Name};
                    cleanedFile = string.Concat(textArray1);
                    var isEmptyElement = reader2.IsEmptyElement;
                    if (reader2.HasAttributes)
                    {
                        var list = new List<AttributeValuePair>(reader2.AttributeCount);
                        for (var i = 0; i < reader2.AttributeCount; i++)
                        {
                            reader2.MoveToAttribute(i);
                            if (!AttributeValuePair.IsCommonDefault(name, reader2.Name, reader2.Value))
                                list.Add(new AttributeValuePair(name, reader2.Name, reader2.Value));
                        }
                        list.Sort();
                        str2 = "";
                        var str8 = "";
                        var depth = reader2.Depth;
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
                    reader2.Read();
                    continue;
                    Label_0388:
                    str2 = str2 + IndentString;
                    num2++;
                    Label_039D:
                    if (num2 < reader2.Depth)
                        goto Label_0388;
                    var str5 = cleanedFile;
                    string[] textArray4 = {str5, "\r\n", str2, "</", reader2.Name, ">"};
                    cleanedFile = string.Concat(textArray4);
                    reader2.Read();
                    continue;
                    Label_03F8:
                    str2 = str2 + "    ";
                    num3++;
                    Label_040D:
                    if (num3 < reader2.Depth)
                        goto Label_03F8;
                    var str6 = cleanedFile;
                    string[] textArray5 = {str6, "\r\n", str2, "<?Mapping ", reader2.Value, " ?>"};
                    cleanedFile = string.Concat(textArray5);
                    reader2.Read();
                    continue;
                    Label_0465:
                    if (num4 < reader2.Depth)
                    {
                        str2 = str2 + IndentString;
                        num4++;
                    }
                    string[] textArray6 = {cleanedFile, "\r\n", str2, "<!--", reader2.Value, "-->"};
                    cleanedFile = string.Concat(textArray6);
                    reader2.Read();
                    continue;
                    Label_04CD:
                    reader2.Read();
                }

                reader2.Close();

                cleanedFile.WriteTextToFile(dirtyFile.FilePath);
                dirtyFile.IsClean = true;
            }
            catch (Exception exception)
            {
                dirtyFile.IsClean = false;
                _logger.Error($"FileName: {dirtyFile.FileName}, Exception: {exception.Message}");
            }
        }

        private XmlReader StartXmlReader(string dirtyFile, bool isFilePath = true)
        {
            var fileContent = isFilePath ? File.ReadAllText(dirtyFile) : dirtyFile;
            var stream = new MemoryStream(fileContent.Length);
            var writer = new StreamWriter(stream);
            writer.Write(fileContent);
            writer.Flush();
            stream.Seek(0L, SeekOrigin.Begin);
            var reader = new StreamReader(stream);
            var reader2 = XmlReader.Create(reader.BaseStream);
            return reader2;
        }
    }
}
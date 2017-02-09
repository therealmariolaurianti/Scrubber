using System.Collections.Generic;
using System.IO;
using System.Xml;
using Scrubber.Helpers;
using Scrubber.Objects;

namespace Scrubber.Workers
{
    public class Soap
    {
        private static int AttributeCounteTolerance => 1;
        private static string IndentString => "\t";

        private bool Format(DirtyFile dirtyFile)
        {
            var str = Indent(File.ReadAllText(dirtyFile.File));
            var fileSpinResult = Order(str);
            return WriteToFile(dirtyFile, fileSpinResult);
        }

        private static string Indent(string fileContent)
        {
            var stream = new MemoryStream(fileContent.Length);
            var writer = new StreamWriter(stream);
            writer.Write(fileContent);
            writer.Flush();
            stream.Seek(0L, SeekOrigin.Begin);
            var reader = new StreamReader(stream);
            var reader2 = XmlReader.Create(reader.BaseStream);
            reader2.Read();
            var str = "";
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
                        str2 = "";
                        num4 = 0;
                        goto Label_0465;

                    case XmlNodeType.Whitespace:
                    {
                        reader2.Read();
                        continue;
                    }
                    case XmlNodeType.EndElement:
                        str2 = "";
                        num2 = 0;
                        goto Label_039D;

                    case XmlNodeType.Element:
                        str2 = "";
                        num = 0;
                        goto Label_015F;

                    case XmlNodeType.Text:
                    {
                        var str7 =
                            reader2.Value.Replace("&", "&amp;")
                                .Replace("<", "&lt;")
                                .Replace(">", "&gt;")
                                .Replace("\"", "&quot;");
                        str = str + str7;
                        reader2.Read();
                        continue;
                    }
                    case XmlNodeType.ProcessingInstruction:
                        str2 = "";
                        num3 = 0;
                        goto Label_040D;

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
                var str4 = str;
                string[] textArray1 = {str4, "\r\n", str2, "<", reader2.Name};
                str = string.Concat(textArray1);
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
                        var str9 = str;
                        if (list.Count > AttributeCounteTolerance && !AttributeValuePair.ForceNoLineBreaks(name))
                        {
                            string[] textArray2 = {str9, "\r\n", str2, str8, pair.Name, "=\"", pair.Value, "\""};
                            str = string.Concat(textArray2);
                        }
                        else
                        {
                            string[] textArray3 = {str9, " ", pair.Name, "=\"", pair.Value, "\""};
                            str = string.Concat(textArray3);
                        }
                    }
                }
                if (isEmptyElement)
                    str = str + "/";
                str = str + ">";
                reader2.Read();
                continue;
                Label_0388:
                str2 = str2 + IndentString;
                num2++;
                Label_039D:
                if (num2 < reader2.Depth)
                    goto Label_0388;
                var str5 = str;
                string[] textArray4 = {str5, "\r\n", str2, "</", reader2.Name, ">"};
                str = string.Concat(textArray4);
                reader2.Read();
                continue;
                Label_03F8:
                str2 = str2 + "    ";
                num3++;
                Label_040D:
                if (num3 < reader2.Depth)
                    goto Label_03F8;
                var str6 = str;
                string[] textArray5 = {str6, "\r\n", str2, "<?Mapping ", reader2.Value, " ?>"};
                str = string.Concat(textArray5);
                reader2.Read();
                continue;
                Label_0465:
                if (num4 < reader2.Depth)
                {
                    str2 = str2 + IndentString;
                    num4++;
                }
                string[] textArray6 = {str, "\r\n", str2, "<!--", reader2.Value, "-->"};
                str = string.Concat(textArray6);
                reader2.Read();
                continue;
                Label_04CD:
                reader2.Read();
            }
            reader2.Close();
            return str;
        }

        private Result<string> Order(string dirtyFile) => Result<string>.CreateSuccess(dirtyFile);

        public void Scrub(DirtyFile dirtyFile) => dirtyFile.IsClean = Format(dirtyFile);

        private static bool WriteToFile(DirtyFile dirtyFile, Result<string> fileSpinResult)
        {
            if (!fileSpinResult.Success)
                return false;
            File.WriteAllText(dirtyFile.File, fileSpinResult.ResultValue);
            return true;
        }
    }
}
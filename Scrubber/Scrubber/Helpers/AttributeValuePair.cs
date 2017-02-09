using System;

namespace Scrubber
{
    public class AttributeValuePair : IComparable
    {
        public readonly AttributeType AttributeType = AttributeType.Other;
        public readonly string Name = "";
        public readonly string Value = "";

        public AttributeValuePair(string elementname, string name, string value)
        {
            Name = name;
            Value = value;

            // compute the AttributeType
            if (name.StartsWith("xmlns"))
                AttributeType = AttributeType.Namespace;
            else
                switch (name)
                {
                    case "Key":
                    case "x:Key":
                        AttributeType = AttributeType.Key;
                        break;

                    case "Name":
                    case "x:Name":
                        AttributeType = AttributeType.Name;
                        break;

                    case "x:Class":
                        AttributeType = AttributeType.Class;
                        break;

                    case "Canvas.Top":
                    case "Canvas.Left":
                    case "Canvas.Bottom":
                    case "Canvas.Right":
                    case "Grid.Row":
                    case "Grid.RowSpan":
                    case "Grid.Column":
                    case "Grid.ColumnSpan":
                        AttributeType = AttributeType.AttachedLayout;
                        break;

                    case "Width":
                    case "Height":
                    case "MaxWidth":
                    case "MinWidth":
                    case "MinHeight":
                    case "MaxHeight":
                        AttributeType = AttributeType.CoreLayout;
                        break;

                    case "Margin":
                    case "VerticalAlignment":
                    case "HorizontalAlignment":
                    case "Panel.ZIndex":
                        AttributeType = AttributeType.StandardLayout;
                        break;

                    case "mc:Ignorable":
                    case "d:IsDataSource":
                    case "d:LayoutOverrides":
                    case "d:IsStaticText":

                        AttributeType = AttributeType.BlendGoo;
                        break;

                    default:
                        AttributeType = AttributeType.Other;
                        break;
                }
        }

        #region IComparable Members

        public int CompareTo(object obj)
        {
            var other = obj as AttributeValuePair;

            if (other != null)
                if (AttributeType == other.AttributeType)
                {
                    // some common special cases where we want things to be out of the normal order

                    if (Name.Equals("StartPoint") && other.Name.Equals("EndPoint")) return -1;
                    if (Name.Equals("EndPoint") && other.Name.Equals("StartPoint")) return 1;

                    if (Name.Equals("Width") && other.Name.Equals("Height")) return -1;
                    if (Name.Equals("Height") && other.Name.Equals("Width")) return 1;

                    if (Name.Equals("Offset") && other.Name.Equals("Color")) return -1;
                    if (Name.Equals("Color") && other.Name.Equals("Offset")) return 1;

                    if (Name.Equals("TargetName") && other.Name.Equals("Property")) return -1;
                    if (Name.Equals("Property") && other.Name.Equals("TargetName")) return 1;

                    return Name.CompareTo(other.Name);
                }
                else
                {
                    return AttributeType.CompareTo(other.AttributeType);
                }

            return 0;
        }

        public static bool IsCommonDefault(string elementname, string name, string value)
        {
            if (
                name == "HorizontalAlignment" && value == "Stretch" ||
                name == "VerticalAlignment" && value == "Stretch" ||
                name == "Margin" && value == "0" ||
                name == "Margin" && value == "0,0,0,0" ||
                name == "Opacity" && value == "1" ||
                name == "FontWeight" && value == "{x:Null}" ||
                name == "Background" && value == "{x:Null}" ||
                name == "Stroke" && value == "{x:Null}" ||
                name == "Fill" && value == "{x:Null}" ||
                name == "Visibility" && value == "Visible" ||
                name == "Grid.RowSpan" && value == "1" ||
                name == "Grid.ColumnSpan" && value == "1" ||
                name == "BasedOn" && value == "{x:Null}" ||

                //(elementname == "ScaleTransform" && name == "ScaleX" && value == "1") ||
                //(elementname == "ScaleTransform" && name == "ScaleY" && value == "1") ||
                //(elementname == "SkewTransform" && name == "AngleX" && value == "0") ||
                //(elementname == "SkewTransform" && name == "AngleY" && value == "0") ||
                //(elementname == "RotateTransform" && name == "Angle" && value == "0") ||
                //(elementname == "TranslateTransform" && name == "X" && value == "0") ||
                //(elementname == "TranslateTransform" && name == "Y" && value == "0") ||
                elementname != "ColumnDefinition" && elementname != "RowDefinition" && name == "Width" &&
                value == "Auto" ||
                elementname != "ColumnDefinition" && elementname != "RowDefinition" && name == "Height" &&
                value == "Auto"
            )
                return true;

            return false;
        }

        public static bool ForceNoLineBreaks(string elementname)
        {
            if (
                elementname == "RadialGradientBrush" ||
                elementname == "GradientStop" ||
                elementname == "LinearGradientBrush" ||
                elementname == "ScaleTransfom" ||
                elementname == "SkewTransform" ||
                elementname == "RotateTransform" ||
                elementname == "TranslateTransform" ||
                elementname == "Trigger" ||
                elementname == "Setter"
            )
                return true;
            return false;
        }

        #endregion
    }
}
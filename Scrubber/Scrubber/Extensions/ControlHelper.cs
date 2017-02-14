using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Scrubber.Objects;
using Control = Scrubber.Objects.Control;

namespace Scrubber.Extensions
{
    public static class ControlHelper
    {
        //reflection to get types and attributes
        public static readonly List<Control> Controls = new List<Control>
        {
            new Control(nameof(Grid)),
            new Control(nameof(DockPanel)),
            new Control(nameof(StackPanel)),
            new Control(nameof(Expander)),
            new Control(nameof(Button)),
            new Control(nameof(TextBox)),
            new Control(nameof(GroupBox))
        };

        public static List<ControlAttribute> GetFieldsByControlType(string controlName)
        {
            var type = FindType($"System.Windows.Controls.{controlName}");
            if (type == null)
                return Enumerable.Empty<ControlAttribute>().ToList();

            var fields = typeof(FrameworkElement).GetFields()
                .Where(field => !field.Name.Contains("Event") ||
                !field.Name.Contains("Style")).ToList();

            fields.AddRange(type.GetFields().ToList());

            var controlAttributes = fields.Select(fieldInfo => new ControlAttribute(fieldInfo.Name.FormatAttribute(), controlName)).OrderBy(x => x.Name).ToList();
            return controlAttributes;
        }

        public static List<AttributeValue> GetValuesByAttribute(string attributeName, string controlName)
        {
            var type = FindType($"System.Windows.Controls.{controlName}");
            var fieldValues = type.GetProperties().SingleOrDefault(field => field.Name == attributeName);
            var propertyType = fieldValues?.PropertyType.GetFields();

            return propertyType?.Select(t => new AttributeValue(t.Name)).ToList();
        }

        private static Type FindType(string qualifiedTypeName)
        {
            var type = Type.GetType(qualifiedTypeName);

            if (type != null)
            {
                return type;
            }
            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                type = assembly.GetType(qualifiedTypeName);
                if (type != null)
                    return type;
            }

            return null;
        }

       
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows.Controls;
using Scrubber.Extensions;
using Scrubber.Objects;
using Control = Scrubber.Objects.Control;

namespace Scrubber.Helpers
{
    public static class ControlHelper
    {
        public static readonly List<Control> Controls = new List<Control>
        {
            new Control(nameof(Grid)),
            new Control(nameof(DockPanel)),
            new Control(nameof(StackPanel)),
            new Control(nameof(Expander)),
            new Control(nameof(Button)),
            new Control(nameof(TextBox)),
            new Control(nameof(GroupBox)),
            new Control(nameof(TabControl)),
            new Control(nameof(TabItem)),
            new Control(nameof(RadioButton)),
            new Control(nameof(Border)),
            new Control(nameof(CheckBox)),
            new Control(nameof(ContentControl)),
            new Control(nameof(TextBlock)),
        };

        public static List<ControlAttribute> GetFieldsByControlType(string controlName)
        {
            var type = FindType(controlName);
            if (type == null)
                return Enumerable.Empty<ControlAttribute>().ToList();

            var fields = type.GetProperties(BindingFlags.Public | BindingFlags.Instance).ToList();

            var controlAttributes = fields.Select(fieldInfo =>
                    new ControlAttribute(fieldInfo.Name.FormatAttribute(), controlName)).ToList();
            return controlAttributes;
        }

        public static List<AttributeValue> GetValuesByAttribute(string attributeName, string controlName)
        {
            var type = FindType(controlName);
            if (type == null)
                return Enumerable.Empty<AttributeValue>().ToList();

            var fields = new List<FieldInfo>();

            var fieldValues = type.GetProperties().SingleOrDefault(field => field.Name == attributeName);
            if (fieldValues == null)
            {
                var attributeType = FindType(attributeName);
                if (attributeType != null)
                    fields = attributeType.GetFields().ToList();
            }
            else
                fields = fieldValues.PropertyType.GetFields().ToList();

            RemoveDefaultValue(fields);
            return fields.Select(t => new AttributeValue(t.Name)).ToList();
        }

        private static void RemoveDefaultValue(List<FieldInfo> propertyType)
        {
            if (propertyType == null)
                return;

            var defaultValue = propertyType.SingleOrDefault(pt => pt.Name == "value__");
            if (defaultValue != null)
                propertyType.Remove(defaultValue);
        }

        private static Type FindType(string qualifiedTypeName)
        {
            var typeName = $"System.Windows.Controls.{qualifiedTypeName}";
            var type = Type.GetType(typeName);

            if (type != null)
                return type;

            var assembly = typeof(Grid).Assembly;
            type = assembly.GetType(typeName);
            return type;
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
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
            new Control(nameof(TabItem))
        };

        public static List<ControlAttribute> GetFieldsByControlType(string controlName)
        {
            var type = FindType(controlName);
            if (type == null)
                return Enumerable.Empty<ControlAttribute>().ToList();

            var fields = type.GetFields().ToList();

            var controlAttributes = fields.Select(fieldInfo =>
                    new ControlAttribute(fieldInfo.Name.FormatAttribute(), controlName))
                .OrderBy(x => x.Name).ToList();
            return controlAttributes;
        }

        public static List<AttributeValue> GetValuesByAttribute(string attributeName, string controlName)
        {
            var type = FindType(controlName);
            if (type == null)
                return Enumerable.Empty<AttributeValue>().ToList();

            var fieldValues = type.GetProperties().SingleOrDefault(field => field.Name == attributeName);
            if (fieldValues == null)
                return Enumerable.Empty<AttributeValue>().ToList();

            var propertyType = fieldValues.PropertyType.GetFields().ToList();

            RemoveDefaultValue(propertyType);

            return propertyType?.Select(t => new AttributeValue(t.Name)).ToList();
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
            {
                return type;
            }

            var assembly = typeof(Grid).Assembly;
            type = assembly.GetType(typeName);
            if (type != null)
                return type;


            return null;
        }
    }
}
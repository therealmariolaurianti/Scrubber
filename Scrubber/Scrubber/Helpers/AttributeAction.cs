using System;
using System.Collections.Generic;
using System.Xml;
using Scrubber.Extensions;
using Scrubber.Objects;

namespace Scrubber.Helpers
{
    public class AttributeAction
    {
        private static void Add(XmlNode node, XmlDocument xDoc, InputAttribute inputAttribute)
        {
            if (node.LocalName != inputAttribute.ControlName)
                return;

            if (node.Attributes != null && node.Attributes.Count > 0)
                node.RemoveExistingAttribute(inputAttribute.AttributeName);

            var attribute = xDoc.CreateAttribute(inputAttribute.AttributeName);

            if (inputAttribute.IsDesignTimeAttribute)
                attribute.Prefix = inputAttribute.NamespaceXmnlsCharacter;

            attribute.Value = inputAttribute.AttributeValue.ToString();

            if (node.Attributes != null)
                node.Attributes.Append(attribute);
            else
                throw new Exception();
        }

        private static void Remove(XmlNode node, InputAttribute attribute)
        {
            if (node.Name != attribute.ControlName)
                return;

            node.RemoveExistingAttribute(attribute.AttributeName);
        }

        public void AddMany(XmlNode node, XmlDocument xDoc, ICollection<InputAttribute> inputAttributes)
        {
            foreach (var inputAttribute in inputAttributes)
                Add(node, xDoc, inputAttribute);
            
            //Add(node, xDoc, new InputAttribute("DoubleTextBox", "NumberDecimalDigits", 5));
            //Add(node, xDoc, new InputAttribute("DoubleTextBox", "MinValue", 0));
        }

        public void RemoveMany(XmlNode node, ICollection<InputAttribute> existingAttributes)
        {
            foreach (var existingAttribute in existingAttributes)
                Remove(node, existingAttribute);
        }
    }
}
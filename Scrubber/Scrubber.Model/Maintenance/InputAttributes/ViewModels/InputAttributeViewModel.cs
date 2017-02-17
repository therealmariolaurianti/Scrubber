using System.Collections.Generic;
using Scrubber.Extensions;
using Scrubber.Helpers;
using Scrubber.Maintenance;
using Scrubber.Objects;

namespace Scrubber.Model.Maintenance.InputAttributes.ViewModels
{
    public class InputAttributeViewModel : ViewModel<InputAttribute>
    {
        private Control _selectedControl;
        private ControlAttribute _selectedAttribute;
        private AttributeValue _selectedAttributeValue;

        public string ControlName
        {
            get { return Item.ControlName; }
            set
            {
                if (value == Item.ControlName) return;
                Item.ControlName = value;
                NotifyOfPropertyChange();
            }
        }

        public string AttributeName
        {
            get { return Item.AttributeName; }
            set
            {
                if (value == Item.AttributeName) return;
                Item.AttributeName = value;
                NotifyOfPropertyChange();
            }
        }

        public object AttributeValue
        {
            get { return Item.AttributeValue; }
            set
            {
                if (Equals(value, Item.AttributeValue)) return;
                Item.AttributeValue = value;
                NotifyOfPropertyChange();
            }
        }

        public List<Control> Controls => ControlHelper.Controls;
        public List<ControlAttribute> ControlAttributes => SelectedControl?.ControlAttributes;
        public List<AttributeValue> AttributeValues => SelectedAttribute?.AttributeValues;

        public Control SelectedControl
        {
            get { return _selectedControl; }
            set
            {
                if (Equals(value, _selectedControl)) return;
                _selectedControl = value;
                NotifyOfPropertyChange();
                NotifyOfPropertyChange(nameof(ControlAttributes));

                Item.ControlName = value != null ? value.Name : string.Empty;
            }
        }

        public ControlAttribute SelectedAttribute
        {
            get { return _selectedAttribute; }
            set
            {
                if (Equals(value, _selectedAttribute)) return;
                _selectedAttribute = value;
                NotifyOfPropertyChange();
                NotifyOfPropertyChange(nameof(AttributeValues));

                if (value != null)
                {
                    Item.AttributeName = value.Name;
                    NotifyOfPropertyChange(nameof(Item.NamespaceXmnlsCharacter));
                }
                else
                {
                    Item.AttributeName = string.Empty;
                }
            }
        }

        public AttributeValue SelectedAttributeValue
        {
            get { return _selectedAttributeValue; }
            set
            {
                if (Equals(value, _selectedAttributeValue)) return;
                _selectedAttributeValue = value;
                NotifyOfPropertyChange();

                if (value != null)
                {
                    Item.AttributeValue = value.Name;
                    NotifyOfPropertyChange(nameof(Item.IsDesignTimeAttribute));
                }
                else
                {
                    Item.AttributeValue = null;
                }
            }
        }
    }
}
using System.Collections.Generic;
using System.Linq;
using Scrubber.Helpers;
using Scrubber.Maintenance;
using Scrubber.Objects;

namespace Scrubber.Model.Maintenance.InputAttributes.ViewModels
{
    public class InputAttributeViewModel : ViewModel<InputAttribute>
    {
        private ControlAttribute _selectedAttribute;
        private AttributeValue _selectedAttributeValue;
        private Control _selectedControl;

        public string AttributeName
        {
            get => Item.AttributeName;
            set
            {
                if (value == Item.AttributeName) return;
                Item.AttributeName = value;
                NotifyOfPropertyChange();
            }
        }

        public object AttributeValue
        {
            get => Item.AttributeValue;
            set
            {
                if (Equals(value, Item.AttributeValue)) return;
                Item.AttributeValue = value;
                NotifyOfPropertyChange();
            }
        }

        public List<AttributeValue> AttributeValues => SelectedAttribute?.AttributeValues.OrderBy(attribute => attribute.Name).ToList();
        public List<ControlAttribute> ControlAttributes => SelectedControl?.ControlAttributes.OrderBy(controlAttribute => controlAttribute.Name).ToList();

        public string ControlName
        {
            get => Item.ControlName;
            set
            {
                if (value == Item.ControlName) return;
                Item.ControlName = value;
                NotifyOfPropertyChange();
            }
        }

        public List<Control> Controls => ControlHelper.Controls.OrderBy(control => control.Name).ToList();

        public ControlAttribute SelectedAttribute
        {
            get => _selectedAttribute;
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
            get => _selectedAttributeValue;
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

        public Control SelectedControl
        {
            get => _selectedControl;
            set
            {
                if (Equals(value, _selectedControl)) return;
                _selectedControl = value;
                NotifyOfPropertyChange();
                NotifyOfPropertyChange(nameof(ControlAttributes));

                Item.ControlName = value != null ? value.Name : string.Empty;
            }
        }
    }
}
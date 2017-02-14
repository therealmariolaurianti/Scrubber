using System.Collections.Generic;
using Scrubber.Extensions;
using Scrubber.Maintenance;
using Scrubber.Objects;

namespace Scrubber.Model.Maintenance.InputAttributes.ViewModels
{
    public class InputAttributeViewModel : ViewModel
    {
        private object _value;
        private string _name;
        private Control _selectedControl;
        private ControlAttribute _selectedAttribute;

        public string Name
        {
            get { return _name; }
            set
            {
                if (value == _name) return;
                _name = value;
                NotifyOfPropertyChange();
            }
        }

        public object Value
        {
            get { return _value; }
            set
            {
                if (Equals(value, _value)) return;
                _value = value;
                NotifyOfPropertyChange();
            }
        }

        public List<Control> Controls => CommonControls.Controls;
        public List<ControlAttribute> ControlAttributes => SelectedControl.ControlAttributes;

        public Control SelectedControl
        {
            get { return _selectedControl; }
            set
            {
                if (Equals(value, _selectedControl)) return;
                _selectedControl = value;
                NotifyOfPropertyChange();
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
            }
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using Caliburn.Micro;
using Ninject;
using Scrubber.Helpers;

namespace Scrubber.Maintenance
{
    public class ViewModel : Screen
    {
        public ViewModel()
        {
            Id = NextId.GetNext();
        }

        public int Id { get; private set; }

        public ICommand CloseCommand => new DelegateCommand(Close);

        public void Close()
        {
            TryClose();
        }

        public List<T> GetEnumValuesByType<T>()
        {
            return Enum.GetValues(typeof(T)).Cast<T>().ToList();
        }
    }

    public class ViewModel<T> : Screen, IViewModel<T> where T : new()
    {
        public ViewModel()
        {
            Item = new T();
            Id = NextId.GetNext();
        }

        public int Id { get; private set; }
        public T Item { get; }
    }
}
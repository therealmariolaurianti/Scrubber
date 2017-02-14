using System;
using System.Collections.Generic;
using System.Linq;
using Caliburn.Micro;
using Ninject;

namespace Scrubber.Maintenance
{
    public class ViewModel : Screen
    {
        public ViewModel()
        {
            Id = NextId.GetNext();
        }

        public int Id { get; private set; }

        public void Close()
        {
            TryClose();
        }

        public List<T> GetEnumValuesByType<T>()
        {
            return Enum.GetValues(typeof(T)).Cast<T>().ToList();
        }
    }

    public class ViewModel<T> : Screen, IViewModel<T>
    {
        public ViewModel(T item)
        {
            Item = item;
        }

        public T Item { get; }
    }
}
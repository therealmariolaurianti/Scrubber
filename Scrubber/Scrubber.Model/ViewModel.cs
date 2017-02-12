using Caliburn.Micro;

namespace Scrubber.Model
{
    public class ViewModel : Screen
    {
        public void Close()
        {
            TryClose();
        }
    }
}
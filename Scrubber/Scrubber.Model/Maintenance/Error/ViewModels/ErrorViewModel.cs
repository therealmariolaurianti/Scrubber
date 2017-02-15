using Scrubber.Maintenance;

namespace Scrubber.Model.Maintenance.Error.ViewModels
{
    public class ErrorViewModel : ViewModel
    {
        public ErrorViewModel(string errorMessage)
        {
            ErrorMessage = errorMessage;
        }

        public string ErrorMessage { get; set; }
    }
}
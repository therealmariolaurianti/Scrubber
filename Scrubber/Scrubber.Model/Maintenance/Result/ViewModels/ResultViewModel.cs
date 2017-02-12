using System.Collections.Generic;
using System.Linq;
using Caliburn.Micro;
using Scrubber.Helpers;
using Scrubber.Objects;

namespace Scrubber.Model.Maintenance.Result.ViewModels
{
    public class ResultViewModel : Screen
    {
        public Result<Dictionary<bool, List<DirtyFile>>> Result { get; }

        public ResultViewModel(Result<Dictionary<bool, List<DirtyFile>>> result)
        {
            Result = result;
        }

        protected override void OnActivate()
        {
            DisplayName = "Results";
            base.OnActivate();
        }

        public void Close()
        {
            TryClose();
        }

        public string ResultString
        {
            get
            {
                var cleaned = Result.ResultValue.Any(r => r.Key)
                    ? Result.ResultValue[true].Count
                    : 0;

                if (Result.Success)
                    return $"Operation Completed. {cleaned} Cleaned. 0 Failed.";

                var dirty = Result.ResultValue[false].Count;

                return $"Operation Completed With Errors. {cleaned} Cleaned. {dirty} Failed.";
            }
        }

    }
}
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Scrubber.Helpers;
using Scrubber.Interfaces;
using Scrubber.Objects;

namespace Scrubber.Workers
{
    public class Bathtub
    {
        private readonly IOptions _options;
        private readonly Soap _soap;

        public Bathtub(Soap soap, IOptions options)
        {
            _soap = soap;
            _options = options;
        }

        private List<DirtyFile> DirtyFiles { get; } = new List<DirtyFile>();
        public string FolderPath => _options.FolderPath;

        public Result<Dictionary<bool, List<DirtyFile>>> Drain()
        {
            var result = DirtyFiles.GroupBy(item => item.IsClean)
                .ToDictionary(x => x.Key, x => x.ToList());

            return DirtyFiles.Any(df => !df.IsClean)
                ? Result<Dictionary<bool, List<DirtyFile>>>.CreateFail(result)
                : Result<Dictionary<bool, List<DirtyFile>>>.CreateSuccess(result);
        }

        public void FinishScrub(Result<Dictionary<bool, List<DirtyFile>>> result)
        {
            string messageText;

            if (!result.Success)
            {
                var cleaned = result.ResultValue[true].Count;
                var dirty = result.ResultValue[false].Count;

                messageText = $"Operation Completed With Errors. {cleaned} Cleaned. {dirty} Failed.";
            }
            else
            {
                messageText = $"Operation Completed. {result.ResultValue.Count} Cleaned.";
            }

            MessageBox.Show(messageText);
        }

        public void Fill() => FolderPath.FileByExtenstion("*.xaml").ForEach(file => DirtyFiles.Add(new DirtyFile(file)));

        public void Rinse() => DirtyFiles.ForEach(dirtyFile => _soap.Scrub(dirtyFile));
    }
}
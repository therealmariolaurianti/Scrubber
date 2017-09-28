using System;
using System.Collections.Generic;
using System.Linq;
using Scrubber.Enums;
using Scrubber.Extensions;
using Scrubber.Factories;
using Scrubber.Helpers;
using Scrubber.Objects;
using Scrubber.Options;

namespace Scrubber.Workers
{
    public class Bathtub
    {
        private readonly IScrubberOptions _bathtubOptions;
        private readonly ISoapFactory _soapFactory;

        public Bathtub(IScrubberOptions bathtubOptions, ISoapFactory soapFactory)
        {
            _bathtubOptions = bathtubOptions;
            _soapFactory = soapFactory;
        }

        private List<DirtyFile> DirtyFiles { get; } = new List<DirtyFile>();

        public Result<Dictionary<bool, List<DirtyFile>>> Drain()
        {
            var result = DirtyFiles.GroupBy(item => item.IsClean)
                .ToDictionary(x => x.Key, x => x.ToList());

            return DirtyFiles.Any(df => !df.IsClean)
                ? Result<Dictionary<bool, List<DirtyFile>>>.CreateFail(result)
                : Result<Dictionary<bool, List<DirtyFile>>>.CreateSuccess(result);
        }

        public void FillAndRinse()
        {
            Fill();
            Rinse();
        }

        private void Fill()
        {
            var filesByExtenstion = new List<string>();
            switch (_bathtubOptions.FolderOrFile)
            {
                case FolderOrFile.Folder:
                    filesByExtenstion = _bathtubOptions.Path.GetFilesByExtenstion("xaml");
                    break;
                case FolderOrFile.File:
                    filesByExtenstion.Add(_bathtubOptions.Path);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            foreach (var file in filesByExtenstion)
                DirtyFiles.Add(new DirtyFile(file));
        }

        public void Rinse()
        {
            var soap = _soapFactory.Create(_bathtubOptions);
            DirtyFiles.ForEach(dirtyFile => soap.Scrub(dirtyFile));
        }
    }
}
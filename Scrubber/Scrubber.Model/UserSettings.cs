using Scrubber.Enums;
using Scrubber.Model.Maintenance.Shell.ViewModels;
using Scrubber.Model.Properties;

namespace Scrubber.Model
{
    public class UserSettings
    {
        public UserSettings()
        {
            Path = Settings.Path;
            ClearComments = Settings.ClearComments;
            FormatFiles = Settings.FormatFiles;
            FolderOrFile = Settings.FolderOrFile;
        }

        private static Settings Settings => Settings.Default;
        public string Path { get; }
        public bool ClearComments { get; }
        public bool FormatFiles { get; }
        public FolderOrFile FolderOrFile { get; }

        public void Load(ShellViewModel shellViewModel)
        {
            shellViewModel.Path = Path;
            shellViewModel.ClearComments = ClearComments;
            shellViewModel.FormatFiles = FormatFiles;
            shellViewModel.FolderOrFile = FolderOrFile;
        }

        public void SaveSingle(string settingName, object newValue)
        {
            Settings.Default[settingName] = newValue;
            SettingsSave();
        }

        private static void SettingsSave()
        {
            Settings.Default.Save();
            Settings.Default.Reload();
        }
    }
}
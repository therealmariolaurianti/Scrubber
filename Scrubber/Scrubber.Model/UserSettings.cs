using Scrubber.Model.Maintenance.Shell.ViewModels;
using Scrubber.Model.Properties;

namespace Scrubber.Model
{
    public class UserSettings
    {
        public UserSettings()
        {
            FolderPath = Settings.FolderPath;
            ClearComments = Settings.ClearComments;
            FormatFiles = Settings.FormatFiles;
        }

        private static Settings Settings => Settings.Default;
        public string FolderPath { get; set; }
        public bool ClearComments { get; set; }
        public bool FormatFiles { get; set; }

        public void Load(ShellViewModel shellViewModel)
        {
            shellViewModel.FolderPath = FolderPath;
            shellViewModel.ClearComments = ClearComments;
            shellViewModel.FormatFiles = FormatFiles;
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
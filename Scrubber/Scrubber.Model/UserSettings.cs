using Scrubber.Model.Maintenance.Shell.ViewModels;
using Scrubber.Model.Properties;

namespace Scrubber.Model
{
    public class UserSettings
    {
        public UserSettings()
        {
            Initialize();
        }

        private static Settings Settings => Settings.Default;

        public string FolderPath { get; set; }

        private void Initialize()
        {
            FolderPath = Settings.FolderPath;
        }

        public void Load(ShellViewModel shellViewModel)
        {
            shellViewModel.FolderPath = FolderPath;
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
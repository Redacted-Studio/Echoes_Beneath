namespace RDCT.Menu.SettingsMenu
{
    public interface ISettingsMenu
    {
        public void InitializeSettings(SOSettings settings);
        //public void ResetSettings(SOSettings defaultSettings);
        public void SaveSettings(SOSettings settings);
        public void LoadSettings(SOSettings settings);
    }
}
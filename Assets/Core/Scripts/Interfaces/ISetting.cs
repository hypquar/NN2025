namespace Assembly_CSharp
{
    internal interface ISetting
    {
        public void SaveSetting(SettingsData settingsData);
        public void LoadSetting(SettingsData settingsData);
    }
}

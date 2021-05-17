namespace DiscordBotFramework.Utilities
{
    public static class LibrarySettings
    {
        public static void Save(string setting, string value)
        {
            Properties.Settings.Default[setting] = value;
            Properties.Settings.Default.Save();
        }
    }
}

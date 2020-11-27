namespace PatentsHelperUi
{
    public static class ModernWpfThemeManager
    {
        public static void StartTheme()
        {
            var themeId = new PatentsHelperSettings.UserSettings().AppTheme;

            StartTheme(themeId);
        }

        public static void StartTheme(int themeId)
        {
            if (themeId == 0)
            {
                ModernWpf.ThemeManager.Current.ApplicationTheme = ModernWpf.ApplicationTheme.Light;
            }
            else
            {
                ModernWpf.ThemeManager.Current.ApplicationTheme = ModernWpf.ApplicationTheme.Dark;
            }
        }


    }
}

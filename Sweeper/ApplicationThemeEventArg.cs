using System;

namespace Sweeper
{
    public class ApplicationThemeEventArg : EventArgs
    {
        string theme;

        public string Theme
        {
            get { return theme; }
            private set { theme = value; }
        }
        private string themeID;
        public string ThemeID
        {
            get { return themeID;  }
            set { themeID = value; }
        }
        public ApplicationThemeEventArg(string strTheme)
        {
            theme = strTheme;
            themeID = strTheme.ToUpper();
        }
    }
}

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
        public ApplicationThemeEventArg(string strTheme)
        {
            theme = strTheme;
        }
    }
}

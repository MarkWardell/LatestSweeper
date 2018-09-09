using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sweeper.ViewModels
{
    interface IChangeTheme
    {
        /// <summary>
        /// 1)In your constructor of implementing class Call: App.ChangeThemeEvent += App_ChangeThemeEvent;
        /// 2)In your implementation of this method cast EventArgs to Application
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"> Cast this ApplicationThemeEvent to some ate and call Method ChangeTheme(ate.Theme)</param>
        void App_ChangeThemeEvent(object sender, EventArgs e);
        /// <summary>
        /// Implemet Here what Chanfing Theme means to your Part of the Application
        /// </summary>
        /// <param name="themeName">themName is a Valid and Supported Theme</param>
        void ChangeTheme(string themeName);

    }

}

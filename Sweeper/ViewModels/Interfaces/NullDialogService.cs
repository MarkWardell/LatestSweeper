using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sweeper.ViewModels
{
    public class NullDialogService : IDialogService
    {
        public void ShowAbout()
        {
            // Meant to be empty
        }

        public void ShowGameResult()
        {
            // Meant to be empty
        }

        public void ShowLogoptions()
        {
            // Meant to be empty
        }

        public bool ShowCustomGame()
        {
            // Meant to be empty
            return true;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sweeper.ViewModels
{
    public interface IDialogService
    {
        void ShowAbout();
        void ShowGameResult();
        void ShowLogoptions();
        bool ShowCustomGame();

    }
}

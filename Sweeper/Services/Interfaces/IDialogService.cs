using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Sweeper.ViewModels.Inerfaces
{
    public interface IDialogService
    {
        Window UndoRedoView { get; }
        void ShowAbout();
        void ShowUndoRedo(UndoRedoViewModel vm);
        void ShowLogoptions();
        bool ShowCustomGame();
        


    }
}

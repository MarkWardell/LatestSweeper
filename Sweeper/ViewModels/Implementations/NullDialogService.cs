using Sweeper.ViewModels.Inerfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Sweeper.ViewModels
{
    public class NullDialogService : IDialogService
    {
        public Window UndoRedoView {
        get {
                return null;
            }
        }

        public void ShowAbout()
        {
            throw new NotImplementedException();
        }

        public bool ShowCustomGame()
        {
            throw new NotImplementedException();
        }

        public void ShowLogoptions()
        {
            throw new NotImplementedException();
        }

        public void ShowUndoRedo(UndoRedoViewModel vm)
        {
            throw new NotImplementedException();
        }
    };

      
}

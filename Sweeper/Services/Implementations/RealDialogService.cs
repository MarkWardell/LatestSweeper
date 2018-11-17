using Sweeper.ViewModels.Inerfaces;
using Sweeper.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Sweeper.ViewModels
{
    public class RealDialogService : IDialogService
    {
        private static  Window undoRedoView = null;
        public Window UndoRedoView
        {
            get
            {
                if (undoRedoView == null)
                    undoRedoView = new UndoRedoView();
                return undoRedoView;
            }
        }
      

        public void ShowAbout()
        {
            Window w = new About();
            w.ShowDialog();
        }

        public void ShowUndoRedo(UndoRedoViewModel vm)
        {
            
            UndoRedoView.DataContext = vm;

            UndoRedoView.Show();
        }

        public void ShowLogoptions()
        {
            throw new NotImplementedException();
        }

        public bool ShowCustomGame()
        {
            throw new NotImplementedException();
        }
    }
}

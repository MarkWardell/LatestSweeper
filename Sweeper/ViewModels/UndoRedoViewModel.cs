using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Sweeper.ViewModels
{
    public class UndoRedoViewModel 
    {
        ObservableCollection<CommandItem> lUndo = new ObservableCollection<CommandItem>();
        public UndoRedoViewModel()
        {
           // App.ChangeThemeEvent += App_ChangeThemeEvent;
        }
        public ObservableCollection<CommandItem> LUndo
        {
            get { return lUndo; }
            set { lUndo = value; }
        }
        ObservableCollection<CommandItem> lRedo = new ObservableCollection<CommandItem>();

        public ObservableCollection<CommandItem> LRedo
        {
            get { return lRedo; }
            set { lRedo = value; }
        }
        private RelayCommand refreshCommand = null;
        public RelayCommand RefreshCommand
        {
            get { return refreshCommand; }
            set { refreshCommand = value; }
        }

        private RelayCommand undoCommand = null;

        public RelayCommand UndoCommand
        {
            get { return undoCommand; }
            set { undoCommand = value; }
        }

        private RelayCommand redoCommand = null;

        public RelayCommand RedoCommand
        {
            get { return redoCommand; }
            set { redoCommand = value; }
        }

        private ObservableCollection<CommandItem> lBusTub = new ObservableCollection<CommandItem>();
        public ObservableCollection<CommandItem> LBusTub { get { return lBusTub; } set { lBusTub = value; } }

        internal void SetStacks(Stack<CommandItem> undoStack, Stack<CommandItem> redoStack, Stack<CommandItem> busTubStack)
        {
            lUndo.Clear();
            foreach (CommandItem ci in undoStack)
            {
                lUndo.Add(ci);
            }
            lRedo.Clear();
            foreach (CommandItem ci in redoStack)
            {
                lRedo.Add(ci);
            }
            lBusTub.Clear();
            foreach (CommandItem ci in busTubStack)
            {
                lBusTub.Add(ci);
            }
        }
    }
}

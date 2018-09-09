
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Shapes;
using Sweeper.ViewModels;
using System.Windows.Media;

namespace Sweeper.ViewModels
{

    public partial class GamePiece : INotifyPropertyChanged, IChangeTheme, IDisposable
    {
        static Sweeper.ViewModels.SweeperViewModel gameBoard = null;
        public GamePiece(int nItem, GameConstants.PieceValues pv)
        {
            linearIndex = nItem;
           // IsPlayed = false;
            value = pv;
        }

        public GamePiece(ViewModels.SweeperViewModel sweeperViewModel, int p, int r, int c, GameConstants.PieceValues actualValue)
        {

            this.sweeperViewModel = sweeperViewModel;
            linearIndex = p;
            this.r = r;
            this.c = c;
            value = GameConstants.PieceValues.BUTTON;
            if (gameBoard == null)
                gameBoard = sweeperViewModel;
            //this._GridBorderBrush = sweeperViewModel.GridBorderBrush;
            //this._HiGridBorderBrush = sweeperViewModel.HiGridBorderBrush;
            
            App.ChangeThemeEvent += App_ChangeThemeEvent;
        }
        private int linearIndex;

        public int LinearIndex
        {
            get { return linearIndex; }
            set { linearIndex = value; }
        }


        public bool IsPlayed
        {
            get { if ((this.value == this.actualValue) || (this.value == GameConstants.PieceValues.WRONGCHOICE))
                    return true;
                else
                    return false;
            }
            //set { this.value = this.actualValue; }
        }

        public bool IsFlagged
        {
            get { return value == GameConstants.PieceValues.FLAGGED; }

        }
        public bool IsPressed
        {
            get { return this.value == GameConstants.PieceValues.PRESSED; }

        }
        private GameConstants.PieceValues value;

        public GameConstants.PieceValues Value
        {
            get { return this.value; }
            set
            {
                
                    this.value = value;
                    OnPropertyChanged("Value");
                

            }
        }
        private GameConstants.PieceValues actualValue;

        public GameConstants.PieceValues ActualValue
        {
            get { return actualValue; }
            set { actualValue = value; }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private int r;

        public int R
        {
            get { return r; }
            set { r = value; }
        }
        private int c;

        public int C
        {
            get { return c; }
            set { c = value; }
        }

        public void OnPropertyChanged(string prop)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
            }
        }

        public void App_ChangeThemeEvent(object sender, EventArgs e)
        {
            ChangeTheme("");
        }

        public void ChangeTheme(string themeName)
        {
            
            OnPropertyChanged("Value");
        }
            
        #region DISPOSE
        bool disposed = false;
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposed)
                return;

            if (disposing)
            {
                Debug.WriteLine("Removing event handler for GamePiece[" + this.R + "," + this.C + "]");
                App.ChangeThemeEvent -= this.App_ChangeThemeEvent;
                this._mouseEnterCommand = null;
                this._mouseLeaveCommand = null;
                this._mouseLeftButtonDownCommand = null;
                this._mouseLeftButtonUpCommand = null;
                this._mouseRightButtonUpCommand = null;
            }

            disposed = true;
        }
        ~GamePiece()
        {
            Dispose(false);
        }
        #endregion
    }

}

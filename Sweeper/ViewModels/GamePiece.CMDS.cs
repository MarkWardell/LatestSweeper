
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Sweeper.ViewModels
{
    public partial class GamePiece
    {
        private ViewModels.SweeperViewModel sweeperViewModel;
        #region COMMANDS
        const string CATEGORY = "MOUSE";

        #region MOUSE_LEFT_BUTTON_DOWN
        private RelayCommand _mouseLeftButtonDownCommand;
        public RelayCommand MouseLeftButtonDownCommand
        {
            get
            {
                if (_mouseLeftButtonDownCommand == null)
                {
                    _mouseLeftButtonDownCommand = new RelayCommand(param => ExecuteMouseLeftButtonDown((MouseEventArgs)param),
                                                                   param => CanExecuteMouseLeftButtonDown(null),
                                                                   "MouseLeftButtonDownCommand", CATEGORY);
                   

                }
                return _mouseLeftButtonDownCommand;
            }
            set { _mouseLeftButtonDownCommand = value; }
        }
        private bool CanExecuteMouseLeftButtonDown(object o)
        {
            return ((!this.IsPlayed) && !(this.IsFlagged));

        }
        private void ExecuteMouseLeftButtonDown(MouseEventArgs e)
        {
            Debug.WriteLine("Mouse Down : " + e.GetPosition((IInputElement)e.Source));

            if (e.LeftButton == MouseButtonState.Pressed)
            {

                if (this.Value == GameConstants.PieceValues.BUTTON)
                {
                    this.Value = GameConstants.PieceValues.PRESSED;
                    gameBoard.GameState = GameConstants.GameStates.IN_DECISION;
                }
                else
                    if (gameBoard.GameState != GameConstants.GameStates.WON &&
                        gameBoard.GameState != GameConstants.GameStates.LOST)
                        gameBoard.GameState = GameConstants.GameStates.IN_PLAY;


            }

        }
        #endregion

        #region MOUSE_LEFT_BUTTON_UP
        RelayCommand _mouseLeftButtonUpCommand;
        public RelayCommand MouseLeftButtonUpCommand
        {
            get
            {
                if (_mouseLeftButtonUpCommand == null)
                {
                    _mouseLeftButtonUpCommand = new RelayCommand(param => ExecuteMouseLeftButtonUp((MouseEventArgs)param),
                                                                 (param) => this.Value == GameConstants.PieceValues.PRESSED,
                                                                 "MouseLeftButtonUpCommand", CATEGORY);
                       
                }
                return _mouseLeftButtonUpCommand;
            }
            set { _mouseLeftButtonUpCommand = value; }
        }

        private void ExecuteMouseLeftButtonUp(MouseEventArgs e)
        {

            gameBoard.PlayCommand.Execute(new System.Windows.Point(this.R, this.C));

        }
        private bool CanExecuteMouseLeftButtonUp(object o)
        {
            return (IsPressed);
        }

        #endregion

        #region MOUSE_ENTER
        private RelayCommand _mouseEnterCommand;
        public RelayCommand MouseEnterCommand
        {
            get
            {
                if (_mouseEnterCommand == null)
                {
                    _mouseEnterCommand = new RelayCommand(param => ExecuteMouseEnter((MouseEventArgs)param),
                                                          param => CanExecuteMouseEnter((MouseEventArgs)param), 
                                                          "MouseEnterCommand",CATEGORY);
                    
                }
                return _mouseEnterCommand;
            }
            set { _mouseEnterCommand = value; }
        }

        private bool CanExecuteMouseEnter(MouseEventArgs mouseEventArgs)
        {
            // Want the Code in comments to Execute but when we execute that blocking condition 
            // Smiley is always pressed/TENSE FACE :(

            if (!this.IsPlayed)
                if (mouseEventArgs.LeftButton == MouseButtonState.Pressed)
                    return true;
            return false;

        }

        private void ExecuteMouseEnter(MouseEventArgs e)
        {
            //Debug.WriteLine("Mouse Enter : " + e.GetPosition((IInputElement)e.Source));

            if (Value == GameConstants.PieceValues.BUTTON)
            {
                gameBoard.GameState = GameConstants.GameStates.IN_DECISION;
                Value = GameConstants.PieceValues.PRESSED;

            }
            else
                if (gameBoard.GameState != GameConstants.GameStates.WON &&
                    gameBoard.GameState != GameConstants.GameStates.LOST)
                    gameBoard.GameState = GameConstants.GameStates.IN_PLAY;
        }

        #endregion

        #region MOUSE_LEAVE
        private RelayCommand _mouseLeaveCommand;
        public RelayCommand MouseLeaveCommand
        {
            get
            {
                if (_mouseLeaveCommand == null)
                {
                    _mouseLeaveCommand = new RelayCommand(param => ExecuteMouseLeave((MouseEventArgs)param),
                                                          param => CanExecuteMouseLeave((MouseEventArgs)param),
                                                          "MouseLeaveCommand", CATEGORY);
                }
                return _mouseLeaveCommand;
            }
            set { _mouseLeaveCommand = value; }
        }
        private bool CanExecuteMouseLeave(MouseEventArgs me)
        {
            if (!this.IsPlayed && this.IsPressed)
                return true;
            else
                return false;


        }

        private void ExecuteMouseLeave(MouseEventArgs e)
        {
            //Debug.WriteLine("Mouse Leave : " + e.GetPosition((IInputElement)e.Source));

            this.Value = GameConstants.PieceValues.BUTTON;
            this.sweeperViewModel.GameState = GameConstants.GameStates.IN_PLAY;
        }
        #endregion

        #region MOUSE_RB_UP
        private RelayCommand _mouseRightButtonUpCommand;

        public RelayCommand MouseRightButtonUpCommand
        {
            get
            {
                if (_mouseRightButtonUpCommand == null)
                {
                    _mouseRightButtonUpCommand = new RelayCommand(param => ExecuteMouseRightButtonUp((MouseEventArgs)param),
                                                                  param => CanExecuteMouseRightButtonUp(null),
                                                                  "MouseRightButtonUpCommand", CATEGORY);
                    
                }
                return _mouseRightButtonUpCommand;
            }
            set { _mouseRightButtonUpCommand = value; }
        }
        private bool CanExecuteMouseRightButtonUp(object o)
        {
            return !this.IsPlayed;
        }
        private void ExecuteMouseRightButtonUp(MouseEventArgs e)
        {
            //Debug.WriteLine("Mouse RightButtonUp : " + e.GetPosition((IInputElement)e.Source));
            gameBoard.FlagCommand.Execute(new System.Windows.Point(this.R, this.C));
        }
        #endregion

        #endregion


        
    }
}

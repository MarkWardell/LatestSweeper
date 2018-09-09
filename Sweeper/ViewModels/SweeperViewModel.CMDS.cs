using Sweeper.Views;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Sweeper.ViewModels
{
    public partial class SweeperViewModel
    {
        #region GAME_COMMANDS
        const string CATEGORY = "GAME";
        const string OPTION = "OPTION";
        #region NEW_GAME
        private RelayCommand _NewGameCommand;
        public RelayCommand NewGameCommand
        {
            get
            {
                if (_NewGameCommand == null)
                {
                    _NewGameCommand = new RelayCommand(param => ExecuteNewGame((string)param),
                                                       param => true, "NewGameCommand", CATEGORY,
                                                       param => UnExecuteNewGame(), param => true);
                    
                }
                return _NewGameCommand;
            }
            set { _NewGameCommand = value; }
        }

        private void ExecuteNewGame(string param)
        {

            Debug.WriteLine("New Game : " + param);
            switch (param.ToUpper().Trim())
            {
                case ("SAMEGAME"):
                    //We Keep Same Parms no need to do anything 
                    break;
                case ("BEGINNER"):
                    SetGameParms(GameConstants.GameTypes.BEGINNER);
                    break;
                case ("INTERMEDIATE"):
                    SetGameParms(GameConstants.GameTypes.INTERMEDIATE);
                    break;
                case ("ADVANCED"):
                    SetGameParms(GameConstants.GameTypes.ADVANCED);
                    break;
                case ("CUSTOM"):
                    // May result in a dialog being shown (Depends on the Dialog Service)
                    SetGameParms(GameConstants.GameTypes.CUSTOM);
                    break;
                default:
                    throw new Exception("Unsupported GameType(" + param + ")");
            }
            NewGame();
            
        }
        private void UnExecuteNewGame()
        {
            Time = 0;
            tmr.Enabled = false;
        }

        #endregion        
        
        #region PLAY
        private RelayCommand _PlayCommand;
        public RelayCommand PlayCommand
        {
            get
            {
                if (_PlayCommand == null)
                {
                    _PlayCommand = new RelayCommand((param) => ExecutePlay((Point)param, true),  //Execute Command
                                                    (param) => {    Point pt = (Point)param;     //CanExecute as anonymous Method
                                                                    int r = (int)pt.X;
                                                                    int c = (int)pt.Y;
                                                                    GamePiece gp = Item(r, c);
                                                                    return !gp.IsPlayed;},
                                                    "PlayCommand",                               //Set Display Text
                                                    CATEGORY,                                    //Set Category to "GAME" for Filtering
                                                    (param)=>UnPlay((Point)param),               //Set UnExecute() Method 
                                                    (param)=>CanUnExecutePlay((Point)param));    //Set CanUnExecute() Method               
                }
                return _PlayCommand;
            }
            set { _PlayCommand = value; }            
        }

        private bool CanUnExecutePlay(Point point)
        {
           GamePiece gp = Item((int)point.X,(int)point.Y);
           return gp.IsPlayed;
        }
        private void ExecutePlay(Point pt, bool soundson)
        {
            Play(pt, soundson);
        }

        #endregion
        
        #region FLAG
        private RelayCommand _FlagCommand;
        public RelayCommand FlagCommand
        {
            get
            {
                if (_FlagCommand == null)
                {
                    _FlagCommand = new RelayCommand(param => ExecuteFlag((Point)param),
                                                    param => true,
                                                    "FlagCommand",
                                                    CATEGORY,
                                                    param => ExecuteFlag((Point)param),
                                                    param=>true);
                
                }
                return _FlagCommand;
            }
            set { _FlagCommand = value; }
        }
        bool insidemacvm = false;
        // mac vm code here DateTime 
        DateTime lastFlag = DateTime.Now;
        
        private void ExecuteFlag(Point pt)
        {
            // For Some reason I need to do this when running inside my mac Virtual machine grrr
            if (insidemacvm)
            {
                DateTime now = DateTime.Now;
                if (now.Subtract(lastFlag).Milliseconds > 50)
                    Flag((int)pt.X, (int)pt.Y);
                lastFlag = DateTime.Now;
            }else
                Flag((int)pt.X, (int)pt.Y);
            
        }

        #endregion
        
        #region GAME_ABOUT
        private RelayCommand _aboutCommand;
        public RelayCommand AboutCommand
        {
            get
            {
                if (_aboutCommand == null)
                {
                    _aboutCommand = new RelayCommand(param => ExecuteAboutCommand(),
                                                     param => CanExecuteAboutCommand(),
                                                     "AboutCommand", CATEGORY);
                    
                }
                return _aboutCommand;
            }
            set { _aboutCommand = value; }
        }

        private void ExecuteAboutCommand()
        {
            Debug.WriteLine("About : ");
            Window w = new About();
            w.ShowDialog();          
        }
        private bool CanExecuteAboutCommand()
        {
            return true;
        }
        #endregion
        //Delete this
        #region PRESS_SMILEY
        private RelayCommand _pressSmileyCommand;
        public RelayCommand PressSmileyCommand
        {
            get
            {
                if (_pressSmileyCommand == null)
                {
                    _pressSmileyCommand = new RelayCommand(param => ExecutePressSmileyCommand(),
                                                    param => true, "PressSmileyCommand",CATEGORY);
                    ;
                }
                return _pressSmileyCommand;
            }
            set { _pressSmileyCommand = value; }
        }
        GameConstants.GameStates lastSmileyState = GameConstants.GameStates.NOT_DETERMINED;
        private void ExecutePressSmileyCommand()
        {
            lastSmileyState = gameState;
            Debug.WriteLine("PressSmiley : ");
            GameState = GameConstants.GameStates.IN_DECISION;
            
        }

        #endregion
        
        #region UNPRESS_SMILEY
        private RelayCommand _unpressSmileyCommand;
        public RelayCommand UnPressSmileyCommand
        {
            get
            {
                if (_unpressSmileyCommand == null)
                {
                    _unpressSmileyCommand = new RelayCommand(param => ExecuteUnPressSmileyCommand(),
                                                             param => CanExecuteUnPressSmileyCommand(),
                                                             "UnPressSmileyCommand", CATEGORY);
                    
                }
                return _unpressSmileyCommand;
            }
            set { _unpressSmileyCommand = value; }
        }

        private bool CanExecuteUnPressSmileyCommand()
        {
            return GameState == GameConstants.GameStates.IN_DECISION;
            
        }

        private void ExecuteUnPressSmileyCommand()
        {
            Debug.WriteLine("UnPressSmiley : ");
            this.NewGameCommand.Execute("SAMEGAME");
        }

        #endregion

        #region LEAVE_SMILEY
        private RelayCommand _leaveSmileyCommand;
        public RelayCommand LeaveSmileyCommand
        {
            get
            {
                if (_leaveSmileyCommand == null)
                {
                    _leaveSmileyCommand = new RelayCommand(param => ExecuteLeaveSmileyCommand((MouseEventArgs)param),
                                                           param => CanExecuteLeaveSmileyCommand((MouseEventArgs)param),
                                                           "LeaveSmileyCommand", CATEGORY);
                    
                }
                return _leaveSmileyCommand;
            }
            set { _leaveSmileyCommand = value; }
        }
        private bool CanExecuteLeaveSmileyCommand(MouseEventArgs me)
        {

            return (me.LeftButton == MouseButtonState.Pressed &&
                    GameState == GameConstants.GameStates.IN_DECISION);
                    
                
        }
        private void ExecuteLeaveSmileyCommand(MouseEventArgs me)
        {
            Debug.WriteLine("LeaveSmiley : ");
            if (!GameInProgress)
                GameState = result;
            else
                GameState = GameConstants.GameStates.IN_PLAY;
        }

        #endregion
 
        #region GAME_EXIT
        private RelayCommand _gameExitCommand;
        public RelayCommand GameExitCommand
        {
            get
            {
                if (_gameExitCommand == null)
                {
                    _gameExitCommand = new RelayCommand(param => ExecuteGameExitCommand(),
                                                        param => true, "GameExitCommand", CATEGORY);
                    
                }
                return _gameExitCommand;
            }
            set { _gameExitCommand = value; }
        }

        private void ExecuteGameExitCommand()
        {
            Debug.WriteLine(_gameExitCommand.DisplayText);
            Application.Current.Shutdown();
        }

        #endregion
        
        #region GAME_CLR_MSG
        private RelayCommand _gameClearMessagesCommand;
        public RelayCommand GameClearMessages
        {
            get
            {
                if (_gameClearMessagesCommand == null)
                {
                    _gameClearMessagesCommand = new RelayCommand(param => ExecuteGameClearMessages(),
                                                    param => true, "GameClearMessages", OPTION);
                    
                }
                return _gameClearMessagesCommand;
            }
            set { _gameClearMessagesCommand = value; }
        }


        private void ExecuteGameClearMessages()
        {
            Debug.WriteLine(_gameClearMessagesCommand.DisplayText);
            this.ClearMessages();
        }
        #endregion
        
        #region GAME_OPTIONS
        private RelayCommand _gameOptionsCommand;
        public RelayCommand GameOptionsCommand
        {
            get
            {
                if (_gameOptionsCommand == null)
                {
                    _gameOptionsCommand = new RelayCommand(go => ExecuteGameOptionsCommand((GameOptions)go),
                                                           go => CanExecuteGameOptionsCommand(),"GameOptionsCommand",OPTION);
                    
                }
                return _gameOptionsCommand;
            }
            set { _gameOptionsCommand = value; }
        }

        private bool CanExecuteGameOptionsCommand()
        {
            return true;
        }

        private void ExecuteGameOptionsCommand(GameOptions go)
        {
           // App.SetTheme(go.Theme);
            Debug.WriteLine(_gameOptionsCommand.DisplayText);  
        }
        #endregion

        #region GAME_THEME
        private RelayCommand _gameThemeCommand;
        public RelayCommand GameThemeCommand
        {
            get
            {
                if (_gameThemeCommand == null)
                {
                    _gameThemeCommand = new RelayCommand(theme => ExecuteGameThemeCommand((string)theme),
                                                         theme => true, "GameThemeCommand", OPTION);
                                                        
                       
                }
                return _gameThemeCommand;
            }


            set { _gameThemeCommand = value; }
        }

       

        private void ExecuteGameThemeCommand(string strTheme)
        {
            App.SetTheme(strTheme.ToUpper());
            
            Debug.WriteLine(_gameThemeCommand.DisplayText);
            Debug.WriteLine("Theme={" + strTheme + "}");
            
            //This code may seem odd byt it causes the Converter to Get Called
            // Without the correct theme Item never gets checked
            Themes.Clear();

            String[] l = (String[])App.Current.FindResource("Themes");
            foreach (String s in l)
            {
                Themes.Add(s);

            }
        }
        #endregion

        #region GAMEOOPENTHEMES
        private RelayCommand _openThemesCommand;
        public RelayCommand OpenThemesCommand
        {
            get
            {
                if (_openThemesCommand == null)
                {
                    _openThemesCommand = new RelayCommand(theme => ExecuteOpenThemesCommand((string)theme),
                                                         theme => true, "OpenThemesCommand", OPTION);


                }
                return _openThemesCommand;
            }


            set { _openThemesCommand = value; }
        }



        private void ExecuteOpenThemesCommand(string strTheme)
        {
            List<String> l = (List<String>)Application.Current.FindResource("Themes");
        }
        #endregion
        //#region GAME_CUSTOM_CHANGE
        //private RelayCommand _gameCustomGameCommand;
        //public RelayCommand GameCustomGameCommand
        //{
        //    get
        //    {
        //        if (_gameCustomGameCommand == null)
        //        {
        //            _gameCustomGameCommand = new RelayCommand(cgs => ExecuteGameCustomGameCommand((CustomGameStruct)cgs),
        //                                                      cgs => CanExecuteGameCustomGameCommand((CustomGameStruct)cgs),
        //                                                      "GameCustomGameCommand", CATEGORY);
                    
        //        }
        //        return _gameCustomGameCommand;
        //    }


        //    set { _gameCustomGameCommand = value; }
        //}

        //private bool CanExecuteGameCustomGameCommand(CustomGameStruct cgs)
        //{
        //    //TODO Make Sure it is Valid
        //    return true;
        //}

        //private void ExecuteGameCustomGameCommand(CustomGameStruct cgs)
        //{
        //    //App.SetCustomGame(strCustomGame);
        //    Debug.WriteLine(_gameCustomGameCommand.DisplayText);
        //}
        //#endregion

        #region GAME_LOG_SHOWN
        private RelayCommand _gameLogShownCommand;
        public RelayCommand GameLogShownCommand
        {
            get
            {
                if (_gameLogShownCommand == null)
                {
                    _gameLogShownCommand = new RelayCommand(cgs => ExecuteGameLogShownCommand(),
                                                            cgs => CanExecuteGameLogShownCommand(),
                                                            "GameLogShownCommand","GAME",
                                                            cgs => ExecuteGameLogShownCommand(),
                                                            (cgs) => true);
                    
                }
                return _gameLogShownCommand;
            }


            set { _gameLogShownCommand = value; }
        }

        private bool CanExecuteGameLogShownCommand()
        {
            //TODO Make Sure it is Valid
            return true;
        }

        private void ExecuteGameLogShownCommand()
        {
            IsLogWindowShown = !IsLogWindowShown;
            Debug.WriteLine(_gameLogShownCommand.DisplayText);
        }
        #endregion

        #region GAME_VIEW_UNDO_REDO
        private RelayCommand _viewUndoRedoCommand;
        public RelayCommand ViewUndoRedoCommand
        {
            get
            {
                if (_viewUndoRedoCommand == null)
                {
                    _viewUndoRedoCommand = new RelayCommand((param) => ExecuteViewUndoRedoCommand(),
                                                         (param) => true,
                                                         "ViewUndoRedoCommand", 
                                                         "DIALOG");

                }
                return _viewUndoRedoCommand;
            }
            set { _viewUndoRedoCommand = value; }
        }

        private static UndoRedoView urV = null;
        private UndoRedoView UrV
        {
            get
            {
                if (urV == null)
                    urV = new UndoRedoView();
                return urV;
            }
        }
        private static UndoRedoViewModel urVm= null;
        private UndoRedoViewModel UrVm
        {
            get
            {
                if (urVm == null)
                    urVm = new UndoRedoViewModel();
                return urVm;
            }
        }
        //UndoRedoView urv = new UndoRedoView();
        private void ExecuteViewUndoRedoCommand()
        {
            //if (urVm == null)
            //{
            //    urVm = new UndoRedoViewModel();
            //}
            
            //UndoRedoView urv = new UndoRedoView();
            
            //UrVm.RefreshCommand = RefreshStacksCommand;
            //UrVm.RedoCommand = this.RedoAllOrLastCommand;
            //UrVm.UndoCommand = this.UndoAllOrLastCommand;
            //RefreshStacksCommand.Execute(null);
            UrV.DataContext = UrVm;
            
            UrV.Show();
            Debug.WriteLine(_viewUndoRedoCommand.DisplayText);
        }
        
        #endregion
             #region RFRESH_STACKS
        
        
      
        private RelayCommand _RefreshStacksCommand;
        public RelayCommand RefreshStacksCommand
        {
            

            get
            {
                if (_RefreshStacksCommand == null)
                {
                    _RefreshStacksCommand = new RelayCommand(param => ExecuteRefreshStacks(),
                                                         param => true, "RefreshStacksCommand", "STACK");
                   


                }
                return _RefreshStacksCommand;
            }
            set { _RefreshStacksCommand = value; }
        }

        private void ExecuteRefreshStacks()
        {
            if (urVm != null)
            { 
                urVm.SetStacks(undoStack, redoStack, busTubStack);
            }
            
            
        }

        #endregion        

        #region UNDO_ALL_OR_LAST
        
        
      
        private RelayCommand _UndoAllOrLastCommand;
        public RelayCommand UndoAllOrLastCommand
        {
            

            get
            {
                if (_UndoAllOrLastCommand == null)
                {
                    _UndoAllOrLastCommand = new RelayCommand(param => ExecuteUndoAllOrLast((string)param),
                                                         param => true, "UndoAllOrLastCommand", "STACK");
                   


                }
                return _UndoAllOrLastCommand;
            }
            set { _UndoAllOrLastCommand = value; }
        }

        private void ExecuteUndoAllOrLast(string param)
        {

            Debug.WriteLine("ExecuteUndoAllOrLast : " + param);
            switch (param.ToUpper().Trim())
            {
                case ("ALL"):
                    this.UndoAll();
                    break;
                case ("LAST"):
                    this.UndoLast();
                    break;
                default:
                    throw new Exception("ExecutedUndoAllOrLast Usupported (" + param + ")");
                 
            }
            ExecuteRefreshStacks();
            
        }

        #endregion        

        #region REDO_ALL_OR_LAST
        private RelayCommand _RedoAllOrLastCommand;
        public RelayCommand RedoAllOrLastCommand
        {
            get
            {
                if (_RedoAllOrLastCommand == null)
                {
                    _RedoAllOrLastCommand = new RelayCommand(param => ExecuteRedoAllOrLast((string)param),
                                                         param => true, "RedoAllOrLastCommand", "STACK");

                }
                return _RedoAllOrLastCommand;
            }
            set { _RedoAllOrLastCommand = value; }
        }

        private void ExecuteRedoAllOrLast(string param)
        {

            Debug.WriteLine("ExecuteRedoAllOrLast " + param);
            switch (param.ToUpper().Trim())
            {
                case ("ALL"):
                    this.RedoAll();
                    break;
                case ("LAST"):
                    this.RedoLast();

                    break;
                default:
                    throw new Exception("ExecuteRedoAllOrLast Usupported (" + param + ")");
            }
            ExecuteRefreshStacks();

        }


        #endregion        
        #endregion

        public bool GameInProgress { get { return gameBoardEnabled; } }
    }
}

#define LINQ
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Sweeper.ViewModels
{
    public partial class SweeperViewModel : INotifyPropertyChanged, IDisposable
    {        
        private Timer tmr;
        ObservableCollection<GamePiece> _cells = new ObservableCollection<GamePiece>();
        public event PropertyChangedEventHandler PropertyChanged = null;
        private IAdornGameWithSounds soundAdornment;
        private IDialogService dialogService;

        #region CONSTRUCTORS

        public SweeperViewModel()
            : this(new RealDialogService(), new SoundAdornmentRes())
        {

        }
        /// <summary>
        ///  This Constructor is called everytime to ensure consistency.  
        ///  This Constructor should be the one called DIRECTLY by all Test Harness applications.
        ///  Allows you to call with your own IDialogService or Adornment that will not leave a Dialog or 
        ///  Message box on the Screen.
        /// </summary>
        /// <param name="iDlg"> </param>
        /// <param name="iSnd"></param>
        public SweeperViewModel(IDialogService iDlg, IAdornGameWithSounds iSnd)
        {
            if (System.Windows.Application.Current.MainWindow == null)
                soundAdornment = new NullSoundAdornment();  // Turn off Sounds in Designer
            else
                soundAdornment = iSnd;
            dialogService = iDlg;
            tmr = new Timer(1000);
            tmr.Elapsed += tmr_Elapsed;
            //Although this is Static it is ok because the two objects have the same lifetimes
            RelayCommand.NewCommandItemEvent += RelayCommand_NewCommandItemEvent;
            if (System.Windows.Application.Current.MainWindow == null)
                NewGame();
            App.ChangeThemeEvent += App_ChangeThemeEvent;
            //UndoRedoView urv = new UndoRedoView();
            RelayCommand.RefreshStacksEvent += RelayCommand_RefreshStacksEvent;
           //App.SetTheme("CHOCALATE");
            Themes.Clear();
            String [] l = (String [])App.Current.FindResource("Themes");
            foreach (String s in l)
            {
               Themes.Add(s);

            }
            String[] gt = (String[])App.Current.FindResource("GameTypes");
            foreach (String s in gt)
            {
                GameTypes.Add(s);
            }
            

            UrVm.RefreshCommand = RefreshStacksCommand;
            UrVm.RedoCommand = this.RedoAllOrLastCommand;
            UrVm.UndoCommand = this.UndoAllOrLastCommand;
            //UrV.DataContext = UrVm;
        }

        private void RelayCommand_RefreshStacksEvent(object sender, EventArgs e)
        {
            ExecuteRefreshStacks();
        }

        private void App_ChangeThemeEvent(object sender, EventArgs e)
        {
            GameConstants.GameStates last = GameState;
            GameState = GameConstants.GameStates.IN_DECISION;
            GameState = last;

        }

        ObservableCollection<String> gameTypes = new ObservableCollection<string>();

        public ObservableCollection<String> GameTypes
        {
            get { return gameTypes; }
            set { gameTypes = value; }
        }
        ObservableCollection <String> themes = new ObservableCollection<string>();

        public ObservableCollection<String> Themes
        {
          get { return themes; }
          set { themes = value; }
        }

        void RelayCommand_NewCommandItemEvent(object sender, EventArgs e)
        {
            CommandItem ci = (CommandItem)e;
            _cmdItems.Add(ci);
            CountCmdItems++;
            if (ci.Category == "GAME" || ci.TheRelayCommand.CanUnExecute(ci.Parm))
            {
                if (ci.TheRelayCommand.DisplayText != "NewGameCommand")
                {
                    undoStack.Push(ci);
                    CountGameCmdItems++;
                }else
                {
                    //Make Sure We Only Add the "NewGameCommand" once.
                    if (undoStack.Count(log=>  log.TheRelayCommand.DisplayText == "NewGameCommand" )==0)
                        undoStack.Push(ci);                  
                }
            }
            else
                busTubStack.Push(ci);
            if (ci.Category == "MOUSE")
                CountMouseCmdItems++;
        }

        #endregion

        #region PROPERTIES
        GameConstants.GameStates result = GameConstants.GameStates.NOT_STARTED;
        GameConstants.GameStates gameState = GameConstants.GameStates.NOT_STARTED;
        Stack<CommandItem> undoStack = new Stack<CommandItem>();
        Stack<CommandItem> redoStack = new Stack<CommandItem>();
        Stack<CommandItem> busTubStack = new Stack<CommandItem>();
        public GameConstants.GameStates GameState
        {
            get { return gameState; }
            set
            {
                if (value != gameState)
                {
                    if (value == GameConstants.GameStates.IN_PLAY)
                    {
                        GameBoardEnabled = true;
                        tmr.Enabled = true;
                    }
                    //else
                    //    tmr.Enabled = false;
                    //if (value == GameConstants.GameStates.LOST ||
                    //    value == GameConstants.GameStates.WON)
                    //    GameBoardEnabled = false;

                    gameState = value;
                    switch (gameState)
                    {
                        case (GameConstants.GameStates.LOST):                           
                            //this.soundAdornment.Lost();
                            result = GameConstants.GameStates.LOST;
                            gameState = GameConstants.GameStates.LOST;                            
                            break;

                        case (GameConstants.GameStates.WON):
                            //this.soundAdornment.Won();
                            result = GameConstants.GameStates.WON;
                            gameState = GameConstants.GameStates.WON;

                            break;
                    }
                    OnPropertyChanged("GameState");
                }
            }
        }
        private ObservableCollection<CommandItem> _cmdItems = null;
        public ObservableCollection<CommandItem> CmdItems
        {
            get
            {
                if (_cmdItems == null)
                    _cmdItems = new ObservableCollection<CommandItem>();

                return _cmdItems;
            }
            set
            {
                _cmdItems = value;
            }
        }
        public IEnumerable<GamePiece> Board
        {
            get
            {
                return _cells;
            }
        }
        public bool IsBeginner
        {
            get
            {
                return gameTypeBinding[(int)GameConstants.GameTypes.BEGINNER];
            }
        }
        public bool IsIntermediate
        {
            get
            {
                return gameTypeBinding[(int)GameConstants.GameTypes.INTERMEDIATE];
            }
        }
        public bool IsAdvanced
        {
            get
            {
                return gameTypeBinding[(int)GameConstants.GameTypes.ADVANCED];
            }
        }
        public bool IsCustom
        {
            get
            {
                return gameTypeBinding[(int)GameConstants.GameTypes.CUSTOM];
            }
        }
        private string debugRow = "0";
        public string DebugRow
        {

            get { return debugRow; }
            set
            {
                debugRow = value;
                OnPropertyChanged("DebugRow");
            }
        }
        public bool IsLogWindowShown
        {
            get { return debugRow == "*"; }
            set
            {
                if (debugRow == "*")
                    DebugRow = "0";
                else if (debugRow == "0")
                    DebugRow = "*";
                OnPropertyChanged("IsLogWindowShown");
            }
        }
        private bool isExpanded = false;
        public bool IsExpanded
        {
            get { return isExpanded; }
            set
            {
                isExpanded = value;
            }
        }
        int rows = 9;
       


        bool soundsOn = true;

        public bool SoundsOn
        {
            get { return soundsOn; }
            set { soundsOn = value; }
        }
        public int Rows
        {
            get { return rows; }
            private set
            {
                rows = value;
                OnPropertyChanged("Rows");
            }
        }
        int columns = 9;
        public int Columns
        {
            get { return columns; }
            private set
            {
                columns = value;
                OnPropertyChanged("Columns");
            }
        }
        int mines = 10;
        public int Mines
        {
            get { return mines; }
            private set
            {
                mines = value;
                OnPropertyChanged("Mines");
            }
        }
        int time = 0;
        public int Time
        {
            get { return time; }
            private set
            {
                time = value; //TimeString = String.Format("{0}", time); 
                OnPropertyChanged("Time");
            }
        }
        int currCol = 0;
        public int CurrCol
        {
            get { return currCol; }
            private set
            {
                currCol = value;
                OnPropertyChanged("CurrCol");
            }
        }
        int currRow = 0;
        public int CurrRow
        {
            get { return currRow; }
            private set
            {
                currRow = value;
                OnPropertyChanged("CurrRow");
            }
        }
        bool gameBoardEnabled = true;
        public bool GameBoardEnabled
        {
            get { return gameBoardEnabled; }
            private set
            {
                gameBoardEnabled = value;
                OnPropertyChanged("GameBoardEnabled");
            }
        }
        private int minWidth = 325;
        public int MinWidth
        {
            get { return minWidth; }
            set
            {
                minWidth = value;
                OnPropertyChanged("MinWidth");
            }
        }
        private int maxWidth = 450;
        public int MaxWidth
        {
            get { return maxWidth; }
            set
            {
                maxWidth = value;
                OnPropertyChanged("MaxWidth");
            }
        }
        private int minHeight = 325;
        public int MinHeight
        {
            get { return minHeight; }
            set
            {
                minHeight = value;
                OnPropertyChanged("MinHeight");
            }
        }
        private int maxHeight = 1550;
        public int MaxHeight
        {
            get { return maxHeight; }
            set
            {
//               SystemParameters.FullPrimaryScreenHeight
// 
                maxHeight = value ;
                OnPropertyChanged("MaxHeight");
            }
        }
        private int height = 475;
        public int Height
        {
            get { return height; }
            set
            {
                if (value != height)
                    height = value;
               // height = (int)((double)value * (SystemParameters.FullPrimaryScreenHeight/GameConstants.DESIGNHEIGHTH));
                
                OnPropertyChanged("Height");
            }
        }
        private int width = 400;
        public int Width
        {
            get { return width; }
            set
            {
                if (width != value)
                   width = value;
                //width = (int)((double)value * (SystemParameters.FullPrimaryScreenWidth / GameConstants.DESIGNWIDTH));
                OnPropertyChanged("Width");
            }
        }
        string title = "Mark's Sweeper Game";
        public string Title
        {
            get { return title; }
            set { title = value; OnPropertyChanged("Title"); }
        }
        private GameConstants.GameTypes gameType;
        public GameConstants.GameTypes GameType
        {
            get { return gameType; }
            private set
            {
                gameType = value;
                OnPropertyChanged("gameType");
            }
        }
        private int countCmdItems = 0;
        public int CountCmdItems
        {
            get { return countCmdItems; }
            set
            {
                countCmdItems = value;
                OnPropertyChanged("CountCmdItems");
            }
        }
        private int countMouseCmdItems = 0;
        public int CountMouseCmdItems
        {
            get { return countMouseCmdItems; }
            set
            {
                countMouseCmdItems = value;
                OnPropertyChanged("CountMouseCmdItems");
            }
        }
        private int countGameCmdItems = 0;
        public int CountGameCmdItems
        {
            get { return countGameCmdItems; }
            set
            {
                countGameCmdItems = value;
                OnPropertyChanged("CountGameCmdItems");
            }
        }

       
        private string theme = "DEFAULT";
        public string Theme { get { return theme; } set { theme = value; OnPropertyChanged("Theme"); } }

        #endregion

        #region METHODS
        #region PRIVATE
        private void ClearMessages()
        {
            this.CountCmdItems = 0;
            this.CountMouseCmdItems = 0;
            this.CountGameCmdItems = 0;
            this.CmdItems.Clear();
        }
        private void UndoAll() 
        {
            CommandItem li = undoStack.Peek();
            while (undoStack.Count > 1 )
            {
                if (li.TheRelayCommand.CanUnExecute(li.Parm) )
                    UndoLast();
                else
                {
                   li = undoStack.Pop();
                   busTubStack.Push(li);
               //    Debugger.Break();
                }
                if (undoStack.Count > 1)
                    li = undoStack.Peek();
            }
        }
        private void UndoLast()
        {   if (undoStack.Count > 0)
            {
                //If the next command is new Game Execute And Return
                CommandItem li = undoStack.Peek();
                if (li.TheRelayCommand.DisplayText == "NewGameCommand")
                {
                    li.TheRelayCommand.Execute(li.Parm);
                    return;
                }
                //Some Commands are no longer available for Undo Put them in the BusTub
                while(!li.TheRelayCommand.CanUnExecute(li.Parm) && undoStack.Count>0)               
                {
                    if (li.TheRelayCommand.DisplayText != "NewGameCommand")
                    {
                        li = undoStack.Pop();
                        busTubStack.Push(li);
                    }
                    li = undoStack.Peek();
                }   
                //Now the next Command is either NewGameCommand or available for re-execution
                if (undoStack.Count > 0)
                    {   li = undoStack.Peek();
                        if (li.TheRelayCommand.DisplayText != "NewGameCommand")
                        {
                            li = undoStack.Pop();
                            redoStack.Push(li);
                            li.TheRelayCommand.UnExecute(li.Parm);
                        }
                        else
                            li.TheRelayCommand.Execute(li.Parm);
                    }                   
                }
        }
        
        private bool inUndoSet(string p)
        {
            return p == "GAME";
        }
        private void RedoAll()
        {
            while (redoStack.Count > 0)
            {
                RedoLast();
            }
        }
        private void RedoLast()
        {
            if (redoStack.Count > 0)
            { 
                CommandItem ci = redoStack.Peek();
                if (!ci.TheRelayCommand.CanExecute(ci.Parm))
                {
                    while (!ci.TheRelayCommand.CanExecute(ci.Parm) && redoStack.Count > 0)
                        ci = redoStack.Pop();
                }
                else if (redoStack.Count > 0)
                    ci = redoStack.Pop();
                ci.TheRelayCommand.Execute(ci.Parm);
            }
        }
        
        void tmr_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (time >= GameConstants.MAXGAMETIME)
            {
                
                EndGame(GameConstants.GameStates.LOST);
            }
            this.Time += 1;
        }
        
        private void NewGame()
        {
            SetGameParms(gameType);
            NewGame(Rows, Columns, Mines);
        }
        
        private int thisGamesMines = 0;
        
        private void NewGame(int r, int c, int mines)
        {
            GameBoardEnabled = false;
            result = GameConstants.GameStates.IN_PLAY;
            this.ClearMessages();
            redoStack.Clear();
            undoStack.Clear();
            busTubStack.Clear();
            GameBoardEnabled = false;

            tmr.Enabled = false;
            foreach (GamePiece gp in _cells)
            {
                gp.Dispose();
            }
            _cells.Clear();

            int maxSeed = r * c;
            

            Random ra = new Random();
            int n = 0;
            for (int i = 0; i < r; i++)
            {
                for (int j = 0; j < c; j++)
                {
                    GameConstants.PieceValues pv = GameConstants.PieceValues.NOMINE;
                    _cells.Add(new GamePiece(this, n++, i, j, pv));
                }
            }
            // Plant the Mines
            int nMinesPlanted = 0;
            while (nMinesPlanted < mines)
            {
                int nNextMinePlace = ra.Next(0, maxSeed);
                GamePiece gp = (GamePiece)_cells[nNextMinePlace];
                while (gp.ActualValue == GameConstants.PieceValues.MINE)
                {
                    nNextMinePlace = ra.Next(0, maxSeed);
                    gp = (GamePiece)_cells[nNextMinePlace];
                }
                gp.ActualValue = GameConstants.PieceValues.MINE;
                ++nMinesPlanted;
            }

            //Set Neighbor Values
            for (int i = 0; i < r; i++)
            {
                for (int j = 0; j < c; j++)
                {
                    if (Item(i, j).ActualValue != GameConstants.PieceValues.MINE)
                    {
                        GameConstants.PieceValues pv = GameConstants.PieceValues.NOMINE;
                        List<GamePiece> Lst = (List<GamePiece>)Neighbors(i, j);
                        int nMines = Lst.Count(gp=>gp.ActualValue==GameConstants.PieceValues.MINE);
                        pv = (GameConstants.PieceValues)((int)GameConstants.PieceValues.NOMINE + nMines);
                        GamePiece gItem = Item(i, j);
                        gItem.ActualValue = pv;
                    }
                }
            }
            Rows = r;
            Columns = c;
            Mines = thisGamesMines = mines;
            Time = 0;
            GameState = GameConstants.GameStates.NOT_STARTED;
            GameBoardEnabled = true;
            soundAdornment.NewGame();
            ShowGame();
        }

        
        private bool isNeighbor(GamePiece gp, int r, int c)
        {
            if (gp.R == r && gp.C == c)
                return false;    //self
            if (gp.R >= r - 1 && gp.R <= r + 1)
                if (gp.C >= c - 1 && gp.C <= c + 1)
                    return true;
            return false;
        }


bool LINQ = false;
        private IEnumerable<GamePiece> Neighbors(int r, int c)
        {
            if (LINQ)

            {
                // TODO refactor to Linq below.  It crashes in the XAML when i use it :(
                // I just hate XAML Crashes
                //We really do not need to traverse the entire collection to solve this every time
                var neighbors =
                    from gp in _cells
                    where (gp.R >= r - 1 && gp.R <= r + 1) &&
                          (gp.C >= c - 1 && gp.C <= c + 1) &&
                         !(gp.R == r     && gp.C == c    )
                    select gp;

                return neighbors;              
            }
            else
            {
                List<GamePiece> retVal = new List<GamePiece>(8);
                foreach (GamePiece gp in _cells)
                {
                    if (isNeighbor(gp, r, c))
                    {
                        retVal.Add(gp);
                        if (retVal.Count >= 8)
                            break;
                    }
                }
                return retVal;
            }
        }

        private GamePiece Item(int r, int c)
        {
           
            var  retval = from gp in _cells
                      where gp.R == r && gp.C == c
                      select gp;
            return retval.First();            
        }

        private IEnumerable<GamePiece> GetRow(int r)
        {

            var retVal = from gp in _cells
                         where gp.R == r
                         select gp;

            return retVal;
        }

        private IEnumerable<GamePiece> GetCol(int c)
        {
            var retVal = from gp in _cells
                         where gp.C == c
                         select gp;

            return retVal;
        }
        private void UnPlay(Point pt)
        {
            //Handle WON n LOST CONDITIONS TOO
            UnPlay((int)pt.X,(int) pt.Y);         
        }

        private void UnPlay(int r, int c)
        {
            GamePiece gp = Item(r, c);
            UnPlay(gp.R, gp.C, true);
           
        }
        private void Play(Point pt, bool sendClickSound )
        {
            Play((int)pt.X, (int)pt.Y, sendClickSound);

        }
        private void Play(int r, int c, bool allowclick = false)
        {
            GamePiece gp = Item(r, c);
            if (!gp.IsPlayed && gp.Value != GameConstants.PieceValues.FLAGGED)
            {
                if (gp.ActualValue == GameConstants.PieceValues.MINE)
                {
                    gp.Value = GameConstants.PieceValues.WRONGCHOICE;
                    EndGame(GameConstants.GameStates.LOST);
                    
                }
                else
                {
                    StartGameIfNeeded();
                    gp.Value = gp.ActualValue;
                    
                    if (allowclick)
                        soundAdornment.ClickItemOK();
                    
                    if (gp.Value == GameConstants.PieceValues.NOMINE)
                    {
                        PlayNeighbors(gp);
                        GameState = GameConstants.GameStates.IN_BONUSPLAY;
                    }else
                        GameState = GameConstants.GameStates.IN_PLAY;
                   
                }
            }
        }

        private void UnPlay(int r, int c, bool allowclick = false)
        {
            GamePiece gp = Item(r, c);
            if (gp.IsPlayed && gp.Value != GameConstants.PieceValues.FLAGGED)
            {                
                    gp.Value = GameConstants.PieceValues.BUTTON;                   
                    if (gp.ActualValue == GameConstants.PieceValues.NOMINE)
                        UnPlayNeighbors(gp);
                    GameState = GameConstants.GameStates.IN_PLAY;
                    if (allowclick == true)
                    {
                        this.soundAdornment.Swoosh();

                    }

                
            }
        }

        private void UnPlayNeighbors(GamePiece gp)
        {
            List<GamePiece> lst = (List<GamePiece>)(this.Neighbors(gp.R, gp.C));
            foreach (GamePiece gmPiece in lst)
            {
                if (gmPiece.Value != GameConstants.PieceValues.BUTTON)
                    UnPlay(gmPiece.R, gmPiece.C, true);
            }
        }

        private void PlayNeighbors(GamePiece gp)
        {
            GameState = GameConstants.GameStates.IN_BONUSPLAY;
            List<GamePiece> lst = (List<GamePiece>)(this.Neighbors(gp.R, gp.C));
            foreach (GamePiece gmPiece in lst)
            {
                Play(new Point(gmPiece.R, gmPiece.C), false);
            }
        }

        private void StartGameIfNeeded()
        {
            if (!tmr.Enabled)
            {
                GameState = GameConstants.GameStates.IN_PLAY;
                tmr.Enabled = true;
            }
        }

       

        private void Flag(int r, int c)
        {
            GamePiece gp = Item(r, c);
            if (gp.Value == GameConstants.PieceValues.BUTTON)
            {
                gp.Value = GameConstants.PieceValues.FLAGGED;
                Mines--;
                soundAdornment.ClickItemOK();
                GameState = GameConstants.GameStates.IN_PLAY;

            }
            else if (gp.Value == GameConstants.PieceValues.FLAGGED)
            {
                gp.Value = GameConstants.PieceValues.BUTTON;
                Mines++;
                soundAdornment.ClickItemOK();
                GameState = GameConstants.GameStates.IN_PLAY;
            }
            if (Mines == 0)
                if (EvaluateWin())
                    
                    EndGame(GameConstants.GameStates.WON);
        }

        private void EndGame(GameConstants.GameStates gState)
        {
            switch (gState)
            {
                case (GameConstants.GameStates.LOST):
                    soundAdornment.Lost();
                    break;
                case (GameConstants.GameStates.WON):
                    soundAdornment.Won();
                    break;
                
            }
            tmr.Enabled = GameBoardEnabled = false;
            GameState = gState;
        }

        private bool EvaluateWin()
        {
            //TODO  1) Evaluate entry conditions
            //      2) Optimize the linq
            bool allFlagsSetCorrectly = true;
            bool continueEval = true;
            var mineesCollection = from gp in _cells
                                   where gp.ActualValue == GameConstants.PieceValues.MINE
                                   select gp;
            // Every one of the mines has to have a flag in order to win
            for (int i = 0;
                 i < mineesCollection.Count() && continueEval;
                 i++)
            {

                if (mineesCollection.ElementAt(i).Value != GameConstants.PieceValues.FLAGGED)
                    {
                        continueEval = allFlagsSetCorrectly = false;
                    }
            }

            return allFlagsSetCorrectly;
        }

        
        // Create the OnPropertyChanged method to raise the event 
        protected void OnPropertyChanged(string name)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(name));
            }
        }

        bool[] gameTypeBinding = { true, false, false, false };
        private void ShowGame()
        {
            int newW = this.width;
            int newH = this.height;

            Height = newH;
            Width = newW;

            MinWidth = (int)0.75 * newW;
            MaxWidth = (int)1.25 * newW;
            MinHeight = (int)0.75 * newH;
            MaxHeight = (int)1.25 * newH;

        }
        private void SetGameParms(GameConstants.GameTypes gType)
        {
            int ndx = (int)gType;
            rows = GameConstants.GameTypeArray[ndx, GameConstants.NDX_ROW];
            columns = GameConstants.GameTypeArray[ndx, GameConstants.NDX_COL];
            mines = GameConstants.GameTypeArray[ndx, GameConstants.NDX_MINE];
            width = GameConstants.GameTypeArray[ndx, GameConstants.NDX_WIDTH];
            height = GameConstants.GameTypeArray[ndx, GameConstants.NDX_HEIGHT];

            for (GameConstants.GameTypes gt = GameConstants.GameTypes.BEGINNER;
                                         gt <= GameConstants.GameTypes.CUSTOM;
                                         gt++)
            {
                int itemIndex = (int)gt;
                if (ndx != itemIndex)
                    gameTypeBinding[itemIndex] = false;
                else
                    gameTypeBinding[itemIndex] = true;

            }
            gameType = gType;
            Rows = rows;
            Columns = columns;
            Mines = mines;
            Height =  (int)((double)height * (SystemParameters.FullPrimaryScreenHeight / GameConstants.DESIGNHEIGHT)); ;
            Width =   (int)((double)width  * (SystemParameters.FullPrimaryScreenWidth / GameConstants.DESIGNWIDTH));
            OnPropertyChanged("IsBeginner");
            OnPropertyChanged("IsIntermediate");
            OnPropertyChanged("IsAdvanced");
            OnPropertyChanged("IsCustom");
        }
        #endregion
       
        #region PUBLIC
      
        #endregion
        #endregion

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
                tmr.Elapsed -= tmr_Elapsed;
                tmr.Dispose();
                RelayCommand.NewCommandItemEvent -= RelayCommand_NewCommandItemEvent;
               
            }
            disposed = true;
        }
        ~SweeperViewModel()
        {
            Dispose(false);
        }
        #endregion

    }//END CLASS
}//END NAMESPACE

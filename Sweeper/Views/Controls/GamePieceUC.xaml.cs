using Sweeper.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;


namespace Sweeper.Views.Controls
{
	/// <summary>
	/// Interaction logic for GamePieceUC.xaml
	/// </summary>
	public partial class GamePieceUC : UserControl
	{
        //private ViewModels.SweeperViewModel sweeperViewModel;
        public static readonly DependencyProperty ValueProperty =
        DependencyProperty.Register("Value", typeof(Sweeper.ViewModels.GameConstants.PieceValues),
       typeof(GamePieceUC), new PropertyMetadata(GameConstants.PieceValues.BUTTON));
        
       public static readonly DependencyProperty StrokeProperty =
       DependencyProperty.Register("Stroke", typeof(System.Windows.Media.Brush),
      typeof(GamePieceUC), new PropertyMetadata(null));

       public static readonly DependencyProperty FillProperty =
      DependencyProperty.Register("Fill", typeof(System.Windows.Media.Brush),
     typeof(GamePieceUC), new PropertyMetadata(null));
      static DrawingBrush []PieceBrushes    = new DrawingBrush[15];
      private void LoadBrushConverter()
      {
          PieceBrushes[(int)GameConstants.PieceValues.BLANK] = new DrawingBrush(null);
          PieceBrushes[(int)GameConstants.PieceValues.BUTTON] = (DrawingBrush)Application.Current.FindResource("BUTTON");
          PieceBrushes[(int)GameConstants.PieceValues.PRESSED] = (DrawingBrush)Application.Current.FindResource("PRESSED");
          PieceBrushes[(int)GameConstants.PieceValues.FLAGGED] = (DrawingBrush)Application.Current.FindResource("FLAG");
          PieceBrushes[(int)GameConstants.PieceValues.WRONGCHOICE] = (DrawingBrush)Application.Current.FindResource("P_MINESHADOW-WRONG");
          PieceBrushes[(int)GameConstants.PieceValues.MINE] = (DrawingBrush)Application.Current.FindResource("P_MINE");
          PieceBrushes[(int)GameConstants.PieceValues.NOMINE] = (DrawingBrush)Application.Current.FindResource("P_BLANK");//new DrawingBrush(null)
          PieceBrushes[(int)GameConstants.PieceValues.ONEMINE] = (DrawingBrush)Application.Current.FindResource("P_ONE");
          PieceBrushes[(int)GameConstants.PieceValues.TWOMINE] = (DrawingBrush)Application.Current.FindResource("P_TWO");
          PieceBrushes[(int)GameConstants.PieceValues.THREEMINE] = (DrawingBrush)Application.Current.FindResource("P_THREE");
          PieceBrushes[(int)GameConstants.PieceValues.FOURMINE] = (DrawingBrush)Application.Current.FindResource("P_FOUR");
          PieceBrushes[(int)GameConstants.PieceValues.FIVEMINE] = (DrawingBrush)Application.Current.FindResource("P_FIVE");
          PieceBrushes[(int)GameConstants.PieceValues.SIXMINE] = (DrawingBrush)Application.Current.FindResource("P_SIX");
          PieceBrushes[(int)GameConstants.PieceValues.SEVENMINE] = (DrawingBrush)Application.Current.FindResource("P_SEVEN");
          PieceBrushes[(int)GameConstants.PieceValues.EIGHTMINE] = (DrawingBrush)Application.Current.FindResource("P_EIGHT");
      }
      public System.Windows.Media.Brush Fill
       {
           get {return  (System.Windows.Media.Brush)GetValue(FillProperty); }
           set { SetValue(FillProperty, value);
                 PieceRectangle.Fill = value; 
           }
       }
      public static readonly DependencyProperty StrokeThicknessProperty =
     DependencyProperty.Register("StrokeThickness", typeof(double),
    typeof(GamePieceUC), new PropertyMetadata(2.0));
        public GameConstants.PieceValues Value
        {
            get { return (GameConstants.PieceValues)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); OnPropertyChanged("Value");
            Fill = PieceBrushes[(int)value];
            }
        }
		public GamePieceUC()
		{
			this.InitializeComponent();
            LayoutRoot.DataContext = this;
            LoadBrushConverter();
		}
        static Sweeper.ViewModels.SweeperViewModel gameBoard = null;
        public GamePieceUC(int nItem, GameConstants.PieceValues pv)
        {
            linearIndex = nItem;
           // IsPlayed = false;
            value = pv;
        }

        public GamePieceUC(ViewModels.SweeperViewModel sweeperViewModel, int p, int r, int c, GameConstants.PieceValues actualValue)
        {

            this.sweeperViewModel = sweeperViewModel;
            linearIndex = p;
            this.r = r;
            this.c = c;
            value = GameConstants.PieceValues.EIGHTMINE;
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
        ~GamePieceUC()
        {
            Dispose(false);
        }
        #endregion

	}
}
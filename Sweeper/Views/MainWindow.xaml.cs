using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Sweeper.ViewModels;
using System.Diagnostics;
//TODO Remove all Code Behind
namespace Sweeper.Views
{   
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, IDisposable
    { 
        SweeperViewModel game;    
        public MainWindow()
        {
            InitializeComponent();
            game = new SweeperViewModel();
            this.DataContext = game;
            game.NewGameCommand.Execute("BEGINNER");           
        }

        #region DISPOSE
        /// <summary>
        /// Okay You said you wanted no Code Behind.  Close.  But the Analyze tool in VS.Net 2013 
        /// Told me to dispose the Sweeper Window
        /// </summary>
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
                game.Dispose();
            }

            disposed = true;
        }
        ~MainWindow()
        {
            Dispose(false);
        }
        #endregion

       
        private void Window_Closing_1(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}

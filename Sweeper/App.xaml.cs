using Sweeper.Properties;
using Sweeper.ViewModels;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;


namespace Sweeper
{
    
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        App mySweeperApp = null;

        public App MySweeperApp
        {
            get { return mySweeperApp; }
            
        }
        public App()
        {
            mySweeperApp = this;
        }
        private string theme = "DEFAULT";

        public string Theme
        {
            get { return theme; }
            set { theme = value;
                  RaiseChangeThemeEvent(theme);
            }
        }
        private static EventHandler changeThemeEvent;
        public static event EventHandler ChangeThemeEvent
        {
            add { changeThemeEvent += value; }
            remove { changeThemeEvent -= value; }
        }
       

       
        protected static void OnChangeThemeEvent(ApplicationThemeEventArg appTheme)
        {
          //  ObservableCollection<ResourceDictionary> rd = (List)<ResourceDictionary>)Application.Current.Resources.MergedDictionaries[0];
            
            if (changeThemeEvent != null)
            {

                if (appTheme == null)
                    Debugger.Break();
                changeThemeEvent(null, appTheme );
            }
        }
        private static void RaiseChangeThemeEvent(string str)
        {
            OnChangeThemeEvent(new ApplicationThemeEventArg(str));
        }
        protected override void OnExit(ExitEventArgs e)
        {
            foreach (Window w in Application.Current.Windows)
                w.Close();
            base.OnExit(e);
        }
        protected override void OnStartup(StartupEventArgs e)
        {
            PresentationTraceSources.Refresh();
            PresentationTraceSources.DataBindingSource.Listeners.Add(new ConsoleTraceListener());
            PresentationTraceSources.DataBindingSource.Listeners.Add(new DebugTraceListener());
            PresentationTraceSources.DataBindingSource.Switch.Level = SourceLevels.Warning | SourceLevels.Error;
            base.OnStartup(e);
        }

        internal static void SetTheme(string p)
        {
            ResourceDictionary myresourcedictionary = new ResourceDictionary();

            Uri u = new Uri("Resources/"+p+"-THEME.xaml",
                    UriKind.Relative);
            //Uri u = new Uri("Resources//IMAGE-ITEMS//THEMES//CHOCALATE//THEME.xaml",
            //        UriKind.Relative);
            myresourcedictionary.Source = u;

            Application.Current.Resources.MergedDictionaries.Clear();
           
            Application.Current.Resources.MergedDictionaries.Add(myresourcedictionary);
           
            RaiseChangeThemeEvent(p);
        }
    }

    public class DebugTraceListener : TraceListener
    {
        public override void Write(string message)
        {
            // I will solve this and then reinstate the trace listeners by uncommenting.  BAFFLING Error. grrrrr  
            // Comes From MainWindow.xaml <Setter Property="Fill" Value="{Binding Value, Converter={StaticResource formatter}}"/>
            //System.Windows.Data Error: 40 : BindingExpression path error: 'Value' property not found on 'object' ''SweeperViewModel' (HashCode=57952773)'. 
            //BindingExpression:Path=Value; DataItem='SweeperViewModel' (HashCode=57952773); target element is 'Rectangle' (Name=''); target property is 'Fill' (type 'Brush')
             //Debugger.Break();
             //Write(message);
        }

        public override void WriteLine(string message)
        {
            //Debugger.Break();
            // WriteLine(message);
        }
    }
}

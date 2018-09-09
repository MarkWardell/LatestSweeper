using Sweeper.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace Sweeper.Views
{
    public class FaceButtonConverter : System.Windows.Data.IValueConverter,  IDisposable
    {     
             
        public FaceButtonConverter()
        {            
             
        }
        
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {          
            VisualBrush returnBrush = null;          
            GameConstants.GameStates gs = (GameConstants.GameStates)value;
            
            try { 
            switch (gs)
            {
                case (GameConstants.GameStates.NOT_DETERMINED   ):
                case (GameConstants.GameStates.NOT_STARTED      ):
                case (GameConstants.GameStates.IN_PLAY          ):         
                    returnBrush = BrushByTheme("SMILEUP","");
                    break;
                case (GameConstants.GameStates.IN_BONUSPLAY):
                    returnBrush = BrushByTheme("WINKUP","");
                    break;
                case (GameConstants.GameStates.IN_DECISION):      
                    returnBrush = BrushByTheme("SURPRISEDN","");
                    break;
                case (GameConstants.GameStates.WON):
                    returnBrush = BrushByTheme("GRINUP","");
                    break;

                case (GameConstants.GameStates.LOST):
                    returnBrush = BrushByTheme("SADUP", "");
                    break;

            }          
            }catch (Exception e)
            {
                Debug.WriteLine("GameState{" + gs + "} " + "Exception{" + e.Message);
                Debugger.Break();
            }
            return returnBrush;
        }

        

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
        VisualBrush BrushByTheme(string partialKey, string themeName)
        {
            try {
               return  (VisualBrush)Application.Current.FindResource(partialKey);
               //return (VisualBrush)Application.Current.FindResource(themeName + "_" + partialKey);
            }
            catch
            {
                Debug.WriteLine("BrushByTheme - Could Not Find{" + themeName + "_" + partialKey + "}");
                Debugger.Break();
                return null;
            }


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
                //App.ChangeThemeEvent -= this.App_ChangeThemeEvent;    
            }

            disposed = true;
        }
        ~FaceButtonConverter()
        {
          Dispose(false);
        }
        #endregion
    }
}

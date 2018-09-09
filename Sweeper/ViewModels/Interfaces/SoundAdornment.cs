using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

using System.Diagnostics;


namespace Sweeper.ViewModels
{
    public class SoundAdornment : IAdornGameWithSounds
    {
        const int OK     = 0;
        const int LOST   = 1;
        const int START  = 2;
        const int RECORD = 3;
        const int WON   = 4;
        const int SWOOSH = 5;


        
        MediaPlayer[] mps = new MediaPlayer[5];
        MediaPlayer mp = new MediaPlayer();
        static string MyPath
        {
            get { return Environment.CurrentDirectory+@"\Resources\Sounds\"; }
        }
        public SoundAdornment()
        {
            mps[OK]     = new MediaPlayer();
            mps[OK].Open(new Uri(MyPath + "CLICK.WAV"));
            mps[LOST]   = new MediaPlayer();
            mps[LOST].Open(new Uri(MyPath + "EXPLODE.WAV"));
            mps[START]  = new MediaPlayer();
            mps[START].Open(new Uri(MyPath + "START.WAV"));
            mps[RECORD] = new MediaPlayer();
            mps[RECORD].Open(new Uri(MyPath + "Tada.wav"));
            mps[WON] = new MediaPlayer();
            mps[WON].Open(new Uri(MyPath + "Tada.wav"));
            mps[SWOOSH] = new MediaPlayer();
            mps[SWOOSH].Open(new Uri(MyPath + "Shoosh.wav"));

        }
        public void ClickItemOK()
        {
            mps[OK].Play();
            mps[OK].Position = new TimeSpan(0);
           
           
        }

        public void Lost()
        {
            mps[LOST].Play();
            mps[LOST].Position = new TimeSpan(0);
        }

        public void NewGame()
        {
            mps[START].Play();
            mps[START].Position = new TimeSpan(0);
           
        }

        public void NewRecord()
        {
            mps[RECORD].Play();
            mps[RECORD].Position = new TimeSpan(0);
            
        }
        public void Won()
        {
            mps[WON].Play();
            mps[WON].Position = new TimeSpan(0);

        }
        public void Swoosh()
        {
            mps[SWOOSH].Play();
            mps[SWOOSH].Position = new TimeSpan(0);

        }
         #region DISPOSE
        // No Need to Dispose here it has no Unmanaged resource 
        public void Dispose()
        {
            
        }
        #endregion



       
    }
}

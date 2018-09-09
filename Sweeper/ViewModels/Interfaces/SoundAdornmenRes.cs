using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

using System.Diagnostics;
using System.Media;


namespace Sweeper.ViewModels
{
    public class SoundAdornmentRes : IAdornGameWithSounds, IDisposable
    {
        const int OK = 0;
        const int LOST = 1;
        const int START = 2;
        const int RECORD = 3;
        const int WON = 4;
        const int SWOOSH = 5;
        SoundPlayer[] mps = new SoundPlayer[6];


        public SoundAdornmentRes()
        {
            mps[OK] = new SoundPlayer(Sweeper.Properties.Resources.CLICK);
            mps[LOST] = new SoundPlayer(Sweeper.Properties.Resources.EXPLODE);
            mps[START] = new SoundPlayer(Sweeper.Properties.Resources.Start);
            mps[RECORD] = new SoundPlayer(Sweeper.Properties.Resources.CLICK);
            mps[WON] = new SoundPlayer(Sweeper.Properties.Resources.TaDa);
            mps[SWOOSH] = new SoundPlayer(Sweeper.Properties.Resources.SWOOSH);
        }
        public void ClickItemOK()
        {

            mps[OK].Play();
        }

        public void Lost()
        {
            mps[LOST].Play();
        }

        public void NewGame()
        {
            mps[START].Play();
        }

        public void NewRecord()
        {

        }
        public void Won()
        {
            mps[WON].Play();
        }
        public void Swoosh()
        {
            mps[SWOOSH].Play();
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
                foreach (SoundPlayer sp in mps)
                {
                    sp.Dispose();
                }
            }

            disposed = true;
        }
        ~SoundAdornmentRes()
        {
          Dispose(false);
        }
        #endregion
    }
}

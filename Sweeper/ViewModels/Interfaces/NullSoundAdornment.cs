using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sweeper.ViewModels
{
    public class NullSoundAdornment : IAdornGameWithSounds
    {
        public void ClickItemOK()
        {
            // Meant to be empty
        }

        public void Lost()
        {
            // Meant to be empty
        }

        public void NewGame()
        {
            // Meant to be empty
        }

        public void NewRecord()
        {
            // Meant to be empty
        }
        public void Won()
        {
            // Meant to be empty
        }
        public void Swoosh()
        {
            // Meant to be empty

        }
        public void Dispose() { }
    }
}

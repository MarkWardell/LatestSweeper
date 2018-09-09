using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sweeper.ViewModels
{
    public interface IAdornGameWithSounds
    {
        void ClickItemOK();
        void Lost();
        void NewGame();
        void Won();
        void NewRecord();
        void Swoosh();
        void Dispose();
    }
}

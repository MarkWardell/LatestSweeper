using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sweeper.ViewModels
{
    interface IModifyCustomGame
    {
        int CustomMines
        {
            get;
            set;
        }
        int CustomRows
        {
            get;
            set;
        }
        int CustomColumns
        {
            get;
            set;

        }
    }
}

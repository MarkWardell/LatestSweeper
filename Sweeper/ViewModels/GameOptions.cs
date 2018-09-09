using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sweeper.ViewModels
{
    public class GameOptions
    {
        private string theme;

        public string Theme
        {
            get { return theme; }
            set { theme = value; }
        }
        private int customRows;
        public int CustomRows
        {
            get { return customRows; }
            set { customRows = value; }
        }
        private int customColumns;

        public int CustomColumns
        {
            get { return customColumns;  }
            set { customColumns = value; }
        }
    }
}

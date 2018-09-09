#define SEPARATE_CMDS
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sweeper.ViewModels
{
    public class GameConstants
    {
        /// <summary>
        /// When the timer Hits MAXGAMETIME another way to lose occurrs
        /// </summary>
        public const int MAXGAMETIME = 1000;
        
        /// <summary>
        /// GameTypes one for each row of GameTypeArray
        /// </summary>
        public enum GameTypes
        {
            BEGINNER,
            INTERMEDIATE,
            ADVANCED,
            CUSTOM
        }
        public const double DESIGNWIDTH    = 2560;
        public const double DESIGNHEIGHT  = 1418;
        /// <summary>
        /// Array holds Game Definitions for the Game types
        /// Rows   Col    Mines  Width  Height
        ///                                 //Beginner
        ///                                 //Intermediate
        ///                                 //Advanced
        ///                                 //Custom
        ///  All is good At 2560 X 1418                               
        /// </summary>
        public static int [,]GameTypeArray = {
                                         {9,    9,      10,     500,    600 }, //Beginner
                                         {16,   16,     40,     1050,   1000 }, //Intermediate
                                         {16,   30,     99,     1500,   1050 }, //Advanced
                                         {20,   30,     115,    950,    1100}  //Custom
                                        };

        // Index Interpretations for above GameTypeArray Columns
        public const int NDX_ROW    = 0;
        public const int NDX_COL    = 1;
        public const int NDX_MINE   = 2;
        public const int NDX_WIDTH  = 3;
        public const int NDX_HEIGHT = 4;

        /// <summary>
        /// This array has a one to one mapping to the Pieces on the Board
        /// Look at BrushConverter.cs
        /// </summary>
        public enum PieceValues
        {
            NOMINE      ,
            ONEMINE     ,
            TWOMINE     ,
            THREEMINE   ,
            FOURMINE    ,
            FIVEMINE    ,
            SIXMINE     ,
            SEVENMINE   ,
            EIGHTMINE   ,   
            BLANK       ,
            BUTTON      ,
            PRESSED     ,
            FLAGGED     ,
            WRONGCHOICE ,
            MINE                  
                  
        }

        /// <summary>
        /// The State Values for the Game
        /// </summary>
        public enum GameStates
        {
            NOT_DETERMINED,
            NOT_STARTED,
            IN_DECISION,
            IN_PLAY,
            IN_BONUSPLAY,
            WON,
            LOST
        }

        /// <summary>
        /// This array has a one to one mapping to the Pieces available for the face
        /// Look at FaceConverter.cs
        /// </summary>
        public enum FaceStates
        {
            SMILE,
            SMILE_PRESSED,
            TENSE,
            TENSE_PRESSED,
            WINK,
            WINK_PRESSED,
            GRIN,
            GRIN_PRESSED,
            SAD,
            SAD_PRESSED
        }
       
    }
}

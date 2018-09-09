using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Shapes;

namespace Sweeper.ViewModels
{
    public  class
      RelayCommand : ICommand
    {
        #region Fields
        private static int seq = 0;

        public int Seq
        {
            get { return seq; }
            set { seq = value; }
        }
        protected   int ExeSequence
        {
            get { return seq++; }
        }
       

        protected static int NextExeSequence
        {
            get { return currentExeSequence++; }
            
        }

        private static int currentExeSequence=0;

        protected static int CurrentExeSequence
        {
            get { return currentExeSequence; }
            
        }
        
        protected int ?sequence = null;        
        private readonly Action<object> _execute;
        private readonly Predicate<object> _canExecute;

        protected  Action<object> _unexecute;
        protected  Predicate<object> _canUnExecute;

        public Predicate<object> CanUnExecute
        {
            get { return _canUnExecute; }
        } 


        private string _displayText;
        private string _category;
        

        //bool alwaysUnExecute()
        //{
        //    return true;
        //}
        //public delegate void NewCommand(RelayCommand rcmd);
        private static EventHandler newCommandItemEvent;
        public static event EventHandler NewCommandItemEvent
        {
            add    { newCommandItemEvent += value; }
            remove { newCommandItemEvent -= value; }
        }
      
        protected virtual void OnNewCommandItemEvent(CommandItem li)
        {
            if (newCommandItemEvent != null)
            {
                newCommandItemEvent(this,li);
            }
        }
        private void RaiseNewCommandItemEvent(object o, CommandItem li)
        {
            OnNewCommandItemEvent(li);
        }

        private static EventHandler refreshStacksEvent;
        public static event EventHandler RefreshStacksEvent
        {
            add { refreshStacksEvent += value; }
            remove { refreshStacksEvent -= value; }
        }

        protected virtual void OnRefreshStacksEvent(CommandItem li)
        {
            if (refreshStacksEvent != null)
            {
                refreshStacksEvent(this, li);
            }
        }
       
        //
        public string Category
        {
            get { return _category; }
            set { _category = value; }
        }

        #endregion // Fields

        #region Constructors

        //<summary>
        //Creates a new command that can always execute.
        //</summary>
        //<param name="execute">The execution logic.</param>
        //public RelayCommand(Action<object> execute)
        //    : this(execute, null)
        //{

        //}

        protected RelayCommand(Action<object> execute,
                            Predicate<object> canExecute)
        {
            if (execute == null)
                throw new ArgumentNullException("execute");

            _execute = execute;
            _canExecute = canExecute;

            _unexecute = this.UnExecute;
            _canUnExecute = this.CanUnexecute;



        }

        private static bool AlwaysUnExecute(Point obj)
        {
            return true;
        }
        //<summary>
        //Creates a new command.
        //</summary>
        //<param name="execute">The execution logic.</param>
        //<param name="canExecute">The execution status logic.</param>
        //public RelayCommand(Action<object> execute, Predicate<object> canExecute)
        //    : this(execute, canExecute, "")
        //{
        //}

        public RelayCommand(Action<object> execute,
                            Predicate<object> canExecute,
                            string displayText,
                            string cat)
        {
            if (execute == null)
                throw new ArgumentNullException("execute");

            _execute = execute;
            _canExecute = canExecute;
            _displayText = displayText;
            _canUnExecute = NoWeCant;
            _unexecute = defaultUnexecute;
            _category = cat;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>

        public RelayCommand(Action<object> execute,
                            Predicate<object> canExecute,
                            string displayText,
                            string category,
                            Action<object> unexecute,
                            Predicate<object> canUnExecute)
            : this(execute, canExecute, displayText, category)
        {
            if (execute == null)
                throw new ArgumentNullException("execute");

            _unexecute = unexecute;
            _canUnExecute = canUnExecute;

        }
        private bool YesWeCan(object obj = null)
        {
            return true;
        }
        private bool NoWeCant(object obj = null)
        {
            return false;
        }



        private void defaultUnexecute(object o)
        {
            // RaiseNewLogItemEvent(new LogItem(this, this.DisplayText, o, this.Category));
        }

        private bool defaultCanUnexecute(object o)
        {
            return true;
        }


        public string DisplayText
        {
            get { return _displayText; }
            set
            {
                _displayText = value;
            }
        }

        #endregion // Constructors

        #region ICommand Members

        // [DebuggerStepThrough]
        public bool CanExecute(object parameter)
        {
            if (_canExecute == null)
                return true;
            else
                return _canExecute(parameter);

        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }
        object parm = null;

        public object Parm
        {
            get { return parm; }
           
        }
        private string summary;

        public string Summary
        {
            get { return summary; }
            
        }
        DateTime timeStamp;

        public DateTime TimeStamp
        {
            get { return timeStamp; }
            set { timeStamp = value; }
        }
        public void Execute(object parameter)
        {
            parm = parameter;
            _execute(parameter);
            timeStamp = DateTime.Now;
            seq++;
            string str = "";
            if (this._category == "MOUSE")
            {
                if (parameter is MouseEventArgs)
                {
                    MouseEventArgs me = (MouseEventArgs)parameter;
                    if (me.OriginalSource is Rectangle)
                    {
                        Rectangle r = (Rectangle)me.OriginalSource;
                        GamePiece gp = (GamePiece)r.Tag;
                        str = String.Format("({0})({1})\t{2:}.{3} ({4},{5})", seq,DateTime.Now.ToLocalTime(), _category,
                                 _displayText, gp.R,gp.C);
                    }
                }
            }
            else
            {
                str = String.Format("({0})({1:G})\t{2}.{3} ({4})", seq, DateTime.Now.ToLocalTime(), _category,
                                _displayText, parameter);

            }
            summary = str;
            CommandItem li = new CommandItem(this, str, parameter, _category, seq); 
            RaiseNewCommandItemEvent(this,li);
            RaiseRefreshStacksEvent(this,li);
        }

        private void RaiseRefreshStacksEvent(RelayCommand relayCommand, CommandItem li)
        {
            OnRefreshStacksEvent( li);
        }

        public override String ToString()
        { return summary; }

        public void UnExecute(object parameter)
        {
            _unexecute(parameter);
            RaiseRefreshStacksEvent(null,null);
            Debug.WriteLine("UnExecute{" + _displayText + "} parm={" + parameter + "}");
            Debug.WriteLine(this);
            //RaiseNewLogItemEvent(new LogItem(this, str, parameter, _category));

        }
        public bool CanUnexecute(object  parm)
        {
            return false;
        }
        #endregion // ICommand Members


        public static int sequencer = 0;

        internal static int GetNextSequencer()
        {
            return sequencer++;
        }
    }



}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sweeper.ViewModels
{
    public class RelayCommandSequenceInitiator : RelayCommand
    {
        //protected override  int ExeSequence
        //{
        //    get{return RelayCommand.NextExeSequence;}
        //}
   
        public RelayCommandSequenceInitiator(Action<object> execute,
                                      Predicate<object> canExecute,
                                      string displayText,
                                      string cat)
            : base(execute,
                  canExecute,
                  displayText,
                  cat)
        {
            Seq = ExeSequence;

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>

        public RelayCommandSequenceInitiator(Action<object> execute,
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
            Seq =  ExeSequence;

        }

    }
}

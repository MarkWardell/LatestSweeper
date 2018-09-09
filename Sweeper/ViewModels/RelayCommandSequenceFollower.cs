using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sweeper.ViewModels
{
    public class RelayCommandFollower : RelayCommand
    {
        //protected override int ExeSequence
        //{
        //    get { return RelayCommand.CurrentExeSequence+1; }
        //}
        public RelayCommandFollower(Action<object> execute,
                                              Predicate<object> canExecute,
                                              string displayText,
                                              string cat)
            : base(execute,
                   canExecute,
                   displayText,
                   cat)
        {
            

        }

         //<summary>
         
         //</summary>
         //<param name="obj"></param>
         //<returns></returns>

        public RelayCommandFollower(Action<object> execute,
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
    }
}

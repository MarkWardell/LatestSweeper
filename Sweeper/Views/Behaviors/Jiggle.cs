using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Interactivity;
//using Microsoft.Expression.Interactivity.Core;

namespace Sweeper
{
	public class Jiggle : Behavior<VisualBrush>
	{
		public Jiggle()
		{
			// Insert code required on object creation below this point.

			//
			// The line of code below sets up the relationship between the command and the function
			// to call. Uncomment the below line and add a reference to Microsoft.Expression.Interactions
			// if you choose to use the commented out version of MyFunction and MyCommand instead of
			// creating your own implementation.
			//
			// The documentation will provide you with an example of a simple command implementation
			// you can use instead of using ActionCommand and referencing the Interactions assembly.
			//
			//this.MyCommand = new ActionCommand(this.MyFunction);
		}

		protected override void OnAttached()
		{
			base.OnAttached();

			// Insert code that you would want run when the Behavior is attached to an object.
		}

		protected override void OnDetaching()
		{
			base.OnDetaching();

			// Insert code that you would want run when the Behavior is removed from an object.
		}

		/*
		public ICommand MyCommand
		{
			get;
			private set;
		}
		 
		private void MyFunction()
		{
			// Insert code that defines what the behavior will do when invoked.
		}
		*/
	}
}
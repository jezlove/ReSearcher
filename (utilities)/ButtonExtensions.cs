/*
	C# "ButtonExtensions.cs"
	by Jeremy Love <mailto:jez@jezlove.uk>
	copyright 2020
*/

using System;
using System.Windows.Forms;

namespace ReSearcher {

	public static class ButtonExtensions {

		#region actions

			public static Button withAction(this Button thisButton, Action action) {
				thisButton.Click += new EventHandler((sender, eventArgs) => { action(); });
				return(thisButton);
			}

		#endregion

	}

}
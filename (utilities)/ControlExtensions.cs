/*
	C# "ControlExtensions.cs"
	by Jeremy Love <mailto:jez@jezlove.uk>
	copyright 2020
*/

using System;
using System.Windows.Forms;

namespace ReSearcher {

	public static class ControlExtensions {

		#region sub-controls

			public static void appendControls(this Control thisControl, params Control[] controls) {
				using(new ControlLayoutSuspension(thisControl)) {
					thisControl.Controls.AddRange(controls);
				}
			}

			public static TControl withControls<TControl>(this TControl thisControl, params Control[] controls) where TControl : Control {
				thisControl.appendControls(controls);
				return(thisControl);
			}

		#endregion

		#region help

			public static TControl withHelpPopup<TControl>(this TControl thisControl, String message) where TControl : Control {
				thisControl.HelpRequested += new HelpEventHandler((s, a) => { Help.ShowPopup(thisControl, message, a.MousePos); });
				return(thisControl);
			}

		#endregion

	}

}
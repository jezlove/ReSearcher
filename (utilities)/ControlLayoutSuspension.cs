/*
	C# "ControlLayoutSuspension.cs"
	by Jeremy Love <mailto:jez@jezlove.uk>
	copyright 2020
*/

using System;
using System.Windows.Forms;

namespace ReSearcher {

	public class ControlLayoutSuspension :
		IDisposable {

		private Control control;
		private Boolean perform;

		public ControlLayoutSuspension(Control control, Boolean perform = true) {
			this.control = control;
			this.perform = perform;
			control.SuspendLayout();
		}

		public void Dispose() {
			control.ResumeLayout(perform);
		}

	}

}
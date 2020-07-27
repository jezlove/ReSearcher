/*
	C# "ToolStripItemExtensions.cs"
	by Jeremy Love <mailto:jez@jezlove.uk>
	copyright 2020
*/

using System;
using System.Windows.Forms;

namespace ReSearcher {

	public static class ToolStripItemExtensions {

		public static ToolStripItem withAction(this ToolStripItem thisToolStripItem, Action action) {
			thisToolStripItem.Click += new EventHandler((sender, eventArgs) => { action(); });
			return(thisToolStripItem);
		}

	}

}
/*
	C# "ToolStripExtensions.cs"
	by Jeremy Love <mailto:jez@jezlove.uk>
	copyright 2020
*/

using System;
using System.Windows.Forms;

namespace ReSearcher {

	public static class ToolStripExtensions {

		public static void appendItems(this ToolStrip thisToolStrip, params ToolStripItem[] toolStripItems) {
			thisToolStrip.Items.AddRange(toolStripItems);
		}

		public static TToolStrip withItems<TToolStrip>(this TToolStrip thisToolStrip, params ToolStripItem[] toolStripItems) where TToolStrip : ToolStrip {
			thisToolStrip.appendItems(toolStripItems);
			return(thisToolStrip);
		}

	}

}
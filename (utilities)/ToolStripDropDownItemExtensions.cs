/*
	C# "ToolStripDropDownItemExtensions.cs"
	by Jeremy Love <mailto:jez@jezlove.uk>
	copyright 2020
*/

using System;
using System.Windows.Forms;

namespace ReSearcher {

	public static class ToolStripDropDownItemExtensions {

		public static void appendDropDownItems(this ToolStripDropDownItem thisToolStripDropDownItem, params ToolStripItem[] toolStripItems) {
			thisToolStripDropDownItem.DropDownItems.AddRange(toolStripItems);
		}

		public static TToolStripDropDownItem withDropDownItems<TToolStripDropDownItem>(this TToolStripDropDownItem thisToolStripDropDownItem, params ToolStripItem[] toolStripItems) where TToolStripDropDownItem : ToolStripDropDownItem {
			thisToolStripDropDownItem.appendDropDownItems(toolStripItems);
			return(thisToolStripDropDownItem);
		}

	}

}
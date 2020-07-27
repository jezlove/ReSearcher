/*
	C# "ListViewItemExtensions.cs"
	by Jeremy Love <mailto:jez@jezlove.uk>
	copyright 2020
*/

using System;
using System.Windows.Forms;

namespace ReSearcher {

	public static class ListViewItemExtensions {

		#region sub-items

			public static void appendSubItems(this ListViewItem thisListViewItem, params ListViewItem.ListViewSubItem[] listViewSubItems) {
				thisListViewItem.SubItems.AddRange(listViewSubItems);
			}

			public static TListViewItem withSubItems<TListViewItem>(this TListViewItem thisListViewItem, params ListViewItem.ListViewSubItem[] listViewSubItems) where TListViewItem : ListViewItem {
				thisListViewItem.appendSubItems(listViewSubItems);
				return(thisListViewItem);
			}

		#endregion

	}

}
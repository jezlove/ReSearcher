/*
	C# "ListViewExtensions.cs"
	by Jeremy Love <mailto:jez@jezlove.uk>
	copyright 2020
*/

using System;
using System.Windows.Forms;

namespace ReSearcher {

	public static class ListViewExtensions {

		#region columns

			public static void appendColumns(this ListView thisListView, params ColumnHeader[] columnHeaders) {
				using(new ListViewUpdate(thisListView)) {
					thisListView.Columns.AddRange(columnHeaders);
				}
			}

			public static TListView withColumns<TListView>(this TListView thisListView, params ColumnHeader[] columnHeaders) where TListView : ListView {
				thisListView.appendColumns(columnHeaders);
				return(thisListView);
			}

		#endregion

		#region checking-and-unchecking

			public static void setAllItemsCheckedTo(this ListView thisListView, Boolean boolean) {
				using(new ListViewUpdate(thisListView)) {
					foreach(ListViewItem listViewItem in thisListView.Items) {
						listViewItem.Checked = boolean;
					}
				}
			}

			public static void checkAll(this ListView thisListView) {
				thisListView.setAllItemsCheckedTo(true);
			}

			public static void uncheckAll(this ListView thisListView) {
				thisListView.setAllItemsCheckedTo(false);
			}

		#endregion

	}

}
/*
	C# "TabularListView.cs"
	by Jeremy Love <mailto:jez@jezlove.uk>
	copyright 2020
*/

using System;
using System.Windows.Forms;

namespace ReSearcher {

    public class TabularListView :
		ListView {

		private TabularListViewColumnSorter tabularListViewColumnSorter;

		public TabularListView(TabularListViewColumnSorter tabularListViewColumnSorter) {
			this.tabularListViewColumnSorter = tabularListViewColumnSorter;
			View = View.Details;
			AllowColumnReorder = true;
			FullRowSelect = true;
			GridLines = true;
			ListViewItemSorter = tabularListViewColumnSorter;
		}

		protected override void OnColumnClick(ColumnClickEventArgs columnClickEventArgs) {
			if(null == tabularListViewColumnSorter || tabularListViewColumnSorter != ListViewItemSorter) {
				return;
			}
			if(columnClickEventArgs.Column == tabularListViewColumnSorter.sortColumn) {
				tabularListViewColumnSorter.sortOrder = (SortOrder.Ascending == tabularListViewColumnSorter.sortOrder) ? SortOrder.Descending : SortOrder.Ascending;
			}
			else {
				tabularListViewColumnSorter.sortColumn = columnClickEventArgs.Column;
				tabularListViewColumnSorter.sortOrder = SortOrder.Ascending;
			}
			Sort();
			base.OnColumnClick(columnClickEventArgs);
		}

	}

}
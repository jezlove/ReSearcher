/*
	C# "SimpleTabularListViewColumnSorter.cs"
	by Jeremy Love <mailto:jez@jezlove.uk>
	copyright 2020
*/

using System;
using System.Collections;
using System.IO;
using System.Windows.Forms;

namespace ReSearcher {

	public class SimpleTabularListViewColumnSorter :
		TabularListViewColumnSorter {

		private static readonly CaseInsensitiveComparer caseInsensitiveComparer = new CaseInsensitiveComparer();

		public SimpleTabularListViewColumnSorter(SortOrder sortOrder = default(SortOrder)) :
			base(sortOrder) {
		}

		public override int Compare(ListViewItem listViewItemA, ListViewItem listViewItemB) {
			int comparison = doCompare(listViewItemA, listViewItemB);
			if(SortOrder.Ascending == sortOrder) {
				return(comparison);
			}
			if(SortOrder.Descending == sortOrder) {
				return(-comparison);
			}
			return(0);
		}

		protected int doCompare(ListViewItem listViewItemA, ListViewItem listViewItemB) {
			return(caseInsensitiveComparer.Compare(listViewItemA.SubItems[sortColumn].Text, listViewItemB.SubItems[sortColumn].Text));
		}

	}
	
}
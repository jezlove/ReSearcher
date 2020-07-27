/*
	C# "TabularListViewColumnSorter.cs"
	by Jeremy Love <mailto:jez@jezlove.uk>
	copyright 2020
*/

using System;
using System.Collections;
using System.IO;
using System.Windows.Forms;

namespace ReSearcher {

	public abstract class TabularListViewColumnSorter :
		IComparer {

		public int sortColumn { get; set; }
		public SortOrder sortOrder { get; set; }

		public TabularListViewColumnSorter(SortOrder sortOrder = default(SortOrder)) {
			this.sortOrder = sortOrder;
		}

		public virtual int Compare(Object objectA, Object objectB) {
			ListViewItem listViewItemA = objectA as ListViewItem;
			ListViewItem listViewItemB = objectB as ListViewItem;
			if(null == listViewItemA || null == listViewItemB) {
				return(0);
			}
			return(Compare(listViewItemA, listViewItemB));
		}

		public abstract int Compare(ListViewItem listViewItemA, ListViewItem listViewItemB);

	}
	
}
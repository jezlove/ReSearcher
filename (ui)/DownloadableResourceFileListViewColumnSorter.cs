/*
	C# "DownloadableResourceFileListViewColumnSorter.cs"
	by Jeremy Love <mailto:jez@jezlove.uk>
	copyright 2020
*/

using System;
using System.Collections;
using System.IO;
using System.Windows.Forms;

namespace ReSearcher {

	public class DownloadableResourceFileListViewColumnSorter :
		TabularListViewColumnSorter {

		private static readonly CaseInsensitiveComparer caseInsensitiveComparer = new CaseInsensitiveComparer();

		public DownloadableResourceFileListViewColumnSorter(SortOrder sortOrder = default(SortOrder)) :
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

			// sorting by checked
			if(0 == sortColumn) {
				return(listViewItemA.Checked.CompareTo(listViewItemB.Checked));
			}

			// sorting by name, type, or location
			if(1 == sortColumn || 2 == sortColumn || 4 == sortColumn) {
				return(caseInsensitiveComparer.Compare(listViewItemA.SubItems[sortColumn].Text, listViewItemB.SubItems[sortColumn].Text));
			}

			// sorting by size
			if(3 == sortColumn) {
				IDownloadableResourceFile downloadableResourceFileA = listViewItemA.Tag as IDownloadableResourceFile;
				IDownloadableResourceFile downloadableResourceFileB = listViewItemB.Tag as IDownloadableResourceFile;
				long fileSizeA = null == downloadableResourceFileA ? 0 : downloadableResourceFileA.size;
				long fileSizeB = null == downloadableResourceFileB ? 0 : downloadableResourceFileB.size;
				return(fileSizeA.CompareTo(fileSizeB));
			}

			return(0);
		}

	}
	
}
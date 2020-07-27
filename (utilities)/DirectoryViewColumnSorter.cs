/*
	C# "DirectoryViewColumnSorter.cs"
	by Jeremy Love <mailto:jez@jezlove.uk>
	copyright 2020
*/

using System;
using System.Collections;
using System.IO;
using System.Windows.Forms;

namespace ReSearcher {

	public class DirectoryViewColumnSorter :
		TabularListViewColumnSorter {

		private static readonly CaseInsensitiveComparer caseInsensitiveComparer = new CaseInsensitiveComparer();

		public DirectoryViewColumnSorter(SortOrder sortOrder = default(SortOrder)) :
			base(sortOrder) {
		}

		public override int Compare(ListViewItem listViewItemA, ListViewItem listViewItemB) {
			if("." == listViewItemA.Text || ".." == listViewItemA.Text) {
				return(0);
			}
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

			// sorting by name or type
			if(0 == sortColumn || 1 == sortColumn) {
				return(caseInsensitiveComparer.Compare(listViewItemA.SubItems[sortColumn].Text, listViewItemB.SubItems[sortColumn].Text));
			}

			// sorting by size
			if(2 == sortColumn) {
				FileInfo fileInfoA = listViewItemA.Tag as FileInfo;
				FileInfo fileInfoB = listViewItemB.Tag as FileInfo;
				long fileSizeA = null == fileInfoA ? 0 : fileInfoA.Length;
				long fileSizeB = null == fileInfoB ? 0 : fileInfoB.Length;
				return(fileSizeA.CompareTo(fileSizeB));
			}

			// sorting by date
			if(3 == sortColumn) {
				FileSystemInfo fileSystemInfoA = listViewItemA.Tag as FileSystemInfo;
				FileSystemInfo fileSystemInfoB = listViewItemB.Tag as FileSystemInfo;
				return(fileSystemInfoA.LastWriteTime.CompareTo(fileSystemInfoB.LastWriteTime));
			}

			return(0);
		}

	}
	
}
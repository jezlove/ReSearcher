/*
	C# "DownloadableResourceFileListView.cs"
	by Jeremy Love <mailto:jez@jezlove.uk>
	copyright 2020
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace ReSearcher {

    public class DownloadableResourceFileListView :
		TabularListView {

		public DownloadableResourceFileListView() :
			base(new DownloadableResourceFileListViewColumnSorter()) {
			CheckBoxes = true;
			this.appendColumns(
				new ColumnHeader() { Text = String.Empty, Width = -1 },
				new ColumnHeader() { Text = "Name", Width = -1 },
				new ColumnHeader() { Text = "Type", Width = -1 },
				new ColumnHeader() { Text = "Size", Width = -1, TextAlign = HorizontalAlignment.Right },
				new ColumnHeader() { Text = "Location", Width = -1 }
			);
		}

		public void display(IEnumerable<KeyValuePair<String, List<IDownloadableResourceFile>>> ungroupedDownloadMappings) {
			display(ungroupedDownloadMappings.regrouped());
		}

		public void display(IEnumerable<Tuple<String, IEnumerable<KeyValuePair<String, List<IDownloadableResourceFile>>>>> downloadMappings) {
			using(new ListViewUpdate(this)) {
				Groups.Clear();
				Items.Clear();
				foreach(Tuple<String, IEnumerable<KeyValuePair<String, List<IDownloadableResourceFile>>>> grouping in downloadMappings) {
					ListViewGroup listViewGroup = new ListViewGroup(grouping.Item1, HorizontalAlignment.Left) { Tag = grouping };
					Groups.Add(listViewGroup);
					foreach(KeyValuePair<String, List<IDownloadableResourceFile>> kvp in grouping.Item2) {

						// TODO: allow user to select what to do in case of conflict

						IDownloadableResourceFile downloadableResourceFile = kvp.Value[0];

						Items.Add(
							new ListViewItem() { Tag = kvp, Checked = true, Group = listViewGroup }.withSubItems(
								new ListViewItem.ListViewSubItem() { Text = downloadableResourceFile.name },
								new ListViewItem.ListViewSubItem() { Text = downloadableResourceFile.type },
								new ListViewItem.ListViewSubItem() { Text = IecByteMultiples.format(downloadableResourceFile.size) },
								new ListViewItem.ListViewSubItem() { Text = downloadableResourceFile.location }
							)
						);
					}
				}
				AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
			}
		}

		public IList<Tuple<IDownloadableResourceFile, String>> listChecked() {
			return(
				CheckedItems
					.Cast<ListViewItem>()
					.Select(
						(i) => {
							KeyValuePair<String, List<IDownloadableResourceFile>> kvp = (KeyValuePair<String, List<IDownloadableResourceFile>>)(i.Tag);
							return(new Tuple<IDownloadableResourceFile, String>(kvp.Value[0], kvp.Key));
						}
					)
					.ToList()
			);
		}
		
		public long sumChecked() {
			return(
				CheckedItems
					.Cast<ListViewItem>()
					.Select(i => ((KeyValuePair<String, List<IDownloadableResourceFile>>)(i.Tag)).Value[0].size)
					.Sum()
			);
		}

	}

}
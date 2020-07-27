/*
	C# "PriorSearchDetailsRetriever.cs"
	by Jeremy Love <mailto:jez@jezlove.uk>
	copyright 2020
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using System.Windows.Forms;

namespace ReSearcher {

    public sealed class PriorSearchDetailsRetriever :
		Form {

		private readonly TabularListView listView;

		private PriorSearchDetailsRetriever() {
			Text = "Search history";
			FormBorderStyle = FormBorderStyle.SizableToolWindow;
			ShowInTaskbar = false;
			Size = new Size(700, 600);
			StartPosition = FormStartPosition.CenterParent;
			Padding = new Padding(8);
			this.appendControls(
				listView = new TabularListView(new SimpleTabularListViewColumnSorter()) { Dock = DockStyle.Fill, MultiSelect = false },
				new Panel() { Dock = DockStyle.Bottom, Height = 30, Padding = new Padding(0, 4, 0, 0) }.withControls(
					new Button() { Text = "Okay", DialogResult = DialogResult.OK, Dock = DockStyle.Right },
					new Button() { Text = "Cancel", DialogResult = DialogResult.Cancel, Dock = DockStyle.Right }
				)
			);

			// TODO: combine: type, pattern and options into one column, for example: /.../gix

			listView.appendColumns(
				new ColumnHeader() { Text = "Type", Width = -1 },
				new ColumnHeader() { Text = "Pattern", Width = -1 },
				new ColumnHeader() { Text = "Options", Width = -2 },
				new ColumnHeader() { Text = "XPath", Width = -1 },
				new ColumnHeader() { Text = "Filter", Width = -2 },
				new ColumnHeader() { Text = "Paths", Width = -1 }
			);
		}

		public void populate() {
			using(new ListViewUpdate(listView)) {
				listView.Items.Clear();
				foreach(SearchDetails searchDetails in SearchDetailsRepository.enumerate()) {
					listView.Items.Add(
						new ListViewItem() { Text = searchDetails.searchType.ToString(), Tag = searchDetails }.withSubItems(
							new ListViewItem.ListViewSubItem() { Text = searchDetails.pattern },
							new ListViewItem.ListViewSubItem() { Text = searchDetails.regexOptions.ToString() },
							new ListViewItem.ListViewSubItem() { Text = searchDetails.xpath },
							new ListViewItem.ListViewSubItem() { Text = searchDetails.filter },
							new ListViewItem.ListViewSubItem() { Text = String.Join(", ", searchDetails.paths) }
						)
					);
				}
				listView.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
			}
		}

		public static SearchDetails retrieve(IWin32Window ownerWindow) {
			using(PriorSearchDetailsRetriever priorSearchDetailsRetriever = new PriorSearchDetailsRetriever()) {
				priorSearchDetailsRetriever.populate();
				if(DialogResult.OK != priorSearchDetailsRetriever.ShowDialog(ownerWindow)) {
					return(null);
				}
				return(
					(0 == priorSearchDetailsRetriever.listView.SelectedItems.Count) ?
						null :
						(SearchDetails)(priorSearchDetailsRetriever.listView.SelectedItems[0].Tag)
				);
			}
		}

	}

}
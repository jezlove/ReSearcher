/*
	C# "DownloadPicker.cs"
	by Jeremy Love <mailto:jez@jezlove.uk>
	copyright 2020
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using System.Windows.Forms;

namespace ReSearcher {

    public sealed class DownloadPicker :
		Form {

		private DownloadableResourceFileListView downloadableResourceFileListView;
		private ToolStripStatusLabel numberOfFilesSelectedToolStripStatusLabel;
		private ToolStripStatusLabel totalSizeToolStripStatusLabel;

		private static readonly Font font = new Font("Arial", 8);

		private DownloadPicker(IEnumerable<KeyValuePair<String, List<IDownloadableResourceFile>>> downloadMappings) {

			Text = "Downloads...";
			StartPosition = FormStartPosition.CenterParent;
			Size = new Size(
				(int)(Screen.PrimaryScreen.Bounds.Width * 0.7f),
				(int)(Screen.PrimaryScreen.Bounds.Height * 0.7f)
			);

			// assemble UI components
			// extension methods are in use here to provide a fluent and elegant component hierarchy
			this.appendControls(
				downloadableResourceFileListView = new DownloadableResourceFileListView() { Dock = DockStyle.Fill, Padding = new Padding(4), Font = font },
				new Panel() { Dock = DockStyle.Bottom, Height = 30 }.withControls(

					new Button() { Text = "Uncheck all", Dock = DockStyle.Left, TabIndex = 4 }.withAction(uncheckAll),
					new Button() { Text = "Check all", Dock = DockStyle.Left, TabIndex = 3 }.withAction(checkAll),

					new Button() { Text = "Download...", DialogResult = DialogResult.OK, Dock = DockStyle.Right, TabIndex = 1 },
					new Button() { Text = "Cancel", DialogResult = DialogResult.Cancel, Dock = DockStyle.Right, TabIndex = 2 }

				),
				new StatusStrip() { Dock = DockStyle.Bottom }.withItems(
					numberOfFilesSelectedToolStripStatusLabel = new ToolStripStatusLabel(),
					new ToolStripSeparator(),
					totalSizeToolStripStatusLabel = new ToolStripStatusLabel()
				)
			);

			downloadableResourceFileListView.display(downloadMappings);
			downloadableResourceFileListView.ItemChecked += onItemChecked;
			updateStatusStrip();
		}

		private void onItemChecked(Object sender, ItemCheckedEventArgs itemCheckedEventArgs) {
			updateStatusStrip();
		}

		public void updateStatusStrip() {
			numberOfFilesSelectedToolStripStatusLabel.Text = String.Format("{0} file(s) selected", downloadableResourceFileListView.CheckedItems.Count);
			totalSizeToolStripStatusLabel.Text = String.Format("Size: {0}", IecByteMultiples.format(downloadableResourceFileListView.sumChecked()));
		}

		// pause event when checking/unchecking all items

		public void checkAll() {
			downloadableResourceFileListView.ItemChecked -= onItemChecked;
			{
				downloadableResourceFileListView.checkAll();
			}
			downloadableResourceFileListView.ItemChecked += onItemChecked;
			updateStatusStrip();
		}

		public void uncheckAll() {
			downloadableResourceFileListView.ItemChecked -= onItemChecked;
			{
				downloadableResourceFileListView.uncheckAll();
			}
			downloadableResourceFileListView.ItemChecked += onItemChecked;
			updateStatusStrip();
		}

		public static IList<Tuple<IDownloadableResourceFile, String>> pickDownloads(IEnumerable<KeyValuePair<String, List<IDownloadableResourceFile>>> downloadMappings, IWin32Window ownerWindow) {
			using(DownloadPicker downloadPicker = new DownloadPicker(downloadMappings)) {
				if(DialogResult.OK != downloadPicker.ShowDialog(ownerWindow)) {
					return(null);
				}
				return(downloadPicker.downloadableResourceFileListView.listChecked());
			}
		}

	}

}
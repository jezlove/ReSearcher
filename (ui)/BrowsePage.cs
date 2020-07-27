/*
	C# "BrowsePage.cs"
	by Jeremy Love <mailto:jez@jezlove.uk>
	copyright 2020
*/

using System;
using System.Collections.Generic;
using System.IO;
using System.Drawing;
using System.Windows.Forms;
using System.Diagnostics;

namespace ReSearcher {

    public class BrowsePage :
		TabPage {

		public DirectoryInfo directoryInfo { get; private set; }

		protected TextBox directoryPathTextBox { get; private set; }
		protected ComboBox filterComboBox { get; private set; }
		protected DirectoryView directoryView { get; private set; }

		private static readonly Font monospaceFont = new Font("Consolas", 9);

		public BrowsePage() {
			Padding = new Padding(8);
			this.appendControls(
				directoryView = new DirectoryView() { Dock = DockStyle.Fill, Margin = new Padding(4), Font = monospaceFont },
				new Panel() { Dock = DockStyle.Top, Margin = new Padding(4), Height = 30 }.withControls(
					new Panel() { Dock = DockStyle.Top, Margin = new Padding(4), Height = 30 }.withControls(
						directoryPathTextBox = new TextBox() { Dock = DockStyle.Fill, Font = monospaceFont },
						filterComboBox = new ComboBox() { Dock = DockStyle.Right, Width = 80, Font = monospaceFont }
					)
				)
			);
			directoryPathTextBox.KeyUp += onKeyReleased;
			filterComboBox.Items.AddRange(Filters.all);
			filterComboBox.KeyUp += onKeyReleased;
			directoryView.ItemActivate += onDirectoryViewItemActivated;
		}

		public BrowsePage(String directoryPath, String filter = Filters.any) :
			this() {
			browseTo(directoryPath, filter);
		}

		public BrowsePage(DirectoryInfo directoryInfo, String filter = Filters.any) :
			this() {
			browseTo(directoryInfo, filter);
		}

		public void browseTo(String directoryPath, String filter = Filters.any) {
			browseTo(new DirectoryInfo(directoryPath), filter);
		}

		public void browseTo(DirectoryInfo directoryInfo, String filter = Filters.any) {
			this.directoryInfo = directoryInfo;
			Text = directoryInfo.Name;
			directoryPathTextBox.Text = directoryInfo.FullName;
			filterComboBox.Text = filter;
			directoryView.display(directoryInfo, filter);
		}

		protected void onKeyReleased(Object sender, KeyEventArgs keyEventArgs) {
			if(Keys.Enter == keyEventArgs.KeyCode || Keys.Return == keyEventArgs.KeyCode) {
				String directoryPath = directoryPathTextBox.Text;
				if(String.IsNullOrWhiteSpace(directoryPath)) {
					directoryPath = directoryInfo.FullName;
				}
				String filter = filterComboBox.Text;
				if(String.IsNullOrWhiteSpace(filter)) {
					filter = Filters.any;
				}
				browseTo(directoryPath, filter);
			}
		}

		protected void onDirectoryViewItemActivated(Object sender, EventArgs eventArgs) {
			if(1 == directoryView.SelectedItems.Count) {
				FileSystemInfo fileSystemInfo = directoryView.SelectedItems[0].Tag as FileSystemInfo;
				DirectoryInfo directoryInfo = fileSystemInfo as DirectoryInfo;
				if(null != directoryInfo) {
					browseTo(directoryInfo);
					return;
				}
				FileInfo fileInfo = fileSystemInfo as FileInfo;
				if(null != fileInfo) {
					Process.Start(fileInfo.FullName);
				}
			}
		}

		public IEnumerable<FileSystemInfo> enumerateSelectedFileSystemInfos() {
			foreach(ListViewItem listViewItem in directoryView.SelectedItems) {
				yield return(listViewItem.Tag as FileSystemInfo);
			}
		}

		public String selectedFilter {
			get {
				return(filterComboBox.Text);
			}
			set {
				filterComboBox.Text = value;
			}
		}

	}

}
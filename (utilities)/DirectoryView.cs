/*
	C# "DirectoryView.cs"
	by Jeremy Love <mailto:jez@jezlove.uk>
	copyright 2020
*/

using System;
using System.Collections.Generic;
using System.IO;
using System.Drawing;
using System.Windows.Forms;

namespace ReSearcher {

    public class DirectoryView :
		TabularListView {

		/*

			note: for simplicity display:
				- file sizes in IEC units e.g. KiB (1024 bytes)
				- dates in ISO 8601

		*/

		private static readonly Dictionary<String, String> typeNameCache = new Dictionary<String, String>();
		private static readonly ImageList smallImageCache = new ImageList();

		public DirectoryView() :
			base(new DirectoryViewColumnSorter()) {
			SmallImageList = smallImageCache;
			this.appendColumns(
				new ColumnHeader() { Text = "Name", Width = -1 },
				new ColumnHeader() { Text = "Type", Width = -1 },
				new ColumnHeader() { Text = "Size", Width = -1, TextAlign = HorizontalAlignment.Right },
				new ColumnHeader() { Text = "Date", Width = -1, TextAlign = HorizontalAlignment.Center }
			);
		}

		public DirectoryView(String directoryPath, String filter = Filters.any) :
			this() {
			display(directoryPath, filter);
		}

		public DirectoryView(DirectoryInfo directoryInfo, String filter = Filters.any) :
			this() {
			display(directoryInfo, filter);
		}

		public void display(String directoryPath, String filter = Filters.any) {
			display(new DirectoryInfo(directoryPath), filter);
		}

		public void display(DirectoryInfo directoryInfo, String filter = Filters.any) {
			using(new ListViewUpdate(this)) {
				if(!smallImageCache.Images.ContainsKey(directoryImageKey)) {
					smallImageCache.Images.Add(directoryImageKey, IconUtilities.loadSmallIconAssociatedWithDirectory(directoryInfo.FullName));
				}
				Items.Clear();
				Items.Add(currentDirectory(directoryInfo));
				DirectoryInfo parentDirectoryInfo = directoryInfo.Parent;
				if(null != parentDirectoryInfo) {
					Items.Add(parentDirectory(parentDirectoryInfo));
				}
				if(directoryInfo.Exists) {
					foreach(FileSystemInfo fileSystemInfo in directoryInfo.EnumerateFileSystemInfos(filter)) {
						if(fileSystemInfo.Attributes.HasFlag(FileAttributes.Hidden)) {
							continue;
						}
						FileInfo fileInfo = fileSystemInfo as FileInfo;
						if(null != fileInfo) {
							Items.Add(file(fileInfo));
						}
						else {
							Items.Add(subDirectory((DirectoryInfo)(fileSystemInfo)));
						}
					}
				}
				AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
			}
		}

		protected static ListViewItem directory(String displayText, DirectoryInfo directoryInfo) {
			return(
				new ListViewItem() { Text = displayText, ImageKey = directoryImageKey, Tag = directoryInfo }.withSubItems(
					new ListViewItem.ListViewSubItem(),
					new ListViewItem.ListViewSubItem(),
					new ListViewItem.ListViewSubItem() { Text = directoryInfo.LastWriteTime.ToString(iso8601FormatString) }
				)
			);
		}

		protected static ListViewItem currentDirectory(DirectoryInfo directoryInfo) {
			return(directory(".", directoryInfo));
		}

		protected static ListViewItem parentDirectory(DirectoryInfo parentDirectoryInfo) {
			return(directory("..", parentDirectoryInfo));
		}

		protected static ListViewItem subDirectory(DirectoryInfo subDirectoryInfo) {
			return(directory(subDirectoryInfo.Name, subDirectoryInfo));
		}

		protected static ListViewItem file(FileInfo fileInfo) {
			if(!smallImageCache.Images.ContainsKey(fileInfo.Extension)) {
				Tuple<String, Icon> tuple = IconUtilities.getFileTypeAndIcon(fileInfo.FullName, true);
				typeNameCache.Add(fileInfo.Extension, tuple.Item1);
				smallImageCache.Images.Add(fileInfo.Extension, tuple.Item2);
			}
			return(
				new ListViewItem() { Text = fileInfo.Name, ImageKey = fileInfo.Extension, Tag = fileInfo }.withSubItems(
					new ListViewItem.ListViewSubItem() { Text = typeNameCache[fileInfo.Extension] },
					new ListViewItem.ListViewSubItem() { Text = IecByteMultiples.format(fileInfo.Length) },
					new ListViewItem.ListViewSubItem() { Text = fileInfo.LastWriteTime.ToString(iso8601FormatString) }
				)
			);
		}

		protected const String directoryImageKey = "***"; // remember: wildcards not valid in a path

		private const String iso8601FormatString = "yyyy-MM-dd";

	}

}
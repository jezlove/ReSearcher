/*
	C# "IconUtilities.cs"
	by Jeremy Love <mailto:jez@jezlove.uk>
	copyright 2020
*/

using System;
using System.Runtime.InteropServices;
using System.Drawing;

namespace ReSearcher {

	public static class IconUtilities {

		#region system-declarations

			[DllImport("Kernel32.dll")]
			internal static extern IntPtr GetModuleHandle(String moduleName);

			internal const int FILE_ATTRIBUTE_NORMAL    = 0x00000080;
			internal const int FILE_ATTRIBUTE_DIRECTORY = 0x00000010;

			[DllImport("User32.dll")]
			internal static extern IntPtr LoadIcon(IntPtr hInstance, IntPtr iconName);

			[DllImport("User32.dll")]
			internal static extern Boolean DestroyIcon(IntPtr hIcon);

			[StructLayout(LayoutKind.Sequential)]
			internal struct SHFILEINFO {
				public IntPtr hIcon;
				public Int32 iIcon;
				public UInt32 dwAttributes;
				[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
				public String szDisplayName;
				[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 80)]
				public String szTypeName;
			};

			internal const uint SHGFI_LARGEICON         = 0x000000000;
			internal const uint SHGFI_SMALLICON         = 0x000000001;
			internal const uint SHGFI_USEFILEATTRIBUTES = 0x000000010;
			internal const uint SHGFI_ICON              = 0x000000100;
			internal const uint SHGFI_TYPENAME          = 0x000000400;

			[DllImport("Shell32.dll")]
			internal static extern IntPtr SHGetFileInfo(String pszPath, uint dwFileAttributes, ref SHFILEINFO psfi, uint cbFileInfo, uint uFlags);

		#endregion

		/*
			note: System.Drawing.Icon.FromHandle will not take care of calling DestroyIcon, hence the need for:
		*/

		public static Icon createManagedIcon(IntPtr hIcon) {
			using(Icon icon = Icon.FromHandle(hIcon)) {
				Icon clone = (Icon)(icon.Clone());
				DestroyIcon(hIcon);
				return(clone);
			}
		}

		public static Icon loadCurrentModuleIcon() {
			IntPtr hInstance = GetModuleHandle(null);
			IntPtr hIcon = LoadIcon(hInstance, new IntPtr(32512));
			return(IntPtr.Zero == hIcon ? null : createManagedIcon(hIcon));
		}

		/*

			note: System.Drawing.Icon.ExtractAssociatedIcon will not extract small icon frames and will not work for directories, hence the need for:

		*/

		public static Icon loadSmallIconAssociatedWithFile(String filePath) {
			SHFILEINFO shFileInfo = new SHFILEINFO();
			SHGetFileInfo(filePath, FILE_ATTRIBUTE_NORMAL, ref shFileInfo, (uint)(Marshal.SizeOf(shFileInfo)), SHGFI_ICON | SHGFI_SMALLICON | SHGFI_USEFILEATTRIBUTES);
			return(IntPtr.Zero == shFileInfo.hIcon ? null : createManagedIcon(shFileInfo.hIcon));
		}

		public static Icon loadLargeIconAssociatedWithFile(String filePath) {
			SHFILEINFO shFileInfo = new SHFILEINFO();
			SHGetFileInfo(filePath, FILE_ATTRIBUTE_NORMAL, ref shFileInfo, (uint)(Marshal.SizeOf(shFileInfo)), SHGFI_ICON | SHGFI_LARGEICON | SHGFI_USEFILEATTRIBUTES);
			return(IntPtr.Zero == shFileInfo.hIcon ? null : createManagedIcon(shFileInfo.hIcon));
		}

		/*

			note: for our purposes there is no need to see if the user has selected a custom icon for their directories (specified in a desktop.ini file),
			just use the SHGFI_USEFILEATTRIBUTES

			note also: the only caller of this function ReSearcher.DirectoryView.display caches directory images under a single image key

			continued reading: https://devblogs.microsoft.com/oldnewthing/20170501-00/?p=96075

		*/

		public static Icon loadSmallIconAssociatedWithDirectory(String directoryPath) {
			SHFILEINFO shFileInfo = new SHFILEINFO();
			SHGetFileInfo(directoryPath, FILE_ATTRIBUTE_DIRECTORY, ref shFileInfo, (uint)(Marshal.SizeOf(shFileInfo)), SHGFI_ICON | SHGFI_SMALLICON | SHGFI_USEFILEATTRIBUTES);
			return(IntPtr.Zero == shFileInfo.hIcon ? null : createManagedIcon(shFileInfo.hIcon));
		}

		public static Icon loadLargeIconAssociatedWithDirectory(String directoryPath) {
			SHFILEINFO shFileInfo = new SHFILEINFO();
			SHGetFileInfo(directoryPath, FILE_ATTRIBUTE_DIRECTORY, ref shFileInfo, (uint)(Marshal.SizeOf(shFileInfo)), SHGFI_ICON | SHGFI_LARGEICON | SHGFI_USEFILEATTRIBUTES);
			return(IntPtr.Zero == shFileInfo.hIcon ? null : createManagedIcon(shFileInfo.hIcon));
		}

		public static Tuple<String, Icon> getFileTypeAndIcon(String filePath, Boolean small) {
			SHFILEINFO shFileInfo = new SHFILEINFO();
			SHGetFileInfo(filePath, FILE_ATTRIBUTE_NORMAL, ref shFileInfo, (uint)(Marshal.SizeOf(shFileInfo)), SHGFI_TYPENAME | SHGFI_ICON | (small ? SHGFI_SMALLICON : SHGFI_LARGEICON) | SHGFI_USEFILEATTRIBUTES);
			return(new Tuple<String, Icon>(shFileInfo.szTypeName, IntPtr.Zero == shFileInfo.hIcon ? null : createManagedIcon(shFileInfo.hIcon)));
		}

	}

}
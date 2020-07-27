/*
	C# "MessageBoxes.cs"
	by Jeremy Love <mailto:jez@jezlove.uk>
	copyright 2020
*/

using System;
using System.Windows.Forms;

namespace ReSearcher {

	public static class MessageBoxes {

		#region standardised-message-boxes

			public static void inform(this IWin32Window thisIWin32Window, String message, String title = "Information") {
				MessageBox.Show(thisIWin32Window, message, title, MessageBoxButtons.OK, MessageBoxIcon.Information);
			}

			public static void warn(this IWin32Window thisIWin32Window, String message, String title = "Warning") {
				MessageBox.Show(thisIWin32Window, message, title, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			}

			public static void error(this IWin32Window thisIWin32Window, String message, String title = "Error") {
				MessageBox.Show(thisIWin32Window, message, title, MessageBoxButtons.OK, MessageBoxIcon.Error);
			}

			public static void inform(String message, String title = "Information") {
				MessageBox.Show(message, title, MessageBoxButtons.OK, MessageBoxIcon.Information);
			}

			public static void warn(String message, String title = "Warning") {
				MessageBox.Show(message, title, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			}

			public static void error(String message, String title = "Error") {
				MessageBox.Show(message, title, MessageBoxButtons.OK, MessageBoxIcon.Error);
			}

		#endregion

	}

}
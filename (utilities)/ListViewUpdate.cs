/*
	C# "ListViewUpdate.cs"
	by Jeremy Love <mailto:jez@jezlove.uk>
	copyright 2020
*/

using System;
using System.Windows.Forms;

namespace ReSearcher {

	public class ListViewUpdate :
		IDisposable {

		private ListView listView;

		public ListViewUpdate(ListView listView) {
			this.listView = listView;
			listView.BeginUpdate();
		}

		public void Dispose() {
			listView.EndUpdate();
		}

	}

}
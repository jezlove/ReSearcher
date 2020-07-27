/*
	C# "DebugIndentation.cs"
	by Jeremy Love <mailto:jez@jezlove.uk>
	copyright 2020
*/

using System;
using System.Diagnostics;

namespace ReSearcher {

	public class DebugIndentation :
		IDisposable {

		public DebugIndentation() {
			Debug.Indent();
		}

		public void Dispose() {
			Debug.Unindent();
		}

	}

}
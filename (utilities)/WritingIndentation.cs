/*
	C# "WritingIndentation.cs"
	by Jeremy Love <mailto:jez@jezlove.uk>
	copyright 2020
*/

using System;
using System.Diagnostics;

namespace ReSearcher {

	public class WritingIndentation :
		IDisposable {

		private IndentedWriter indentedWriter;

		public WritingIndentation(IndentedWriter indentedWriter) {
			this.indentedWriter = indentedWriter;
			indentedWriter.indent++;
		}

		public void Dispose() {
			indentedWriter.indent--;
		}

	}

}
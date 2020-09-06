/*
	C# "IndentedWriter.cs"
	by Jeremy Love <mailto:jez@jezlove.uk>
	copyright 2020
*/

using System;
using System.IO;
using System.Text;

namespace ReSearcher {

    public sealed class IndentedWriter :
		TextWriter {

		private TextWriter textWriter;
		private Char lastCharacter = default(Char);

		public int indent { get; set; }
		public String indentString { get; set; }

		public IndentedWriter(TextWriter textWriter, String indentString = "\t", int indent = 0) {
			this.textWriter = textWriter;
			this.indent = indent;
			this.indentString = indentString;
		}

		public IndentedWriter(TextWriter textWriter, Char indentChar, int indent = 0) :
			this(textWriter, indentChar.ToString(), indent) {
		}

		public override Encoding Encoding {
			get {
				return(textWriter.Encoding);
			}
		}

		private void writeIndent() {
			for(int i = 0; i < indent; i++) {
				textWriter.Write(indentString);
			}
		}

		public sealed override void Write(Char character) {
			if('\n' == lastCharacter || ('\r' == lastCharacter && '\n' != character) || default(Char) == lastCharacter) {
				writeIndent();
			}
			textWriter.Write(character);
			lastCharacter = character;
		}

	}

}
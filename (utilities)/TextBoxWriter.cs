/*
	C# "TextBoxWriter.cs"
	by Jeremy Love <mailto:jez@jezlove.uk>
	copyright 2020
*/

using System;
using System.Text;
using System.IO;
using System.Windows.Forms;
 
namespace ReSearcher {

	public class TextBoxWriter :
		TextWriter {

    	protected readonly TextBox textBox;
 
    	public TextBoxWriter(TextBox textBox) {
        	this.textBox = textBox;
    	}
 
    	public override void Write(Char character) {
        	textBox.AppendText(character.ToString());
    	}
  
    	public override Encoding Encoding {
        	get { return(Encoding.UTF8); }
    	}

	}

}
/*
	C# "PdfLinearTextExtractionStrategy.cs"
	by Jeremy Love <mailto:jez@jezlove.uk>
	copyright 2020
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.parser;

namespace ReSearcher.Pdf {

	public class PdfLinearTextExtractionStrategy :
		ITextExtractionStrategy {

		public List<String> lines = new List<String>();
		private StringBuilder stringBuilder = new StringBuilder();
		private Vector lastBaseLine;

		public void RenderText(TextRenderInfo renderInfo) {
			Vector currentBaseline = renderInfo.GetBaseline().GetStartPoint();
			if((lastBaseLine != null) && (currentBaseline[Vector.I2] != lastBaseLine[Vector.I2])) {
				if((!String.IsNullOrWhiteSpace(stringBuilder.ToString()))) {
					lines.Add(stringBuilder.ToString());
				}
				stringBuilder.Clear();
			}
			stringBuilder.Append(renderInfo.GetText());
			lastBaseLine = currentBaseline;
		}

		public String GetResultantText() {
			if((!String.IsNullOrWhiteSpace(stringBuilder.ToString()))) {
				lines.Add(stringBuilder.ToString());
			}
			return(null);
		}

		public void BeginTextBlock() {}

		public void EndTextBlock() {}

		public void RenderImage(ImageRenderInfo imageRenderInfo) {}

	}

}
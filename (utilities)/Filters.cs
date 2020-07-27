/*
	C# "Filters.cs"
	by Jeremy Love <mailto:jez@jezlove.uk>
	copyright 2020
*/

using System;
using System.Collections.Generic;

namespace ReSearcher {

    public static class Filters {

		public const String any = "*.*";

		public const String anyEpub = "*.epub";

		public const String anyDocx = "*.docx";

		public const String anyPdf = "*.pdf";

		public static String[] all = new [] {any, anyEpub, anyDocx, anyPdf};

	}

}
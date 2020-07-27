/*
	C# "BatchItemProcessor.cs"
	by Jeremy Love <mailto:jez@jezlove.uk>
	copyright 2020
*/

using System;
using System.IO;

namespace ReSearcher {

	public delegate int BatchItemProcessor<TItem>(TItem item, TextWriter log);

}
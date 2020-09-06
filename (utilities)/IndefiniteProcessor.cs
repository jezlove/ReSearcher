/*
	C# "IndefiniteProcessor.cs"
	by Jeremy Love <mailto:jez@jezlove.uk>
	copyright 2020
*/

using System;
using System.IO;

namespace ReSearcher {

	public delegate Boolean IndefiniteProcessor(Func<Boolean> cancellationRequestedChecker, TextWriter log);

}
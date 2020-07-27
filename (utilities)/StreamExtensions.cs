/*
	C# "StreamExtensions.cs"
	by Jeremy Love <mailto:jez@jezlove.uk>
	copyright 2020
*/

using System;
using System.IO;

namespace ReSearcher {

	public static class StreamExtensions {

		public static String readToString(this Stream thisStream) {
			using(StreamReader streamReader = new StreamReader(thisStream)) {
				return(streamReader.ReadToEnd());
			}
		}

	}

}
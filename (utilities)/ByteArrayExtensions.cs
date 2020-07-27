/*
	C# "ByteArrayExtensions.cs"
	by Jeremy Love <mailto:jez@jezlove.uk>
	copyright 2020
*/

using System;
using System.Collections.Generic;
using System.Text;

namespace ReSearcher {

	public static partial class ByteArrayExtensions {

		#region converting-to-utf8

			public static String toUtf8String(this Byte[] thisByteArray) {
				return(Encoding.UTF8.GetString(thisByteArray));
			}

		#endregion

	}

}
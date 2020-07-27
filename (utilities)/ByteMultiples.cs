/*
	C# "ByteMultiples.cs"
	by Jeremy Love <mailto:jez@jezlove.uk>
	copyright 2020
*/

using System;
using System.Text.RegularExpressions;

namespace ReSearcher {

	using ByteCount = Int64;

    public static class IecByteMultiples {

		// kibibyte = 1024 ^ 1 bytes
		#region kibibyte

			public static Double inKib(ByteCount byteCount) {
				return(byteCount / 1024f);
			}

			public static ByteCount fromKib(Double kibCount) {
				return((ByteCount)(kibCount * 1024f));
			}

		#endregion

		// mebibyte = 1024 ^ 2 bytes
		#region mebibyte

			public static Double inMib(ByteCount byteCount) {
				return(byteCount / 1024f / 1024f);
			}

			public static ByteCount fromMib(Double mibCount) {
				return((ByteCount)(mibCount * 1024f * 1024f));
			}

		#endregion

		// gibibyte = 1024 ^ 3 bytes
		#region gibibyte

			public static Double inGib(ByteCount byteCount) {
				return(byteCount / 1024f / 1024f / 1024f);
			}

			public static ByteCount fromGib(Double gibCount) {
				return((ByteCount)(gibCount * 1024f * 1024f * 1024f));
			}

		#endregion

		#region formatting

			public static String format(ByteCount byteCount) {
				if(byteCount >= 1024 * 1024 * 1024) { return(String.Format("{0:n0} GiB", inGib(byteCount))); }
				if(byteCount >= 1024 * 1024) { return(String.Format("{0:n0} MiB", inMib(byteCount))); }
				if(byteCount >= 1024) { return(String.Format("{0:n0} KiB", inKib(byteCount))); }
				return(String.Format("{0:n0} B", byteCount));
			}

		#endregion

	}

	public static class SiByteMultiples {

		// kilobyte = 1000 ^ 1 bytes
		#region kilobyte

			public static Double inKb(ByteCount byteCount) {
				return(byteCount / 1000f);
			}

			public static ByteCount fromKb(Double kbCount) {
				return((ByteCount)(kbCount * 1000f));
			}

		#endregion

		// megabyte = 1000 ^ 2 bytes
		#region megabyte

			public static Double inMb(ByteCount byteCount) {
				return(byteCount / 1000f / 1000f);
			}

			public static ByteCount fromMb(Double mbCount) {
				return((ByteCount)(mbCount * 1000f * 1000f));
			}

		#endregion

		// gigabyte = 1000 ^ 3 bytes
		#region gigabyte

			public static Double inGb(ByteCount byteCount) {
				return(byteCount / 1000f / 1000f / 1000f);
			}

			public static ByteCount fromGb(Double gbCount) {
				return((ByteCount)(gbCount * 1000f * 1000f * 1000f));
			}

		#endregion

		#region parsing

			public static Boolean tryParse(String text, out long byteCount) {
				byteCount = 0;
				Match match = regex.Match(text);
				if(!match.Success) {
					return(false);
				}
				float n = Single.Parse(match.Groups[1].Value);
				String m = match.Groups[2].Value.ToLowerInvariant();
				if("kb" == m) {
					byteCount = fromKb(n);
					return(true);
				}
				if("mb" == m) {
					byteCount = fromMb(n);
					return(true);
				}
				/*("gb" == m)*/ {
					byteCount = fromGb(n);
					return(true);
				}
			}

			public static readonly Regex regex = new Regex(@"^\s*([\d\.]+)\s*([kmg]?b)\s*$", RegexOptions.Compiled | RegexOptions.IgnoreCase);

		#endregion

	}

}
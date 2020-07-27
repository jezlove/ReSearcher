/*
	C# "IEnumerableExtensions.cs"
	by Jeremy Love <mailto:jez@jezlove.uk>
	copyright 2020
*/

using System;
using System.Collections.Generic;
using System.Linq;

namespace ReSearcher {

	public static class IEnumerableExtensions {

		#region null-or-empty

			public static Boolean isNullOrEmpty<T>(this IEnumerable<T> thisIEnumerable) {
				return(null == thisIEnumerable || !thisIEnumerable.Any());
			}

		#endregion

	}

}
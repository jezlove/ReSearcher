/*
	C# "IExpressableIEnumerableExtensions.cs"
	by Jeremy Love <mailto:jez@jezlove.uk>
	copyright 2020
*/

using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace ReSearcher {

	public static class IExpressableIEnumerableExtensions {

		#region expressing

			public static IEnumerable<XNode> expressAll(this IEnumerable<IExpressable> thisIExpressableIEnumerable) {
				foreach(IExpressable iExpressable in thisIExpressableIEnumerable) {
					foreach(XNode xNode in iExpressable.express()) {
						yield return(xNode);
					}
				}
			}

		#endregion

	}

}
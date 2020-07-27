/*
	C# "NameValueCollectionExtensions.cs"
	by Jeremy Love <mailto:jez@jezlove.uk>
	copyright 2020
*/

using System;
using System.Collections.Generic;
using System.Collections.Specialized;

namespace ReSearcher {

	public static partial class NameValueCollectionExtensions {

		#region adding

			public static TNameValueCollection with<TNameValueCollection>(this TNameValueCollection thisNameValueCollection, String name, String value) where TNameValueCollection : NameValueCollection {
				thisNameValueCollection.Add(name, value);
				return(thisNameValueCollection);
			}

			public static TNameValueCollection with<TNameValueCollection>(this TNameValueCollection thisNameValueCollection, Tuple<String, String> nameValueTuple) where TNameValueCollection : NameValueCollection {
				thisNameValueCollection.Add(nameValueTuple.Item1, nameValueTuple.Item2);
				return(thisNameValueCollection);
			}

		#endregion

	}

}
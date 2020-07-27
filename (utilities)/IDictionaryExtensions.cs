/*
	C# "IDictionaryExtensions.cs"
	by Jeremy Love <mailto:jez@jezlove.uk>
	copyright 2020
*/

using System;
using System.Collections.Generic;
using System.Linq;

namespace ReSearcher {

	public static class IDictionaryExtensions {

		#region retrieving

			public static TValue getValueOrNull<TKey, TValue>(this IDictionary<TKey, TValue> thisDictionary, TKey key) where TValue : class {
				TValue value;
				return(thisDictionary.TryGetValue(key, out value) ? value : null);
			}

			public static TValue getValueOrNew<TValue, TKey>(this IDictionary<TKey, TValue> thisDictionary, TKey key) where TValue : class, new() {
				TValue value = thisDictionary.getValueOrNull(key);
				if(null == value) {
					value = new TValue();
					thisDictionary.Add(key, value);
				}
				return(value);
			}

		#endregion

	}

}
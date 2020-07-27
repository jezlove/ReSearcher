/*
	C# "IDownloadableResourceFileCollectionIEnumerableExtensions.cs"
	by Jeremy Love <mailto:jez@jezlove.uk>
	copyright 2020
*/

using System;
using System.Collections.Generic;

namespace ReSearcher {

	public static class IDownloadableResourceFileCollectionIEnumerableExtensions {

		#region flattening

			public static IEnumerable<IDownloadableResourceFile> flattened(this IEnumerable<IDownloadableResourceFileCollection> thisIDownloadableResourceFileCollectionIEnumerable) {
				foreach(IDownloadableResourceFileCollection downloadResourceFileCollection in thisIDownloadableResourceFileCollectionIEnumerable) {
					foreach(IDownloadableResourceFile downloadResourceFile in downloadResourceFileCollection.files) {
						yield return(downloadResourceFile);
					}
				}
			}

		#endregion

	}

}
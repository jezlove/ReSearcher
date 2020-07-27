/*
	C# "IDownloaderExtensions.cs"
	by Jeremy Love <mailto:jez@jezlove.uk>
	copyright 2020
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;

namespace ReSearcher {

	public static class IDownloaderExtensions {

		#region headless-downloading

			public static void downloadAll(this IDownloader thisIDownloader, IEnumerable<KeyValuePair<String, List<IDownloadableResourceFile>>> keyValuePairs, String targetDirectoryPath) {
				foreach(KeyValuePair<String, List<IDownloadableResourceFile>> keyValuePair in keyValuePairs) {
					FileInfo fileInfo = new FileInfo(Path.Combine(targetDirectoryPath, keyValuePair.Key));
					fileInfo.Directory.Create();
					thisIDownloader.download(keyValuePair.Value[0], fileInfo.FullName);
				}
			}

			public static void downloadAllNotDownloaded(this IDownloader thisIDownloader, IEnumerable<KeyValuePair<String, List<IDownloadableResourceFile>>> keyValuePairs, String targetDirectoryPath) {
				thisIDownloader.downloadAll(keyValuePairs.enumerateNotDownloaded(targetDirectoryPath), targetDirectoryPath);
			}

		#endregion

	}

}
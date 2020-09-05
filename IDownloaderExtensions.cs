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

			public static Boolean downloadAll(this IDownloader thisIDownloader, IEnumerable<KeyValuePair<String, List<IDownloadableResourceFile>>> keyValuePairs, String targetDirectoryPath) {
				foreach(KeyValuePair<String, List<IDownloadableResourceFile>> keyValuePair in keyValuePairs) {
					FileInfo fileInfo = new FileInfo(Path.Combine(targetDirectoryPath, keyValuePair.Key));
					fileInfo.Directory.Create();
					if(!thisIDownloader.download(keyValuePair.Value[0], fileInfo.FullName)) {
						Console.Error.WriteLine("Downloading of uri: {0} failed", keyValuePair.Value[0]);
						if(fileInfo.Exists) {
							Console.Error.WriteLine("Deleting partially downloaded file: {0}", fileInfo.FullName);
							fileInfo.Delete();
						}
						return(false);
					}
				}
				return(true);
			}

			public static Boolean downloadAllNotDownloaded(this IDownloader thisIDownloader, IEnumerable<KeyValuePair<String, List<IDownloadableResourceFile>>> keyValuePairs, String targetDirectoryPath) {
				return(thisIDownloader.downloadAll(keyValuePairs.enumerateNotDownloaded(targetDirectoryPath), targetDirectoryPath));
			}

		#endregion

	}

}
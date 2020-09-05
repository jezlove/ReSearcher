/*
	C# "MockDownloader.cs"
	by Jeremy Love <mailto:jez@jezlove.uk>
	copyright 2020
*/

using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Diagnostics;

namespace ReSearcher {

    internal class MockDownloader :
		IDownloader {

		private IEnumerable<IDownloadableResourceFileCollection> downloadableResourceFileCollections;

		public MockDownloader(IEnumerable<IDownloadableResourceFileCollection> downloadableResourceFileCollections) {
			this.downloadableResourceFileCollections = downloadableResourceFileCollections;
		}

		public virtual IEnumerable<IDownloadableResourceFileCollection> enumerateResourceFileCollections() {
			return(downloadableResourceFileCollections);
		}

		public virtual Boolean download(IDownloadableResourceFile downloadableResourceFile, String filePath) {
			Debug.WriteLine("Downloading: {0} to {1}", downloadableResourceFile.downloadUri, filePath);
			using(File.Create(filePath)) {}
			return(true);
		}

	}

}
/*
	C# "SleepyMockDownloader.cs"
	by Jeremy Love <mailto:jez@jezlove.uk>
	copyright 2020
*/

using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Diagnostics;

namespace ReSearcher {

    internal class SleepyMockDownloader :
		MockDownloader {

		private int enumerateDuration = 1000;
		private int downloadDuration = 200;

		public SleepyMockDownloader(IEnumerable<IDownloadableResourceFileCollection> downloadableResourceFileCollections) :
			base(downloadableResourceFileCollections) {
		}

		public override IEnumerable<IDownloadableResourceFileCollection> enumerateResourceFileCollections() {
			Thread.Sleep(enumerateDuration);
			return(base.enumerateResourceFileCollections());
		}

		public override Boolean download(IDownloadableResourceFile downloadableResourceFile, String filePath) {
			Boolean downloaded = base.download(downloadableResourceFile, filePath);
			Thread.Sleep(downloadDuration);
			return(downloaded);
		}

	}

}
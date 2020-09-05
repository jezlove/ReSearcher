/*
	C# "IDownloader.cs"
	by Jeremy Love <mailto:jez@jezlove.uk>
	copyright 2020
*/

using System;
using System.Collections.Generic;
using System.IO;

namespace ReSearcher {

    public interface IDownloader {

		IEnumerable<IDownloadableResourceFileCollection> enumerateResourceFileCollections();

		Boolean download(IDownloadableResourceFile downloadableResourceFile, String filePath);

	}

}
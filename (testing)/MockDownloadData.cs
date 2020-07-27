/*
	C# "MockDownloadData.cs"
	by Jeremy Love <mailto:jez@jezlove.uk>
	copyright 2020
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Diagnostics;
using ReSearcher;
using ReSearcher.Ou;

namespace ReSearcher {

	internal static class MockDownloadData {

		internal const String csvFilePath = "OuDownloadList.csv";

		public static void downloadFromOuWebsite() {
			IEnumerable<IDownloadableResourceFileCollection> downloadableResourceFileCollections = OuDownloader.enumerateResourceFileCollections(ProgramSettings.ouUsername, ProgramSettings.ouPassword);
			CsvDownloadLists.save(csvFilePath, downloadableResourceFileCollections);
		}

		public static IEnumerable<IDownloadableResourceFileCollection> load() {
			return(CsvDownloadLists.loadFile(csvFilePath));
		}

	}

}
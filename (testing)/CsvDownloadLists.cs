/*
	C# "CsvDownloadLists.cs"
	by Jeremy Love <mailto:jez@jezlove.uk>
	copyright 2020
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.IO;
using ReSearcher.Ou;

namespace ReSearcher {

	internal static class CsvDownloadLists {

		#region loading

			public static IEnumerable<DownloadableResourceFileCollection> loadFile(String csvFilePath) {
				IDictionary<String, List<IDownloadableResourceFile>> map = new Dictionary<String, List<IDownloadableResourceFile>>();
				String[] csvLines = File.ReadAllLines(csvFilePath);
				foreach(String csvLine in csvLines) {
					Match csvLineMatch = csvLineRegex.Match(csvLine);
					if(!csvLineMatch.Success) {
						Console.Error.WriteLine("Error: could not parse: {0}", csvLine);
						continue;
					}
					map.getValueOrNew(csvLineMatch.Groups[6].Value).Add(
						new DownloadableResourceFile(
							downloadUri: new Uri(csvLineMatch.Groups[1].Value),
							name: csvLineMatch.Groups[2].Value,
							type: csvLineMatch.Groups[3].Value,
							size: Int64.Parse(csvLineMatch.Groups[4].Value),
							location: csvLineMatch.Groups[5].Value
						)
					);
				}
				return(map.Select(e => new DownloadableResourceFileCollection(e.Key, e.Value)));
			}

			public static readonly Regex csvLineRegex = new Regex(@"^""([^""]*)"", ""([^""]*)"", ""([^""]*)"", ""([^""]*)"", ""([^""]*)"", ""([^""]*)""$", RegexOptions.Compiled);

		#endregion

		#region saving

			public static void save(String csvFilePath, IEnumerable<IDownloadableResourceFileCollection> downloadableResourceFileCollections) {
				using(StreamWriter streamWriter = new StreamWriter(csvFilePath)) {
					foreach(DownloadableResourceFileCollection downloadableResourceFileCollection in downloadableResourceFileCollections) {
						foreach(DownloadableResourceFile downloadableResourceFile in downloadableResourceFileCollection.files) {
							streamWriter.WriteLine(
								csvLineFormatString,
								downloadableResourceFile.downloadUri,
								downloadableResourceFile.name,
								downloadableResourceFile.type,
								downloadableResourceFile.size,
								downloadableResourceFile.location ?? "",
								downloadableResourceFileCollection.name
							);
						}
					}
				}
			}

			private const String csvLineFormatString = "\"{0}\", \"{1}\", \"{2}\", \"{3}\", \"{4}\", \"{5}\"";

		#endregion

	}

}
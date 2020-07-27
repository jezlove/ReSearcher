/*
	C# "IDownloadableResourceFileCollectionMapping.cs"
	by Jeremy Love <mailto:jez@jezlove.uk>
	copyright 2020
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;

namespace ReSearcher {

	public static class IDownloadableResourceFileCollectionMapping {

		#region mapping

			public static IDictionary<String, List<IDownloadableResourceFile>> map(this IEnumerable<IDownloadableResourceFileCollection> downloadableResourceFileCollections) {
				IDictionary<String, List<IDownloadableResourceFile>> map = new Dictionary<String, List<IDownloadableResourceFile>>();
				foreach(IDownloadableResourceFileCollection downloadableResourceFileCollection in downloadableResourceFileCollections) {
					foreach(IDownloadableResourceFile downloadableResourceFile in downloadableResourceFileCollection.files) {
						String fileName = Path.GetFileName(Uri.UnescapeDataString(downloadableResourceFile.downloadUri.AbsolutePath));
						String filePath = Path.Combine(
							sanitiseFileName(
								downloadableResourceFileCollection.name
							),
							sanitiseLocationName(
								prettifyName(
									String.IsNullOrWhiteSpace(downloadableResourceFile.location) ? String.Empty : downloadableResourceFile.location
								)
							),
							sanitiseFileName(
								prettifyName(
									fileName
								)
							)
						);
						map.getValueOrNew(filePath).Add(downloadableResourceFile);
					}
				}
				return(map);
			}

		#endregion

		#region prettifying

			// TODO: allow for custom prettification of names and file names also

			/*
				Possible prettification options:
					- whitespace
						- preserving
						- contracting
						- contracting and replacing
						- replacing
					- symbols ( & @ # ... )
						- preserving
						- substituting
					- etc.
			*/

			internal static String prettifyName(String name) {
				return(name.replaceAllWhitespace('_'));
			}

		#endregion

		#region sanitising

			// TODO: unsure about whether to allow this:

			/*
				TM470 actually seems to be the only module (for me) upsetting this:
					"TM470-20B\Basing your project on TM354/M363\basing_your_project_on_tm354_m363.epub"
			*/

			internal static Boolean allowHierarchicalLocations = false;

			private readonly static Char[] invalidFileNameCharacters;

			private readonly static Char[] invalidLocationNameCharacters;

			static IDownloadableResourceFileCollectionMapping() {
				invalidFileNameCharacters = Path.GetInvalidFileNameChars();
				invalidLocationNameCharacters = Path.GetInvalidPathChars().Concat(new[] {':', '*', '?'}).ToArray();
				// (allow for slashes in location name when allowHierarchicalLocations)
			}

			internal static Char invalidCharacterReplacementCharacter = '_';

			internal static String sanitiseFileName(String name) {
				return(name.replaceAll(invalidFileNameCharacters, invalidCharacterReplacementCharacter));
			}

			internal static String sanitiseLocationName(String name) {
				return(!allowHierarchicalLocations ? sanitiseFileName(name) : name.replaceAll(invalidLocationNameCharacters, invalidCharacterReplacementCharacter));
			}

		#endregion

		#region map-querying

			public static IEnumerable<KeyValuePair<String, List<IDownloadableResourceFile>>> enumerateCollisions(this IEnumerable<KeyValuePair<String, List<IDownloadableResourceFile>>> thisMap) {
				return(thisMap.Where(kvp => kvp.Value.Count > 1));
			}

			public static Boolean hasCollisions(this IEnumerable<KeyValuePair<String, List<IDownloadableResourceFile>>> thisMap) {
				return(thisMap.enumerateCollisions().Any());
			}

			public static IEnumerable<String> enumerateExtensions(this IEnumerable<KeyValuePair<String, List<IDownloadableResourceFile>>> thisMap) {
				return(thisMap.Select(kvp => Path.GetExtension(kvp.Key)));
			}

			public static IEnumerable<KeyValuePair<String, List<IDownloadableResourceFile>>> enumerateNotExtensioned(this IEnumerable<KeyValuePair<String, List<IDownloadableResourceFile>>> thisMap) {
				return(thisMap.Where(kvp => String.IsNullOrEmpty(Path.GetExtension(kvp.Key))));
			}

			public static IEnumerable<KeyValuePair<String, List<IDownloadableResourceFile>>> enumerateNotDownloaded(this IEnumerable<KeyValuePair<String, List<IDownloadableResourceFile>>> thisMap, String directoryPath) {
				return(thisMap.Where(kvp => !File.Exists(Path.Combine(directoryPath, kvp.Key))));
			}

		#endregion

		#region regrouping

			public static IEnumerable<Tuple<String, IEnumerable<KeyValuePair<String, List<IDownloadableResourceFile>>>>> regrouped(this IEnumerable<KeyValuePair<String, List<IDownloadableResourceFile>>> thisMap) {
				return(
					thisMap
						.OrderBy(kvp => kvp.Key)
						.GroupBy(kvp => getRootDirectoryName(kvp.Key))
						.Select(g => new Tuple<String, IEnumerable<KeyValuePair<String, List<IDownloadableResourceFile>>>>(g.Key, g))
				);
			}

			internal static String getRootDirectoryName(String path) {
				for(
					String parentDirectoryPath;
					!String.IsNullOrEmpty(parentDirectoryPath = Path.GetDirectoryName(path));
					path = parentDirectoryPath) {
				}
				return(path);
			}

		#endregion

	}

}
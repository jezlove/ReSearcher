/*
	C# "SearchDetailsRepository.cs"
	by Jeremy Love <mailto:jez@jezlove.uk>
	copyright 2020
*/

using System;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text.RegularExpressions;
using Newtonsoft.Json;

namespace ReSearcher {

    internal sealed class SearchDetailsRepository {

		private String listFilePath;
		private IList<SearchDetails> list;

		private SearchDetailsRepository() {

			listFilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "ReSearcher.json");

			list = tryLoadList(listFilePath);
		}

		~SearchDetailsRepository() {
			if(null != list) {
				trySaveList(list, listFilePath);
			}
		}

		private static readonly SearchDetailsRepository searchDetailsRepository = new SearchDetailsRepository();

		private static IList<SearchDetails> tryLoadList(String listFilePath) {
			try {
				if(!File.Exists(listFilePath)) {
					return(null);
				}
				return(JsonConvert.DeserializeObject<List<SearchDetails>>(File.ReadAllText(listFilePath)).Cast<SearchDetails>().ToList());
			}
			catch(Exception exception) {
				Console.Error.WriteLine("Error: could not load search repository due to exception: {0}", exception);
				return(null);
			}
		}

		private static void trySaveList(IList<SearchDetails> list, String listFilePath) {
			try {
				File.WriteAllText(listFilePath, JsonConvert.SerializeObject(list, Formatting.Indented));
			}
			catch(Exception exception) {
				Console.Error.WriteLine("Error: could not save search repository due to exception: {0}", exception);
			}
		}

		public static void commit(SearchDetails searchDetails) {
			if(null == searchDetailsRepository.list) {
				searchDetailsRepository.list = new List<SearchDetails>();
			}
			searchDetailsRepository.list.Add(searchDetails);
		}

		public static IEnumerable<SearchDetails> enumerate() {
			return((null == searchDetailsRepository.list) ? Enumerable.Empty<SearchDetails>() : searchDetailsRepository.list);
		}

	}

}
/*
	C# "SearchDetails.cs"
	by Jeremy Love <mailto:jez@jezlove.uk>
	copyright 2020
*/

using System;
using System.Text.RegularExpressions;
using Newtonsoft.Json;

namespace ReSearcher {

    public class SearchDetails {

		public String[] paths { get; set; }
		public String filter { get; set; }
		public String xpath { get; set; }

		[JsonIgnore]
		public SearchType searchType { get; set; }

		[JsonProperty("searchType")]
		public String searchTypeString {
			get { return(searchType.ToString()); }
			set {
				SearchType searchType;
				Enum.TryParse(value, out searchType);
				this.searchType = searchType;
			}
		}

		public String pattern { get; set; }

		public RegexOptions regexOptions { get; set; }

	}

}
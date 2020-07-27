/*
	C# "UnsophisticatedSearcher.cs"
	by Jeremy Love <mailto:jez@jezlove.uk>
	copyright 2020
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.IO.Compression;
using System.Text.RegularExpressions;

using ReSearcher.Epub;

namespace ReSearcher {

    public class UnsophisticatedSearcher :
		AbstractMultiFormatSearcher {

		public UnsophisticatedSearcher(Regex regex, String filter = Filters.any) :
			base(regex, filter) {
		}

		#region searching-epub-documents

			protected override EpubEntrySearchResults searchEpubEntry(String entryName, Stream stream) {
				String streamString = stream.readToString();
				MatchCollection matchCollection = regex.Matches(streamString);
				if(0 == matchCollection.Count) {
					return(null);
				}
				Match htmlTitleMatch = htmlTitleRegex.Match(streamString);
				String entryTitle = (htmlTitleMatch.Success) ? htmlTitleMatch.Groups[1].Value.Trim() : null;
				return(new EpubEntrySearchResults(entryName, entryTitle, new RegexSearchResultMatchCollection(matchCollection)));
			}

			private static readonly Regex htmlTitleRegex = new Regex(@"<title[^>]*>([^<]*)</title>", RegexOptions.Compiled);

		#endregion

	}

}
/*
	C# "XmlNodeListExtensions.cs"
	by Jeremy Love <mailto:jez@jezlove.uk>
	copyright 2020
*/

using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.XPath;

namespace ReSearcher {

    public static class XmlNodeListExtensions {

		#region matching

			public static IEnumerable<Tuple<XmlNode, MatchCollection>> matchWithinAll(this XmlNodeList thisXmlNodeList, Regex regex) {
				foreach(XmlNode xmlNode in thisXmlNodeList) {
					MatchCollection matchCollection = regex.Matches(xmlNode.InnerText);
					if(0 == matchCollection.Count) {
						continue;
					}
					yield return(new Tuple<XmlNode, MatchCollection>(xmlNode, matchCollection));
				}
			}

		#endregion

	}

}
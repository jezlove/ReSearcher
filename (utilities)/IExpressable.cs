/*
	C# "IExpressable.cs"
	by Jeremy Love <mailto:jez@jezlove.uk>
	copyright 2020
*/

using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace ReSearcher {

	public interface IExpressable {

		IEnumerable<XNode> express();

	}

}
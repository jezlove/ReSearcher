/*
	C# "IDownloadableResourceFileCollection.cs"
	by Jeremy Love <mailto:jez@jezlove.uk>
	copyright 2020
*/

using System;
using System.Collections.Generic;

namespace ReSearcher {

	public interface IDownloadableResourceFileCollection :
		IDownloadableResource {

		IList<IDownloadableResourceFile> files { get; }

	}

}
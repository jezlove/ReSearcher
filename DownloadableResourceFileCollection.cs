/*
	C# "DownloadableResourceFileCollection.cs"
	by Jeremy Love <mailto:jez@jezlove.uk>
	copyright 2020
*/

using System;
using System.Collections.Generic;

namespace ReSearcher {

	public class DownloadableResourceFileCollection :
		IDownloadableResourceFileCollection {

		public String name { get; private set; }

		public IList<IDownloadableResourceFile> files { get; private set; }

		public DownloadableResourceFileCollection(String name, IList<IDownloadableResourceFile> files) {
			this.name = name;
			this.files = files;
		}

		public override String ToString() {
			return(name);
		}

	}

}
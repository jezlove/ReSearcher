/*
	C# "DownloadableResourceFile.cs"
	by Jeremy Love <mailto:jez@jezlove.uk>
	copyright 2020
*/

using System;

namespace ReSearcher {

	public class DownloadableResourceFile :
		IDownloadableResourceFile {

		public Uri downloadUri { get; private set; }

		public String name { get; private set; }

		public String type { get; private set; }

		public long size { get; private set; }

		public String location { get; private set; }

		public DownloadableResourceFile(Uri downloadUri, String name, String type, long size, String location) {
			this.downloadUri = downloadUri;
			this.name = name;
			this.type = type;
			this.size = size;
			this.location = location;
		}

		public override String ToString() {
			return(downloadUri.ToString());
		}

	}

}
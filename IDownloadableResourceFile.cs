/*
	C# "IDownloadableResourceFile.cs"
	by Jeremy Love <mailto:jez@jezlove.uk>
	copyright 2020
*/

using System;

namespace ReSearcher {

	public interface IDownloadableResourceFile :
		IDownloadableResource {

		long size { get; }

		String type { get; }

		String location { get; }

		Uri downloadUri { get; }

	}

}
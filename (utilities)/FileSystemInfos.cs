/*
	C# "FileSystemInfos.cs"
	by Jeremy Love <mailto:jez@jezlove.uk>
	copyright 2020
*/

using System;
using System.IO;

namespace ReSearcher {

    public static class FileSystemInfos {

		#region presuming

			public static FileSystemInfo presumeFor(String path) {
				if(Directory.Exists(path)) {
					return(new DirectoryInfo(path));
				}

				// note: in all other cases just assume it's a file or the intention is that it is a file,
				// remember that FileSystemInfo can represent non-existent paths also

				return(new FileInfo(path));
			}

		#endregion

	}

}
/*
	C# "AbstractSearcher.cs"
	by Jeremy Love <mailto:jez@jezlove.uk>
	copyright 2020
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;

namespace ReSearcher {

    public abstract class AbstractSearcher :
		ISearcher {

		protected readonly String filter;

		protected AbstractSearcher(String filter = Filters.any) {
			this.filter = filter;
		}

		#region searching-file-system

			public SelectionSearchResults search(IEnumerable<FileSystemInfo> fileSystemInfos) {
				return(new SelectionSearchResults(searchItems(fileSystemInfos)));
			}

			protected IEnumerable<ISearchResultBranch> searchItems(IEnumerable<FileSystemInfo> fileSystemInfos) {
				foreach(FileSystemInfo fileSystemInfo in fileSystemInfos) {
					ISearchResultBranch iSearchResultBranch = searchItem(fileSystemInfo);
					if(null != iSearchResultBranch) {
						yield return(iSearchResultBranch);
					}
				}
			}

			protected ISearchResultBranch searchItem(FileSystemInfo fileSystemInfo) {
				FileInfo fileInfo = fileSystemInfo as FileInfo;
				if(null != fileInfo) {
					return(searchFile(fileInfo));
				}
				else {
					return(searchDirectory((DirectoryInfo)(fileSystemInfo)));
				}
			}

			protected DirectorySearchResults searchDirectory(DirectoryInfo directoryInfo) {
				return(new DirectorySearchResults(directoryInfo, searchItems(directoryInfo.EnumerateFileSystemInfos(filter))));
			}

			protected abstract FileSearchResults searchFile(FileInfo fileInfo);

		#endregion

	}

}
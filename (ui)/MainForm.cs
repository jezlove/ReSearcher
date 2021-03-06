/*
	C# "MainForm.cs"
	by Jeremy Love <mailto:jez@jezlove.uk>
	copyright 2020
*/

#if DEBUG
//#define USE_MOCK_SEARCHER
//#define USE_MOCK_DOWNLOADER
#endif

using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Reflection;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using System.Drawing;
using System.Windows.Forms;

using ReSearcher.Ou;

namespace ReSearcher {

    public class MainForm :
		Form {

		private readonly TabControl tabControl;
		private readonly StatusStrip statusStrip;

		public MainForm() {

			Text = "ReSearcher";
			Icon = IconUtilities.loadCurrentModuleIcon();

			StartPosition = FormStartPosition.CenterScreen;
			Size = new Size(
				(int)(Screen.PrimaryScreen.Bounds.Width * 0.6f),
				(int)(Screen.PrimaryScreen.Bounds.Height * 0.6f)
			);

			// assemble UI components
			// extension methods are in use here to provide a fluent and elegant component hierarchy
			this.appendControls(
				tabControl = new TabControl() { Dock = DockStyle.Fill, Padding = new Point(24, 4) },
				statusStrip = new StatusStrip(),
				new ToolStripPanel() { Dock = DockStyle.Top }.withControls(
					new ToolStrip().withItems(
						new ToolStripButton() { Text = "Search..." }.withAction(search),
						new ToolStripSeparator(),
						new ToolStripButton() { Text = "Browse..." }.withAction(browse),
						new ToolStripButton() { Text = "Open..." }.withAction(open),
						new ToolStripSeparator(),
						new ToolStripButton() { Text = "Download..." }.withAction(download)
					)
				),
				MainMenuStrip = new MenuStrip() { Dock = DockStyle.Top }.withItems(
					new ToolStripMenuItem() { Text = "ReSearcher" }.withDropDownItems(
						new ToolStripMenuItem() { Text = "Search..." }.withAction(search),
						new ToolStripSeparator(),
						new ToolStripMenuItem() { Text = "Browse..." }.withAction(browse),
						new ToolStripMenuItem() { Text = "Open..." }.withAction(open),
						new ToolStripSeparator(),
						new ToolStripMenuItem() { Text = "Download..." }.withAction(download),
						new ToolStripSeparator(),
						new ToolStripMenuItem() { Text = "Settings" }.withAction(showSettings),
						new ToolStripSeparator(),
						new ToolStripMenuItem() { Text = "Quit" }.withAction(quit)
					),
					new ToolStripMenuItem() { Text = "Tab" }.withDropDownItems(
						new ToolStripMenuItem() { Text = "Close" }.withAction(closeSelectedTab)
					),
					new ToolStripMenuItem() { Text = "Help" }.withDropDownItems(
						new ToolStripMenuItem() { Text = "Help table of contents" }.withAction(showHelp),
						new ToolStripMenuItem() { Text = "Help index" }.withAction(showHelpIndex),
						new ToolStripSeparator(),
						new ToolStripMenuItem() { Text = "Help online" }.withAction(showHelpOnline),
						new ToolStripSeparator(),
						new ToolStripMenuItem() { Text = "About" }.withAction(showHelpAbout)
					)
				)
			);
		}

		protected override void OnLoad(EventArgs eventArgs) {
			if(!String.IsNullOrWhiteSpace(ProgramSettings.startingDirectoryPath)) {
				tabControl.appendControls(new BrowsePage(ProgramSettings.startingDirectoryPath));
			}
			base.OnLoad(eventArgs);
		}

		public IEnumerable<FileSystemInfo> getSelectedFileSystemInfosOrCurrentDirectoryOrEmpty() {
			BrowsePage browsePage = tabControl.SelectedTab as BrowsePage;
			if(null != browsePage) {
				IEnumerable<FileSystemInfo> fileSystemInfos = browsePage.enumerateSelectedFileSystemInfos();
				if(fileSystemInfos.Any()) {
					return(fileSystemInfos);
				}
				if(null != browsePage.directoryInfo) {
					return(new [] {browsePage.directoryInfo});
				}
			}
			return(Enumerable.Empty<FileSystemInfo>());
		}

		public String getSelectedFilterOrAny() {
			BrowsePage browsePage = tabControl.SelectedTab as BrowsePage;
			return(null == browsePage ? Filters.any : browsePage.selectedFilter);
		}

		public void search() {
			SearchCriteria searchCriteria;
			SearchDetails searchDetails;
			if(!SearchForm.parameterise(out searchCriteria, out searchDetails, getSelectedFileSystemInfosOrCurrentDirectoryOrEmpty(), getSelectedFilterOrAny(), this)) {
				return;
			}
			SearchDetailsRepository.commit(searchDetails);
			String filter = (String.IsNullOrWhiteSpace(searchCriteria.filter)) ? Filters.any : searchCriteria.filter;
			ISearcher searcher;
			try {

				#if USE_MOCK_SEARCHER

					searcher = new SleepyMockSearcher(filter);

				#else

					if(null == searchCriteria.xpath) {
						searcher = new UnsophisticatedSearcher(searchCriteria.regex, filter);
					}
					else {
						searcher = new SophisticatedSearcher(searchCriteria.regex, searchCriteria.xpath, filter);
					}

				#endif

				searchWith(searcher, searchCriteria);
			}
			catch(Exception exception) {
				this.error(exception);
			}
		}

		internal void searchWith(ISearcher searcher, SearchCriteria searchCriteria) {

			XDocument resultsPageXDocument = null;

			IndefiniteProcessingForm.process(
				(cancellationRequestedChecker, log) => {
					log.WriteLine("Searching...");

					resultsPageXDocument = new [] {
						searcher.search(searchCriteria.fileSystemInfos, cancellationRequestedChecker, log)
					}.toXDocument();

					return(false);
				},
				"Searching...",
				this
			);
			if(null == resultsPageXDocument) {
				this.error("An error occured whilst searching.");
				return;
			}

			// note: resultsPage has to be constructed in this UI thread
			ResultsPage resultsPage = new ResultsPage(resultsPageXDocument);

			tabControl.appendControls(resultsPage);
			tabControl.SelectedTab = resultsPage;
		}

		public void browse() {
			using(FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog()) {
				if(DialogResult.OK == folderBrowserDialog.ShowDialog(this)) {
					browse(folderBrowserDialog.SelectedPath);
				}
			}
		}

		public void browse(String directoryPath) {
			try {
				BrowsePage browsePage = new BrowsePage(directoryPath);
				tabControl.appendControls(browsePage);
				tabControl.SelectedTab = browsePage;
			}
			catch(Exception exception) {
				this.error(exception);
			}			
		}

		public void open() {
			try {
				foreach(FileSystemInfo fileSystemInfo in getSelectedFileSystemInfosOrCurrentDirectoryOrEmpty()) {
					Process.Start(fileSystemInfo.FullName);
				}
			}
			catch(Exception exception) {
				this.error(exception);
			}	
		}

		public void download() {
			try {

				#if USE_MOCK_DOWNLOADER

					downloadAllNotDownloadedWith(new SleepyMockDownloader(MockDownloadData.load()), ProgramSettings.downloadsDirectoryPath);

				#else

					OuSignedInWebSession ouSignedInWebSession = OuSignInForm.signIn(this);
					if(null == ouSignedInWebSession) {
						return;
					}
					using(ouSignedInWebSession) {
						downloadAllNotDownloadedWith(new OuDownloader(ouSignedInWebSession), ProgramSettings.downloadsDirectoryPath);
					}

				#endif

			}
			catch(Exception exception) {
				this.error(exception);
			}	
		}

		internal void downloadAllNotDownloadedWith(IDownloader downloader, String downloadsDirectoryPath) {

			IEnumerable<IDownloadableResourceFileCollection> available = null;
			IndefiniteProcessingForm.process(
				(cancellationRequestedChecker, log) => {
					log.WriteLine("Searching for new content...");

					// TODO: consider re-writing enumerateResourceFileCollections to be lazily consumable, could perhaps avert the need for a cancellation checker?

					available = downloader.enumerateResourceFileCollections(cancellationRequestedChecker, log);
					return(false);
				},
				"Searching...",
				this
			);
			if(available.isNullOrEmpty()) {
				return;
			}

			IEnumerable<KeyValuePair<String, List<IDownloadableResourceFile>>> notDownloaded = available.map().enumerateNotDownloaded(downloadsDirectoryPath);
			if(!notDownloaded.Any()) {
				this.inform("All files checked are up-to-date.", "Downloads");
				return;
			}
			IList<Tuple<IDownloadableResourceFile, String>> picks = DownloadPicker.pickDownloads(notDownloaded, this);
			if(picks.isNullOrEmpty()) {
				return;
			}

			long downloadedByteCount = 0;
			long totalByteCount = picks.Select(t => t.Item1.size).Sum();

			BatchProcessingForm<Tuple<IDownloadableResourceFile, String>>.process(
				picks,
				(tuple, log) => {
					IDownloadableResourceFile downloadableResourceFile = tuple.Item1;
					String relativeFilePath = tuple.Item2;
					log.WriteLine("Downloading: \"{0}\"", downloadableResourceFile.name);
					FileInfo fileInfo = new FileInfo(Path.Combine(downloadsDirectoryPath, relativeFilePath));
					fileInfo.Directory.Create();
					if(!downloader.download(downloadableResourceFile, fileInfo.FullName)) {
						Console.Error.WriteLine("Downloading of uri: {0} failed", downloadableResourceFile);
						if(fileInfo.Exists) {
							Console.Error.WriteLine("Deleting partially downloaded file: {0}", fileInfo.FullName);
							fileInfo.Delete();
						}
						return(-1);
					}
					downloadedByteCount += downloadableResourceFile.size;
					return((int)(Math.Ceiling((Double)(downloadedByteCount) / (Double)(totalByteCount) * 100d)));
				},
				"Downloading...",
				"{0}% downloaded",
				this
			);
		}

		public void closeSelectedTab() {
			try {
				if(null != tabControl.SelectedTab) {
					tabControl.TabPages.Remove(tabControl.SelectedTab);
				}
			}
			catch(Exception exception) {
				this.error(exception);
			}
		}

		public void showSettings() {
			try {
				using(SettingsForm settingsForm = new SettingsForm()) {
					settingsForm.ShowDialog();
				}
			}
			catch(Exception exception) {
				this.error(exception);
			}
		}

		public void showHelp() {
			try {
				String chmFilePath = getHelpFilePath();
				if(!File.Exists(chmFilePath)) {
					this.error("Could not locate help file: " + chmFilePath);
				}
				Help.ShowHelp(this, chmFilePath);
			}
			catch(Exception exception) {
				this.error(exception);
			}
		}

		public void showHelpIndex() {
			try {
				String chmFilePath = getHelpFilePath();
				if(!File.Exists(chmFilePath)) {
					this.error("Could not locate help file: " + chmFilePath);
				}
				Help.ShowHelpIndex(this, chmFilePath);
			}
			catch(Exception exception) {
				this.error(exception);
			}
		}

		internal static String getHelpFilePath() {
			return(Path.Combine(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location), "ReSearcher.chm"));
		}

		public void showHelpOnline() {
			try {
				ProcessStartInfo processStartInfo = new ProcessStartInfo();
				processStartInfo.UseShellExecute = true;
				processStartInfo.FileName = "http://help.jezlove.com/ReSearcher/";
				Process.Start(processStartInfo);
			}
			catch(Exception exception) {
				this.error(exception);
			}
		}

		public void showHelpAbout() {
			this.inform("ReSearcher is a tool for searching through course materials using regular expressions.", "About");
		}

		public void quit() {
			try {
				Dispose();
			}
			catch(Exception exception) {
				this.error(exception);
			}
		}

	}

}
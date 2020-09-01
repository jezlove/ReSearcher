/*
	C# "ProgramSettings.cs"
	by Jeremy Love <mailto:jez@jezlove.uk>
	copyright 2020
*/

using System;
using System.IO;
using System.Configuration;
using System.Windows.Forms;

namespace ReSearcher {

    internal sealed class ProgramSettings {

		#region cached-settings

			internal static String epubViewerProgramFilePath;
			internal static String epubViewerOpenArgumentFormatString;
			internal static String epubViewerOpenToSectionArgumentFormatString;
			internal static String wordViewerProgramFilePath;
			internal static String wordViewerOpenArgumentFormatString;
			internal static String pdfViewerProgramFilePath;
			internal static String pdfViewerOpenArgumentFormatString;
			internal static String pdfViewerOpenToPageArgumentFormatString;
			internal static String downloadsDirectoryPath;
			internal static String startingDirectoryPath;
			internal static String ouUsername;
			private static String ouPasswordEncrypted;
			internal static String ouPassword {
				get { return((String.IsNullOrWhiteSpace(ouPasswordEncrypted)) ? null : ouPasswordEncrypted.decrypt()); }
				set { ouPasswordEncrypted = (String.IsNullOrWhiteSpace(value)) ? null : value.encrypt(); }
			}

		#endregion

		private Configuration configuration;

		private ProgramSettings() {
			this.configuration = tryLoadConfiguration();
		}

		~ProgramSettings() {
			if(null != configuration) {
				trySaveConfiguration(configuration);
			}
		}

		private static readonly ProgramSettings programSettings = new ProgramSettings();

		private static Configuration tryLoadConfiguration() {
			try {
				Configuration configuration = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
				configuration.AppSettings.Settings

					.take("epubViewerProgramFilePath", out epubViewerProgramFilePath, getCommonEpubViewerProgramFilePathOrNull)
					.take("epubViewerOpenArgumentFormatString", out epubViewerOpenArgumentFormatString, defaultArgumentFormatString)
					.take("epubViewerOpenToSectionArgumentFormatString", out epubViewerOpenToSectionArgumentFormatString,
						() => isMostLikelyCalibre2(epubViewerProgramFilePath) ? "--open-at toc:\"{1}\" \"{0}\"" : defaultArgumentFormatString
					)

					.take("wordViewerProgramFilePath", out wordViewerProgramFilePath, getCommonWordViewerProgramFilePathOrNull)
					.take("wordViewerOpenArgumentFormatString", out wordViewerOpenArgumentFormatString, defaultArgumentFormatString)

					.take("pdfViewerProgramFilePath", out pdfViewerProgramFilePath, getCommonPdfViewerProgramFilePathOrNull)
					.take("pdfViewerOpenArgumentFormatString", out pdfViewerOpenArgumentFormatString, defaultArgumentFormatString)
					.take("pdfViewerOpenToPageArgumentFormatString", out pdfViewerOpenToPageArgumentFormatString, 
						() => isMostLikelyAdobeAcrobatReaderDC(pdfViewerProgramFilePath) ? "page={1} \"{0}\"" : defaultArgumentFormatString
					)

					.take("downloadsDirectoryPath", out downloadsDirectoryPath, getUserDownloadsDirectoryPathOrUserProfileDirectoryPath)
					.take("startingDirectoryPath", out startingDirectoryPath, () => Environment.GetFolderPath(Environment.SpecialFolder.UserProfile))

					.take("ouUsername", out ouUsername)
					.take("ouPassword", out ouPasswordEncrypted);

				return(configuration);
			}
			catch(Exception exception) {
				Console.Error.WriteLine("Error: could not load program settings due to exception: {0}", exception);
				MessageBoxes.error("Could not load program settings");
				return(null);
			}
		}

		private static void trySaveConfiguration(Configuration configuration) {
			try {
				configuration.AppSettings.Settings
					.with("epubViewerProgramFilePath", epubViewerProgramFilePath)
					.with("epubViewerOpenArgumentFormatString", epubViewerOpenArgumentFormatString)
					.with("epubViewerOpenToSectionArgumentFormatString", epubViewerOpenToSectionArgumentFormatString)
					.with("wordViewerProgramFilePath", wordViewerProgramFilePath)
					.with("wordViewerOpenArgumentFormatString", wordViewerOpenArgumentFormatString)
					.with("pdfViewerProgramFilePath", pdfViewerProgramFilePath)
					.with("pdfViewerOpenArgumentFormatString", pdfViewerOpenArgumentFormatString)
					.with("pdfViewerOpenToPageArgumentFormatString", pdfViewerOpenToPageArgumentFormatString)
					.with("downloadsDirectoryPath", downloadsDirectoryPath)
					.with("startingDirectoryPath", startingDirectoryPath)
					.with("ouUsername", ouUsername)
					.with("ouPassword", ouPasswordEncrypted);
				configuration.Save(ConfigurationSaveMode.Full);
			}
			catch(Exception exception) {
				// suppress any errors, the program is exiting not much can be done here
				Console.Error.WriteLine("Error: could not save program settings due to exception: {0}", exception);
				MessageBoxes.error("Could not save program settings");
			}
		}

		internal static String getUserDownloadsDirectoryPathOrUserProfileDirectoryPath() {
			String userProfilePath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);

			// for now make an assumption about English language localisation
			// TODO: use P/Invoke to get actual directory path

			String downloadsDirectoryPath = Path.Combine(userProfilePath, "Downloads");
			return(Directory.Exists(downloadsDirectoryPath) ? downloadsDirectoryPath : userProfilePath);
		}

		internal static String getCommonEpubViewerProgramFilePathOrNull() {
			return(
				getInstalledProgramFilePathOrNull(@"Calibre2\ebook-viewer.exe")
			);
		}

		internal static String getCommonWordViewerProgramFilePathOrNull() {
			return(

				getInstalledProgramFilePathOrNull(@"Microsoft Office\Office16\WINWORD.EXE") ?? // Office 2016
				getInstalledProgramFilePathOrNull(@"Microsoft Office\Office15\WINWORD.EXE") ?? // Office 2013
				getInstalledProgramFilePathOrNull(@"Microsoft Office\Office14\WINWORD.EXE") ?? // Office 2010
				// (no 13)
				getInstalledProgramFilePathOrNull(@"Microsoft Office\Office12\WINWORD.EXE") ?? // Office 2007
				getInstalledProgramFilePathOrNull(@"Microsoft Office\Office11\WINWORD.EXE") ?? // Office 2003
				getInstalledProgramFilePathOrNull(@"Microsoft Office\Office10\WINWORD.EXE") ?? // Office XP

				// TODO: Open Office/Libre Office

				getInstalledProgramFilePathOrNull(@"Windows NT\Accessories\wordpad.exe") // provides rudimentary viewing capability

			);
		}

		internal static String getCommonPdfViewerProgramFilePathOrNull() {
			return(

				getInstalledProgramFilePathOrNull(@"Adobe\Acrobat Reader DC\Reader\AcroRd.exe") ??
				getInstalledProgramFilePathOrNull(@"Adobe\Acrobat Reader DC\Reader\AcroRd32.exe") ??

				// TODO: Foxit, SimplePDF

				getInstalledProgramFilePathOrNull(@"Google\Chrome\Application\chrome.exe") // provides rudimentary viewing capability

			);
		}

		internal static String getInstalledProgramFilePathOrNull(String relativeToProgramFilesProgramFilePath) {
			String filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles), relativeToProgramFilesProgramFilePath);
			if(File.Exists(filePath)) {
				return(filePath);
			}
			filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86), relativeToProgramFilesProgramFilePath);
			if(File.Exists(filePath)) {
				return(filePath);
			}
			return(null);
		}

		internal static Boolean isMostLikelyCalibre2(String programFilePath) {
			return(!String.IsNullOrWhiteSpace(programFilePath) && "ebook-viewer.exe" == Path.GetFileName(programFilePath).ToLowerInvariant());
		}

		internal static Boolean isMostLikelyAdobeAcrobatReaderDC(String programFilePath) {
			if(String.IsNullOrWhiteSpace(programFilePath)) {
				return(false);
			}
			String fileName = Path.GetFileName(programFilePath).ToLowerInvariant();
			return("acrord.exe" == fileName || "acrord32.exe" == fileName);
		}

		internal const String defaultArgumentFormatString = "\"{0}\"";

	}

}
/*
	C# "ProgramSettings.cs"
	by Jeremy Love <mailto:jez@jezlove.uk>
	copyright 2020
*/

using System;
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
					.take("epubViewerProgramFilePath", out epubViewerProgramFilePath)
					.take("epubViewerOpenArgumentFormatString", out epubViewerOpenArgumentFormatString)
					.take("epubViewerOpenToSectionArgumentFormatString", out epubViewerOpenToSectionArgumentFormatString)
					.take("wordViewerProgramFilePath", out wordViewerProgramFilePath)
					.take("wordViewerOpenArgumentFormatString", out wordViewerOpenArgumentFormatString)
					.take("pdfViewerProgramFilePath", out pdfViewerProgramFilePath)
					.take("pdfViewerOpenArgumentFormatString", out pdfViewerOpenArgumentFormatString)
					.take("pdfViewerOpenToPageArgumentFormatString", out pdfViewerOpenToPageArgumentFormatString)
					.take("downloadsDirectoryPath", out downloadsDirectoryPath)
					.take("startingDirectoryPath", out startingDirectoryPath)
					.take("ouUsername", out ouUsername)
					.take("ouPassword", out ouPasswordEncrypted);
				if(String.IsNullOrWhiteSpace(downloadsDirectoryPath)) {

					// TODO: select user "Downloads" folder as default

					downloadsDirectoryPath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
				}
				if(String.IsNullOrWhiteSpace(startingDirectoryPath)) {
					startingDirectoryPath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
				}
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

	}

}
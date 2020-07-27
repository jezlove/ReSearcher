/*
	C# "ResultsPageObjectForScripting.cs"
	by Jeremy Love <mailto:jez@jezlove.uk>
	copyright 2020
*/

using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace ReSearcher {

	[ComVisible(true)]
	public class ResultsPageObjectForScripting {

		private ResultsPage resultsPage;

		public ResultsPageObjectForScripting(ResultsPage resultsPage) {
			this.resultsPage = resultsPage;
		}

		public void openWithDefaultAssociatedProgram(String path) {
			try {
				Process.Start(path);
			}
			catch {
				resultsPage.error(String.Format("Could not open \"{0}\"", path));
			}
		}

		public void openDirectory(String directoryPath) {
			MainForm mainForm = resultsPage.FindForm() as MainForm;
			mainForm.browse(directoryPath);
		}

		public void openFile(String filePath) {
			openWithDefaultAssociatedProgram(filePath);
		}

		public void openEpubFile(String epubFilePath) {
			if(String.IsNullOrEmpty(ProgramSettings.epubViewerProgramFilePath)) {
				openFile(epubFilePath);
				return;
			}
			try {
				Process.Start(ProgramSettings.epubViewerProgramFilePath, String.Format(ProgramSettings.epubViewerOpenArgumentFormatString, epubFilePath));
			}
			catch {
				resultsPage.error(String.Format("Could not open Epub file \"{0}\"", epubFilePath));
			}
		}

		public void openEpubFileToSection(String title, String epubFilePath) {
			if(String.IsNullOrEmpty(ProgramSettings.epubViewerOpenToSectionArgumentFormatString)) {
				openEpubFile(epubFilePath);
				return;
			}
			if(String.IsNullOrEmpty(ProgramSettings.epubViewerProgramFilePath)) {
				openFile(epubFilePath);
				return;
			}
			try {
				Process.Start(ProgramSettings.epubViewerProgramFilePath, String.Format(ProgramSettings.epubViewerOpenToSectionArgumentFormatString, epubFilePath, title));
			}
			catch {
				resultsPage.error(String.Format("Could not open Epub file \"{0}\", to section \"{1}\"", epubFilePath, title));
			}
		}

		public void openWordFile(String wordFilePath) {
			if(String.IsNullOrEmpty(ProgramSettings.wordViewerProgramFilePath)) {
				openFile(wordFilePath);
				return;
			}
			try {
				Process.Start(ProgramSettings.wordViewerProgramFilePath, String.Format(ProgramSettings.wordViewerOpenArgumentFormatString, wordFilePath));
			}
			catch {
				resultsPage.error(String.Format("Could not open Word file \"{0}\"", wordFilePath));
			}
		}

		public void openPdfFile(String pdfFilePath) {
			if(String.IsNullOrEmpty(ProgramSettings.pdfViewerProgramFilePath)) {
				openFile(pdfFilePath);
				return;
			}
			try {
				Process.Start(ProgramSettings.pdfViewerProgramFilePath, String.Format(ProgramSettings.pdfViewerOpenArgumentFormatString, pdfFilePath));
			}
			catch {
				resultsPage.error(String.Format("Could not open PDF file \"{0}\"", pdfFilePath));
			}
		}

		public void openPdfFileToPage(int pageNumber, String pdfFilePath) {
			if(String.IsNullOrEmpty(ProgramSettings.pdfViewerOpenArgumentFormatString)) {
				openPdfFile(pdfFilePath);
				return;
			}
			if(String.IsNullOrEmpty(ProgramSettings.pdfViewerProgramFilePath)) {
				openFile(pdfFilePath);
				return;
			}
			try {
				Process.Start(ProgramSettings.pdfViewerProgramFilePath, String.Format(ProgramSettings.pdfViewerOpenToPageArgumentFormatString, pdfFilePath, pageNumber));
			}
			catch {
				resultsPage.error(String.Format("Could not open PDF file \"{0}\"", pdfFilePath));
			}
		}

	}

}
/*
	C# "AbstractMultiFormatSearcher.cs"
	by Jeremy Love <mailto:jez@jezlove.uk>
	copyright 2020
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text.RegularExpressions;
using System.Diagnostics;

// for searching epub documents
using ReSearcher.Epub;
using System.IO.Compression;

// for searching word documents
using ReSearcher.Word;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.Win32;
using Microsoft.Office.Core;
using Wd = Microsoft.Office.Interop.Word;

// for searching pdf documents
using ReSearcher.Pdf;
using iTextSharp.text.pdf;

namespace ReSearcher {

    public abstract class AbstractMultiFormatSearcher :
		AbstractSearcher {

		protected readonly Regex regex;

		protected AbstractMultiFormatSearcher(Regex regex, String filter = Filters.any) :
			base(filter) {
			this.regex = regex;
		}

		#region searching-files

			protected override FileSearchResults searchFile(FileInfo fileInfo) {
				String extension = fileInfo.Extension.ToLowerInvariant();
				if(".epub" == extension) {
					return(searchEpubFile(fileInfo));
				}
				if(".docx" == extension || ".doc" == extension) {
					return(searchWordFile(fileInfo));
				}
				if(".pdf" == extension) {
					return(searchPdfFile(fileInfo));
				}
				return(null);
			}

		#endregion

		#region searching-epub-documents

			protected EpubFileSearchResults searchEpubFile(FileInfo epubFileInfo) {
				using(ZipArchive zipArchive = ZipFile.Open(epubFileInfo.FullName, ZipArchiveMode.Read)) {
					IEnumerable<EpubEntrySearchResults> epubEntrySearchResults = searchEpubFile(zipArchive);
					if(epubEntrySearchResults.Any()) {
						// note: must condense to list, as zipArchive will have been disposed upon enumeration
						return(new EpubFileSearchResults(epubFileInfo, epubEntrySearchResults.ToList()));
					}
					return(null);
				}
			}

			protected IEnumerable<EpubEntrySearchResults> searchEpubFile(ZipArchive zipArchive) {
				foreach(ZipArchiveEntry zipArchiveEntry in zipArchive.Entries) {
					String entryName = zipArchiveEntry.FullName;
					String entryNameLowercase = entryName.ToLowerInvariant();
					if(opsHtmlLowercaseRegex.IsMatch(entryNameLowercase)) {
						using(Stream stream = zipArchiveEntry.Open()) {
							EpubEntrySearchResults epubEntrySearchResults = searchEpubEntry(entryName, stream);
							if(null != epubEntrySearchResults) {
								yield return(epubEntrySearchResults);
							}
						}
					}
				}
			}

			protected abstract EpubEntrySearchResults searchEpubEntry(String entryName, Stream stream);

			private static readonly Regex opsHtmlLowercaseRegex = new Regex(@"^ops/(.+)(\.html?)$", RegexOptions.Compiled);

		#endregion

		#region searching-word-documents

			protected WordFileSearchResults searchWordFile(FileInfo wordFileInfo) {
				return(
					wordFileInfo.Extension.EndsWith("x") ?
						searchWordFileUsingOpenXmlApi(wordFileInfo) :
						searchWordFileUsingInteropApi(wordFileInfo)
				);
			}

			protected WordFileSearchResults searchWordFileUsingOpenXmlApi(FileInfo wordFileInfo) {
				try {
					using(WordprocessingDocument wordprocessingDocument = WordprocessingDocument.Open(wordFileInfo.FullName, isEditable: false)) {
						using(StreamReader streamReader = new StreamReader(wordprocessingDocument.MainDocumentPart.GetStream())) {
							String text = streamReader.ReadToEnd();
							MatchCollection matchCollection = regex.Matches(text);
							if(0 == matchCollection.Count) {
								return(null);
							}
							return(new WordFileSearchResults(wordFileInfo, new RegexSearchResultMatchCollection(matchCollection)));
						}
					}
				}
				catch(Exception exception) {
					Console.Error.WriteLine("Error: exception: {0}", exception);
					return(null);
				}
			}

			protected static Boolean isWordInstalled() {
				using(RegistryKey registryKey = Registry.ClassesRoot.OpenSubKey("Word.Application")) {
					return(null != registryKey);
				}
			}

			protected WordFileSearchResults searchWordFileUsingInteropApi(FileInfo wordFileInfo) {

				// silently ignore file if word is not installed
				if(!isWordInstalled()) {
					return(null);
				}

				Object missing = System.Reflection.Missing.Value;
				Wd.Application wordApplication = null;
				Wd.Document wordDocument = null;
				try {
					wordApplication = new Wd.Application() {
						Visible = false,
						ShowAnimation = false,
						DisplayAlerts = Wd.WdAlertLevel.wdAlertsNone
					};
					wordDocument = wordApplication.Documents.Open(wordFileInfo.FullName, ReadOnly: true);
					String text = wordDocument.Content.Text;
					MatchCollection matchCollection = regex.Matches(text);
					if(0 == matchCollection.Count) {
						return(null);
					}
					return(new WordFileSearchResults(wordFileInfo, new RegexSearchResultMatchCollection(matchCollection)));
				}
				catch(Exception exception) {
					Console.Error.WriteLine("Error: exception: {0}", exception);
					return(null);
				}
				finally {
					if(null != wordDocument) {
						((Wd._Document)(wordDocument)).Close(ref missing, ref missing, ref missing);
						wordDocument = null;
					}
					if(null != wordApplication) {
						((Wd._Application)(wordApplication)).Quit(ref missing, ref missing, ref missing);
						wordApplication = null;
					}
				}
			}

		#endregion

		#region searching-pdf-documents

			protected PdfFileSearchResults searchPdfFile(FileInfo pdfFileInfo) {
				IList<Tuple<int, MatchCollection>> matchCollections = searchPdfFileByPage(pdfFileInfo).ToList();
				if(0 == matchCollections.Count) {
					return(null);
				}
				return(new PdfFileSearchResults(pdfFileInfo, matchCollections.Select(t => new PdfPageSearchResults(t.Item1, new RegexSearchResultMatchCollection(t.Item2))).ToArray()));
			}

			protected IEnumerable<Tuple<int, MatchCollection>> searchPdfFileByPage(FileInfo pdfFileInfo) {
				using(PdfReader pdfReader = new PdfReader(pdfFileInfo.FullName)) {
					int pageCount = pdfReader.NumberOfPages;
					PdfLinearTextExtractionStrategy pdfLinearTextExtractionStrategy = new PdfLinearTextExtractionStrategy();
					for(int pageNumber = 1; pageNumber <= pageCount; pageNumber++) {
						pdfLinearTextExtractionStrategy.lines.Clear();
						iTextSharp.text.pdf.parser.PdfTextExtractor.GetTextFromPage(pdfReader, pageNumber, pdfLinearTextExtractionStrategy);
						String text = String.Join(Environment.NewLine, pdfLinearTextExtractionStrategy.lines);
						MatchCollection matchCollection = regex.Matches(text);
						if(0 != matchCollection.Count) {
							yield return(new Tuple<int, MatchCollection>(pageNumber, matchCollection));
						}
					}
				}
			}

		#endregion

	}

}
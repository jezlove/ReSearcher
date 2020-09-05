/*
	C# "SettingsForm.cs"
	by Jeremy Love <mailto:jez@jezlove.uk>
	copyright 2020
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Drawing;
using System.Windows.Forms;
using System.Xml.XPath;

namespace ReSearcher {

    public sealed class SettingsForm :
		Form {

		private readonly TextBox downloadsDirectoryPathTextBox;
		private readonly TextBox startingDirectoryPathTextBox;
		private readonly TabControl tabControl;
		private readonly TextBox epubViewerProgramFilePathTextBox;
		private readonly TextBox epubViewerOpenArgumentFormatTextBox;
		private readonly TextBox epubViewerOpenToSectionArgumentFormatTextBox;
		private readonly TextBox wordViewerProgramFilePathTextBox;
		private readonly TextBox wordViewerOpenArgumentFormatTextBox;
		private readonly TextBox pdfViewerProgramFilePathTextBox;
		private readonly TextBox pdfViewerOpenArgumentFormatTextBox;
		private readonly TextBox pdfViewerOpenToPageArgumentFormatTextBox;

		public SettingsForm() {

			Text = "Settings";
			Padding = new Padding(8);
			Size = new Size(700, 450);
			StartPosition = FormStartPosition.CenterParent;
			FormBorderStyle = FormBorderStyle.FixedDialog;
			ShowInTaskbar = false;
			MinimizeBox = false;
			MaximizeBox = false;

			Padding tabPagePadding = new Padding(12);
			this.appendControls(
				new Panel() { Dock = DockStyle.Fill }.withControls(
					tabControl = new TabControl() { Dock = DockStyle.Fill, Padding = new Point(24, 4) }.withControls(
						new TabPage() { Text = "EPUB", Padding = tabPagePadding }.withControls(
							new TableLayoutPanel() { Dock = DockStyle.Fill, ColumnCount = 1, Padding = new Padding(2) }.withControls(
								new Label() { Text = "Open with:" },
								new Panel() { Dock = DockStyle.Fill, Height = 24 }.withControls(
									epubViewerProgramFilePathTextBox = new TextBox() { Dock = DockStyle.Fill, Font = Fonts.monospace, Text = ProgramSettings.epubViewerProgramFilePath },
									new Button() { Text = "...", Dock = DockStyle.Right }.withAction(() => { browseForProgramFilePathToUpdate(epubViewerProgramFilePathTextBox); })
								),
								new Label() { Text = "To file:" },
								epubViewerOpenArgumentFormatTextBox = new TextBox() { Dock = DockStyle.Fill, Font = Fonts.monospace, Text = ProgramSettings.epubViewerOpenArgumentFormatString },
								new Label() { Text = "To section in file:" },
								epubViewerOpenToSectionArgumentFormatTextBox = new TextBox() { Dock = DockStyle.Fill, Font = Fonts.monospace, Text = ProgramSettings.epubViewerOpenToSectionArgumentFormatString }
							)
						),
						new TabPage() { Text = "Word", Padding = tabPagePadding }.withControls(
							new TableLayoutPanel() { Dock = DockStyle.Fill, ColumnCount = 1, Padding = new Padding(2) }.withControls(
								new Label() { Text = "Open with:" },
								new Panel() { Dock = DockStyle.Fill, Height = 24 }.withControls(
									wordViewerProgramFilePathTextBox = new TextBox() { Dock = DockStyle.Fill, Font = Fonts.monospace, Text = ProgramSettings.wordViewerProgramFilePath },
									new Button() { Text = "...", Dock = DockStyle.Right }.withAction(() => { browseForProgramFilePathToUpdate(wordViewerProgramFilePathTextBox); })
								),
								new Label() { Text = "To file:" },
								wordViewerOpenArgumentFormatTextBox = new TextBox() { Dock = DockStyle.Fill, Font = Fonts.monospace, Text = ProgramSettings.wordViewerOpenArgumentFormatString }
							)
						),
						new TabPage() { Text = "PDF", Padding = tabPagePadding }.withControls(
							new TableLayoutPanel() { Dock = DockStyle.Fill, ColumnCount = 1, Padding = new Padding(2) }.withControls(
								new Label() { Text = "Open with:" },
								new Panel() { Dock = DockStyle.Fill, Height = 24 }.withControls(
									pdfViewerProgramFilePathTextBox = new TextBox() { Dock = DockStyle.Fill, Font = Fonts.monospace, Text = ProgramSettings.pdfViewerProgramFilePath },
									new Button() { Text = "...", Dock = DockStyle.Right }.withAction(() => { browseForProgramFilePathToUpdate(pdfViewerProgramFilePathTextBox); })
								),
								new Label() { Text = "To file:" },
								pdfViewerOpenArgumentFormatTextBox = new TextBox() { Dock = DockStyle.Fill, Font = Fonts.monospace, Text = ProgramSettings.pdfViewerOpenArgumentFormatString },
								new Label() { Text = "To section in file:" },
								pdfViewerOpenToPageArgumentFormatTextBox = new TextBox() { Dock = DockStyle.Fill, Font = Fonts.monospace, Text = ProgramSettings.pdfViewerOpenToPageArgumentFormatString }
							)
						)
					)
				),
				new GroupBox() { Text = "Open to:", Dock = DockStyle.Top, Height = 70, Padding = new Padding(16) }.withControls(
					startingDirectoryPathTextBox = new TextBox() { Dock = DockStyle.Fill, Font = Fonts.monospace, Text = ProgramSettings.startingDirectoryPath },
					new Button() { Text = "...", Dock = DockStyle.Right }.withAction(() => { browseForDirectoryPathToUpdate(startingDirectoryPathTextBox); })
				),
				new GroupBox() { Text = "Download to:", Dock = DockStyle.Top, Height = 70, Padding = new Padding(16) }.withControls(
					downloadsDirectoryPathTextBox = new TextBox() { Dock = DockStyle.Fill, Font = Fonts.monospace, Text = ProgramSettings.downloadsDirectoryPath },
					new Button() { Text = "...", Dock = DockStyle.Right }.withAction(() => { browseForDirectoryPathToUpdate(downloadsDirectoryPathTextBox); })
				),
				new Panel() { Dock = DockStyle.Bottom, Height = 30, Padding = new Padding(0, 4, 0, 0) }.withControls(
					new Button() { Text = "Done", Dock = DockStyle.Right, DialogResult = DialogResult.OK }
				)
			);

			epubViewerProgramFilePathTextBox.TextChanged += new EventHandler((s, a) => { ProgramSettings.epubViewerProgramFilePath = epubViewerProgramFilePathTextBox.Text; });
			epubViewerOpenArgumentFormatTextBox.TextChanged += new EventHandler((s, a) => { ProgramSettings.epubViewerOpenArgumentFormatString = epubViewerOpenArgumentFormatTextBox.Text; });
			epubViewerOpenToSectionArgumentFormatTextBox.TextChanged += new EventHandler((s, a) => { ProgramSettings.epubViewerOpenToSectionArgumentFormatString = epubViewerOpenToSectionArgumentFormatTextBox.Text; });
			wordViewerProgramFilePathTextBox.TextChanged += new EventHandler((s, a) => { ProgramSettings.wordViewerProgramFilePath = wordViewerProgramFilePathTextBox.Text; });
			wordViewerOpenArgumentFormatTextBox.TextChanged += new EventHandler((s, a) => { ProgramSettings.wordViewerOpenArgumentFormatString = wordViewerOpenArgumentFormatTextBox.Text; });
			pdfViewerProgramFilePathTextBox.TextChanged += new EventHandler((s, a) => { ProgramSettings.pdfViewerProgramFilePath = pdfViewerProgramFilePathTextBox.Text; });
			pdfViewerOpenArgumentFormatTextBox.TextChanged += new EventHandler((s, a) => { ProgramSettings.pdfViewerOpenArgumentFormatString = pdfViewerOpenArgumentFormatTextBox.Text; });
			pdfViewerOpenToPageArgumentFormatTextBox.TextChanged += new EventHandler((s, a) => { ProgramSettings.pdfViewerOpenToPageArgumentFormatString = pdfViewerOpenToPageArgumentFormatTextBox.Text; });
			startingDirectoryPathTextBox.TextChanged += new EventHandler((s, a) => { ProgramSettings.startingDirectoryPath = startingDirectoryPathTextBox.Text; });
			downloadsDirectoryPathTextBox.TextChanged += new EventHandler((s, a) => { ProgramSettings.downloadsDirectoryPath = downloadsDirectoryPathTextBox.Text; });

		}

		private void browseForProgramFilePathToUpdate(TextBox textBox) {
			try {
				using(OpenFileDialog openFileDialog = new OpenFileDialog() {
					Filter = "Executable files (*.exe)|*.exe|All files (*.*)|*.*",
					InitialDirectory = !String.IsNullOrWhiteSpace(textBox.Text) ?
						Path.GetDirectoryName(textBox.Text) ?? Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles) :
						Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles),
				}) {
					if(DialogResult.OK == openFileDialog.ShowDialog(this)) {
						textBox.Text = openFileDialog.FileName;
					}
				}
			}
			catch(Exception exception) {

				#if DEBUG
					Console.Error.WriteLine("Error: exception: {0}", exception);
				#endif

				MessageBoxes.error(exception.Message);

			}
		}

		private void browseForDirectoryPathToUpdate(TextBox textBox) {
			try {
				using(FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog() {
					SelectedPath = !String.IsNullOrWhiteSpace(textBox.Text) ?
						textBox.Text :
						Environment.GetFolderPath(Environment.SpecialFolder.UserProfile)
				}) {
					if(DialogResult.OK == folderBrowserDialog.ShowDialog(this)) {
						textBox.Text = folderBrowserDialog.SelectedPath;
					}
				}
			}
			catch(Exception exception) {

				#if DEBUG
					Console.Error.WriteLine("Error: exception: {0}", exception);
				#endif

				MessageBoxes.error(exception.Message);

			}
		}

	}

}
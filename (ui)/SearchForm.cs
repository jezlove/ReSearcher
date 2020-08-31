/*
	C# "SearchForm.cs"
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

    public sealed class SearchForm :
		Form {

		private readonly TextBox pathsTextBox;
		private readonly ComboBox filterComboBox;
		private readonly TabControl tabControl;
		private readonly TextBox xpathTextBox;
		private readonly TextBox simplePatternTextBox;
		private readonly TextBox booleanPatternTextBox;
		private readonly TextBox regexPatternTextBox;
		private readonly CheckBox regexCaseInsensitiveCheckBox;
		private readonly CheckBox regexMultilineCheckBox;
		private readonly CheckBox regexSinglelineCheckBox;
		private readonly CheckBox regexIgnorePatternWhitespaceCheckBox;

		private SearchForm() {
			Text = "Search...";
			FormBorderStyle = FormBorderStyle.SizableToolWindow;
			ShowInTaskbar = false;
			Size = new Size(700, 600);
			StartPosition = FormStartPosition.CenterParent;
			Padding = new Padding(8);
			Padding tabPagePadding = new Padding(12);
			this.appendControls(
				new Panel() { Dock = DockStyle.Fill }.withControls(
					tabControl = new TabControl() { Dock = DockStyle.Fill, Padding = new Point(24, 4) }.withControls(
						new TabPage() { Text = "Simple", Padding = tabPagePadding }.withControls(
							simplePatternTextBox = new TextBox() { Dock = DockStyle.Fill, Font = Fonts.monospace, Multiline = true, AcceptsReturn = true, ScrollBars = ScrollBars.Vertical }
						),
						new TabPage() { Text = "Boolean", Padding = tabPagePadding }.withControls(
							booleanPatternTextBox = new TextBox() { Dock = DockStyle.Fill, Font = Fonts.monospace, Multiline = true, AcceptsReturn = true, ScrollBars = ScrollBars.Vertical }
						),
						new TabPage() { Text = "Regex", Padding = tabPagePadding }.withControls(
							regexPatternTextBox = new TextBox() { Dock = DockStyle.Fill, Font = Fonts.monospace, Multiline = true, WordWrap = false, AcceptsReturn = true, ScrollBars = ScrollBars.Both },
							new FlowLayoutPanel() { Dock = DockStyle.Bottom, Height = 30, FlowDirection = FlowDirection.RightToLeft }.withControls(
								regexIgnorePatternWhitespaceCheckBox = new CheckBox() { Text = "'x' - ignore whitespace", AutoSize = true, Checked = true },
								regexMultilineCheckBox = new CheckBox() { Text = "'m' - multiline", AutoSize = true },
								regexSinglelineCheckBox = new CheckBox() { Text = "'s' - singleline", AutoSize = true },
								regexCaseInsensitiveCheckBox = new CheckBox() { Text = "'i' - ignore case", AutoSize = true, Checked = true },
								new Label() { Text = "Options:", TextAlign = ContentAlignment.MiddleRight, AutoSize = true }
							)
						)
					)
				),
				new GroupBox() { Text = "XPath (within Epub only):", Dock = DockStyle.Top, Height = 100, Padding = new Padding(16) }.withControls(
					xpathTextBox = new TextBox() { Multiline = true, WordWrap = false, AcceptsReturn = true, ScrollBars = ScrollBars.Both, Dock = DockStyle.Fill, Font = Fonts.monospace }
				),
				new GroupBox() { Text = "Path(s) and filter:", Dock = DockStyle.Top, Height = 200, Padding = new Padding(16) }.withControls(
					pathsTextBox = new TextBox() { Multiline = true, WordWrap = false, AcceptsReturn = true, ScrollBars = ScrollBars.Both, Dock = DockStyle.Fill, Font = Fonts.monospace },
					filterComboBox = new ComboBox() { Dock = DockStyle.Right, Width = 80, Font = Fonts.monospace }
				),
				new Panel() { Dock = DockStyle.Bottom, Height = 30, Padding = new Padding(0, 4, 0, 0) }.withControls(
					new Button() { Text = "Load...", Dock = DockStyle.Left }.withAction(loadPreviousSearch),
					new Button() { Text = "Search...", Dock = DockStyle.Right }.withAction(prepareSearch),
					new Button() { Text = "Cancel", DialogResult = DialogResult.Cancel, Dock = DockStyle.Right }
				)
			);
			filterComboBox.Items.AddRange(Filters.all);
			tabControl.SelectedIndex = 2;
			regexPatternTextBox.TabIndex = 0;
		}

		public IEnumerable<FileSystemInfo> fileSystemInfos {
			get {
				foreach(String path in pathsTextBox.Text.Split(new[] {"\r\n", "\r", "\n"}, StringSplitOptions.RemoveEmptyEntries)) {
					if(String.IsNullOrWhiteSpace(path)) {
						continue;
					}
					yield return(FileSystemInfos.presumeFor(path.Trim()));
				}
			}
			set {
				pathsTextBox.Text = String.Join(Environment.NewLine, value.Select(fsi => fsi.FullName));
			}
		}

		public String filter {
			get { return(filterComboBox.Text); }
			set { filterComboBox.Text = value; }
		}

		public String xpath {
			get { return(xpathTextBox.Text); }
			set { xpathTextBox.Text = value; }
		}

		public SearchType searchType {
			get { return((SearchType)(tabControl.SelectedIndex)); }
			set { tabControl.SelectedIndex = (int)(value); }
		}

		private String pattern {
			get {
				if(SearchType.regex == searchType) { return(regexPattern); }
				if(SearchType.boolean == searchType) { return(booleanPattern); }
				if(SearchType.simple == searchType) { return(simplePattern); }
				return(null);
			}
			set {
				if(SearchType.regex == searchType) {
					regexPattern = value;
					return;
				}
				if(SearchType.boolean == searchType) {
					booleanPattern = value;
					return;
				}
				if(SearchType.simple == searchType) {
					simplePattern = value;
					return;
				}
			}
		}

		public String simplePattern {
			get { return(simplePatternTextBox.Text); }
			set { simplePatternTextBox.Text = value; }
		}

		public String booleanPattern {
			get { return(booleanPatternTextBox.Text); }
			set { booleanPatternTextBox.Text = value; }
		}

		public String regexPattern {
			get { return(regexPatternTextBox.Text); }
			set { regexPatternTextBox.Text = value; }
		}

		public RegexOptions regexOptions {
			get {
				RegexOptions value = default(RegexOptions);
				if(regexCaseInsensitiveCheckBox.Checked) {
					value |= RegexOptions.IgnoreCase;
				}
				if(regexMultilineCheckBox.Checked) {
					value |= RegexOptions.Multiline;
				}
				if(regexSinglelineCheckBox.Checked) {
					value |= RegexOptions.Singleline;
				}
				if(regexIgnorePatternWhitespaceCheckBox.Checked) {
					value |= RegexOptions.IgnorePatternWhitespace;
				}
				return(value);
			}
			set {
				regexCaseInsensitiveCheckBox.Checked = value.HasFlag(RegexOptions.IgnoreCase);
				regexMultilineCheckBox.Checked = value.HasFlag(RegexOptions.Multiline);
				regexSinglelineCheckBox.Checked = value.HasFlag(RegexOptions.Singleline);
				regexIgnorePatternWhitespaceCheckBox.Checked = value.HasFlag(RegexOptions.IgnorePatternWhitespace);
			}
		}

		private XPathExpression compiledXpath;

		private Regex compiledRegex;

		public void prepareSearch() {
			if(String.IsNullOrWhiteSpace(pathsTextBox.Text)) {
				this.warn("No paths specified");
				return;
			}
			if(String.IsNullOrWhiteSpace(xpath)) {
				compiledXpath = null;
			}
			else {
				if(null == (compiledXpath = tryCompileXpath(xpath))) {
					return;
				}
			}
			if(null == (compiledRegex = tryCompileRegex())) {
				return;
			}
			DialogResult = DialogResult.OK;
			Close();
		}

		private XPathExpression tryCompileXpath(String xpath) {
			try {
				return(XPathExpression.Compile(xpath));
			}
			catch(XPathException xpathException) {
				this.warn(String.Format("Could not compile expression: {0}, due to exception: {1}", xpath, xpathException.Message));
				return(null);
			}
		}

		private Regex tryCompileRegex(String regexPattern, RegexOptions regexOptions = default(RegexOptions)) {
			try {
				return(new Regex(regexPattern, RegexOptions.Compiled | regexOptions));
			}
			catch(Exception exception) {
				Console.Error.WriteLine("Error: exception: {0}", exception);
				this.warn(String.Format("Could not compile expression: {0}, due to exception: {1}", regexPattern, exception.Message));
				return(null);
			}
		}

		private Regex tryCompileRegex() {
			if(SearchType.regex == searchType) {
				if(String.Empty == regexPattern) {
					this.warn("Regex is empty");
					return(null);
				}
				return(tryCompileRegex(regexPattern, regexOptions));
			}
			if(SearchType.boolean == searchType) {
				if(String.IsNullOrWhiteSpace(booleanPattern)) {
					this.warn("Boolean expression is empty");
					return(null);
				}
				String convertedRegexPattern = BooleanExpressions.combined(BooleanExpressions.bifurcated(SimpleExpressions.tokenised(booleanPattern)));
				return(tryCompileRegex(convertedRegexPattern, RegexOptions.IgnoreCase | RegexOptions.Singleline));
			}
			if(SearchType.simple == searchType) {
				if(String.IsNullOrWhiteSpace(simplePattern)) {
					this.warn("Simple expression is empty");
					return(null);
				}
				String convertedRegexPattern = SimpleExpressions.joined(SimpleExpressions.tokenised(simplePattern));
				return(tryCompileRegex(convertedRegexPattern, RegexOptions.IgnoreCase | RegexOptions.Singleline));
			}
			return(null);
		}

		public void loadPreviousSearch() {
			SearchDetails searchDetails = PriorSearchDetailsRetriever.retrieve(ownerWindow: this);
			if(null != searchDetails) {
				loadSearchDetails(searchDetails);
			}
		}

		public void loadSearchDetails(SearchDetails searchDetails) {
			fileSystemInfos = searchDetails.paths.Select(path => FileSystemInfos.presumeFor(path));
			filter = searchDetails.filter;
			xpath = searchDetails.xpath;
			searchType = searchDetails.searchType;
			pattern = searchDetails.pattern;
			regexOptions = searchDetails.regexOptions;
		}

		public SearchCriteria createSearchCriteria() {
			return(
				new SearchCriteria() {
					fileSystemInfos = fileSystemInfos,
					filter = filter,
					xpath = compiledXpath,
					searchType = searchType,
					regex = compiledRegex
				}
			);
		}

		public SearchDetails createSearchDetails() {
			return(
				new SearchDetails() {
					paths = fileSystemInfos.Select(fsi => fsi.FullName).ToArray(),
					filter = filter,
					xpath = xpath,
					searchType = searchType,
					pattern = pattern,
					regexOptions = regexOptions					
				}
			);
		}

		public static Boolean parameterise(out SearchCriteria searchCriteria, out SearchDetails searchDetails, IEnumerable<FileSystemInfo> fileSystemInfos, String filter, IWin32Window ownerWindow) {
			using(SearchForm searchForm = new SearchForm() { fileSystemInfos = fileSystemInfos, filter = filter }) {
				if(DialogResult.OK != searchForm.ShowDialog(ownerWindow)) {
					searchCriteria = null;
					searchDetails = null;
					return(false);
				}
				else {
					searchCriteria = searchForm.createSearchCriteria();
					searchDetails = searchForm.createSearchDetails();
					return(true);
				}
			}
		}

	}

}
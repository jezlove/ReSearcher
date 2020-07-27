/*
	C# "OuStudentModule.cs"
	by Jeremy Love <mailto:jez@jezlove.uk>
	copyright 2020
*/

using System;
using System.Text.RegularExpressions;

namespace ReSearcher.Ou {

	public sealed class OuStudentModule {

		public String moduleCode { get; private set; }

		public String presentation { get; private set; }

		public String name {
			get {
				// e.g. TM470-2020B
				return(String.Format("{0}-{1}", moduleCode, presentation));
			}
		}

		public String shortName {
			get {
				// e.g. TM470-20B
				return(String.Format("{0}-{1}", moduleCode, shortPresentationRegex.Match(presentation).Value));
			}
		}

		public String title { get; private set; }

		public int type { get; private set; }

		public OuStudentModule(String moduleCode, String presentation, String title, int type) {
			this.moduleCode = moduleCode;
			this.presentation = presentation;
			this.title = title;
			this.type = type;
		}

		public override String ToString() {
			return(String.Format("{0} ({1})", name, title));
		}

		private static readonly Regex shortPresentationRegex = new Regex(@"\d\d[A-Za-z]$", RegexOptions.Compiled);

	}

}
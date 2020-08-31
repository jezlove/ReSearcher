/*
	C# "Program.cs"
	by Jeremy Love <mailto:jez@jezlove.uk>
	copyright 2020
*/

using System;
using System.Windows.Forms;
using System.Diagnostics;

namespace ReSearcher {

    public static partial class Program {

		[STAThread]
		public static int Main(String[] arguments) {
			try {

				#if DEBUG
					ConsoleTraceListener consoleTraceListener = new ConsoleTraceListener(true);
					Debug.Listeners.Add(consoleTraceListener);
				#endif

				Application.EnableVisualStyles();

//Experiments.saveResourceList();
//Experiments.listCollisions();
//Experiments.listExtensions();
//Experiments.listNotExtensioned();
//Experiments.mockDownloadAll();
//Experiments.listNotDownloaded();
//Experiments.mockDownloadAllNotDownloaded();
//Experiments.listNotDownloaded();
//Experiments.downloadAllNotDownloaded();
//Experiments.findDownloadsThatAreMarkup();


/**/
				using(MainForm mainForm = new MainForm()) {
					Application.Run(mainForm);
				}
//*/

				#if DEBUG
					Debug.Listeners.Remove(consoleTraceListener);
				#endif

			}
			catch(Exception exception) {

				#if DEBUG
					Console.Error.WriteLine("Error: exception: {0}", exception);
				#endif

				MessageBoxes.error(exception.Message);

				return(1);
			}
			return(0);
		}

	}

}
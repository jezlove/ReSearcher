/*
	C# "BackgroundWorkerExtensions.cs"
	by Jeremy Love <mailto:jez@jezlove.uk>
	copyright 2020
*/

using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace ReSearcher {

    public static partial class BackgroundWorkerExtensions {

		#region cancellation-support

			public static TBackgroundWorker withCancellationSupport<TBackgroundWorker>(this TBackgroundWorker thisBackgroundWorker, Boolean cancellationSupport = true) where TBackgroundWorker : BackgroundWorker {
				thisBackgroundWorker.WorkerSupportsCancellation = cancellationSupport;
				return(thisBackgroundWorker);
			}

		#endregion

		#region progress-reports

			public static TBackgroundWorker withProgressReports<TBackgroundWorker>(this TBackgroundWorker thisBackgroundWorker, Boolean progressReports = true) where TBackgroundWorker : BackgroundWorker {
				thisBackgroundWorker.WorkerReportsProgress = progressReports;
				return(thisBackgroundWorker);
			}

			public static TBackgroundWorker withProgressReports<TBackgroundWorker>(this TBackgroundWorker thisBackgroundWorker, ProgressChangedEventHandler progressChangedEventHandler) where TBackgroundWorker : BackgroundWorker {
				thisBackgroundWorker.WorkerReportsProgress = true;
				thisBackgroundWorker.ProgressChanged += progressChangedEventHandler;
				return(thisBackgroundWorker);
			}

			public static TBackgroundWorker withProgressReportsTo<TBackgroundWorker>(this TBackgroundWorker thisBackgroundWorker, ProgressBar progressBar) where TBackgroundWorker : BackgroundWorker {
				return(
					thisBackgroundWorker.withProgressReports(
						(Object sender, ProgressChangedEventArgs progressChangedEventArgs) => {
							progressBar.Value = progressChangedEventArgs.ProgressPercentage;
						}
					)
				);
			}

			public static TBackgroundWorker withProgressReportsTo<TBackgroundWorker>(this TBackgroundWorker thisBackgroundWorker, ToolStripProgressBar toolStripProgressBar) where TBackgroundWorker : BackgroundWorker {
				return(
					thisBackgroundWorker.withProgressReports(
						(Object sender, ProgressChangedEventArgs progressChangedEventArgs) => {
							toolStripProgressBar.Value = progressChangedEventArgs.ProgressPercentage;
						}
					)
				);
			}

			public static TBackgroundWorker withProgressReportsAsTextTo<TBackgroundWorker>(this TBackgroundWorker thisBackgroundWorker, Control control, String formatString = "{0}%") where TBackgroundWorker : BackgroundWorker {
				return(
					thisBackgroundWorker.withProgressReports(
						(Object sender, ProgressChangedEventArgs progressChangedEventArgs) => {
							control.Text = String.Format(formatString, progressChangedEventArgs.ProgressPercentage);
						}
					)
				);
			}

			public static TBackgroundWorker withProgressReportsTo<TBackgroundWorker>(this TBackgroundWorker thisBackgroundWorker, Label label, String formatString = "{0}%") where TBackgroundWorker : BackgroundWorker {
				return(thisBackgroundWorker.withProgressReportsAsTextTo(label, formatString));
			}

			public static TBackgroundWorker withProgressReportsTo<TBackgroundWorker>(this TBackgroundWorker thisBackgroundWorker, ToolStripLabel toolStripLabel, String formatString = "{0}%") where TBackgroundWorker : BackgroundWorker {
				return(
					thisBackgroundWorker.withProgressReports(
						(Object sender, ProgressChangedEventArgs progressChangedEventArgs) => {
							toolStripLabel.Text = String.Format(formatString, progressChangedEventArgs.ProgressPercentage);
						}
					)
				);
			}

		#endregion

		#region work

			public static TBackgroundWorker withWork<TBackgroundWorker>(this TBackgroundWorker thisBackgroundWorker, DoWorkEventHandler doWorkEventHandler) where TBackgroundWorker : BackgroundWorker {
				thisBackgroundWorker.DoWork += doWorkEventHandler;
				return(thisBackgroundWorker);
			}

			public static TBackgroundWorker withWork<TArgument, TResult, TBackgroundWorker>(this TBackgroundWorker thisBackgroundWorker, Func<TArgument, BackgroundWorker, DoWorkEventArgs, TResult> function) where TBackgroundWorker : BackgroundWorker {
				thisBackgroundWorker.DoWork += new DoWorkEventHandler(
					(Object sender, DoWorkEventArgs doWorkEventArgs) => {
						doWorkEventArgs.Result = function((TArgument)(doWorkEventArgs.Argument), sender as BackgroundWorker, doWorkEventArgs);
					}
				);
				return(thisBackgroundWorker);
			}

		#endregion

		#region completion

			public static TBackgroundWorker whenCompleted<TBackgroundWorker>(this TBackgroundWorker thisBackgroundWorker, RunWorkerCompletedEventHandler runWorkerCompletedEventHandler) where TBackgroundWorker : BackgroundWorker {
				thisBackgroundWorker.RunWorkerCompleted += runWorkerCompletedEventHandler;
				return(thisBackgroundWorker);
			}

			public static TBackgroundWorker whenCompleted<TBackgroundWorker>(this TBackgroundWorker thisBackgroundWorker, Action action) where TBackgroundWorker : BackgroundWorker {
				thisBackgroundWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(
					(Object sender, RunWorkerCompletedEventArgs runWorkerCompletedEventArgs) => {
						action();
					}
				);
				return(thisBackgroundWorker);
			}

			public static TBackgroundWorker whenCompleted<TResult, TBackgroundWorker>(this TBackgroundWorker thisBackgroundWorker, Action<TResult> action) where TBackgroundWorker : BackgroundWorker {
				thisBackgroundWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(
					(Object sender, RunWorkerCompletedEventArgs runWorkerCompletedEventArgs) => {
						action((TResult)(runWorkerCompletedEventArgs.Result));
					}
				);
				return(thisBackgroundWorker);
			}

		#endregion

	}

}
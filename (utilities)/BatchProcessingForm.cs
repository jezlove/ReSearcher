/*
	C# "BatchProcessingForm.cs"
	by Jeremy Love <mailto:jez@jezlove.uk>
	copyright 2020
*/

using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace ReSearcher {

    public class BatchProcessingForm<TItem> :
		Form {

		protected TextBox logTextBox { get; private set; }
		protected TextBoxWriter logTextBoxWriter { get; private set; }
		protected ProgressBar progressBar { get; private set; }
		protected Label progressLabel { get; private set; }
		private BackgroundWorker backgroundWorker;
		private IList<TItem> batch;
		private BatchItemProcessor<TItem> batchItemProcessor;

		private BatchProcessingForm(IList<TItem> batch, BatchItemProcessor<TItem> batchItemProcessor, String titleText, String progressTextStatusPattern) {

			this.batch = batch;
			this.batchItemProcessor = batchItemProcessor;

			Text = titleText;
			StartPosition = FormStartPosition.CenterParent;
			ClientSize = new Size(600, 500);
			FormBorderStyle = FormBorderStyle.FixedDialog;
			ShowInTaskbar = false;

			// assemble UI components
			// extension methods are in use here to provide a fluent and elegant component hierarchy
			this.appendControls(
				logTextBox = new TextBox() { ReadOnly = true, Multiline = true, ScrollBars = ScrollBars.Both, WordWrap = false, Dock = DockStyle.Fill },
				progressBar = new ProgressBar() { Value = 0, Step = 1, Dock = DockStyle.Bottom },
				progressLabel = new Label() { TextAlign = ContentAlignment.MiddleCenter, Dock = DockStyle.Bottom, Text = String.Format(progressTextStatusPattern, 0) },
				new Panel() { Dock = DockStyle.Bottom, Height = 30, Padding = new Padding(4) }.withControls(
					new Button() { Text = "Stop", Dock = DockStyle.Right, TabIndex = 1 }.withAction(stop)
				)
			);

			logTextBoxWriter = new TextBoxWriter(logTextBox);

			backgroundWorker = new BackgroundWorker()
				.withCancellationSupport()
				.withProgressReportsTo(progressBar)
				.withProgressReportsTo(progressLabel, progressTextStatusPattern)
				.withWork<Object, Object, BackgroundWorker>(doWork)
				.whenCompleted(onBackgroundWorkerRunWorkerCompleted);
		}

		protected override void OnShown(EventArgs eventArgs) {
			base.OnShown(eventArgs);
			start();
		}

		public void start() {
			backgroundWorker.RunWorkerAsync(null);
		}

		public void stop() {
			Debug.WriteLine("Stop requested");
			backgroundWorker.CancelAsync();
		}

        protected void onBackgroundWorkerRunWorkerCompleted(Object sender, RunWorkerCompletedEventArgs runWorkerCompletedEventArgs) {
            if(null != runWorkerCompletedEventArgs.Error) {
                this.error(runWorkerCompletedEventArgs.Error.Message);
				DialogResult = DialogResult.Abort;
            }
			else {
				DialogResult = runWorkerCompletedEventArgs.Cancelled ? DialogResult.Cancel : DialogResult.OK;
			}
			Close();
        }

        protected Object doWork(Object argument, BackgroundWorker backgroundWorker, DoWorkEventArgs doWorkEventArgs) {
			for(int i = 0; i < batch.Count; i++) {
				if(backgroundWorker.CancellationPending) {
					doWorkEventArgs.Cancel = true;
					break;
				}
				else {
					int percentCompleted = batchItemProcessor(batch[i], logTextBoxWriter);
					if(percentCompleted < 0) {
						doWorkEventArgs.Cancel = true;
						break;
					}
					backgroundWorker.ReportProgress(percentCompleted);
				}
			}
			return(null);
		}

		public static DialogResult process(
			IList<TItem> batch,
			BatchItemProcessor<TItem> batchItemProcessor,
			String titleText,
			String progressTextStatusPattern,
			IWin32Window ownerWindow
		) {
			using(BatchProcessingForm<TItem> batchProcessingForm = new BatchProcessingForm<TItem>(batch, batchItemProcessor, titleText, progressTextStatusPattern)) {
				return(batchProcessingForm.ShowDialog(ownerWindow));
			}
		}

	}

}
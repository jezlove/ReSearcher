/*
	C# "IndefiniteProcessingForm.cs"
	by Jeremy Love <mailto:jez@jezlove.uk>
	copyright 2020
*/

using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace ReSearcher {

    public class IndefiniteProcessingForm :
		Form {

		protected TextBox logTextBox { get; private set; }
		protected TextBoxWriter logTextBoxWriter { get; private set; }
		protected ProgressBar progressBar { get; private set; }
		protected BackgroundWorker backgroundWorker { get; private set; }
		private IndefiniteProcessor indefiniteProcessor;

		private IndefiniteProcessingForm(IndefiniteProcessor indefiniteProcessor, String titleText) {

			this.indefiniteProcessor = indefiniteProcessor;

			Text = titleText;
			StartPosition = FormStartPosition.CenterParent;
			ClientSize = new Size(600, 500);
			FormBorderStyle = FormBorderStyle.FixedDialog;
			ShowInTaskbar = false;

			// assemble UI components
			// extension methods are in use here to provide a fluent and elegant component hierarchy
			this.appendControls(
				logTextBox = new TextBox() { ReadOnly = true, Multiline = true, ScrollBars = ScrollBars.Both, WordWrap = false, Dock = DockStyle.Fill },
				progressBar = new ProgressBar() { Step = 1, Dock = DockStyle.Bottom, Style = ProgressBarStyle.Marquee, MarqueeAnimationSpeed = 30 },
				new Panel() { Dock = DockStyle.Bottom, Height = 30, Padding = new Padding(4) }.withControls(
					new Button() { Text = "Stop", Dock = DockStyle.Right, TabIndex = 1 }.withAction(stop)
				)
			);

			logTextBoxWriter = new TextBoxWriter(logTextBox);

			backgroundWorker = new BackgroundWorker()
				.withCancellationSupport()
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
			for(;;) {
				if(backgroundWorker.CancellationPending) {
					doWorkEventArgs.Cancel = true;
					break;
				}
				else {
					if(!indefiniteProcessor(logTextBoxWriter)) {
						doWorkEventArgs.Cancel = true;
						break;
					}
				}
			}
			return(null);
		}

		public static DialogResult process(
			IndefiniteProcessor indefiniteProcessor,
			String titleText,
			IWin32Window ownerWindow
		) {
			using(IndefiniteProcessingForm indefiniteProcessingForm = new IndefiniteProcessingForm(indefiniteProcessor, titleText)) {
				return(indefiniteProcessingForm.ShowDialog(ownerWindow));
			}
		}

	}

}
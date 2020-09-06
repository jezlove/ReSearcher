/*
	C# "AbstractSignInForm.cs"
	by Jeremy Love <mailto:jez@jezlove.uk>
	copyright 2020
*/

using System;
using System.Drawing;
using System.Windows.Forms;

namespace ReSearcher {

    public abstract class AbstractSignInForm :
		Form {

		private readonly TextBox usernameTextBox;
		private readonly TextBox passwordTextBox;
		private readonly CheckBox rememberMeCheckBox;
		private readonly Label messageLabel;

		public String username {
			get { return(usernameTextBox.Text); }
			set { usernameTextBox.Text = value; }
		}

		public String password {
			get { return(passwordTextBox.Text); }
			set { passwordTextBox.Text = value; }
		}

		public Boolean rememberMeChecked {
			get { return(rememberMeCheckBox.Checked); }
			set { rememberMeCheckBox.Checked = value; }
		}

		protected AbstractSignInForm() {

			Text = "Sign in";
			ClientSize = new Size(350, 180);
			StartPosition = FormStartPosition.CenterParent;
			FormBorderStyle = FormBorderStyle.FixedDialog;
			ShowInTaskbar = false;
			MinimizeBox = false;
			MaximizeBox = false;

			this.appendControls(
				new TableLayoutPanel() { Dock = DockStyle.Fill, ColumnCount = 2, BackColor = Color.White, Padding = new Padding(8) }.withControls(
					new Label() { Text = "Username:", TextAlign = ContentAlignment.MiddleRight, Dock = DockStyle.Fill },
					usernameTextBox = new TextBox() { Multiline = false, WordWrap = false, MaxLength = 100, Dock = DockStyle.Fill },
					new Label() { Text = "Password:", TextAlign = ContentAlignment.MiddleRight, Dock = DockStyle.Fill },
					passwordTextBox = new TextBox() { Multiline = false, WordWrap = false, MaxLength = 100, PasswordChar = '*', Dock = DockStyle.Fill },
					rememberMeCheckBox = new CheckBox() { Text = "Remember me", Checked = true, Dock = DockStyle.Top, TabIndex = 3 }
				).withColumnStyles(
					new ColumnStyle() { SizeType = SizeType.Percent, Width = 30 },
					new ColumnStyle() { SizeType = SizeType.Percent, Width = 70 }
				).withCellPosition(rememberMeCheckBox, 2, 3),
				new Panel() { Dock = DockStyle.Bottom, Height = 30, Padding = new Padding(4) }.withControls(
					messageLabel = new Label() { Dock = DockStyle.Fill },
					new Button() { Text = "Sign in", Dock = DockStyle.Right, TabIndex = 1 }.withAction(doSignIn),
					new Button() { Text = "Cancel", DialogResult = DialogResult.Cancel, Dock = DockStyle.Right, TabIndex = 2 }
				)
			);
		}

		public void doSignIn() {
			try {
				messageLabel.ForeColor = Color.Black;
				messageLabel.Text = "Signing in...";
				Application.DoEvents();
				if(trySignIn()) {
					DialogResult = DialogResult.OK;
					Close();
				}
				messageLabel.ForeColor = Color.Red;
				messageLabel.Text = "Invalid username or password.";
			}
			catch(Exception exception) {
				Console.Error.WriteLine("Error: exception: {0}", exception);
				messageLabel.Text = "An error occured.";
			}
		}

		protected abstract Boolean trySignIn();

	}

}
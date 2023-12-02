namespace Iter.Desktop.Forms
{
    partial class LoginForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            submitBtn = new Button();
            label2 = new Label();
            label1 = new Label();
            usernameTxt = new TextBox();
            HeadingLbl = new Label();
            passwordTxt = new TextBox();
            usernameErrorLabel = new Label();
            passwordErrorLabel = new Label();
            label3 = new Label();
            SuspendLayout();
            // 
            // submitBtn
            // 
            submitBtn.BackColor = SystemColors.ButtonHighlight;
            submitBtn.Cursor = Cursors.Hand;
            submitBtn.Font = new Font("High Tower Text", 11F, FontStyle.Regular, GraphicsUnit.Point);
            submitBtn.ForeColor = Color.FromArgb(128, 128, 255);
            submitBtn.Location = new Point(268, 332);
            submitBtn.Name = "submitBtn";
            submitBtn.Size = new Size(125, 29);
            submitBtn.TabIndex = 22;
            submitBtn.Text = "Potvrdi";
            submitBtn.UseVisualStyleBackColor = false;
            submitBtn.Click += submitBtn_Click;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("High Tower Text", 12F, FontStyle.Regular, GraphicsUnit.Point);
            label2.ForeColor = Color.FromArgb(128, 128, 255);
            label2.Location = new Point(136, 249);
            label2.Name = "label2";
            label2.Size = new Size(77, 23);
            label2.TabIndex = 23;
            label2.Text = "Lozinka";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("High Tower Text", 12F, FontStyle.Regular, GraphicsUnit.Point);
            label1.ForeColor = Color.FromArgb(128, 128, 255);
            label1.Location = new Point(136, 163);
            label1.Name = "label1";
            label1.Size = new Size(135, 23);
            label1.TabIndex = 22;
            label1.Text = "Korisničko ime";
            // 
            // usernameTxt
            // 
            usernameTxt.AcceptsTab = true;
            usernameTxt.AllowDrop = true;
            usernameTxt.Font = new Font("High Tower Text", 11F, FontStyle.Regular, GraphicsUnit.Point);
            usernameTxt.Location = new Point(136, 189);
            usernameTxt.Name = "usernameTxt";
            usernameTxt.Size = new Size(257, 29);
            usernameTxt.TabIndex = 20;
            // 
            // HeadingLbl
            // 
            HeadingLbl.AutoSize = true;
            HeadingLbl.FlatStyle = FlatStyle.Flat;
            HeadingLbl.Font = new Font("Harrington", 30F, FontStyle.Regular, GraphicsUnit.Point);
            HeadingLbl.ForeColor = Color.FromArgb(128, 128, 255);
            HeadingLbl.Location = new Point(217, 77);
            HeadingLbl.Name = "HeadingLbl";
            HeadingLbl.Size = new Size(110, 59);
            HeadingLbl.TabIndex = 19;
            HeadingLbl.Text = "ITer";
            // 
            // passwordTxt
            // 
            passwordTxt.AcceptsTab = true;
            passwordTxt.AllowDrop = true;
            passwordTxt.Font = new Font("High Tower Text", 11F, FontStyle.Regular, GraphicsUnit.Point);
            passwordTxt.Location = new Point(136, 275);
            passwordTxt.Name = "passwordTxt";
            passwordTxt.PasswordChar = '*';
            passwordTxt.Size = new Size(257, 29);
            passwordTxt.TabIndex = 21;
            // 
            // usernameErrorLabel
            // 
            usernameErrorLabel.AutoSize = true;
            usernameErrorLabel.Font = new Font("High Tower Text", 9F, FontStyle.Regular, GraphicsUnit.Point);
            usernameErrorLabel.ForeColor = Color.FromArgb(192, 0, 0);
            usernameErrorLabel.Location = new Point(136, 221);
            usernameErrorLabel.Name = "usernameErrorLabel";
            usernameErrorLabel.Size = new Size(0, 18);
            usernameErrorLabel.TabIndex = 24;
            // 
            // passwordErrorLabel
            // 
            passwordErrorLabel.AutoSize = true;
            passwordErrorLabel.Font = new Font("High Tower Text", 9F, FontStyle.Regular, GraphicsUnit.Point);
            passwordErrorLabel.ForeColor = Color.FromArgb(192, 0, 0);
            passwordErrorLabel.Location = new Point(136, 307);
            passwordErrorLabel.Name = "passwordErrorLabel";
            passwordErrorLabel.Size = new Size(0, 18);
            passwordErrorLabel.TabIndex = 25;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("High Tower Text", 11F, FontStyle.Regular, GraphicsUnit.Point);
            label3.ForeColor = Color.FromArgb(128, 128, 255);
            label3.Location = new Point(136, 377);
            label3.Name = "label3";
            label3.Size = new Size(191, 22);
            label3.TabIndex = 23;
            label3.Text = "Zaboravljena lozinka?";
            // 
            // LoginForm
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.ButtonHighlight;
            ClientSize = new Size(529, 456);
            Controls.Add(passwordErrorLabel);
            Controls.Add(usernameErrorLabel);
            Controls.Add(passwordTxt);
            Controls.Add(label3);
            Controls.Add(submitBtn);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(usernameTxt);
            Controls.Add(HeadingLbl);
            ForeColor = SystemColors.ButtonHighlight;
            Name = "LoginForm";
            Text = "LoginForm";
            Load += LoginForm_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label3;
        private Button submitBtn;
        private Label label2;
        private Label label1;
        private TextBox usernameTxt;
        private Label HeadingLbl;
        private TextBox passwordTxt;
        private Label usernameErrorLabel;
        private Label passwordErrorLabel;
    }
}
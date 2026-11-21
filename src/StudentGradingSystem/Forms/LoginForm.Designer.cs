namespace StudentGradingSystem.Forms
{
    partial class LoginForm
    {
        /// <summary>Required designer variable.</summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>Clean up any resources being used.</summary>
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
            this.brandPanel = new System.Windows.Forms.Panel();
            this.lblVersion = new System.Windows.Forms.Label();
            this.lblFeatures = new System.Windows.Forms.Label();
            this.lblTagline = new System.Windows.Forms.Label();
            this.lblAppName = new System.Windows.Forms.Label();
            this.lblLogo = new System.Windows.Forms.Label();
            this.signInPanel = new System.Windows.Forms.Panel();
            this.lnkDemoAccounts = new System.Windows.Forms.LinkLabel();
            this.lnkDatabaseSettings = new System.Windows.Forms.LinkLabel();
            this.lblError = new System.Windows.Forms.Label();
            this.btnSignIn = new System.Windows.Forms.Button();
            this.lnkForgotPassword = new System.Windows.Forms.LinkLabel();
            this.chkRemember = new System.Windows.Forms.CheckBox();
            this.chkShowPassword = new System.Windows.Forms.CheckBox();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.lblPassword = new System.Windows.Forms.Label();
            this.txtUsername = new System.Windows.Forms.TextBox();
            this.lblUsername = new System.Windows.Forms.Label();
            this.lblSignInHint = new System.Windows.Forms.Label();
            this.lblWelcome = new System.Windows.Forms.Label();
            this.brandPanel.SuspendLayout();
            this.signInPanel.SuspendLayout();
            this.SuspendLayout();
            //
            // brandPanel
            //
            this.brandPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(37)))), ((int)(((byte)(99)))), ((int)(((byte)(235)))));
            this.brandPanel.Controls.Add(this.lblVersion);
            this.brandPanel.Controls.Add(this.lblFeatures);
            this.brandPanel.Controls.Add(this.lblTagline);
            this.brandPanel.Controls.Add(this.lblAppName);
            this.brandPanel.Controls.Add(this.lblLogo);
            this.brandPanel.Dock = System.Windows.Forms.DockStyle.Left;
            this.brandPanel.Location = new System.Drawing.Point(0, 0);
            this.brandPanel.Name = "brandPanel";
            this.brandPanel.Size = new System.Drawing.Size(340, 520);
            this.brandPanel.TabIndex = 0;
            this.brandPanel.Tag = "header";
            //
            // lblVersion
            //
            this.lblVersion.AutoSize = true;
            this.lblVersion.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.lblVersion.Location = new System.Drawing.Point(34, 482);
            this.lblVersion.Name = "lblVersion";
            this.lblVersion.Size = new System.Drawing.Size(40, 13);
            this.lblVersion.TabIndex = 4;
            this.lblVersion.Tag = "subtitle";
            this.lblVersion.Text = "v1.0.0";
            //
            // lblFeatures
            //
            this.lblFeatures.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblFeatures.Location = new System.Drawing.Point(34, 290);
            this.lblFeatures.Name = "lblFeatures";
            this.lblFeatures.Size = new System.Drawing.Size(280, 110);
            this.lblFeatures.TabIndex = 3;
            this.lblFeatures.Text = "•  Weighted grade calculation\r\n•  Instructor and student portals\r\n•  Marks entry with validation\r\n•  Salted, hashed passwords";
            //
            // lblTagline
            //
            this.lblTagline.Font = new System.Drawing.Font("Segoe UI", 10.5F);
            this.lblTagline.Location = new System.Drawing.Point(34, 218);
            this.lblTagline.Name = "lblTagline";
            this.lblTagline.Size = new System.Drawing.Size(280, 56);
            this.lblTagline.TabIndex = 2;
            this.lblTagline.Tag = "subtitle";
            this.lblTagline.Text = "Assessments, marks and final grades for instructors and students.";
            //
            // lblAppName
            //
            this.lblAppName.Font = new System.Drawing.Font("Segoe UI Semibold", 20F, System.Drawing.FontStyle.Bold);
            this.lblAppName.Location = new System.Drawing.Point(30, 140);
            this.lblAppName.Name = "lblAppName";
            this.lblAppName.Size = new System.Drawing.Size(290, 76);
            this.lblAppName.TabIndex = 1;
            this.lblAppName.Text = "Student Grading System";
            //
            // lblLogo
            //
            this.lblLogo.BackColor = System.Drawing.Color.White;
            this.lblLogo.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold);
            this.lblLogo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(37)))), ((int)(((byte)(99)))), ((int)(((byte)(235)))));
            this.lblLogo.Location = new System.Drawing.Point(36, 70);
            this.lblLogo.Name = "lblLogo";
            this.lblLogo.Size = new System.Drawing.Size(58, 58);
            this.lblLogo.TabIndex = 0;
            this.lblLogo.Tag = "logo";
            this.lblLogo.Text = "A+";
            this.lblLogo.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            //
            // signInPanel
            //
            this.signInPanel.BackColor = System.Drawing.Color.White;
            this.signInPanel.Controls.Add(this.lnkDemoAccounts);
            this.signInPanel.Controls.Add(this.lnkDatabaseSettings);
            this.signInPanel.Controls.Add(this.lblError);
            this.signInPanel.Controls.Add(this.btnSignIn);
            this.signInPanel.Controls.Add(this.lnkForgotPassword);
            this.signInPanel.Controls.Add(this.chkRemember);
            this.signInPanel.Controls.Add(this.chkShowPassword);
            this.signInPanel.Controls.Add(this.txtPassword);
            this.signInPanel.Controls.Add(this.lblPassword);
            this.signInPanel.Controls.Add(this.txtUsername);
            this.signInPanel.Controls.Add(this.lblUsername);
            this.signInPanel.Controls.Add(this.lblSignInHint);
            this.signInPanel.Controls.Add(this.lblWelcome);
            this.signInPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.signInPanel.Location = new System.Drawing.Point(340, 0);
            this.signInPanel.Name = "signInPanel";
            this.signInPanel.Size = new System.Drawing.Size(520, 520);
            this.signInPanel.TabIndex = 1;
            //
            // lnkDemoAccounts
            //
            this.lnkDemoAccounts.AutoSize = true;
            this.lnkDemoAccounts.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lnkDemoAccounts.LinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(107)))), ((int)(((byte)(114)))), ((int)(((byte)(128)))));
            this.lnkDemoAccounts.Location = new System.Drawing.Point(342, 480);
            this.lnkDemoAccounts.Name = "lnkDemoAccounts";
            this.lnkDemoAccounts.Size = new System.Drawing.Size(91, 15);
            this.lnkDemoAccounts.TabIndex = 11;
            this.lnkDemoAccounts.TabStop = true;
            this.lnkDemoAccounts.Text = "Demo accounts";
            this.lnkDemoAccounts.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkDemoAccounts_LinkClicked);
            //
            // lnkDatabaseSettings
            //
            this.lnkDatabaseSettings.AutoSize = true;
            this.lnkDatabaseSettings.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lnkDatabaseSettings.LinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(107)))), ((int)(((byte)(114)))), ((int)(((byte)(128)))));
            this.lnkDatabaseSettings.Location = new System.Drawing.Point(84, 480);
            this.lnkDatabaseSettings.Name = "lnkDatabaseSettings";
            this.lnkDatabaseSettings.Size = new System.Drawing.Size(100, 15);
            this.lnkDatabaseSettings.TabIndex = 10;
            this.lnkDatabaseSettings.TabStop = true;
            this.lnkDatabaseSettings.Text = "Database settings";
            this.lnkDatabaseSettings.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkDatabaseSettings_LinkClicked);
            //
            // lblError
            //
            this.lblError.Font = new System.Drawing.Font("Segoe UI", 9.75F);
            this.lblError.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(185)))), ((int)(((byte)(28)))), ((int)(((byte)(28)))));
            this.lblError.Location = new System.Drawing.Point(84, 395);
            this.lblError.Name = "lblError";
            this.lblError.Size = new System.Drawing.Size(352, 60);
            this.lblError.TabIndex = 9;
            //
            // btnSignIn
            //
            this.btnSignIn.Location = new System.Drawing.Point(84, 340);
            this.btnSignIn.Name = "btnSignIn";
            this.btnSignIn.Size = new System.Drawing.Size(352, 44);
            this.btnSignIn.TabIndex = 8;
            this.btnSignIn.Tag = "primary";
            this.btnSignIn.Text = "Sign in";
            this.btnSignIn.UseVisualStyleBackColor = true;
            this.btnSignIn.Click += new System.EventHandler(this.btnSignIn_Click);
            //
            // lnkForgotPassword
            //
            this.lnkForgotPassword.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lnkForgotPassword.LinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(37)))), ((int)(((byte)(99)))), ((int)(((byte)(235)))));
            this.lnkForgotPassword.Location = new System.Drawing.Point(296, 302);
            this.lnkForgotPassword.Name = "lnkForgotPassword";
            this.lnkForgotPassword.Size = new System.Drawing.Size(140, 20);
            this.lnkForgotPassword.TabIndex = 7;
            this.lnkForgotPassword.TabStop = true;
            this.lnkForgotPassword.Text = "Forgot password?";
            this.lnkForgotPassword.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.lnkForgotPassword.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkForgotPassword_LinkClicked);
            //
            // chkRemember
            //
            this.chkRemember.AutoSize = true;
            this.chkRemember.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.chkRemember.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(55)))), ((int)(((byte)(65)))), ((int)(((byte)(81)))));
            this.chkRemember.Location = new System.Drawing.Point(86, 303);
            this.chkRemember.Name = "chkRemember";
            this.chkRemember.Size = new System.Drawing.Size(155, 19);
            this.chkRemember.TabIndex = 6;
            this.chkRemember.Text = "Remember my username";
            this.chkRemember.UseVisualStyleBackColor = true;
            //
            // chkShowPassword
            //
            this.chkShowPassword.AutoSize = true;
            this.chkShowPassword.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.chkShowPassword.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(107)))), ((int)(((byte)(114)))), ((int)(((byte)(128)))));
            this.chkShowPassword.Location = new System.Drawing.Point(380, 232);
            this.chkShowPassword.Name = "chkShowPassword";
            this.chkShowPassword.Size = new System.Drawing.Size(55, 19);
            this.chkShowPassword.TabIndex = 4;
            this.chkShowPassword.TabStop = false;
            this.chkShowPassword.Text = "Show";
            this.chkShowPassword.UseVisualStyleBackColor = true;
            this.chkShowPassword.CheckedChanged += new System.EventHandler(this.chkShowPassword_CheckedChanged);
            //
            // txtPassword
            //
            this.txtPassword.Font = new System.Drawing.Font("Segoe UI", 11.25F);
            this.txtPassword.Location = new System.Drawing.Point(84, 255);
            this.txtPassword.MaxLength = 128;
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.Size = new System.Drawing.Size(352, 27);
            this.txtPassword.TabIndex = 5;
            this.txtPassword.UseSystemPasswordChar = true;
            this.txtPassword.TextChanged += new System.EventHandler(this.Credentials_TextChanged);
            //
            // lblPassword
            //
            this.lblPassword.AutoSize = true;
            this.lblPassword.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold);
            this.lblPassword.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(55)))), ((int)(((byte)(65)))), ((int)(((byte)(81)))));
            this.lblPassword.Location = new System.Drawing.Point(82, 232);
            this.lblPassword.Name = "lblPassword";
            this.lblPassword.Size = new System.Drawing.Size(64, 17);
            this.lblPassword.TabIndex = 3;
            this.lblPassword.Text = "Password";
            //
            // txtUsername
            //
            this.txtUsername.Font = new System.Drawing.Font("Segoe UI", 11.25F);
            this.txtUsername.Location = new System.Drawing.Point(84, 186);
            this.txtUsername.MaxLength = 50;
            this.txtUsername.Name = "txtUsername";
            this.txtUsername.Size = new System.Drawing.Size(352, 27);
            this.txtUsername.TabIndex = 2;
            this.txtUsername.TextChanged += new System.EventHandler(this.Credentials_TextChanged);
            //
            // lblUsername
            //
            this.lblUsername.AutoSize = true;
            this.lblUsername.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold);
            this.lblUsername.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(55)))), ((int)(((byte)(65)))), ((int)(((byte)(81)))));
            this.lblUsername.Location = new System.Drawing.Point(82, 163);
            this.lblUsername.Name = "lblUsername";
            this.lblUsername.Size = new System.Drawing.Size(69, 17);
            this.lblUsername.TabIndex = 1;
            this.lblUsername.Text = "Username";
            //
            // lblSignInHint
            //
            this.lblSignInHint.AutoSize = true;
            this.lblSignInHint.Font = new System.Drawing.Font("Segoe UI", 10.5F);
            this.lblSignInHint.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(107)))), ((int)(((byte)(114)))), ((int)(((byte)(128)))));
            this.lblSignInHint.Location = new System.Drawing.Point(82, 112);
            this.lblSignInHint.Name = "lblSignInHint";
            this.lblSignInHint.Size = new System.Drawing.Size(250, 19);
            this.lblSignInHint.TabIndex = 0;
            this.lblSignInHint.Text = "Sign in to continue to your dashboard.";
            //
            // lblWelcome
            //
            this.lblWelcome.AutoSize = true;
            this.lblWelcome.Font = new System.Drawing.Font("Segoe UI Semibold", 20.25F, System.Drawing.FontStyle.Bold);
            this.lblWelcome.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(17)))), ((int)(((byte)(24)))), ((int)(((byte)(39)))));
            this.lblWelcome.Location = new System.Drawing.Point(78, 68);
            this.lblWelcome.Name = "lblWelcome";
            this.lblWelcome.Size = new System.Drawing.Size(185, 37);
            this.lblWelcome.TabIndex = 12;
            this.lblWelcome.Text = "Welcome back";
            //
            // LoginForm
            //
            this.AcceptButton = this.btnSignIn;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(860, 520);
            this.Controls.Add(this.signInPanel);
            this.Controls.Add(this.brandPanel);
            this.Font = new System.Drawing.Font("Segoe UI", 9.75F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "LoginForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Student Grading System - Sign in";
            this.Load += new System.EventHandler(this.LoginForm_Load);
            this.brandPanel.ResumeLayout(false);
            this.brandPanel.PerformLayout();
            this.signInPanel.ResumeLayout(false);
            this.signInPanel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel brandPanel;
        private System.Windows.Forms.Label lblLogo;
        private System.Windows.Forms.Label lblAppName;
        private System.Windows.Forms.Label lblTagline;
        private System.Windows.Forms.Label lblFeatures;
        private System.Windows.Forms.Label lblVersion;
        private System.Windows.Forms.Panel signInPanel;
        private System.Windows.Forms.Label lblWelcome;
        private System.Windows.Forms.Label lblSignInHint;
        private System.Windows.Forms.Label lblUsername;
        private System.Windows.Forms.TextBox txtUsername;
        private System.Windows.Forms.Label lblPassword;
        private System.Windows.Forms.TextBox txtPassword;
        private System.Windows.Forms.CheckBox chkShowPassword;
        private System.Windows.Forms.CheckBox chkRemember;
        private System.Windows.Forms.LinkLabel lnkForgotPassword;
        private System.Windows.Forms.Button btnSignIn;
        private System.Windows.Forms.Label lblError;
        private System.Windows.Forms.LinkLabel lnkDatabaseSettings;
        private System.Windows.Forms.LinkLabel lnkDemoAccounts;
    }
}

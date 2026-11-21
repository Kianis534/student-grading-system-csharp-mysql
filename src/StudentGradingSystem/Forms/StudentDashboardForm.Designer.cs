namespace StudentGradingSystem.Forms
{
    partial class StudentDashboardForm
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
            System.Windows.Forms.DataGridViewCellStyle percentStyle = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle gradeStyle = new System.Windows.Forms.DataGridViewCellStyle();
            this.headerPanel = new System.Windows.Forms.Panel();
            this.lblSubtitle = new System.Windows.Forms.Label();
            this.lblTitle = new System.Windows.Forms.Label();
            this.footerPanel = new System.Windows.Forms.Panel();
            this.lblSummary = new System.Windows.Forms.Label();
            this.btnViewDetails = new System.Windows.Forms.Button();
            this.btnSignOut = new System.Windows.Forms.Button();
            this.contentPanel = new System.Windows.Forms.Panel();
            this.gridCourses = new System.Windows.Forms.DataGridView();
            this.colCode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colTitle = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSemester = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colPercentage = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colGrade = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.headerPanel.SuspendLayout();
            this.footerPanel.SuspendLayout();
            this.contentPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridCourses)).BeginInit();
            this.SuspendLayout();
            //
            // headerPanel
            //
            this.headerPanel.Controls.Add(this.lblSubtitle);
            this.headerPanel.Controls.Add(this.lblTitle);
            this.headerPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.headerPanel.Location = new System.Drawing.Point(0, 0);
            this.headerPanel.Name = "headerPanel";
            this.headerPanel.Size = new System.Drawing.Size(900, 76);
            this.headerPanel.TabIndex = 2;
            this.headerPanel.Tag = "header";
            //
            // lblSubtitle
            //
            this.lblSubtitle.AutoSize = true;
            this.lblSubtitle.Font = new System.Drawing.Font("Segoe UI", 9.75F);
            this.lblSubtitle.Location = new System.Drawing.Point(22, 44);
            this.lblSubtitle.Name = "lblSubtitle";
            this.lblSubtitle.Size = new System.Drawing.Size(390, 17);
            this.lblSubtitle.TabIndex = 1;
            this.lblSubtitle.Tag = "subtitle";
            this.lblSubtitle.Text = "Your courses and results  ·  Double-click a course to see your marks.";
            //
            // lblTitle
            //
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI Semibold", 15.75F, System.Drawing.FontStyle.Bold);
            this.lblTitle.Location = new System.Drawing.Point(19, 10);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(103, 30);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "Welcome";
            //
            // footerPanel
            //
            this.footerPanel.Controls.Add(this.lblSummary);
            this.footerPanel.Controls.Add(this.btnViewDetails);
            this.footerPanel.Controls.Add(this.btnSignOut);
            this.footerPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.footerPanel.Location = new System.Drawing.Point(0, 512);
            this.footerPanel.Name = "footerPanel";
            this.footerPanel.Padding = new System.Windows.Forms.Padding(20, 14, 20, 14);
            this.footerPanel.Size = new System.Drawing.Size(900, 68);
            this.footerPanel.TabIndex = 1;
            this.footerPanel.Tag = "footer";
            //
            // lblSummary
            //
            this.lblSummary.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblSummary.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(107)))), ((int)(((byte)(114)))), ((int)(((byte)(128)))));
            this.lblSummary.Location = new System.Drawing.Point(150, 14);
            this.lblSummary.Name = "lblSummary";
            this.lblSummary.Padding = new System.Windows.Forms.Padding(14, 0, 14, 0);
            this.lblSummary.Size = new System.Drawing.Size(570, 40);
            this.lblSummary.TabIndex = 2;
            this.lblSummary.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            //
            // btnViewDetails
            //
            this.btnViewDetails.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnViewDetails.Location = new System.Drawing.Point(720, 14);
            this.btnViewDetails.Name = "btnViewDetails";
            this.btnViewDetails.Size = new System.Drawing.Size(160, 40);
            this.btnViewDetails.TabIndex = 1;
            this.btnViewDetails.Tag = "primary";
            this.btnViewDetails.Text = "View marks";
            this.btnViewDetails.UseVisualStyleBackColor = true;
            this.btnViewDetails.Click += new System.EventHandler(this.btnViewDetails_Click);
            //
            // btnSignOut
            //
            this.btnSignOut.Dock = System.Windows.Forms.DockStyle.Left;
            this.btnSignOut.Location = new System.Drawing.Point(20, 14);
            this.btnSignOut.Name = "btnSignOut";
            this.btnSignOut.Size = new System.Drawing.Size(130, 40);
            this.btnSignOut.TabIndex = 0;
            this.btnSignOut.Text = "Sign out";
            this.btnSignOut.UseVisualStyleBackColor = true;
            this.btnSignOut.Click += new System.EventHandler(this.btnSignOut_Click);
            //
            // contentPanel
            //
            this.contentPanel.BackColor = System.Drawing.Color.White;
            this.contentPanel.Controls.Add(this.gridCourses);
            this.contentPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.contentPanel.Location = new System.Drawing.Point(0, 76);
            this.contentPanel.Name = "contentPanel";
            this.contentPanel.Padding = new System.Windows.Forms.Padding(20, 16, 20, 12);
            this.contentPanel.Size = new System.Drawing.Size(900, 436);
            this.contentPanel.TabIndex = 0;
            //
            // gridCourses
            //
            this.gridCourses.AllowUserToAddRows = false;
            this.gridCourses.AllowUserToDeleteRows = false;
            this.gridCourses.AllowUserToResizeRows = false;
            this.gridCourses.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colCode,
            this.colTitle,
            this.colSemester,
            this.colPercentage,
            this.colGrade});
            this.gridCourses.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridCourses.Location = new System.Drawing.Point(20, 16);
            this.gridCourses.MultiSelect = false;
            this.gridCourses.Name = "gridCourses";
            this.gridCourses.ReadOnly = true;
            this.gridCourses.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridCourses.Size = new System.Drawing.Size(860, 408);
            this.gridCourses.TabIndex = 0;
            this.gridCourses.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.gridCourses_CellDoubleClick);
            this.gridCourses.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.gridCourses_CellFormatting);
            this.gridCourses.SelectionChanged += new System.EventHandler(this.gridCourses_SelectionChanged);
            //
            // colCode
            //
            this.colCode.DataPropertyName = "Code";
            this.colCode.HeaderText = "Code";
            this.colCode.Name = "colCode";
            this.colCode.ReadOnly = true;
            this.colCode.Width = 90;
            //
            // colTitle
            //
            this.colTitle.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colTitle.DataPropertyName = "Title";
            this.colTitle.HeaderText = "Course";
            this.colTitle.MinimumWidth = 200;
            this.colTitle.Name = "colTitle";
            this.colTitle.ReadOnly = true;
            //
            // colSemester
            //
            this.colSemester.DataPropertyName = "Semester";
            this.colSemester.HeaderText = "Semester";
            this.colSemester.Name = "colSemester";
            this.colSemester.ReadOnly = true;
            this.colSemester.Width = 120;
            //
            // colPercentage
            //
            this.colPercentage.DataPropertyName = "Percentage";
            percentStyle.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            percentStyle.Format = "0.00\'%\'";
            percentStyle.NullValue = "-";
            this.colPercentage.DefaultCellStyle = percentStyle;
            this.colPercentage.HeaderText = "Percentage";
            this.colPercentage.Name = "colPercentage";
            this.colPercentage.ReadOnly = true;
            this.colPercentage.Width = 120;
            //
            // colGrade
            //
            this.colGrade.DataPropertyName = "FinalGrade";
            gradeStyle.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            gradeStyle.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold);
            gradeStyle.NullValue = "Not graded";
            this.colGrade.DefaultCellStyle = gradeStyle;
            this.colGrade.HeaderText = "Grade";
            this.colGrade.Name = "colGrade";
            this.colGrade.ReadOnly = true;
            this.colGrade.Width = 120;
            //
            // StudentDashboardForm
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(900, 580);
            this.Controls.Add(this.contentPanel);
            this.Controls.Add(this.footerPanel);
            this.Controls.Add(this.headerPanel);
            this.Font = new System.Drawing.Font("Segoe UI", 9.75F);
            this.MinimumSize = new System.Drawing.Size(760, 460);
            this.Name = "StudentDashboardForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Student Grading System - My courses";
            this.Load += new System.EventHandler(this.StudentDashboardForm_Load);
            this.headerPanel.ResumeLayout(false);
            this.headerPanel.PerformLayout();
            this.footerPanel.ResumeLayout(false);
            this.contentPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridCourses)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel headerPanel;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Label lblSubtitle;
        private System.Windows.Forms.Panel footerPanel;
        private System.Windows.Forms.Button btnSignOut;
        private System.Windows.Forms.Button btnViewDetails;
        private System.Windows.Forms.Label lblSummary;
        private System.Windows.Forms.Panel contentPanel;
        private System.Windows.Forms.DataGridView gridCourses;
        private System.Windows.Forms.DataGridViewTextBoxColumn colCode;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTitle;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSemester;
        private System.Windows.Forms.DataGridViewTextBoxColumn colPercentage;
        private System.Windows.Forms.DataGridViewTextBoxColumn colGrade;
    }
}

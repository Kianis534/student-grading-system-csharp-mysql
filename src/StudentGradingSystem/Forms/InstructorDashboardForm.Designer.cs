namespace StudentGradingSystem.Forms
{
    partial class InstructorDashboardForm
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
            System.Windows.Forms.DataGridViewCellStyle rightAligned = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle rightAligned2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle weightageStyle = new System.Windows.Forms.DataGridViewCellStyle();
            this.headerPanel = new System.Windows.Forms.Panel();
            this.lblSubtitle = new System.Windows.Forms.Label();
            this.lblTitle = new System.Windows.Forms.Label();
            this.footerPanel = new System.Windows.Forms.Panel();
            this.lblStatus = new System.Windows.Forms.Label();
            this.actionsPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.btnOpenCourse = new System.Windows.Forms.Button();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.btnSignOut = new System.Windows.Forms.Button();
            this.contentPanel = new System.Windows.Forms.Panel();
            this.gridCourses = new System.Windows.Forms.DataGridView();
            this.colCode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colTitle = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSemester = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colStudents = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colAssessments = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colWeightage = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.headerPanel.SuspendLayout();
            this.footerPanel.SuspendLayout();
            this.actionsPanel.SuspendLayout();
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
            this.headerPanel.Size = new System.Drawing.Size(980, 76);
            this.headerPanel.TabIndex = 2;
            this.headerPanel.Tag = "header";
            //
            // lblSubtitle
            //
            this.lblSubtitle.AutoSize = true;
            this.lblSubtitle.Font = new System.Drawing.Font("Segoe UI", 9.75F);
            this.lblSubtitle.Location = new System.Drawing.Point(22, 44);
            this.lblSubtitle.Name = "lblSubtitle";
            this.lblSubtitle.Size = new System.Drawing.Size(58, 17);
            this.lblSubtitle.TabIndex = 1;
            this.lblSubtitle.Tag = "subtitle";
            this.lblSubtitle.Text = "Subtitle";
            //
            // lblTitle
            //
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI Semibold", 15.75F, System.Drawing.FontStyle.Bold);
            this.lblTitle.Location = new System.Drawing.Point(19, 10);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(86, 30);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "Courses";
            //
            // footerPanel
            //
            this.footerPanel.Controls.Add(this.lblStatus);
            this.footerPanel.Controls.Add(this.actionsPanel);
            this.footerPanel.Controls.Add(this.btnSignOut);
            this.footerPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.footerPanel.Location = new System.Drawing.Point(0, 532);
            this.footerPanel.Name = "footerPanel";
            this.footerPanel.Padding = new System.Windows.Forms.Padding(20, 14, 20, 14);
            this.footerPanel.Size = new System.Drawing.Size(980, 68);
            this.footerPanel.TabIndex = 1;
            this.footerPanel.Tag = "footer";
            //
            // lblStatus
            //
            this.lblStatus.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblStatus.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(107)))), ((int)(((byte)(114)))), ((int)(((byte)(128)))));
            this.lblStatus.Location = new System.Drawing.Point(150, 14);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Padding = new System.Windows.Forms.Padding(14, 0, 0, 0);
            this.lblStatus.Size = new System.Drawing.Size(440, 40);
            this.lblStatus.TabIndex = 2;
            this.lblStatus.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            //
            // actionsPanel
            //
            this.actionsPanel.Controls.Add(this.btnOpenCourse);
            this.actionsPanel.Controls.Add(this.btnRefresh);
            this.actionsPanel.Dock = System.Windows.Forms.DockStyle.Right;
            this.actionsPanel.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft;
            this.actionsPanel.Location = new System.Drawing.Point(590, 14);
            this.actionsPanel.Name = "actionsPanel";
            this.actionsPanel.Size = new System.Drawing.Size(370, 40);
            this.actionsPanel.TabIndex = 1;
            this.actionsPanel.WrapContents = false;
            //
            // btnOpenCourse
            //
            this.btnOpenCourse.Location = new System.Drawing.Point(210, 0);
            this.btnOpenCourse.Margin = new System.Windows.Forms.Padding(10, 0, 0, 0);
            this.btnOpenCourse.Name = "btnOpenCourse";
            this.btnOpenCourse.Size = new System.Drawing.Size(160, 40);
            this.btnOpenCourse.TabIndex = 0;
            this.btnOpenCourse.Tag = "primary";
            this.btnOpenCourse.Text = "Open course";
            this.btnOpenCourse.UseVisualStyleBackColor = true;
            this.btnOpenCourse.Click += new System.EventHandler(this.btnOpenCourse_Click);
            //
            // btnRefresh
            //
            this.btnRefresh.Location = new System.Drawing.Point(80, 0);
            this.btnRefresh.Margin = new System.Windows.Forms.Padding(10, 0, 0, 0);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(120, 40);
            this.btnRefresh.TabIndex = 1;
            this.btnRefresh.Text = "Refresh";
            this.btnRefresh.UseVisualStyleBackColor = true;
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
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
            this.contentPanel.Size = new System.Drawing.Size(980, 456);
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
            this.colStudents,
            this.colAssessments,
            this.colWeightage});
            this.gridCourses.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridCourses.Location = new System.Drawing.Point(20, 16);
            this.gridCourses.MultiSelect = false;
            this.gridCourses.Name = "gridCourses";
            this.gridCourses.ReadOnly = true;
            this.gridCourses.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridCourses.Size = new System.Drawing.Size(940, 428);
            this.gridCourses.TabIndex = 0;
            this.gridCourses.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.gridCourses_CellDoubleClick);
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
            // colStudents
            //
            this.colStudents.DataPropertyName = "EnrolledStudents";
            rightAligned.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.colStudents.DefaultCellStyle = rightAligned;
            this.colStudents.HeaderText = "Students";
            this.colStudents.Name = "colStudents";
            this.colStudents.ReadOnly = true;
            this.colStudents.Width = 100;
            //
            // colAssessments
            //
            this.colAssessments.DataPropertyName = "ActivityCount";
            rightAligned2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.colAssessments.DefaultCellStyle = rightAligned2;
            this.colAssessments.HeaderText = "Assessments";
            this.colAssessments.Name = "colAssessments";
            this.colAssessments.ReadOnly = true;
            this.colAssessments.Width = 120;
            //
            // colWeightage
            //
            this.colWeightage.DataPropertyName = "AllocatedWeightage";
            weightageStyle.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            weightageStyle.Format = "0.##\'%\'";
            this.colWeightage.DefaultCellStyle = weightageStyle;
            this.colWeightage.HeaderText = "Weightage set";
            this.colWeightage.Name = "colWeightage";
            this.colWeightage.ReadOnly = true;
            this.colWeightage.Width = 130;
            //
            // InstructorDashboardForm
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(980, 600);
            this.Controls.Add(this.contentPanel);
            this.Controls.Add(this.footerPanel);
            this.Controls.Add(this.headerPanel);
            this.Font = new System.Drawing.Font("Segoe UI", 9.75F);
            this.MinimumSize = new System.Drawing.Size(820, 480);
            this.Name = "InstructorDashboardForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Student Grading System - Courses";
            this.Load += new System.EventHandler(this.InstructorDashboardForm_Load);
            this.headerPanel.ResumeLayout(false);
            this.headerPanel.PerformLayout();
            this.footerPanel.ResumeLayout(false);
            this.actionsPanel.ResumeLayout(false);
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
        private System.Windows.Forms.FlowLayoutPanel actionsPanel;
        private System.Windows.Forms.Button btnOpenCourse;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.Panel contentPanel;
        private System.Windows.Forms.DataGridView gridCourses;
        private System.Windows.Forms.DataGridViewTextBoxColumn colCode;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTitle;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSemester;
        private System.Windows.Forms.DataGridViewTextBoxColumn colStudents;
        private System.Windows.Forms.DataGridViewTextBoxColumn colAssessments;
        private System.Windows.Forms.DataGridViewTextBoxColumn colWeightage;
    }
}

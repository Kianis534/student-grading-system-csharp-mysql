namespace StudentGradingSystem.Forms
{
    partial class StudentCourseDetailsForm
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
            System.Windows.Forms.DataGridViewCellStyle obtainedStyle = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle totalStyle = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle weightStyle = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle contributionStyle = new System.Windows.Forms.DataGridViewCellStyle();
            this.headerPanel = new System.Windows.Forms.Panel();
            this.lblSubtitle = new System.Windows.Forms.Label();
            this.lblTitle = new System.Windows.Forms.Label();
            this.resultPanel = new System.Windows.Forms.Panel();
            this.lblResultDetail = new System.Windows.Forms.Label();
            this.lblResultHeadline = new System.Windows.Forms.Label();
            this.lblGradeCaption = new System.Windows.Forms.Label();
            this.lblGrade = new System.Windows.Forms.Label();
            this.footerPanel = new System.Windows.Forms.Panel();
            this.btnBack = new System.Windows.Forms.Button();
            this.contentPanel = new System.Windows.Forms.Panel();
            this.gridActivities = new System.Windows.Forms.DataGridView();
            this.colActivity = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colObtained = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colTotal = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colWeightage = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colContribution = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.headerPanel.SuspendLayout();
            this.resultPanel.SuspendLayout();
            this.footerPanel.SuspendLayout();
            this.contentPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridActivities)).BeginInit();
            this.SuspendLayout();
            //
            // headerPanel
            //
            this.headerPanel.Controls.Add(this.lblSubtitle);
            this.headerPanel.Controls.Add(this.lblTitle);
            this.headerPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.headerPanel.Location = new System.Drawing.Point(0, 0);
            this.headerPanel.Name = "headerPanel";
            this.headerPanel.Size = new System.Drawing.Size(860, 76);
            this.headerPanel.TabIndex = 3;
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
            this.lblTitle.Size = new System.Drawing.Size(77, 30);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "Course";
            //
            // resultPanel
            //
            this.resultPanel.BackColor = System.Drawing.Color.White;
            this.resultPanel.Controls.Add(this.lblResultDetail);
            this.resultPanel.Controls.Add(this.lblResultHeadline);
            this.resultPanel.Controls.Add(this.lblGradeCaption);
            this.resultPanel.Controls.Add(this.lblGrade);
            this.resultPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.resultPanel.Location = new System.Drawing.Point(0, 76);
            this.resultPanel.Name = "resultPanel";
            this.resultPanel.Size = new System.Drawing.Size(860, 96);
            this.resultPanel.TabIndex = 1;
            //
            // lblResultDetail
            //
            this.lblResultDetail.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblResultDetail.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(107)))), ((int)(((byte)(114)))), ((int)(((byte)(128)))));
            this.lblResultDetail.Location = new System.Drawing.Point(152, 50);
            this.lblResultDetail.Name = "lblResultDetail";
            this.lblResultDetail.Size = new System.Drawing.Size(688, 36);
            this.lblResultDetail.TabIndex = 3;
            this.lblResultDetail.Text = "Detail";
            //
            // lblResultHeadline
            //
            this.lblResultHeadline.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblResultHeadline.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold);
            this.lblResultHeadline.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(17)))), ((int)(((byte)(24)))), ((int)(((byte)(39)))));
            this.lblResultHeadline.Location = new System.Drawing.Point(150, 20);
            this.lblResultHeadline.Name = "lblResultHeadline";
            this.lblResultHeadline.Size = new System.Drawing.Size(690, 28);
            this.lblResultHeadline.TabIndex = 2;
            this.lblResultHeadline.Text = "Headline";
            //
            // lblGradeCaption
            //
            this.lblGradeCaption.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.lblGradeCaption.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(107)))), ((int)(((byte)(114)))), ((int)(((byte)(128)))));
            this.lblGradeCaption.Location = new System.Drawing.Point(20, 68);
            this.lblGradeCaption.Name = "lblGradeCaption";
            this.lblGradeCaption.Size = new System.Drawing.Size(110, 18);
            this.lblGradeCaption.TabIndex = 1;
            this.lblGradeCaption.Text = "FINAL GRADE";
            this.lblGradeCaption.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            //
            // lblGrade
            //
            this.lblGrade.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(219)))), ((int)(((byte)(234)))), ((int)(((byte)(254)))));
            this.lblGrade.Font = new System.Drawing.Font("Segoe UI", 24F, System.Drawing.FontStyle.Bold);
            this.lblGrade.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(29)))), ((int)(((byte)(78)))), ((int)(((byte)(216)))));
            this.lblGrade.Location = new System.Drawing.Point(20, 12);
            this.lblGrade.Name = "lblGrade";
            this.lblGrade.Size = new System.Drawing.Size(110, 54);
            this.lblGrade.TabIndex = 0;
            this.lblGrade.Text = "-";
            this.lblGrade.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            //
            // footerPanel
            //
            this.footerPanel.Controls.Add(this.btnBack);
            this.footerPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.footerPanel.Location = new System.Drawing.Point(0, 532);
            this.footerPanel.Name = "footerPanel";
            this.footerPanel.Padding = new System.Windows.Forms.Padding(20, 14, 20, 14);
            this.footerPanel.Size = new System.Drawing.Size(860, 68);
            this.footerPanel.TabIndex = 2;
            this.footerPanel.Tag = "footer";
            //
            // btnBack
            //
            this.btnBack.Dock = System.Windows.Forms.DockStyle.Left;
            this.btnBack.Location = new System.Drawing.Point(20, 14);
            this.btnBack.Name = "btnBack";
            this.btnBack.Size = new System.Drawing.Size(150, 40);
            this.btnBack.TabIndex = 0;
            this.btnBack.Text = "Back to courses";
            this.btnBack.UseVisualStyleBackColor = true;
            this.btnBack.Click += new System.EventHandler(this.btnBack_Click);
            //
            // contentPanel
            //
            this.contentPanel.BackColor = System.Drawing.Color.White;
            this.contentPanel.Controls.Add(this.gridActivities);
            this.contentPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.contentPanel.Location = new System.Drawing.Point(0, 172);
            this.contentPanel.Name = "contentPanel";
            this.contentPanel.Padding = new System.Windows.Forms.Padding(20, 8, 20, 12);
            this.contentPanel.Size = new System.Drawing.Size(860, 360);
            this.contentPanel.TabIndex = 0;
            //
            // gridActivities
            //
            this.gridActivities.AllowUserToAddRows = false;
            this.gridActivities.AllowUserToDeleteRows = false;
            this.gridActivities.AllowUserToResizeRows = false;
            this.gridActivities.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colActivity,
            this.colObtained,
            this.colTotal,
            this.colWeightage,
            this.colContribution});
            this.gridActivities.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridActivities.Location = new System.Drawing.Point(20, 8);
            this.gridActivities.MultiSelect = false;
            this.gridActivities.Name = "gridActivities";
            this.gridActivities.ReadOnly = true;
            this.gridActivities.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridActivities.Size = new System.Drawing.Size(820, 340);
            this.gridActivities.TabIndex = 0;
            //
            // colActivity
            //
            this.colActivity.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colActivity.DataPropertyName = "ActivityName";
            this.colActivity.HeaderText = "Assessment";
            this.colActivity.Name = "colActivity";
            this.colActivity.ReadOnly = true;
            //
            // colObtained
            //
            this.colObtained.DataPropertyName = "ObtainedMarks";
            obtainedStyle.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            obtainedStyle.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold);
            obtainedStyle.Format = "0.##";
            obtainedStyle.NullValue = "Not marked";
            this.colObtained.DefaultCellStyle = obtainedStyle;
            this.colObtained.HeaderText = "Obtained";
            this.colObtained.Name = "colObtained";
            this.colObtained.ReadOnly = true;
            this.colObtained.Width = 120;
            //
            // colTotal
            //
            this.colTotal.DataPropertyName = "TotalMarks";
            totalStyle.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            totalStyle.Format = "0.##";
            this.colTotal.DefaultCellStyle = totalStyle;
            this.colTotal.HeaderText = "Out of";
            this.colTotal.Name = "colTotal";
            this.colTotal.ReadOnly = true;
            this.colTotal.Width = 100;
            //
            // colWeightage
            //
            this.colWeightage.DataPropertyName = "Weightage";
            weightStyle.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            weightStyle.Format = "0.##\'%\'";
            this.colWeightage.DefaultCellStyle = weightStyle;
            this.colWeightage.HeaderText = "Weightage";
            this.colWeightage.Name = "colWeightage";
            this.colWeightage.ReadOnly = true;
            this.colWeightage.Width = 110;
            //
            // colContribution
            //
            this.colContribution.DataPropertyName = "Contribution";
            contributionStyle.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            contributionStyle.Format = "0.00\'%\'";
            this.colContribution.DefaultCellStyle = contributionStyle;
            this.colContribution.HeaderText = "Counts toward grade";
            this.colContribution.Name = "colContribution";
            this.colContribution.ReadOnly = true;
            this.colContribution.Width = 170;
            //
            // StudentCourseDetailsForm
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(860, 600);
            this.Controls.Add(this.contentPanel);
            this.Controls.Add(this.resultPanel);
            this.Controls.Add(this.footerPanel);
            this.Controls.Add(this.headerPanel);
            this.Font = new System.Drawing.Font("Segoe UI", 9.75F);
            this.MinimumSize = new System.Drawing.Size(740, 480);
            this.Name = "StudentCourseDetailsForm";
            this.Text = "Student Grading System - Course marks";
            this.Load += new System.EventHandler(this.StudentCourseDetailsForm_Load);
            this.headerPanel.ResumeLayout(false);
            this.headerPanel.PerformLayout();
            this.resultPanel.ResumeLayout(false);
            this.footerPanel.ResumeLayout(false);
            this.contentPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridActivities)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel headerPanel;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Label lblSubtitle;
        private System.Windows.Forms.Panel resultPanel;
        private System.Windows.Forms.Label lblGrade;
        private System.Windows.Forms.Label lblGradeCaption;
        private System.Windows.Forms.Label lblResultHeadline;
        private System.Windows.Forms.Label lblResultDetail;
        private System.Windows.Forms.Panel footerPanel;
        private System.Windows.Forms.Button btnBack;
        private System.Windows.Forms.Panel contentPanel;
        private System.Windows.Forms.DataGridView gridActivities;
        private System.Windows.Forms.DataGridViewTextBoxColumn colActivity;
        private System.Windows.Forms.DataGridViewTextBoxColumn colObtained;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTotal;
        private System.Windows.Forms.DataGridViewTextBoxColumn colWeightage;
        private System.Windows.Forms.DataGridViewTextBoxColumn colContribution;
    }
}

namespace StudentGradingSystem.Forms
{
    partial class CourseGradesForm
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
            System.Windows.Forms.DataGridViewCellStyle idStyle = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle percentStyle = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle gradeStyle = new System.Windows.Forms.DataGridViewCellStyle();
            this.headerPanel = new System.Windows.Forms.Panel();
            this.lblSubtitle = new System.Windows.Forms.Label();
            this.lblTitle = new System.Windows.Forms.Label();
            this.statsPanel = new System.Windows.Forms.Panel();
            this.lblStats = new System.Windows.Forms.Label();
            this.lblDistribution = new System.Windows.Forms.Label();
            this.footerPanel = new System.Windows.Forms.Panel();
            this.lblCalculated = new System.Windows.Forms.Label();
            this.btnExport = new System.Windows.Forms.Button();
            this.btnBack = new System.Windows.Forms.Button();
            this.contentPanel = new System.Windows.Forms.Panel();
            this.gridGrades = new System.Windows.Forms.DataGridView();
            this.colStudentId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colStudentName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colPercentage = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colGrade = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.headerPanel.SuspendLayout();
            this.statsPanel.SuspendLayout();
            this.footerPanel.SuspendLayout();
            this.contentPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridGrades)).BeginInit();
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
            this.lblTitle.Size = new System.Drawing.Size(124, 30);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "Final grades";
            //
            // statsPanel
            //
            this.statsPanel.BackColor = System.Drawing.Color.White;
            this.statsPanel.Controls.Add(this.lblDistribution);
            this.statsPanel.Controls.Add(this.lblStats);
            this.statsPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.statsPanel.Location = new System.Drawing.Point(0, 76);
            this.statsPanel.Name = "statsPanel";
            this.statsPanel.Padding = new System.Windows.Forms.Padding(20, 12, 20, 0);
            this.statsPanel.Size = new System.Drawing.Size(860, 70);
            this.statsPanel.TabIndex = 1;
            //
            // lblStats
            //
            this.lblStats.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblStats.Font = new System.Drawing.Font("Segoe UI Semibold", 11.25F, System.Drawing.FontStyle.Bold);
            this.lblStats.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(17)))), ((int)(((byte)(24)))), ((int)(((byte)(39)))));
            this.lblStats.Location = new System.Drawing.Point(20, 12);
            this.lblStats.Name = "lblStats";
            this.lblStats.Size = new System.Drawing.Size(820, 26);
            this.lblStats.TabIndex = 0;
            this.lblStats.Text = "Statistics";
            //
            // lblDistribution
            //
            this.lblDistribution.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblDistribution.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(107)))), ((int)(((byte)(114)))), ((int)(((byte)(128)))));
            this.lblDistribution.Location = new System.Drawing.Point(20, 38);
            this.lblDistribution.Name = "lblDistribution";
            this.lblDistribution.Size = new System.Drawing.Size(820, 24);
            this.lblDistribution.TabIndex = 1;
            this.lblDistribution.Text = "Distribution";
            //
            // footerPanel
            //
            this.footerPanel.Controls.Add(this.lblCalculated);
            this.footerPanel.Controls.Add(this.btnExport);
            this.footerPanel.Controls.Add(this.btnBack);
            this.footerPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.footerPanel.Location = new System.Drawing.Point(0, 552);
            this.footerPanel.Name = "footerPanel";
            this.footerPanel.Padding = new System.Windows.Forms.Padding(20, 14, 20, 14);
            this.footerPanel.Size = new System.Drawing.Size(860, 68);
            this.footerPanel.TabIndex = 2;
            this.footerPanel.Tag = "footer";
            //
            // lblCalculated
            //
            this.lblCalculated.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblCalculated.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(107)))), ((int)(((byte)(114)))), ((int)(((byte)(128)))));
            this.lblCalculated.Location = new System.Drawing.Point(140, 14);
            this.lblCalculated.Name = "lblCalculated";
            this.lblCalculated.Padding = new System.Windows.Forms.Padding(14, 0, 14, 0);
            this.lblCalculated.Size = new System.Drawing.Size(540, 40);
            this.lblCalculated.TabIndex = 2;
            this.lblCalculated.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            //
            // btnExport
            //
            this.btnExport.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnExport.Location = new System.Drawing.Point(680, 14);
            this.btnExport.Name = "btnExport";
            this.btnExport.Size = new System.Drawing.Size(160, 40);
            this.btnExport.TabIndex = 1;
            this.btnExport.Tag = "primary";
            this.btnExport.Text = "Export to CSV";
            this.btnExport.UseVisualStyleBackColor = true;
            this.btnExport.Click += new System.EventHandler(this.btnExport_Click);
            //
            // btnBack
            //
            this.btnBack.Dock = System.Windows.Forms.DockStyle.Left;
            this.btnBack.Location = new System.Drawing.Point(20, 14);
            this.btnBack.Name = "btnBack";
            this.btnBack.Size = new System.Drawing.Size(120, 40);
            this.btnBack.TabIndex = 0;
            this.btnBack.Text = "Back";
            this.btnBack.UseVisualStyleBackColor = true;
            this.btnBack.Click += new System.EventHandler(this.btnBack_Click);
            //
            // contentPanel
            //
            this.contentPanel.BackColor = System.Drawing.Color.White;
            this.contentPanel.Controls.Add(this.gridGrades);
            this.contentPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.contentPanel.Location = new System.Drawing.Point(0, 146);
            this.contentPanel.Name = "contentPanel";
            this.contentPanel.Padding = new System.Windows.Forms.Padding(20, 8, 20, 12);
            this.contentPanel.Size = new System.Drawing.Size(860, 406);
            this.contentPanel.TabIndex = 0;
            //
            // gridGrades
            //
            this.gridGrades.AllowUserToAddRows = false;
            this.gridGrades.AllowUserToDeleteRows = false;
            this.gridGrades.AllowUserToResizeRows = false;
            this.gridGrades.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colStudentId,
            this.colStudentName,
            this.colPercentage,
            this.colGrade});
            this.gridGrades.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridGrades.Location = new System.Drawing.Point(20, 8);
            this.gridGrades.MultiSelect = false;
            this.gridGrades.Name = "gridGrades";
            this.gridGrades.ReadOnly = true;
            this.gridGrades.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridGrades.Size = new System.Drawing.Size(820, 386);
            this.gridGrades.TabIndex = 0;
            this.gridGrades.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.gridGrades_CellFormatting);
            //
            // colStudentId
            //
            this.colStudentId.DataPropertyName = "StudentId";
            idStyle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(107)))), ((int)(((byte)(114)))), ((int)(((byte)(128)))));
            this.colStudentId.DefaultCellStyle = idStyle;
            this.colStudentId.HeaderText = "Student ID";
            this.colStudentId.Name = "colStudentId";
            this.colStudentId.ReadOnly = true;
            this.colStudentId.Width = 110;
            //
            // colStudentName
            //
            this.colStudentName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colStudentName.DataPropertyName = "StudentName";
            this.colStudentName.HeaderText = "Student";
            this.colStudentName.Name = "colStudentName";
            this.colStudentName.ReadOnly = true;
            //
            // colPercentage
            //
            this.colPercentage.DataPropertyName = "Percentage";
            percentStyle.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            percentStyle.Format = "0.00\'%\'";
            this.colPercentage.DefaultCellStyle = percentStyle;
            this.colPercentage.HeaderText = "Percentage";
            this.colPercentage.Name = "colPercentage";
            this.colPercentage.ReadOnly = true;
            this.colPercentage.Width = 140;
            //
            // colGrade
            //
            this.colGrade.DataPropertyName = "FinalGrade";
            gradeStyle.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            gradeStyle.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold);
            this.colGrade.DefaultCellStyle = gradeStyle;
            this.colGrade.HeaderText = "Grade";
            this.colGrade.Name = "colGrade";
            this.colGrade.ReadOnly = true;
            this.colGrade.Width = 110;
            //
            // CourseGradesForm
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(860, 620);
            this.Controls.Add(this.contentPanel);
            this.Controls.Add(this.statsPanel);
            this.Controls.Add(this.footerPanel);
            this.Controls.Add(this.headerPanel);
            this.Font = new System.Drawing.Font("Segoe UI", 9.75F);
            this.MinimumSize = new System.Drawing.Size(720, 480);
            this.Name = "CourseGradesForm";
            this.Text = "Student Grading System - Final grades";
            this.Load += new System.EventHandler(this.CourseGradesForm_Load);
            this.headerPanel.ResumeLayout(false);
            this.headerPanel.PerformLayout();
            this.statsPanel.ResumeLayout(false);
            this.footerPanel.ResumeLayout(false);
            this.contentPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridGrades)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel headerPanel;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Label lblSubtitle;
        private System.Windows.Forms.Panel statsPanel;
        private System.Windows.Forms.Label lblStats;
        private System.Windows.Forms.Label lblDistribution;
        private System.Windows.Forms.Panel footerPanel;
        private System.Windows.Forms.Button btnBack;
        private System.Windows.Forms.Button btnExport;
        private System.Windows.Forms.Label lblCalculated;
        private System.Windows.Forms.Panel contentPanel;
        private System.Windows.Forms.DataGridView gridGrades;
        private System.Windows.Forms.DataGridViewTextBoxColumn colStudentId;
        private System.Windows.Forms.DataGridViewTextBoxColumn colStudentName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colPercentage;
        private System.Windows.Forms.DataGridViewTextBoxColumn colGrade;
    }
}

namespace StudentGradingSystem.Forms
{
    partial class MarksEntryForm
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
            System.Windows.Forms.DataGridViewCellStyle marksStyle = new System.Windows.Forms.DataGridViewCellStyle();
            this.headerPanel = new System.Windows.Forms.Panel();
            this.lblSubtitle = new System.Windows.Forms.Label();
            this.lblTitle = new System.Windows.Forms.Label();
            this.footerPanel = new System.Windows.Forms.Panel();
            this.lblStatus = new System.Windows.Forms.Label();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnBack = new System.Windows.Forms.Button();
            this.contentPanel = new System.Windows.Forms.Panel();
            this.gridMarks = new System.Windows.Forms.DataGridView();
            this.colStudentId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colStudentName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colMarks = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.lblHint = new System.Windows.Forms.Label();
            this.headerPanel.SuspendLayout();
            this.footerPanel.SuspendLayout();
            this.contentPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridMarks)).BeginInit();
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
            this.lblTitle.Size = new System.Drawing.Size(125, 30);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "Enter marks";
            //
            // footerPanel
            //
            this.footerPanel.Controls.Add(this.lblStatus);
            this.footerPanel.Controls.Add(this.btnSave);
            this.footerPanel.Controls.Add(this.btnBack);
            this.footerPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.footerPanel.Location = new System.Drawing.Point(0, 552);
            this.footerPanel.Name = "footerPanel";
            this.footerPanel.Padding = new System.Windows.Forms.Padding(20, 14, 20, 14);
            this.footerPanel.Size = new System.Drawing.Size(860, 68);
            this.footerPanel.TabIndex = 1;
            this.footerPanel.Tag = "footer";
            //
            // lblStatus
            //
            this.lblStatus.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblStatus.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(107)))), ((int)(((byte)(114)))), ((int)(((byte)(128)))));
            this.lblStatus.Location = new System.Drawing.Point(140, 14);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Padding = new System.Windows.Forms.Padding(14, 0, 14, 0);
            this.lblStatus.Size = new System.Drawing.Size(540, 40);
            this.lblStatus.TabIndex = 2;
            this.lblStatus.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            //
            // btnSave
            //
            this.btnSave.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnSave.Location = new System.Drawing.Point(680, 14);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(160, 40);
            this.btnSave.TabIndex = 1;
            this.btnSave.Tag = "primary";
            this.btnSave.Text = "Save marks";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
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
            this.contentPanel.Controls.Add(this.gridMarks);
            this.contentPanel.Controls.Add(this.lblHint);
            this.contentPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.contentPanel.Location = new System.Drawing.Point(0, 76);
            this.contentPanel.Name = "contentPanel";
            this.contentPanel.Padding = new System.Windows.Forms.Padding(20, 8, 20, 12);
            this.contentPanel.Size = new System.Drawing.Size(860, 476);
            this.contentPanel.TabIndex = 0;
            //
            // gridMarks
            //
            this.gridMarks.AllowUserToAddRows = false;
            this.gridMarks.AllowUserToDeleteRows = false;
            this.gridMarks.AllowUserToResizeRows = false;
            this.gridMarks.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colStudentId,
            this.colStudentName,
            this.colMarks});
            this.gridMarks.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridMarks.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.gridMarks.Location = new System.Drawing.Point(20, 44);
            this.gridMarks.MultiSelect = false;
            this.gridMarks.Name = "gridMarks";
            this.gridMarks.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.gridMarks.Size = new System.Drawing.Size(820, 420);
            this.gridMarks.TabIndex = 0;
            this.gridMarks.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.gridMarks_CellEndEdit);
            this.gridMarks.CellEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.gridMarks_CellEnter);
            this.gridMarks.CellParsing += new System.Windows.Forms.DataGridViewCellParsingEventHandler(this.gridMarks_CellParsing);
            this.gridMarks.CellValidating += new System.Windows.Forms.DataGridViewCellValidatingEventHandler(this.gridMarks_CellValidating);
            this.gridMarks.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.gridMarks_CellValueChanged);
            this.gridMarks.CurrentCellDirtyStateChanged += new System.EventHandler(this.gridMarks_CurrentCellDirtyStateChanged);
            this.gridMarks.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.gridMarks_DataError);
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
            // colMarks
            //
            this.colMarks.DataPropertyName = "ObtainedMarks";
            marksStyle.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            marksStyle.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold);
            marksStyle.Format = "0.##";
            marksStyle.NullValue = "";
            this.colMarks.DefaultCellStyle = marksStyle;
            this.colMarks.HeaderText = "Obtained marks";
            this.colMarks.Name = "colMarks";
            this.colMarks.Width = 170;
            //
            // lblHint
            //
            this.lblHint.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblHint.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(107)))), ((int)(((byte)(114)))), ((int)(((byte)(128)))));
            this.lblHint.Location = new System.Drawing.Point(20, 8);
            this.lblHint.Name = "lblHint";
            this.lblHint.Size = new System.Drawing.Size(820, 36);
            this.lblHint.TabIndex = 1;
            this.lblHint.Text = "Type marks in the \"Obtained marks\" column and press Enter to move to the next student. Leave a cell empty if the student hasn\'t been marked yet.";
            this.lblHint.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            //
            // MarksEntryForm
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(860, 620);
            this.Controls.Add(this.contentPanel);
            this.Controls.Add(this.footerPanel);
            this.Controls.Add(this.headerPanel);
            this.Font = new System.Drawing.Font("Segoe UI", 9.75F);
            this.KeyPreview = true;
            this.MinimumSize = new System.Drawing.Size(720, 480);
            this.Name = "MarksEntryForm";
            this.Text = "Student Grading System - Enter marks";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MarksEntryForm_FormClosing);
            this.Load += new System.EventHandler(this.MarksEntryForm_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.MarksEntryForm_KeyDown);
            this.headerPanel.ResumeLayout(false);
            this.headerPanel.PerformLayout();
            this.footerPanel.ResumeLayout(false);
            this.contentPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridMarks)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel headerPanel;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Label lblSubtitle;
        private System.Windows.Forms.Panel footerPanel;
        private System.Windows.Forms.Button btnBack;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.Panel contentPanel;
        private System.Windows.Forms.Label lblHint;
        private System.Windows.Forms.DataGridView gridMarks;
        private System.Windows.Forms.DataGridViewTextBoxColumn colStudentId;
        private System.Windows.Forms.DataGridViewTextBoxColumn colStudentName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colMarks;
    }
}

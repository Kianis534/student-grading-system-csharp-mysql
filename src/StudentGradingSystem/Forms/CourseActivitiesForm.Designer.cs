namespace StudentGradingSystem.Forms
{
    partial class CourseActivitiesForm
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
            System.Windows.Forms.DataGridViewCellStyle totalStyle = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle weightStyle = new System.Windows.Forms.DataGridViewCellStyle();
            this.headerPanel = new System.Windows.Forms.Panel();
            this.lblSubtitle = new System.Windows.Forms.Label();
            this.lblTitle = new System.Windows.Forms.Label();
            this.footerPanel = new System.Windows.Forms.Panel();
            this.actionsPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.btnCalculate = new System.Windows.Forms.Button();
            this.btnViewGrades = new System.Windows.Forms.Button();
            this.btnEnterMarks = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnBack = new System.Windows.Forms.Button();
            this.addPanel = new System.Windows.Forms.Panel();
            this.lblWeightValue = new System.Windows.Forms.Label();
            this.progressWeight = new System.Windows.Forms.ProgressBar();
            this.lblWeightCaption = new System.Windows.Forms.Label();
            this.btnAdd = new System.Windows.Forms.Button();
            this.numWeight = new System.Windows.Forms.NumericUpDown();
            this.lblWeight = new System.Windows.Forms.Label();
            this.numTotal = new System.Windows.Forms.NumericUpDown();
            this.lblTotal = new System.Windows.Forms.Label();
            this.cboName = new System.Windows.Forms.ComboBox();
            this.lblName = new System.Windows.Forms.Label();
            this.lblAddTitle = new System.Windows.Forms.Label();
            this.contentPanel = new System.Windows.Forms.Panel();
            this.gridActivities = new System.Windows.Forms.DataGridView();
            this.colName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colTotal = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colWeight = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.headerPanel.SuspendLayout();
            this.footerPanel.SuspendLayout();
            this.actionsPanel.SuspendLayout();
            this.addPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numWeight)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numTotal)).BeginInit();
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
            this.headerPanel.Size = new System.Drawing.Size(980, 76);
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
            // footerPanel
            //
            this.footerPanel.Controls.Add(this.actionsPanel);
            this.footerPanel.Controls.Add(this.btnBack);
            this.footerPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.footerPanel.Location = new System.Drawing.Point(0, 572);
            this.footerPanel.Name = "footerPanel";
            this.footerPanel.Padding = new System.Windows.Forms.Padding(20, 14, 20, 14);
            this.footerPanel.Size = new System.Drawing.Size(980, 68);
            this.footerPanel.TabIndex = 2;
            this.footerPanel.Tag = "footer";
            //
            // actionsPanel
            //
            this.actionsPanel.Controls.Add(this.btnCalculate);
            this.actionsPanel.Controls.Add(this.btnViewGrades);
            this.actionsPanel.Controls.Add(this.btnEnterMarks);
            this.actionsPanel.Controls.Add(this.btnDelete);
            this.actionsPanel.Dock = System.Windows.Forms.DockStyle.Right;
            this.actionsPanel.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft;
            this.actionsPanel.Location = new System.Drawing.Point(380, 14);
            this.actionsPanel.Name = "actionsPanel";
            this.actionsPanel.Size = new System.Drawing.Size(580, 40);
            this.actionsPanel.TabIndex = 1;
            this.actionsPanel.WrapContents = false;
            //
            // btnCalculate
            //
            this.btnCalculate.Location = new System.Drawing.Point(420, 0);
            this.btnCalculate.Margin = new System.Windows.Forms.Padding(10, 0, 0, 0);
            this.btnCalculate.Name = "btnCalculate";
            this.btnCalculate.Size = new System.Drawing.Size(160, 40);
            this.btnCalculate.TabIndex = 0;
            this.btnCalculate.Tag = "primary";
            this.btnCalculate.Text = "Calculate grades";
            this.btnCalculate.UseVisualStyleBackColor = true;
            this.btnCalculate.Click += new System.EventHandler(this.btnCalculate_Click);
            //
            // btnViewGrades
            //
            this.btnViewGrades.Location = new System.Drawing.Point(280, 0);
            this.btnViewGrades.Margin = new System.Windows.Forms.Padding(10, 0, 0, 0);
            this.btnViewGrades.Name = "btnViewGrades";
            this.btnViewGrades.Size = new System.Drawing.Size(130, 40);
            this.btnViewGrades.TabIndex = 1;
            this.btnViewGrades.Text = "View grades";
            this.btnViewGrades.UseVisualStyleBackColor = true;
            this.btnViewGrades.Click += new System.EventHandler(this.btnViewGrades_Click);
            //
            // btnEnterMarks
            //
            this.btnEnterMarks.Location = new System.Drawing.Point(140, 0);
            this.btnEnterMarks.Margin = new System.Windows.Forms.Padding(10, 0, 0, 0);
            this.btnEnterMarks.Name = "btnEnterMarks";
            this.btnEnterMarks.Size = new System.Drawing.Size(130, 40);
            this.btnEnterMarks.TabIndex = 2;
            this.btnEnterMarks.Tag = "success";
            this.btnEnterMarks.Text = "Enter marks";
            this.btnEnterMarks.UseVisualStyleBackColor = true;
            this.btnEnterMarks.Click += new System.EventHandler(this.btnEnterMarks_Click);
            //
            // btnDelete
            //
            this.btnDelete.Location = new System.Drawing.Point(20, 0);
            this.btnDelete.Margin = new System.Windows.Forms.Padding(10, 0, 0, 0);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(110, 40);
            this.btnDelete.TabIndex = 3;
            this.btnDelete.Tag = "danger";
            this.btnDelete.Text = "Delete";
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
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
            // addPanel
            //
            this.addPanel.BackColor = System.Drawing.Color.White;
            this.addPanel.Controls.Add(this.lblWeightValue);
            this.addPanel.Controls.Add(this.progressWeight);
            this.addPanel.Controls.Add(this.lblWeightCaption);
            this.addPanel.Controls.Add(this.btnAdd);
            this.addPanel.Controls.Add(this.numWeight);
            this.addPanel.Controls.Add(this.lblWeight);
            this.addPanel.Controls.Add(this.numTotal);
            this.addPanel.Controls.Add(this.lblTotal);
            this.addPanel.Controls.Add(this.cboName);
            this.addPanel.Controls.Add(this.lblName);
            this.addPanel.Controls.Add(this.lblAddTitle);
            this.addPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.addPanel.Location = new System.Drawing.Point(0, 462);
            this.addPanel.Name = "addPanel";
            this.addPanel.Size = new System.Drawing.Size(980, 110);
            this.addPanel.TabIndex = 1;
            //
            // lblWeightValue
            //
            this.lblWeightValue.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblWeightValue.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblWeightValue.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(107)))), ((int)(((byte)(114)))), ((int)(((byte)(128)))));
            this.lblWeightValue.Location = new System.Drawing.Point(740, 76);
            this.lblWeightValue.Name = "lblWeightValue";
            this.lblWeightValue.Size = new System.Drawing.Size(220, 20);
            this.lblWeightValue.TabIndex = 10;
            this.lblWeightValue.Text = "0% of 100% allocated";
            //
            // progressWeight
            //
            this.progressWeight.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.progressWeight.Location = new System.Drawing.Point(740, 52);
            this.progressWeight.Name = "progressWeight";
            this.progressWeight.Size = new System.Drawing.Size(220, 18);
            this.progressWeight.TabIndex = 9;
            //
            // lblWeightCaption
            //
            this.lblWeightCaption.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblWeightCaption.AutoSize = true;
            this.lblWeightCaption.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold);
            this.lblWeightCaption.Location = new System.Drawing.Point(737, 28);
            this.lblWeightCaption.Name = "lblWeightCaption";
            this.lblWeightCaption.Size = new System.Drawing.Size(141, 17);
            this.lblWeightCaption.TabIndex = 8;
            this.lblWeightCaption.Text = "Final grade weightage";
            //
            // btnAdd
            //
            this.btnAdd.Location = new System.Drawing.Point(546, 50);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(150, 32);
            this.btnAdd.TabIndex = 7;
            this.btnAdd.Tag = "success";
            this.btnAdd.Text = "Add assessment";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            //
            // numWeight
            //
            this.numWeight.DecimalPlaces = 2;
            this.numWeight.Increment = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.numWeight.Location = new System.Drawing.Point(420, 54);
            this.numWeight.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.numWeight.Name = "numWeight";
            this.numWeight.Size = new System.Drawing.Size(110, 25);
            this.numWeight.TabIndex = 6;
            this.numWeight.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.numWeight.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            //
            // lblWeight
            //
            this.lblWeight.AutoSize = true;
            this.lblWeight.Location = new System.Drawing.Point(417, 32);
            this.lblWeight.Name = "lblWeight";
            this.lblWeight.Size = new System.Drawing.Size(92, 17);
            this.lblWeight.TabIndex = 5;
            this.lblWeight.Text = "Weightage (%)";
            //
            // numTotal
            //
            this.numTotal.DecimalPlaces = 2;
            this.numTotal.Location = new System.Drawing.Point(294, 54);
            this.numTotal.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.numTotal.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.numTotal.Name = "numTotal";
            this.numTotal.Size = new System.Drawing.Size(110, 25);
            this.numTotal.TabIndex = 4;
            this.numTotal.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.numTotal.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            //
            // lblTotal
            //
            this.lblTotal.AutoSize = true;
            this.lblTotal.Location = new System.Drawing.Point(291, 32);
            this.lblTotal.Name = "lblTotal";
            this.lblTotal.Size = new System.Drawing.Size(73, 17);
            this.lblTotal.TabIndex = 3;
            this.lblTotal.Text = "Total marks";
            //
            // cboName
            //
            this.cboName.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cboName.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cboName.FormattingEnabled = true;
            this.cboName.Location = new System.Drawing.Point(20, 54);
            this.cboName.MaxLength = 50;
            this.cboName.Name = "cboName";
            this.cboName.Size = new System.Drawing.Size(258, 25);
            this.cboName.TabIndex = 2;
            //
            // lblName
            //
            this.lblName.AutoSize = true;
            this.lblName.Location = new System.Drawing.Point(17, 32);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(212, 17);
            this.lblName.TabIndex = 1;
            this.lblName.Text = "Name (pick one or type your own)";
            //
            // lblAddTitle
            //
            this.lblAddTitle.AutoSize = true;
            this.lblAddTitle.Font = new System.Drawing.Font("Segoe UI Semibold", 11.25F, System.Drawing.FontStyle.Bold);
            this.lblAddTitle.Location = new System.Drawing.Point(16, 4);
            this.lblAddTitle.Name = "lblAddTitle";
            this.lblAddTitle.Size = new System.Drawing.Size(141, 20);
            this.lblAddTitle.TabIndex = 0;
            this.lblAddTitle.Text = "Add an assessment";
            //
            // contentPanel
            //
            this.contentPanel.BackColor = System.Drawing.Color.White;
            this.contentPanel.Controls.Add(this.gridActivities);
            this.contentPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.contentPanel.Location = new System.Drawing.Point(0, 76);
            this.contentPanel.Name = "contentPanel";
            this.contentPanel.Padding = new System.Windows.Forms.Padding(20, 16, 20, 8);
            this.contentPanel.Size = new System.Drawing.Size(980, 386);
            this.contentPanel.TabIndex = 0;
            //
            // gridActivities
            //
            this.gridActivities.AllowUserToAddRows = false;
            this.gridActivities.AllowUserToDeleteRows = false;
            this.gridActivities.AllowUserToResizeRows = false;
            this.gridActivities.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colName,
            this.colTotal,
            this.colWeight});
            this.gridActivities.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridActivities.Location = new System.Drawing.Point(20, 16);
            this.gridActivities.MultiSelect = false;
            this.gridActivities.Name = "gridActivities";
            this.gridActivities.ReadOnly = true;
            this.gridActivities.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridActivities.Size = new System.Drawing.Size(940, 362);
            this.gridActivities.TabIndex = 0;
            this.gridActivities.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.gridActivities_CellDoubleClick);
            this.gridActivities.SelectionChanged += new System.EventHandler(this.gridActivities_SelectionChanged);
            this.gridActivities.KeyDown += new System.Windows.Forms.KeyEventHandler(this.gridActivities_KeyDown);
            //
            // colName
            //
            this.colName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colName.DataPropertyName = "Name";
            this.colName.HeaderText = "Assessment";
            this.colName.Name = "colName";
            this.colName.ReadOnly = true;
            //
            // colTotal
            //
            this.colTotal.DataPropertyName = "TotalMarks";
            totalStyle.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            totalStyle.Format = "0.##";
            this.colTotal.DefaultCellStyle = totalStyle;
            this.colTotal.HeaderText = "Total marks";
            this.colTotal.Name = "colTotal";
            this.colTotal.ReadOnly = true;
            this.colTotal.Width = 140;
            //
            // colWeight
            //
            this.colWeight.DataPropertyName = "Weightage";
            weightStyle.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            weightStyle.Format = "0.##\'%\'";
            this.colWeight.DefaultCellStyle = weightStyle;
            this.colWeight.HeaderText = "Weightage";
            this.colWeight.Name = "colWeight";
            this.colWeight.ReadOnly = true;
            this.colWeight.Width = 140;
            //
            // CourseActivitiesForm
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(980, 640);
            this.Controls.Add(this.contentPanel);
            this.Controls.Add(this.addPanel);
            this.Controls.Add(this.footerPanel);
            this.Controls.Add(this.headerPanel);
            this.Font = new System.Drawing.Font("Segoe UI", 9.75F);
            this.MinimumSize = new System.Drawing.Size(980, 560);
            this.Name = "CourseActivitiesForm";
            this.Text = "Student Grading System - Course";
            this.Load += new System.EventHandler(this.CourseActivitiesForm_Load);
            this.headerPanel.ResumeLayout(false);
            this.headerPanel.PerformLayout();
            this.footerPanel.ResumeLayout(false);
            this.actionsPanel.ResumeLayout(false);
            this.addPanel.ResumeLayout(false);
            this.addPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numWeight)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numTotal)).EndInit();
            this.contentPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridActivities)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel headerPanel;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Label lblSubtitle;
        private System.Windows.Forms.Panel footerPanel;
        private System.Windows.Forms.Button btnBack;
        private System.Windows.Forms.FlowLayoutPanel actionsPanel;
        private System.Windows.Forms.Button btnCalculate;
        private System.Windows.Forms.Button btnViewGrades;
        private System.Windows.Forms.Button btnEnterMarks;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Panel addPanel;
        private System.Windows.Forms.Label lblAddTitle;
        private System.Windows.Forms.Label lblName;
        private System.Windows.Forms.ComboBox cboName;
        private System.Windows.Forms.Label lblTotal;
        private System.Windows.Forms.NumericUpDown numTotal;
        private System.Windows.Forms.Label lblWeight;
        private System.Windows.Forms.NumericUpDown numWeight;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Label lblWeightCaption;
        private System.Windows.Forms.ProgressBar progressWeight;
        private System.Windows.Forms.Label lblWeightValue;
        private System.Windows.Forms.Panel contentPanel;
        private System.Windows.Forms.DataGridView gridActivities;
        private System.Windows.Forms.DataGridViewTextBoxColumn colName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTotal;
        private System.Windows.Forms.DataGridViewTextBoxColumn colWeight;
    }
}

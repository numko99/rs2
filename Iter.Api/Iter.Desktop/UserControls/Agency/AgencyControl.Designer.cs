using Iter.Desktop.Custom;

namespace Iter.Desktop.UserControls
{
    partial class AgencyControl
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AgencyControl));
            dataGridView1 = new CustomDataGridView();
            paginationPanel = new Panel();
            label1 = new Label();
            addAgencyBtn = new Button();
            Id = new DataGridViewTextBoxColumn();
            AgencyName = new DataGridViewTextBoxColumn();
            City = new DataGridViewTextBoxColumn();
            ContactEmail = new DataGridViewTextBoxColumn();
            ContactPhone = new DataGridViewTextBoxColumn();
            Edit = new DataGridViewImageColumn();
            Details = new DataGridViewImageColumn();
            Delete = new DataGridViewImageColumn();
            ((System.ComponentModel.ISupportInitialize)dataGridView1).BeginInit();
            SuspendLayout();
            // 
            // dataGridView1
            // 
            dataGridView1.BackgroundColor = Color.White;
            dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView1.Columns.AddRange(new DataGridViewColumn[] { Id, AgencyName, City, ContactEmail, ContactPhone, Edit, Details, Delete });
            dataGridView1.Location = new Point(23, 93);
            dataGridView1.Name = "dataGridView1";
            dataGridView1.RowHeadersWidth = 51;
            dataGridView1.RowTemplate.Height = 29;
            dataGridView1.Size = new Size(1414, 475);
            dataGridView1.TabIndex = 0;
            dataGridView1.CellContentClick += dataGridView1_CellContentClick;
            // 
            // paginationPanel
            // 
            paginationPanel.Location = new Point(3, 574);
            paginationPanel.Name = "paginationPanel";
            paginationPanel.Size = new Size(1434, 53);
            paginationPanel.TabIndex = 1;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Bookman Old Style", 13.8F, FontStyle.Regular, GraphicsUnit.Point);
            label1.Location = new Point(23, 11);
            label1.Name = "label1";
            label1.Size = new Size(191, 26);
            label1.TabIndex = 2;
            label1.Text = "Pregled agencija";
            // 
            // addAgencyBtn
            // 
            addAgencyBtn.BackColor = Color.FromArgb(0, 124, 199);
            addAgencyBtn.Cursor = Cursors.Hand;
            addAgencyBtn.Font = new Font("Bookman Old Style", 9F, FontStyle.Regular, GraphicsUnit.Point);
            addAgencyBtn.ForeColor = Color.White;
            addAgencyBtn.Location = new Point(1296, 12);
            addAgencyBtn.Name = "addAgencyBtn";
            addAgencyBtn.Size = new Size(141, 29);
            addAgencyBtn.TabIndex = 3;
            addAgencyBtn.Text = "Dodaj agenciju";
            addAgencyBtn.UseVisualStyleBackColor = false;
            addAgencyBtn.Click += addAgencyBtn_Click;
            // 
            // Id
            // 
            Id.DataPropertyName = "Id";
            Id.HeaderText = "Id";
            Id.MinimumWidth = 6;
            Id.Name = "Id";
            Id.ReadOnly = true;
            Id.Visible = false;
            Id.Width = 125;
            // 
            // AgencyName
            // 
            AgencyName.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            AgencyName.DataPropertyName = "Name";
            AgencyName.HeaderText = "Naziv";
            AgencyName.MinimumWidth = 6;
            AgencyName.Name = "AgencyName";
            AgencyName.ReadOnly = true;
            // 
            // City
            // 
            City.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            City.DataPropertyName = "City";
            City.HeaderText = "Grad";
            City.MinimumWidth = 6;
            City.Name = "City";
            City.ReadOnly = true;
            // 
            // ContactEmail
            // 
            ContactEmail.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            ContactEmail.DataPropertyName = "ContactEmail";
            ContactEmail.HeaderText = "Email";
            ContactEmail.MinimumWidth = 6;
            ContactEmail.Name = "ContactEmail";
            ContactEmail.ReadOnly = true;
            // 
            // ContactPhone
            // 
            ContactPhone.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            ContactPhone.DataPropertyName = "ContactPhone";
            ContactPhone.HeaderText = "Broj telefona";
            ContactPhone.MinimumWidth = 6;
            ContactPhone.Name = "ContactPhone";
            ContactPhone.ReadOnly = true;
            // 
            // Edit
            // 
            Edit.HeaderText = "";
            Edit.Image = (Image)resources.GetObject("Edit.Image");
            Edit.ImageLayout = DataGridViewImageCellLayout.Stretch;
            Edit.MinimumWidth = 6;
            Edit.Name = "Edit";
            Edit.ReadOnly = true;
            Edit.Width = 50;
            // 
            // Details
            // 
            Details.HeaderText = "";
            Details.Image = Properties.Resources.details_icon;
            Details.ImageLayout = DataGridViewImageCellLayout.Stretch;
            Details.MinimumWidth = 6;
            Details.Name = "Details";
            Details.ReadOnly = true;
            Details.Width = 50;
            // 
            // Delete
            // 
            Delete.HeaderText = "";
            Delete.Image = (Image)resources.GetObject("Delete.Image");
            Delete.ImageLayout = DataGridViewImageCellLayout.Stretch;
            Delete.MinimumWidth = 6;
            Delete.Name = "Delete";
            Delete.Width = 50;
            // 
            // AgencyControl
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.White;
            Controls.Add(addAgencyBtn);
            Controls.Add(label1);
            Controls.Add(paginationPanel);
            Controls.Add(dataGridView1);
            Name = "AgencyControl";
            Size = new Size(1466, 690);
            Load += AgencyControl_Load;
            ((System.ComponentModel.ISupportInitialize)dataGridView1).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private CustomDataGridView dataGridView1;
        private Panel paginationPanel;
        private Label label1;
        private Button addAgencyBtn;
        private DataGridViewTextBoxColumn Id;
        private DataGridViewTextBoxColumn AgencyName;
        private DataGridViewTextBoxColumn City;
        private DataGridViewTextBoxColumn ContactEmail;
        private DataGridViewTextBoxColumn ContactPhone;
        private DataGridViewImageColumn Edit;
        private DataGridViewImageColumn Details;
        private DataGridViewImageColumn Delete;
    }
}

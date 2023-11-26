namespace Iter.Desktop
{
    partial class HomeForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(HomeForm));
            headPanel = new Panel();
            headerPicture = new PictureBox();
            headerLabel = new Label();
            contentPanel = new Panel();
            sidebar = new SideBar.Sidebar();
            nameLabel = new Label();
            pictureBox1 = new PictureBox();
            headPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)headerPicture).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            SuspendLayout();
            // 
            // headPanel
            // 
            headPanel.BackColor = Color.White;
            headPanel.BackgroundImageLayout = ImageLayout.None;
            headPanel.BorderStyle = BorderStyle.FixedSingle;
            headPanel.Controls.Add(headerPicture);
            headPanel.Controls.Add(headerLabel);
            headPanel.Location = new Point(150, 59);
            headPanel.Name = "headPanel";
            headPanel.Size = new Size(1572, 98);
            headPanel.TabIndex = 4;
            // 
            // headerPicture
            // 
            headerPicture.BackColor = Color.Transparent;
            headerPicture.Image = Properties.Resources.home_icon1;
            headerPicture.Location = new Point(1436, 22);
            headerPicture.Name = "headerPicture";
            headerPicture.Size = new Size(93, 45);
            headerPicture.SizeMode = PictureBoxSizeMode.StretchImage;
            headerPicture.TabIndex = 7;
            headerPicture.TabStop = false;
            // 
            // headerLabel
            // 
            headerLabel.AutoSize = true;
            headerLabel.Font = new Font("Bookman Old Style", 24F, FontStyle.Regular, GraphicsUnit.Point);
            headerLabel.ForeColor = Color.FromArgb(32, 54, 71);
            headerLabel.Location = new Point(63, 22);
            headerLabel.Name = "headerLabel";
            headerLabel.Size = new Size(173, 45);
            headerLabel.TabIndex = 4;
            headerLabel.Text = "Početna";
            // 
            // contentPanel
            // 
            contentPanel.AutoScroll = true;
            contentPanel.Location = new Point(214, 190);
            contentPanel.Name = "contentPanel";
            contentPanel.Size = new Size(1466, 698);
            contentPanel.TabIndex = 5;
            // 
            // sidebar
            // 
            sidebar.BackColor = Color.FromArgb(0, 124, 199);
            sidebar.Location = new Point(-5, 0);
            sidebar.Name = "sidebar";
            sidebar.Size = new Size(169, 954);
            sidebar.TabIndex = 8;
            // 
            // nameLabel
            // 
            nameLabel.AutoSize = true;
            nameLabel.BackColor = Color.FromArgb(0, 124, 199);
            nameLabel.Font = new Font("Bookman Old Style", 7F, FontStyle.Regular, GraphicsUnit.Point);
            nameLabel.ForeColor = Color.White;
            nameLabel.Location = new Point(1537, 19);
            nameLabel.Name = "nameLabel";
            nameLabel.Padding = new Padding(3);
            nameLabel.Size = new Size(48, 22);
            nameLabel.TabIndex = 8;
            nameLabel.Text = "Name";
            nameLabel.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // pictureBox1
            // 
            pictureBox1.Image = (Image)resources.GetObject("pictureBox1.Image");
            pictureBox1.Location = new Point(1513, 19);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(24, 22);
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox1.TabIndex = 9;
            pictureBox1.TabStop = false;
            // 
            // HomeForm
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1718, 954);
            Controls.Add(pictureBox1);
            Controls.Add(nameLabel);
            Controls.Add(sidebar);
            Controls.Add(contentPanel);
            Controls.Add(headPanel);
            Name = "HomeForm";
            Load += HomeForm_Load_1;
            headPanel.ResumeLayout(false);
            headPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)headerPicture).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private Panel panel1;
        private Panel headPanel;
        private Panel contentPanel;
        private Label headerLabel;
        private PictureBox headerPicture;
        private SideBar.Sidebar sidebar;
        private Label nameLabel;
        private PictureBox pictureBox1;
    }
}
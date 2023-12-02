namespace Iter.Desktop.UserControls.Pagination
{
    partial class PaginationUserControl
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PaginationUserControl));
            lblPage = new Label();
            left_arrow_icon = new PictureBox();
            right_arrow_icon = new PictureBox();
            ((System.ComponentModel.ISupportInitialize)left_arrow_icon).BeginInit();
            ((System.ComponentModel.ISupportInitialize)right_arrow_icon).BeginInit();
            SuspendLayout();
            // 
            // lblPage
            // 
            lblPage.AutoSize = true;
            lblPage.Location = new Point(1187, 8);
            lblPage.Name = "lblPage";
            lblPage.Size = new Size(103, 20);
            lblPage.TabIndex = 2;
            lblPage.Text = "Page 2 od 100";
            // 
            // left_arrow_icon
            // 
            left_arrow_icon.BorderStyle = BorderStyle.FixedSingle;
            left_arrow_icon.Cursor = Cursors.Hand;
            left_arrow_icon.Image = (Image)resources.GetObject("left_arrow_icon.Image");
            left_arrow_icon.Location = new Point(1125, 3);
            left_arrow_icon.Name = "left_arrow_icon";
            left_arrow_icon.Size = new Size(33, 28);
            left_arrow_icon.SizeMode = PictureBoxSizeMode.StretchImage;
            left_arrow_icon.TabIndex = 3;
            left_arrow_icon.TabStop = false;
            left_arrow_icon.Click += left_arrow_icon_Click;
            // 
            // right_arrow_icon
            // 
            right_arrow_icon.BorderStyle = BorderStyle.FixedSingle;
            right_arrow_icon.Cursor = Cursors.Hand;
            right_arrow_icon.Image = (Image)resources.GetObject("right_arrow_icon.Image");
            right_arrow_icon.Location = new Point(1307, 3);
            right_arrow_icon.Name = "right_arrow_icon";
            right_arrow_icon.Size = new Size(33, 28);
            right_arrow_icon.SizeMode = PictureBoxSizeMode.StretchImage;
            right_arrow_icon.TabIndex = 4;
            right_arrow_icon.TabStop = false;
            right_arrow_icon.Click += right_arrow_icon_Click;
            // 
            // PaginationUserControl
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(right_arrow_icon);
            Controls.Add(left_arrow_icon);
            Controls.Add(lblPage);
            Name = "PaginationUserControl";
            Size = new Size(1434, 53);
            ((System.ComponentModel.ISupportInitialize)left_arrow_icon).EndInit();
            ((System.ComponentModel.ISupportInitialize)right_arrow_icon).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private Label lblPage;
        private PictureBox left_arrow_icon;
        private PictureBox right_arrow_icon;
    }
}

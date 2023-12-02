using Iter.Core.Enum;

namespace Iter.Desktop.Forms
{
    public partial class DialogForm : Form
    {
        public DialogForm(string text, ToastrTypes toastrTypes)
        {
            InitializeComponent();
            this.label1.Text = text;
            setColor(toastrTypes);
        }

        private void setColor(ToastrTypes toastrTypes)
        {
            if (toastrTypes == ToastrTypes.Success)
            {
                pictureBox1.Image = Properties.Resources.fa_fa_check;
                BackColor = Color.FromArgb(0, 186, 0);
            }
            if (toastrTypes == ToastrTypes.Warning)
            {
                pictureBox1.Image = Properties.Resources._97_970936_exclamation_mark_png_flosstradamus_warning_sign;
                BackColor = Color.FromArgb(255, 201, 13);
            }
            if (toastrTypes == ToastrTypes.Error)
            {
                pictureBox1.Image = Properties.Resources._97_970936_exclamation_mark_png_flosstradamus_warning_sign;
                BackColor = Color.FromArgb(237, 28, 36);
            }
        }

        private void DialogForm_Load(object sender, EventArgs e)
        {
            Top = 40;
            Left = Screen.PrimaryScreen.Bounds.Width - Width - 40;
            timerClose.Start();
        }

        private void timerClose_Tick(object sender, EventArgs e)
        {
            Close();
        }
    }
}

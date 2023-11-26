using Iter.Desktop.Helper;
using Section = Iter.Desktop.Helper.Section;

namespace Iter.Desktop.SideBar
{
    public partial class Sidebar : UserControl
    {
        public event EventHandler<SectionChangedEventArgs> SectionClicked;
        public Sidebar()
        {
            InitializeComponent();
        }

        private void HomeButton_Click(object sender, EventArgs e)
        {
            SectionClicked?.Invoke(this, new SectionChangedEventArgs(Section.HomeSection));
        }

        private void UsersButton_Click(object sender, EventArgs e)
        {
            SectionClicked?.Invoke(this, new SectionChangedEventArgs(Section.UsersSection));
        }

        private void AgencyButton_Click(object sender, EventArgs e)
        {
            SectionClicked?.Invoke(this, new SectionChangedEventArgs(Section.AgencySection));
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void LogOutButton_Click(object sender, EventArgs e)
        {

        }
    }
}

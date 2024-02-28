using Iter.Desktop.Helper;
using Iter.Desktop.Models;
using Iter.Desktop.UserControls;
using Iter.Desktop.UserControls.Agency;
using Iter.UI.Desktop.Services;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Iter.Desktop
{
    public partial class HomeForm : Form
    {
        public HomeForm()
        {
            InitializeComponent();

            sidebar.SectionClicked += SidebarControl_SectionClicked;
            SetControl(contentPanel, new AgencyControl(), "Početna", Properties.Resources.home_icon1);
        }

        private void SetControl(Panel contenctPanel, Control contentView, string title, Bitmap imageName)
        {
            ShowView(contenctPanel, contentView);
            headerLabel.Text = title;
            headerPicture.Image = imageName;
        }

        private void ShowView(Panel panel, Control view)
        {
            panel.Controls.Clear();
            panel.Controls.Add(view);

            view.Dock = DockStyle.Fill;
        }

        private void SidebarControl_SectionClicked(object sender, SectionChangedEventArgs e)
        {
            switch (e.Section)
            {
                case Section.HomeSection:
                    SetControl(contentPanel, new UsersControl(), "Početna", Properties.Resources.user_icon);
                    break;
                case Section.UsersSection:
                    SetControl(contentPanel, new UsersControl(), "Korisnici", Properties.Resources.user_icon);
                    break;
                case Section.AgencySection:
                    var agency = new AgencyControl();
                    agency.OpenNewFormRequested += AgencyControl_OpenNewFormRequested;
                    SetControl(contentPanel, agency, "Agencije", Properties.Resources.agency_icon1);
                    break;
                default:
                    break;
            }
        }

        private async void HomeForm_Load_1(object sender, EventArgs e)
        {

            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            JwtSecurityToken jwtToken = tokenHandler.ReadJwtToken(ApiService.Token);

            string? name = jwtToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;
            nameLabel.Text = name;
        }

        private void AgencyControl_OpenNewFormRequested(object sender, AgencyEventArgs e)
        {
            var agencyDetails = new AgencyDetailsControl(e.AgencyId);
            ShowView(contentPanel, agencyDetails);
            headerLabel.Text = "Detalji agencije";
            headerPicture.Image = Properties.Resources.agency_icon1;
        }
    }
}
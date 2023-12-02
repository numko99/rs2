using Iter.Core.Responses;
using Iter.UI.Desktop.Services;

namespace Iter.Desktop.UserControls.Agency
{
    public partial class AgencyDetailsControl : UserControl
    {
        private readonly ApiService _apiService = new ApiService("agency");
        private readonly Guid agencyId;
        public AgencyDetailsControl(Guid agencyId)
        {
            InitializeComponent();
            this.agencyId = agencyId;
        }

        private async void AgencyDetailsControl_Load(object sender, EventArgs e)
        {
            var agency = await _apiService.GetById<AgencyResponse>(this.agencyId);
            if (agency == null)
            {
                return;
            }
            NameLbl.Text = agency.Name;
            NameLbl.TextAlign = ContentAlignment.TopRight;
            licenceLbl.Text = agency.LicenseNumber;
            licenceLbl.TextAlign = ContentAlignment.TopRight;
            emailLbl.Text = agency.ContactEmail;
            phoneLbl.Text = agency.ContactPhone;
            websiteLbl.Text = agency?.Website;

            CountryLbl.Text = agency.Address?.Country;
            CityLbl.Text = agency.Address?.City;
            postalCodeLbl.Text = agency.Address?.PostalCode;
            adressLbl.Text = agency.Address?.Street + " " + agency.Address?.HouseNumber;

            dateCreatedLbl.Text = agency.DateCreated.ToString("dd.MM.yyyy");
        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void adressLbl_Click(object sender, EventArgs e)
        {
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}

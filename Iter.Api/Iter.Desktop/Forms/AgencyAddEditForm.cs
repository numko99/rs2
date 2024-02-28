using Iter.Core.Dto;
using Iter.Core.Enum;
using Iter.Core;
using Iter.Desktop.Helper;
using Iter.Desktop.UserControls;
using Iter.UI.Desktop.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Iter.Desktop.Forms
{
    public partial class AgencyAddEditForm : Form
    {
        private readonly ApiService _apiService = new ApiService("agency");

        private readonly AgencyControl? agencyControl;

        private Guid? agencyId;
        public AgencyAddEditForm(Guid? agencyId = null, AgencyControl? agencyControl = null)
        {
            InitializeComponent();
            this.agencyId = agencyId;
            this.agencyControl = agencyControl;
        }

        private async void saveBtn_Click(object sender, EventArgs e)
        {
            saveBtn.Enabled = false;
            try
            {
                AgencyInsertRequest agencyInsertRequest = new AgencyInsertRequest()
                {
                    Name = nameTxtBox.Text,
                    ContactEmail = emailTxtBox.Text,
                    ContactPhone = PhoneTxtBox.Text,
                    Website = webSiteTxtBox.Text,
                    LicenseNumber = licenseTxtBox.Text,

                    City = CityTxtBox.Text,
                    Country = CountryTxtBox.Text,
                    PostalCode = PostalCodeTxtBox.Text,
                    HouseNumber = HouseNumberTxtBox.Text,
                    Street = StreetTxtBox.Text,
                };

                var errorLabels = new Dictionary<string, Label>
                {
                    { nameof(agencyInsertRequest.Name), nameErrorLabel },
                    { nameof(agencyInsertRequest.ContactEmail), emailErrorLabel },
                    { nameof(agencyInsertRequest.ContactPhone), phoneErrorLabel },
                    { nameof(agencyInsertRequest.Website), webSiteErrorLabel },
                    { nameof(agencyInsertRequest.LicenseNumber), licenceErrorLabel},
                    { nameof(agencyInsertRequest.Country), countryErrorLabel},
                    { nameof(agencyInsertRequest.City), cityErrorLabel },
                    { nameof(agencyInsertRequest.Street), streetLabelError },
                    { nameof(agencyInsertRequest.PostalCode), postalCodeErrorLabel },
                    { nameof(agencyInsertRequest.HouseNumber), houseNumberErrorLabel },
                };

                if (!InputValidator.ValidateInputs(agencyInsertRequest, errorLabels, this))
                {
                    return;
                }
                if (agencyId == null)
                {
                    await _apiService.Insert<AgencyInsertRequest>("insert", agencyInsertRequest);
                    using (var dialogForm = new DialogForm("Uspješno ste uredili agenciju", ToastrTypes.Success))
                    {
                        dialogForm.ShowDialog(this);
                    }
                }
                else
                {
                    await _apiService.Update<AgencyInsertRequest>(agencyInsertRequest, agencyId);

                    using (var dialogForm = new DialogForm("Uspješno ste uredili agenciju", ToastrTypes.Success))
                    {
                        dialogForm.ShowDialog(this);
                    }
                }
                OnAgencyDataChanged();
            }
            catch (Exception)
            {
                throw;
            }

        }

        protected async void OnAgencyDataChanged()
        {
            await this.agencyControl.LoadDataAsync();
            var openedForm = this.FindForm();
            openedForm.Hide();
        }

        private async void AgencyAddEditForm_Load(object sender, EventArgs e)
        {
            if (this.agencyId != null)
            {
                try
                {
                    var agency = await _apiService.GetById<AgencyResponse>(this.agencyId);
                    if (agency != null)
                    {
                        nameTxtBox.Text = agency.Name;
                        emailTxtBox.Text = agency.ContactEmail;
                        PhoneTxtBox.Text = agency.ContactPhone;
                        webSiteTxtBox.Text = agency.Website;
                        licenseTxtBox.Text = agency.LicenseNumber;
                        CityTxtBox.Text = agency.Address?.City;
                        CountryTxtBox.Text = agency.Address?.Country;
                        PostalCodeTxtBox.Text = agency.Address?.PostalCode;
                        HouseNumberTxtBox.Text = agency.Address?.HouseNumber;
                        StreetTxtBox.Text = agency.Address?.Street;

                        // Assuming you have a LogoPictureBox control on the form to display the logo
                        // LogoPictureBox.ImageLocation = agency.LogoUrl;

                        // Set any other properties you want to display or use on the form

                        // For example, you can also set the IsActive checkbox value:
                        // isActiveCheckBox.Checked = agency.IsActive;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error fetching agency data: " + ex.Message);
                }
            }
        }

        private void cancelBtn_Click(object sender, EventArgs e)
        {
            var openedForm = this.FindForm();
            openedForm.Hide();
        }
    }
}

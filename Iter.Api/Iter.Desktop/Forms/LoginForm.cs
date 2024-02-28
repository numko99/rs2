using Iter.Core;
using Iter.Core.Dto;
using Iter.Desktop.Helper;
using Iter.UI.Desktop.Services;

using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Iter.Desktop.Forms
{
    public partial class LoginForm : Form
    {
        private readonly ApiService _apiService = new ApiService("userauthentication/login");
        public LoginForm()
        {
            InitializeComponent();
        }

        private async void submitBtn_Click(object sender, EventArgs e)
        {
            try
            {
                UserLoginDto userLogin = new UserLoginDto()
                {
                    UserName = usernameTxt.Text,
                    Password = passwordTxt.Text,
                };

                var errorLabels = new Dictionary<string, Label>
                {
                    { nameof(userLogin.UserName), usernameErrorLabel },
                    { nameof(userLogin.Password), passwordErrorLabel }
                };

                if (!InputValidator.ValidateInputs(userLogin, errorLabels, this))
                {
                    return;
                }

                var result = await _apiService.Login<LoginResponse>(userLogin);
                ApiService.Token = result.Token;

                var openedForm = this.FindForm();
                openedForm.Hide();

                HomeForm homeForm = new HomeForm();
                homeForm.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Korisnik sa unesenim korisničkim podacima nije pronađen.");
            }
        }

        private void LoginForm_Load(object sender, EventArgs e)
        {

        }
    }
}

using Iter.Core;
using Iter.Core.Enum;
using Iter.Desktop.Forms;
using Iter.Desktop.Models;
using Iter.Desktop.UserControls.Pagination;
using Iter.UI.Desktop.Services;

namespace Iter.Desktop.UserControls
{

    public partial class AgencyControl : UserControl
    {
        public event EventHandler<AgencyEventArgs> OpenNewFormRequested;
        private readonly ApiService _apiService = new ApiService("agency");
        PaginationUserControl pagination;
        public AgencyControl()
        {
            InitializeComponent();
            InitialitePagination();
        }

        private async void Pagination_PageChanged(object? sender, EventArgs e)
        {
            await LoadDataAsync();
        }

        private async void AgencyControl_Load(object sender, EventArgs e)
        {
            await LoadDataAsync();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            string colname = dataGridView1.Columns[e.ColumnIndex].Name;
            var agencyId = (Guid)dataGridView1.Rows[e.RowIndex].Cells["Id"]?.Value;

            if (colname == "Edit")
            {
                DisplayUpdateForm(agencyId);
            }
            if (colname == "Details")
            {
                NavigateToDetailsForm(agencyId);
            }
            if (colname == "Delete")
            {
                DeleteAgency(agencyId, e);
            }
        }

        private void NavigateToDetailsForm(Guid agencyId)
        {
            OpenNewFormRequested?.Invoke(this, new AgencyEventArgs(agencyId));
        }

        private async void DeleteAgency(Guid agencyId, DataGridViewCellEventArgs e)
        {
            var name = dataGridView1.Rows[e.RowIndex].Cells["AgencyName"]?.Value.ToString();
            var confirmResult = MessageBox.Show($"Da li ste sigurni da želite izbrisati agenciju {name}",
                                  "Da li ste sigurni?",
                                  MessageBoxButtons.YesNo);
            if (confirmResult == DialogResult.Yes)
            {
                await this._apiService.Delete(agencyId);
                using (var dialogForm = new DialogForm("Uspješno ste uredili agenciju", ToastrTypes.Success))
                {
                    dialogForm.ShowDialog(this);
                }
            }
        }

        private void DisplayUpdateForm(Guid agencyId)
        {
            AgencyAddEditForm editForm = new AgencyAddEditForm(agencyId, this);
            editForm.ShowDialog();
        }

        public async Task LoadDataAsync()
        {
            try
            {
                var queryParams = new Dictionary<string, object?>
                {
                    { "currentPage", this.pagination.CurrentPage},
                    { "pageSize", this.pagination.pageSize}
                };
                var result = await _apiService.Get<List<AgencySearchResponse>>("search", queryParams);

                this.pagination.SetTotalPages(result.FirstOrDefault()?.TotalCount);

                dataGridView1.AutoGenerateColumns = false;
                dataGridView1.DataSource = result;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to load data: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void InitialitePagination()
        {
            this.pagination = new PaginationUserControl();
            paginationPanel.Controls.Add(this.pagination);
            this.pagination.Dock = DockStyle.Fill;

            this.pagination.PageChangeClicked += Pagination_PageChanged;
        }

        private void addAgencyBtn_Click(object sender, EventArgs e)
        {
            AgencyAddEditForm agencyAddEditForm = new AgencyAddEditForm(agencyControl: this);
            agencyAddEditForm.ShowDialog();
        }
    }
}

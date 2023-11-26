namespace Iter.Desktop.UserControls.Pagination
{
    public partial class PaginationUserControl : UserControl
    {
        public int pageSize = 3;
        private int currentPage = 1;
        private int? totalPages;

        public PaginationUserControl()
        {
            InitializeComponent();
        }

        public event EventHandler PageChangeClicked;

        public int CurrentPage
        {
            get { return currentPage; }
            set
            {
                currentPage = value;
                UpdatePageLabel();
            }
        }

        public int? TotalPages
        {
            get { return totalPages; }
            set
            {
                totalPages = value;
                UpdatePageLabel();
            }
        }

        public void SetTotalPages(int? totalCount)
        {
            if (totalCount == null)
            {
                this.TotalPages = 0; return;
            }
            var totalPages = totalCount / this.pageSize;

            if (totalCount % this.pageSize != 0)
            {
                totalPages++;
            }

            this.TotalPages = totalPages;
        }
        private void UpdatePageLabel()
        {
            lblPage.Text = $"Page {currentPage} of {totalPages}";
        }

        private void left_arrow_icon_Click(object sender, EventArgs e)
        {
            if (currentPage > 1)
            {
                currentPage--;
                PageChangeClicked?.Invoke(this, EventArgs.Empty);
                UpdatePageLabel();
            }
        }

        private void right_arrow_icon_Click(object sender, EventArgs e)
        {
            if (currentPage < totalPages)
            {
                currentPage++;
                PageChangeClicked?.Invoke(this, EventArgs.Empty);
                UpdatePageLabel();
            }
        }
    }
}

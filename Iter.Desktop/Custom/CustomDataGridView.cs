namespace Iter.Desktop.Custom
{
    public class CustomDataGridView: DataGridView
    {
        public CustomDataGridView()
        {
            this.CellMouseMove += CustomDataGridView_CellMouseMove;
            this.CellToolTipTextNeeded += CustomDataGridView_CellToolTipTextNeeded;
        }

        private void CustomDataGridView_CellMouseMove(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                string colname = this.Columns[e.ColumnIndex].Name;

                if (colname != "Edit" && colname != "Details" && colname != "Delete")
                {
                    this.Cursor = Cursors.Default;
                }
                else
                {
                    this.Cursor = Cursors.Hand;
                }
            }
        }

        private void CustomDataGridView_CellToolTipTextNeeded(object sender, DataGridViewCellToolTipTextNeededEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0 && this.Columns[e.ColumnIndex] is DataGridViewImageColumn)
            {
                var col = this.Columns[e.ColumnIndex].Name;
                if (col == "Edit")
                {
                    e.ToolTipText = "Uredi";
                }
                if (col == "Details")
                {
                    e.ToolTipText = "Detalji";
                }
                if (col == "Delete")
                {
                    e.ToolTipText = "Obriši";
                }
            }
        }
    }
}

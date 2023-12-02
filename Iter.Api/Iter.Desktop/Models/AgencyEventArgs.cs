namespace Iter.Desktop.Models
{
    public class AgencyEventArgs : EventArgs
    {
        public Guid AgencyId { get; }

        public AgencyEventArgs(Guid agencyId)
        {
            AgencyId = agencyId;
        }
    }
}

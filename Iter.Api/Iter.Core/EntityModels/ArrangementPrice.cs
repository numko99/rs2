namespace Iter.Core.EntityModels
{
    public class ArrangementPrice
    {
        public Guid Id { get; set; }

        public Guid ArrangementId { get; set; }

        public Arrangement Arrangement { get; set; }

        public string AccommodationType { get; set; }


        public bool IsDeleted { get; set; }

        public DateTime DateCreated { get; set; }

        public DateTime DateModified { get; set; }
    }
}

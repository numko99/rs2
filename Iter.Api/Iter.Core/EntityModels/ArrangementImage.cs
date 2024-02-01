namespace Iter.Core.EntityModels
{
    public class ArrangementImage
    {
        public Guid Id { get; set; }

        public Guid ArrangementId { get; set; }

        public Arrangement Arrangement { get; set; }

        public Guid ImageId { get; set; }

        public Image Image { get; set; }

        public bool IsMainImage { get; set; }


        public bool IsDeleted { get; set; }

        public DateTime DateCreated { get; set; }

        public DateTime DateModified { get; set; }
    }
}

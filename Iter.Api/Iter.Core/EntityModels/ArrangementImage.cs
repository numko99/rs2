namespace Iter.Core.EntityModels
{
    public class ArrangementImage : BaseEntity
    {
        public Guid ArrangementId { get; set; }

        public Arrangement Arrangement { get; set; }

        public Guid ImageId { get; set; }

        public Image Image { get; set; }

        public bool IsMainImage { get; set; }

    }
}

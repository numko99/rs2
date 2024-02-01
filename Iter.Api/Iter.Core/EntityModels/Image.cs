namespace Iter.Core.EntityModels
{
    public class Image
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

        public byte[]? ImageContent { get; set; }

        public byte[]? ImageThumb { get; set; }

        public List<ArrangementImage> ArrangementImages { get; set; }
    }
}

namespace Iter.Model
{
    public class ImageResponse
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public byte[]? Image { get; set; }

        public bool IsMainImage { get; set; }

        //public byte[]? ImageThumb { get; set; }
    }
}

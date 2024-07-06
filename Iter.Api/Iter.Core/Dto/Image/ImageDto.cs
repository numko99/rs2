namespace Iter.Core.Dto
{
    public class ImageDto
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public byte[]? Image { get; set; }

        public bool IsMainImage { get; set; }

    }
}

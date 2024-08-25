namespace Iter.Model
{
    public class ImageUpsertRequest
    {
        public string? Id { get; set; }

        public string Name { get; set; }

        public byte[]? Image { get; set; }

        //public byte[]? ImageThumb { get; set; }
    }
}

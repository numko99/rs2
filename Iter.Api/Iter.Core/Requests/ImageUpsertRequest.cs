namespace Iter.Core.Requests
{
    public class ImageUpsertRequest
    {
        public string Name { get; set; }

        public byte[]? Image { get; set; }

        //public byte[]? ImageThumb { get; set; }
    }
}

using Iter.Core.EntityModels;

namespace Iter.Core.Models
{
    public class Country
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public List<City> Cities { get; set; }
    }
}

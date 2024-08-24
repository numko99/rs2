using Iter.Core.RequestParameterModels;

namespace Iter.Core.Search_Models
{
    public class CitySearchModel: BaseSearchModel
    {
        public string? Name { get; set; }

        public string? CountryId { get; set; }
    }
}

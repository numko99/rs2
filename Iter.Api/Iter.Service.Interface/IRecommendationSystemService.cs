using Iter.Core;

namespace Iter.Services.Interface
{
    public interface IRecommendationSystemService
    {
        Task<List<ArrangementSearchResponse>> RecommendArrangement(Guid arrangementId);
    }
}

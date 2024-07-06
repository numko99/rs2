using AutoMapper;
using Iter.Core.Models;

public class PagedResultConverter<TSource, TDestination> : ITypeConverter<PagedResult<TSource>, PagedResult<TDestination>>
{
    public PagedResult<TDestination> Convert(PagedResult<TSource> source, PagedResult<TDestination> destination, ResolutionContext context)
    {
        var mappedItems = context.Mapper.Map<IEnumerable<TDestination>>(source.Result);

        return new PagedResult<TDestination>
        {
            Result = mappedItems.ToList(),
            Count = source.Count
        };
    }
}
using AutoMapper;
using RealTimeEmployee.DataAccess.Models;

namespace RealTimeEmployee.BusinessLogic.Profiles;

public static class MappingExtensions
{
    public static PaginatedResult<TDestination> ToPaginatedResult<TSource, TDestination>(
        this PaginatedResult<TSource> source,
        IMapper mapper)
        where TSource : class
        where TDestination : class
    {
        var items = mapper.Map<IEnumerable<TDestination>>(source.Items);

        return new PaginatedResult<TDestination>(
            items,
            source.TotalCount,
            source.PageNumber,
            source.PageSize);
    }
}

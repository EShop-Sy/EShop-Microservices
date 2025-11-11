namespace BuildingBlocks.Pagination;

public class PaginatedResult<TEntity>(Guid lastId, int pageSize, long count, IEnumerable<TEntity> data)
    where TEntity : class
{
    public Guid LastId { get; } = lastId;

    public int PageSize { get; } = pageSize;

    public long Count { get; } = count;

    public IEnumerable<TEntity> Data { get; } = data;
}

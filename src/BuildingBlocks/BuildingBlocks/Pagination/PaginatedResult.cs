using System.Text.Json.Serialization;

namespace BuildingBlocks.Pagination;

[method: JsonConstructor]
public class PaginatedResult<TEntity>(int? pageNumber, long pageSize, long total, IReadOnlyList<TEntity> data)
    where TEntity : class
{
    [JsonPropertyName("lastId")] public int? PageNumber { get; init; } = pageNumber;

    [JsonPropertyName("pageSize")] public long PageSize { get; init; } = pageSize;

    [JsonPropertyName("total")] public long Total { get; init; } = total;

    [JsonPropertyName("data")] public IReadOnlyList<TEntity> Data { get; init; } = data;
}

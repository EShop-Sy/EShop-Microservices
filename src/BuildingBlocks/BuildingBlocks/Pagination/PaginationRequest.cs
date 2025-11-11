namespace BuildingBlocks.Pagination;

public record PaginationRequest(Guid LastId, int PageSize);

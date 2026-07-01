namespace CRUD.DTOs;

public class PagedResult<T>
{
    public List<T> Data { get; set; } = new();
    public PaginationMetadata Metadata { get; set; } = new();
}

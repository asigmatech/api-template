namespace AsigmaApiTemplate.Api.Helpers;

public class PaginationMetadata
{
    public double TotalItems { get; set; }

    public int PageSize { get; set; }

    public int CurrentPage { get; set; }

    public double TotalPages { get; set; }

    public bool HasNextPage { get; set; }

    public bool HasPreviousPage { get; set; }
}

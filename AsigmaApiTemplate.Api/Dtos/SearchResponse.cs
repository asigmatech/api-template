using AsigmaApiTemplate.Api.Helpers;

namespace AsigmaApiTemplate.Api.Dtos;

public class SearchResponse<T> where T : class
{
    public List<T> Data { get; set; } = [];

    public required PaginationMetadata PaginationMetadata { get; set; }
}
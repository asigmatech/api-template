using AsigmaApiTemplate.Api.Helpers;

namespace AsigmaApiTemplate.Api.Dtos;

public class SearchResponse<T> where T : class
{
    public ICollection<T> Data { get; set; }
    
    public PaginationMetadata PaginationMetadata { get; set; }  
}

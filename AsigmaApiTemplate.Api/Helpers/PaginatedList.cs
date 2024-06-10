using Microsoft.EntityFrameworkCore;

namespace AsigmaApiTemplate.Api.Helpers;

public class PaginatedList<T> : List<T>
{
    public int Page { get; private set; }
    public int PageSize { get; set; }
    public int TotalPages { get; private set; }
    public int TotalCount { get; private set; }

    public PaginatedList(List<T> items, int count, int page, int pageSize, int totalCount)
    {
        Page = page;
        TotalPages = (int)Math.Ceiling(count / (double)pageSize);
        PageSize = pageSize;
        TotalCount = totalCount;
        AddRange(items);
    }

    public bool HasPreviousPage => Page > 1;
    public bool HasNextPage => Page < TotalPages;

    public static async Task<PaginatedList<T>> CreateAsync(IQueryable<T> source, int page, int pageSize, int totalCount)
    {
        var count = await source.CountAsync();
        var items = await source.Skip((page - 1) * pageSize).Take(pageSize)
            .ToListAsync();
        return new PaginatedList<T>(items, count, page, pageSize, totalCount);
    }
     public static async Task<PaginatedList<T>> CreateAsync(IEnumerable<T> source, int page, int pageSize, int totalCount)
    {
        var enumerable = source as T[] ?? source.ToArray();
        var count = await Task.Run(enumerable.Count);
        var items = await Task.Run(() => enumerable.Skip((page - 1) * pageSize).Take(pageSize).ToList());
        return new PaginatedList<T>(items, count, page, pageSize, totalCount);
    }
}
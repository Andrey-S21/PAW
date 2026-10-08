namespace PAW.Web.Models;

/// <summary>Información mínima que necesita el partial de paginación.</summary>
public interface IPagedInfo
{
    int Page { get; }
    int TotalPages { get; }
    int TotalItems { get; }
}

/// <summary>Lista paginada (máximo 25 elementos por página por defecto).</summary>
public class PagedList<T> : IPagedInfo
{
    public const int DefaultPageSize = 25;

    public IReadOnlyList<T> Items { get; }
    public int Page { get; }
    public int PageSize { get; }
    public int TotalItems { get; }
    public int TotalPages { get; }

    public PagedList(IEnumerable<T> source, int page, int pageSize = DefaultPageSize)
    {
        var all = source?.ToList() ?? new List<T>();
        PageSize = pageSize < 1 ? DefaultPageSize : pageSize;
        TotalItems = all.Count;
        TotalPages = Math.Max(1, (int)Math.Ceiling(TotalItems / (double)PageSize));
        Page = Math.Min(Math.Max(page, 1), TotalPages);
        Items = all.Skip((Page - 1) * PageSize).Take(PageSize).ToList();
    }

    public static PagedList<T> Create(IEnumerable<T> source, int page) => new(source, page);
}

namespace BugStore.Requests.Products;

public class Get
{
    public string? TitleContains { get; set; }
    public int Page { get; set; } = 1;  
    public int PageSize { get; set; } = 20; 
    public int Skip => (Page <= 1) ? 0 : (Page - 1) * PageSize;

}
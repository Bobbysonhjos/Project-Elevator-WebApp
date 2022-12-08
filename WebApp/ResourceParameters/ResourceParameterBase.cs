namespace WebApp.ResourceParameters;

public abstract class ResourceParameterBase
{
    protected virtual int MaxPageSize { get; } = 20;
    private int _pageSize = 10;
    private int _currentPage = 1;
    public int CurrentPage
    {
        get => _currentPage;
        set => _currentPage = value < 1 ? 1 : value;
    }
    public virtual int PageSize
    {
        get => _pageSize;
        set => _pageSize = value < 1 ? 10 : value > MaxPageSize ? MaxPageSize : value;
    }
}
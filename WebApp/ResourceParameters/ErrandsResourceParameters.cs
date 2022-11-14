namespace WebApp.ResourceParameters;

public class ErrandsResourceParameters : ResourceParameterBase
{
    private string? _filter;
    public string? Filter
    {
        get => _filter;
        set => _filter = value?.ToLower().Trim() == "none" ? null: value;
    }
    public string? OrderBy { get; set; }
    public string? OrderDirection { get; set; }
}
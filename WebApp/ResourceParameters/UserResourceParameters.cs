namespace WebApp.ResourceParameters
{
    public class UserResourceParameters: ResourceParameterBase
    {
        private string? _filter;
        private string? _orderDirection;
        public string? Filter
        {
            get => _filter;
            set => _filter = value?.ToLower().Trim() == "none" ? null : value;
        }
        public string? OrderDirection
        {
            get => _orderDirection;
            set => _orderDirection = value?.Trim().ToLower() == "asc" ? "asc" : "desc" ?? "asc";
        }
        public string? OrderBy { get; set; }
        public string? SearchQuery { get; set; }
    }
}
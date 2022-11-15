namespace WebApp.ResourceParameters
{
    public class ErrandWithCommentsResourceParameters : ResourceParameterBase
    {
        private int _pageSize = 3;
        protected override int MaxPageSize { get; } = 10;
        public override int PageSize
        {
            get => _pageSize;
            set => _pageSize = value < 1 ? 10 : value > MaxPageSize ? MaxPageSize : value;
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebApp.Models;

namespace WebApp.Pages.Statistic
{
    [BindProperties]
    public class IndexModel : PageModel
    {
        public List<StatisticDto> Statistics { get; set; }
        public List<int> TestList { get; set; } = new List<int>()
        {
            21345,31345,11345,21345,31345
        };


        public IndexModel()
        {
            GetStatisticsAsync().ConfigureAwait(false);

        }
        public async Task GetStatisticsAsync()
        {
            using var http = new HttpClient();
            var result = await http.GetFromJsonAsync<List<StatisticDto>>("https://project-elevator-api.azurewebsites.net/api/statistic");
            Statistics = result;
        }
    }

}

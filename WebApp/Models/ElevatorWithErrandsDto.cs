using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApp.Models
{
    public class ElevatorWithErrandsDto
    {
        public Guid Id { get; set; }
        public string Location { get; set; } = null!;
        public string ElevatorStatus { get; set; } = null!;
        public DateTime CreatedDateUtc { get; set; }
        public IList<ErrandDto> Errands { get; set; } = new List<ErrandDto>();
    }
}

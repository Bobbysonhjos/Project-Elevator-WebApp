using AutoMapper;
using WebApp.Models;
using WebApp.Pages.Elevator;

namespace WebApp.Helpers.AutoMapper
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            // Elevator Folder Viewmodel mappings
            CreateMap<ElevatorDto, IndexModel.ElevatorViewModel>();
            CreateMap<ErrandDto, DetailsModel.ErrandViewModel>();
            CreateMap<ElevatorWithErrandsDto, DetailsModel.ElevatorViewModel>();


            //Errand folder
            CreateMap<ErrandDto, Pages.Errands.IndexModel.ErrandViewModel>();
        }
    }
}

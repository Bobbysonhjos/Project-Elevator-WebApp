﻿using AutoMapper;
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
            CreateMap<ElevatorDto, DetailsModel.ElevatorViewModel>();
            CreateMap<ErrandDto, DetailsModel.ErrandViewModel>();
        }
    }
}
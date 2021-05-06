﻿using AutoMapper;
using HotelReservation.Business.Models;
using HotelReservation.Data.Entities;

namespace HotelReservation.Business
{
    public class ModelEntityMapperProfile : Profile
    {
        public ModelEntityMapperProfile()
        {
            CreateMap<HotelEntity, HotelModel>();
            CreateMap<LocationEntity, LocationModel>();

            CreateMap<CompanyEntity, CompanyModel>()
                .ReverseMap();

            CreateMap<RoomEntity, RoomModel>();
        }
    }
}

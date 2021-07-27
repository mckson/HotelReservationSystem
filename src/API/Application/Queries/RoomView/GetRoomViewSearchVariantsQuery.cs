﻿using HotelReservation.API.Models.ResponseModels;
using HotelReservation.Data.Filters;
using MediatR;
using System.Collections.Generic;

namespace HotelReservation.API.Application.Queries.RoomView
{
    public class GetRoomViewSearchVariantsQuery : IRequest<IEnumerable<RoomViewPromptResponseModel>>
    {
        public RoomViewsFilter RoomViewsFilter { get; set; }
    }
}

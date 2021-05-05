using AutoMapper;
using HotelReservation.Data.Entities;

namespace HotelReservation.API.Mappers
{
    public class HotelMapper /*: IMapper<HotelEntity, HotelResponseModel, HotelRequestModel>*/
    {
        // private readonly Mapper _mapper;
        // public HotelMapper()
        // {
        //     var configuration = new MapperConfiguration(cfg =>
        //     {
        //         cfg.CreateMap<HotelEntity, HotelResponseModel>()
        //             .ForMember("HotelName", opt => opt.MapFrom(entity => entity.Name))
        //             .ForMember(
        //                 "LocationUrl",
        //                 // opt => opt.MapFrom(entity => locationMapper.EntityToResponse(entity.Location)))
        //                 opt => opt.MapFrom(entity => $"https://localhost:5001/api/Hotels/{entity.Id}/Location"))
        //             .ForMember(
        //                 "RoomsUrl",
        //                 // opt => opt.MapFrom(entity => entity.Rooms.Select(r => roomMapper.EntityToResponse(r))))
        //                 opt => opt.MapFrom(entity => $"https://localhost:5001/api/Hotels/{entity.Id}/Rooms"))
        //             .ForMember("CompanyUrl", opt => opt.MapFrom(entity => $"https://localhost:5001/api/Hotels/{entity.Id}/Company"));
        //         cfg.CreateMap<HotelRequestModel, HotelEntity>()
        //             .ForMember("Name", opt => opt.MapFrom(request => request.HotelName));
        //     });
        //     _mapper = new Mapper(configuration);
        // }
        // public HotelResponseModel EntityToResponse(HotelEntity entityModel)
        // {
        //     return _mapper.Map<HotelEntity, HotelResponseModel>(entityModel);
        // }
        // public HotelResponseModel RequestToResponse(HotelRequestModel requestModel)
        // {
        //     throw new System.NotImplementedException();
        // }
        // public HotelEntity RequestToEntity(HotelRequestModel requestModel)
        // {
        //     return _mapper.Map<HotelRequestModel, HotelEntity>(requestModel);
        // }
    }
}

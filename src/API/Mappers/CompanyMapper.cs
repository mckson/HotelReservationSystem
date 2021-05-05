using System.Linq;
using AutoMapper;
using HotelReservation.Data.Entities;

namespace HotelReservation.API.Mappers
{
    public class CompanyMapper/* : IMapper<CompanyEntity, CompanyResponseModel, CompanyRequestModel>*/
    {
        /*private readonly Mapper _mapper;

        public CompanyMapper()
        {
            var configuration = new MapperConfiguration(
                cfg =>
                {
                    cfg.CreateMap<CompanyEntity, CompanyResponseModel>()
                        .ForMember(
                            "CompanyName",
                            opt => opt.MapFrom(entity => entity.Title))
                        .ForMember(
                            "HotelsUrls",
                            opt => opt.MapFrom(entity =>
                                entity.Hotels.Select(hotel => $"https://localhost:5001/api/Hotels/{hotel.Id}")));
                });

            _mapper = new Mapper(configuration);
        }

        public CompanyResponseModel EntityToResponse(CompanyEntity entityModel)
        {
            return _mapper.Map<CompanyEntity, CompanyResponseModel>(entityModel);
        }

        public CompanyResponseModel RequestToResponse(CompanyRequestModel requestModel)
        {
            throw new System.NotImplementedException();
        }

        public CompanyEntity RequestToEntity(CompanyRequestModel requestModel)
        {
            throw new System.NotImplementedException();
        }*/
    }
}

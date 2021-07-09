using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using HotelReservation.API.Application.Queries.Reservation;
using HotelReservation.API.Models.ResponseModels;
using HotelReservation.Business;
using HotelReservation.Business.Interfaces;
using HotelReservation.Data.Interfaces;
using MediatR;
using Serilog;

namespace HotelReservation.API.Application.Handlers.Reservation
{
    public class GetReservationByIdHandler : IRequestHandler<GetReservationByIdQuery, ReservationResponseModel>
    {
        private readonly IReservationRepository _reservationRepository;
        private readonly IManagementPermissionSupervisor _supervisor;
        private readonly ILogger _logger;
        private readonly IMapper _mapper;

        public GetReservationByIdHandler(
            IReservationRepository reservationRepository,
            ILogger logger,
            IMapper mapper,
            IManagementPermissionSupervisor supervisor)
        {
            _reservationRepository = reservationRepository;
            _logger = logger;
            _mapper = mapper;
            _supervisor = supervisor;
        }

        public async Task<ReservationResponseModel> Handle(GetReservationByIdQuery request, CancellationToken cancellationToken)
        {
            _logger.Debug($"Reservation {request.Id} is requesting");

            var reservationEntity = await _reservationRepository.GetAsync(request.Id) ??
                                    throw new BusinessException($"No reservation with such id: {request.Id}", ErrorStatus.NotFound);

            _supervisor.CheckReservationManagementPermission(reservationEntity.Email);

            var reservationResponse = _mapper.Map<ReservationResponseModel>(reservationEntity);

            _logger.Debug($"Reservation {request.Id} requested");

            return reservationResponse;
        }
    }
}

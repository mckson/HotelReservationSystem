using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using HotelReservation.API.Application.Commands.Reservation;
using HotelReservation.API.Models.ResponseModels;
using HotelReservation.Business;
using HotelReservation.Business.Interfaces;
using HotelReservation.Data.Interfaces;
using MediatR;
using Serilog;

namespace HotelReservation.API.Application.Handlers.Reservation
{
    public class DeleteReservationHandler : IRequestHandler<DeleteReservationCommand, ReservationBriefResponseModel>
    {
        private readonly IReservationRepository _reservationRepository;
        private readonly IManagementPermissionSupervisor _supervisor;
        private readonly ILogger _logger;
        private readonly IMapper _mapper;

        public DeleteReservationHandler(IReservationRepository reservationRepository, IManagementPermissionSupervisor supervisor, ILogger logger, IMapper mapper)
        {
            _reservationRepository = reservationRepository;
            _supervisor = supervisor;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<ReservationBriefResponseModel> Handle(DeleteReservationCommand request, CancellationToken cancellationToken)
        {
            _logger.Debug($"Reservation {request.Id} is deleting");

            var checkReservationEntity = await _reservationRepository.GetAsync(request.Id) ??
                                         throw new BusinessException(
                                             $"No reservation with such id: {request.Id}",
                                             ErrorStatus.NotFound);

            _supervisor.CheckReservationManagementPermission(checkReservationEntity.Email);

            var deletedReservationEntity = await _reservationRepository.DeleteAsync(request.Id);
            var deletedReservationResponse = _mapper.Map<ReservationBriefResponseModel>(deletedReservationEntity);

            _logger.Debug($"Reservation {request.Id} deleted");

            return deletedReservationResponse;
        }
    }
}

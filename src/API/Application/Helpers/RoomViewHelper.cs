using HotelReservation.API.Application.Interfaces;
using HotelReservation.Data.Interfaces;
using System.Linq;

namespace HotelReservation.API.Application.Helpers
{
    public class RoomViewHelper : IRoomViewHelper
    {
        private readonly IRoomViewRepository _roomViewRepository;

        public RoomViewHelper(IRoomViewRepository roomViewRepository)
        {
            _roomViewRepository = roomViewRepository;
        }

        public bool IsNameAvailable(string roomViewName)
        {
            var isNameAvailable = true;
            var roomViewEntity = _roomViewRepository.Find(view =>
                view.Name.ToUpper().Equals(roomViewName.ToUpper())).FirstOrDefault();

            if (roomViewEntity != null)
            {
                isNameAvailable = false;
            }

            return isNameAvailable;
        }
    }
}

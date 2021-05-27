using System.Threading.Tasks;
using HotelReservation.API.Helpers;
using HotelReservation.Business.Interfaces;
using HotelReservation.Business.Services;
using HotelReservation.Data.Entities;
using HotelReservation.Data.Interfaces;
using HotelReservation.Data.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace HotelReservation.API.Extensions
{
    public static class ConfigureServicesExtensions
    {
        public static IServiceCollection AddDataAndBusiness(this IServiceCollection services)
        {
            services.AddScoped<IHotelRepository, HotelRepository>();
            services.AddScoped<ILocationRepository, LocationRepository>();
            services.AddScoped<IRepository<RoomEntity>, RoomRepository>();
            services.AddScoped<IRepository<ReservationEntity>, ReservationRepository>();
            services.AddScoped<IRepository<ServiceEntity>, ServiceRepository>();

            services.AddScoped<IHotelsService, HotelsService>();
            services.AddScoped<IUsersService, UsersService>();
            services.AddScoped<IRoomsService, RoomsService>();
            services.AddScoped<IServicesService, ServicesService>();
            services.AddScoped<IReservationsService, ReservationsService>();

            return services;
        }
    }
}

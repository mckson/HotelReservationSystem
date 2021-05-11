using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using HotelReservation.Business;
using Microsoft.AspNetCore.Http;

namespace HotelReservation.API.Middleware
{
    public class ErrorHandlerMiddleware
    {
        private readonly RequestDelegate _next;

        public ErrorHandlerMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception error)
            {
                var response = context.Response;
                response.ContentType = "application/json";

                switch (error)
                {
                    case BusinessException exception:
                        response.StatusCode = exception.Status switch
                        {
                            ErrorStatus.NotFound => (int)HttpStatusCode.NotFound,
                            ErrorStatus.AlreadyExist => (int)HttpStatusCode.Conflict,
                            ErrorStatus.IncorrectInput => (int)HttpStatusCode.UnprocessableEntity,
                            ErrorStatus.EmptyInput => (int)HttpStatusCode.UnsupportedMediaType,
                            _ => (int)HttpStatusCode.BadRequest
                        };
                        break;
                    default:
                        response.StatusCode = (int)HttpStatusCode.InternalServerError;
                        break;
                }

                var result = JsonSerializer.Serialize(new { message = error?.Message });
                await response.WriteAsync(result);
            }
        }
    }
}

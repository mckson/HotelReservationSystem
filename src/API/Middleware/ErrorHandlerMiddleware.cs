using FluentValidation;
using HotelReservation.API.Models.ResponseModels;
using HotelReservation.Business;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Serilog;
using System;
using System.Linq;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;

namespace HotelReservation.API.Middleware
{
    public class ErrorHandlerMiddleware
    {
        private readonly RequestDelegate _next;

        public ErrorHandlerMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context, ILogger logger)
        {
            try
            {
                await _next(context);
            }
            catch (Exception error)
            {
                var response = context.Response;
                response.ContentType = "application/json";

                response.StatusCode = error switch
                {
                    BusinessException exception => exception.Status switch
                    {
                        ErrorStatus.NotFound => (int)HttpStatusCode.NotFound,
                        ErrorStatus.AlreadyExist => (int)HttpStatusCode.Conflict,
                        ErrorStatus.IncorrectInput => (int)HttpStatusCode.UnprocessableEntity,
                        ErrorStatus.EmptyInput => (int)HttpStatusCode.UnsupportedMediaType,
                        ErrorStatus.AccessDenied => (int)HttpStatusCode.Forbidden,
                        _ => (int)HttpStatusCode.BadRequest
                    },
                    ValidationException exception => (int)HttpStatusCode.Forbidden,
                    _ => (int)HttpStatusCode.InternalServerError
                };

                response.HttpContext.Features.Get<IHttpResponseFeature>().ReasonPhrase = error switch
                {
                    BusinessException exception => exception.Message,
                    ValidationException exception => "Validation failed",
                    _ => null
                };

                logger.Error(error, error.Message);

                var result = JsonSerializer.Serialize(new ErrorResponseModel
                {
                    Message = error is ValidationException validationException
                        ? validationException.Errors.First().ErrorMessage
                        : error.Message
                });
                await response.WriteAsync(result);
            }
        }
    }
}

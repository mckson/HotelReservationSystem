using MediatR;
using Serilog;
using System.Threading;
using System.Threading.Tasks;

namespace HotelReservation.API.Application.Behaviors
{
    public class LoggingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
    {
        private readonly ILogger _logger;

        public LoggingBehavior(ILogger logger)
        {
            _logger = logger;
        }

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
           _logger.Information($"Handling {typeof(TRequest).Name}");

           // var type = request.GetType();
           // var props = new List<PropertyInfo>(type.GetProperties())
           // foreach (var prop in props)
           // {
           //     var propValue = prop.GetValue(request, null);
           //     _logger.Information($"{prop.Name} : {propValue}");
           // }
           var response = await next();

           _logger.Information($"Handled {typeof(TResponse).Name}");

           return response;
        }
    }
}

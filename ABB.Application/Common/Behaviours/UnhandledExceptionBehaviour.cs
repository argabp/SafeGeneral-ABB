using System;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Exceptions;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ABB.Application.Common.Behaviours
{
    public class UnhandledExceptionBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
    {
        private readonly ILogger<TRequest> _logger;

        public UnhandledExceptionBehaviour(ILogger<TRequest> logger)
        {
            _logger = logger;
        }

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken,
            RequestHandlerDelegate<TResponse> next)
        {
            var requestName = typeof(TRequest).Name;
            try
            {
                return await next();
            }
            catch (ValidationException ex)
            {
                _logger.LogError(ex, "Error Validation : for Request {Name} {@Request}", requestName, request);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error Unhandle : for Request {Name} {@Request}", requestName, request);

                throw;
            }
        }
    }
}
using Cybtans.Entities;
using Cybtans.Services;
using Grpc.Core;
using Grpc.Core.Interceptors;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Cybtans.Grpc
{
    public class ExceptionHandlerInterceptor : Interceptor
    {
        public class Options
        {
            public bool EnableDetailedErrorMessages { get; set; }
        }

        private readonly ILogger<ExceptionHandlerInterceptor> _logger;
        private readonly Options _options;

        public ExceptionHandlerInterceptor(ILogger<ExceptionHandlerInterceptor> logger, IOptions<Options> options)
        {
            _logger = logger;
            _options = options.Value;
        }


        public override async Task<TResponse> UnaryServerHandler<TRequest, TResponse>(TRequest request, ServerCallContext context, UnaryServerMethod<TRequest, TResponse> continuation)
        {
            try
            {
                return await base.UnaryServerHandler(request, context, continuation).ConfigureAwait(false);
            }
            catch (Exception e)
            {
                throw HandlerException(e, context, request);
            }
        }

        public override async Task ServerStreamingServerHandler<TRequest, TResponse>(TRequest request, IServerStreamWriter<TResponse> responseStream, ServerCallContext context, ServerStreamingServerMethod<TRequest, TResponse> continuation)
        {
            try
            {
                await base.ServerStreamingServerHandler(request, responseStream, context, continuation).ConfigureAwait(false);
            }
            catch (Exception e)
            {
                throw HandlerException(e, context, request);
            }
        }

        public override async Task<TResponse> ClientStreamingServerHandler<TRequest, TResponse>(IAsyncStreamReader<TRequest> requestStream, ServerCallContext context, ClientStreamingServerMethod<TRequest, TResponse> continuation)
        {
            try
            {
                return await base.ClientStreamingServerHandler(requestStream, context, continuation).ConfigureAwait(false);
            }
            catch (Exception e)
            {
                throw HandlerException(e, context);
            }
        }

        public override async Task DuplexStreamingServerHandler<TRequest, TResponse>(IAsyncStreamReader<TRequest> requestStream, IServerStreamWriter<TResponse> responseStream, ServerCallContext context, DuplexStreamingServerMethod<TRequest, TResponse> continuation)
        {
            try
            {
                await base.DuplexStreamingServerHandler(requestStream, responseStream, context, continuation).ConfigureAwait(false);
            }
            catch (Exception e)
            {
                throw HandlerException(e, context);
            }
        }

        private Exception HandlerException(Exception e, ServerCallContext context, object request = null)
        {
            _logger.LogError(e, "{Method} ({Req})", context.Method, request);

            return e switch
            {
                RpcException => e,
                ArgumentException => new RpcException(new Status(StatusCode.InvalidArgument, e.Message, e)),
                EntityNotFoundException => new RpcException(new Status(StatusCode.NotFound, e.Message, e)),
                FileNotFoundException => new RpcException(new Status(StatusCode.NotFound, e.Message, e)),
                UnauthorizedAccessException => new RpcException(new Status(StatusCode.PermissionDenied, e.Message, e)),
                ValidationException ve => new RpcException(new Status(StatusCode.InvalidArgument, e.Message), ve.ToValidationMetadata()),
                NotImplementedException => new RpcException(new Status(StatusCode.Unimplemented, e.Message, e)),
                _ => new RpcException(new Status(StatusCode.Internal, GetErrorMessage(e, context), e)),
            };
        }

        private string GetErrorMessage(Exception e, ServerCallContext context)
        {
            if (!_options.EnableDetailedErrorMessages)
                return $"An error ocurred in {context.Method}";

            if (e.InnerException == null)
                return e.Message;

            StringBuilder sb = new StringBuilder(e.Message);
            sb.AppendLine();
            for (var inner = e.InnerException; inner != null; inner = inner.InnerException)
            {
                sb.AppendLine("------- Inner Exception -------");
                sb.Append(inner.Message);
                sb.AppendLine();
            }

            return sb.ToString();
        }
    }
}

using Cybtans.Validations;
using FluentValidation;
using FluentValidation.Results;
using Grpc.Core;
using Grpc.Core.Interceptors;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cybtans.Grpc
{
    public class ValidationInterceptor : Interceptor
    {
        private readonly IValidatorProvider _validatorProvider;

        public ValidationInterceptor(IValidatorProvider validatorProvider)
        {
            _validatorProvider = validatorProvider;
        }

        public override Task<TResponse> UnaryServerHandler<TRequest, TResponse>(TRequest request, ServerCallContext context, UnaryServerMethod<TRequest, TResponse> continuation)
        {
            ValidateRequest(request);
            return base.UnaryServerHandler(request, context, continuation);
        }

        public override Task ServerStreamingServerHandler<TRequest, TResponse>(TRequest request, IServerStreamWriter<TResponse> responseStream, ServerCallContext context, ServerStreamingServerMethod<TRequest, TResponse> continuation)
        {
            ValidateRequest(request);
            return base.ServerStreamingServerHandler(request, responseStream, context, continuation);
        }

        public override Task<TResponse> ClientStreamingServerHandler<TRequest, TResponse>(IAsyncStreamReader<TRequest> requestStream, ServerCallContext context, ClientStreamingServerMethod<TRequest, TResponse> continuation)
        {
            var validator = GetValidator<TRequest>();
            if (validator == null)
                return base.ClientStreamingServerHandler(requestStream, context, continuation);

            var validatingRequestStream = new ValidatingStreamReader<TRequest>(requestStream, request => ValidateRequest(request, validator));
            return base.ClientStreamingServerHandler(validatingRequestStream, context, continuation);
        }

        public override Task DuplexStreamingServerHandler<TRequest, TResponse>(IAsyncStreamReader<TRequest> requestStream, IServerStreamWriter<TResponse> responseStream, ServerCallContext context, DuplexStreamingServerMethod<TRequest, TResponse> continuation)
        {
            var validator = GetValidator<TRequest>();
            if (validator == null)
                return base.DuplexStreamingServerHandler(requestStream, responseStream, context, continuation);

            var validatingRequestStream = new ValidatingStreamReader<TRequest>(requestStream, request => ValidateRequest(request, validator));
            return base.DuplexStreamingServerHandler(validatingRequestStream, responseStream, context, continuation);
        }

        private void ValidateRequest<TRequest>(TRequest request) where TRequest : class
        {
            var validator = GetValidator<TRequest>();
            if (validator != null)
            {
                ValidateRequest(request, validator);
            }
        }

        private static void ValidateRequest<TRequest>(TRequest request, IValidator<TRequest> validator) where TRequest : class
        {
            ValidationResult results = validator.Validate(request);

            if (!results.IsValid && results.Errors.Any())
            {
                var message = HandleErrors(results.Errors);
                var validationMetadata = results.Errors.ToValidationMetadata();
                throw new RpcException(new Status(StatusCode.InvalidArgument, message), validationMetadata);
            }
        }

        private IValidator<TRequest> GetValidator<TRequest>()
             where TRequest : class
        {
            _validatorProvider.TryGetValidator<TRequest>(out var validator);
            return validator;
        }

        private static string HandleErrors(IList<ValidationFailure> failures)
        {
            var errors = failures
                .Select(f => $"Property {f.PropertyName} failed validation.")
                .ToList();

            return string.Join("\n", errors);
        }

    }
}

using Cybtans.Grpc;
using Cybtans.Tests.Services.Models;
using Cybtans.Validations;
using FluentValidation;
using Grpc.Core;
using Moq;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Cybtans.Tests.Services
{
    public class ValidationInterceptorTest
    {
        private ValidationInterceptor _interceptor;

        public class RequestValidator : AbstractValidator<Request>
        {
            public RequestValidator()
            {
                RuleFor(x => x.Id).NotEmpty();
                RuleFor(x => x.Email).NotEmpty().EmailAddress();
                RuleFor(x => x.Number).GreaterThan(0);
            }
        }

        public ValidationInterceptorTest()
        {
            var validatorProvider = new DefaultValidatorProvider();
            validatorProvider.AddValidatorFromAssembly(typeof(ValidationInterceptorTest).Assembly);

            _interceptor = new ValidationInterceptor(validatorProvider);
        }

        [Fact]
        public async Task UnaryServerHandler()
        {
            var contextMock = new Mock<ServerCallContext>();

            var ex = await Assert.ThrowsAsync<RpcException>(() => _interceptor.UnaryServerHandler(new Request(), contextMock.Object,
            (Request request, ServerCallContext context) =>
            {
                return Task.FromResult(new Response());
            }));

            AssertException(ex);
        }

        [Fact]
        public async Task ServerStreamingServerHandler()
        {
            var contextMock = new Mock<ServerCallContext>();
            var streamRespMock = new Mock<IServerStreamWriter<Response>>();
            var ex = await Assert.ThrowsAsync<RpcException>(() => _interceptor.ServerStreamingServerHandler(new Request(), streamRespMock.Object, contextMock.Object,
            async (Request request, IServerStreamWriter<Response> respose, ServerCallContext context) =>
            {
                await respose.WriteAsync(new Response());
            }));

            AssertException(ex);
        }

        [Fact]
        public async Task ClientStreamingServerHandler()
        {
            var contextMock = new Mock<ServerCallContext>();
            var streamMock = new Mock<IAsyncStreamReader<Request>>();

            int i = 0;
            streamMock.Setup(x => x.MoveNext(It.IsAny<CancellationToken>())).ReturnsAsync(i++ < 1);
            streamMock.Setup(x => x.Current).Returns(new Request());

            var ex = await Assert.ThrowsAsync<RpcException>(() => _interceptor.ClientStreamingServerHandler(streamMock.Object, contextMock.Object,
            async (IAsyncStreamReader<Request> requestStream, ServerCallContext context) =>
            {
                List<Request> items = new List<Request>();
                await foreach (var item in requestStream.ReadAllAsync())
                {
                    items.Add(item);
                }

                return new Response();
            }));

            AssertException(ex);
        }

        [Fact]
        public async Task DuplexStreamingServerHandler()
        {
            var contextMock = new Mock<ServerCallContext>();
            var streamRequestMock = new Mock<IAsyncStreamReader<Request>>();
            int i = 0;
            streamRequestMock.Setup(x => x.MoveNext(It.IsAny<CancellationToken>())).ReturnsAsync(i++ < 1);
            streamRequestMock.Setup(x => x.Current).Returns(new Request());


            var streamResponseMock = new Mock<IServerStreamWriter<Response>>();
            var ex = await Assert.ThrowsAsync<RpcException>(() => _interceptor.DuplexStreamingServerHandler(streamRequestMock.Object, streamResponseMock.Object, contextMock.Object,
            async (IAsyncStreamReader<Request> requestStream, IServerStreamWriter<Response> resposeStream, ServerCallContext context) =>
            {
                List<Request> items = new List<Request>();
                await foreach (var item in requestStream.ReadAllAsync())
                {
                    items.Add(item);
                }

                await resposeStream.WriteAsync(new Response());
            }));

            AssertException(ex);
        }


        private static void AssertException(RpcException ex)
        {
            Assert.NotNull(ex);
            Assert.Equal(StatusCode.InvalidArgument, ex.StatusCode);
            Assert.True(ex.HasValidationFailures());

            var failures = ex.GetValidationResult();
            Assert.NotNull(failures);
            Assert.NotEmpty(failures.Errors);                        
        }
    }
}

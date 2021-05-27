using Cybtans.Entities;
using Cybtans.Grpc;
using Cybtans.Services;
using Cybtans.Tests.Services.Models;
using Grpc.Core;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using System;
using System.IO;
using System.Threading.Tasks;
using Xunit;

namespace Cybtans.Tests.Services
{
    public partial class ExceptionHandlerInterceptorTest
    {

        private readonly ExceptionHandlerInterceptor _interceptor;

        public ExceptionHandlerInterceptorTest()
        {
            var loggerMock = new Mock<ILogger<ExceptionHandlerInterceptor>>();            
            
            _interceptor = new ExceptionHandlerInterceptor(loggerMock.Object, Options.Create(new ExceptionHandlerInterceptor.Options
            {
                EnableDetailedErrorMessages = true
            }));
        }

        private static Exception CreateException(string name)
        {
            return name switch
            {
                "RpcException" => new RpcException(new Status(StatusCode.Internal, "InternalError")),
                "ArgumentException" => new ArgumentException("InvalidArgumen"),
                "EntityNotFoundException" => new EntityNotFoundException("Entity not found"),
                "FileNotFoundException" => new FileNotFoundException("file not found"),
                "UnauthorizedAccessException" => new UnauthorizedAccessException("access denied"),
                "NotImplementedException" => new NotImplementedException(),
                "ValidationException" => new ValidationException("access denied")
                        .AddError("Id","not valid"),
                _ => new Exception(name),
            };
        }

        [Theory]
        [InlineData("RpcException", StatusCode.Internal)]
        [InlineData("ArgumentException", StatusCode.InvalidArgument)]
        [InlineData("EntityNotFoundException", StatusCode.NotFound)]
        [InlineData("FileNotFoundException", StatusCode.NotFound)]
        [InlineData("UnauthorizedAccessException", StatusCode.PermissionDenied)]
        [InlineData("ValidationException", StatusCode.InvalidArgument)]
        [InlineData("NotImplementedException", StatusCode.Unimplemented)]
        [InlineData("Not supported", StatusCode.Internal)]
        public async Task UnaryServerHandler(string exception, StatusCode statusCode)
        {
            var expected = CreateException(exception);
            var contextMock = new Mock<ServerCallContext>();

            var ex = await Assert.ThrowsAsync<RpcException>(() => _interceptor.UnaryServerHandler<Request, Response>(new Request(), contextMock.Object,
            (Request request, ServerCallContext context) =>
            {
                throw CreateException(exception);
            }));

            Assert.NotNull(ex);
            Assert.Equal(statusCode, ex.StatusCode);
        }

        [Theory]
        [InlineData("RpcException", StatusCode.Internal)]
        [InlineData("ArgumentException", StatusCode.InvalidArgument)]
        [InlineData("EntityNotFoundException", StatusCode.NotFound)]
        [InlineData("FileNotFoundException", StatusCode.NotFound)]
        [InlineData("UnauthorizedAccessException", StatusCode.PermissionDenied)]
        [InlineData("ValidationException", StatusCode.InvalidArgument)]
        [InlineData("NotImplementedException", StatusCode.Unimplemented)]
        [InlineData("Not supported", StatusCode.Internal)]
        public async Task ServerStreamingServerHandler(string exception, StatusCode statusCode)
        {
            var expected = CreateException(exception);
            var contextMock = new Mock<ServerCallContext>();
            var streamRespMock = new Mock<IServerStreamWriter<Response>>();
            var ex = await Assert.ThrowsAsync<RpcException>(() => _interceptor.ServerStreamingServerHandler<Request, Response>(new Request(), streamRespMock.Object, contextMock.Object,
            (Request request, IServerStreamWriter<Response> respose, ServerCallContext context) =>
            {
                throw CreateException(exception);
            }));

            Assert.NotNull(ex);
            Assert.Equal(statusCode, ex.StatusCode);
        }

        [Theory]
        [InlineData("RpcException", StatusCode.Internal)]
        [InlineData("ArgumentException", StatusCode.InvalidArgument)]
        [InlineData("EntityNotFoundException", StatusCode.NotFound)]
        [InlineData("FileNotFoundException", StatusCode.NotFound)]
        [InlineData("UnauthorizedAccessException", StatusCode.PermissionDenied)]
        [InlineData("ValidationException", StatusCode.InvalidArgument)]
        [InlineData("NotImplementedException", StatusCode.Unimplemented)]
        [InlineData("Not supported", StatusCode.Internal)]
        public async Task ClientStreamingServerHandler(string exception, StatusCode statusCode)
        {
            var expected = CreateException(exception);
            var contextMock = new Mock<ServerCallContext>();
            var streamMock = new Mock<IAsyncStreamReader<Request>>();
            var ex = await Assert.ThrowsAsync<RpcException>(() => _interceptor.ClientStreamingServerHandler<Request, Response>(streamMock.Object, contextMock.Object,
            (IAsyncStreamReader<Request> requestStream, ServerCallContext context) =>
            {
                throw CreateException(exception);
            }));

            Assert.NotNull(ex);
            Assert.Equal(statusCode, ex.StatusCode);
        }

        [Theory]
        [InlineData("RpcException", StatusCode.Internal)]
        [InlineData("ArgumentException", StatusCode.InvalidArgument)]
        [InlineData("EntityNotFoundException", StatusCode.NotFound)]
        [InlineData("FileNotFoundException", StatusCode.NotFound)]
        [InlineData("UnauthorizedAccessException", StatusCode.PermissionDenied)]
        [InlineData("ValidationException", StatusCode.InvalidArgument)]
        [InlineData("NotImplementedException", StatusCode.Unimplemented)]
        [InlineData("Not supported", StatusCode.Internal)]
        public async Task DuplexStreamingServerHandler(string exception, StatusCode statusCode)
        {
            var expected = CreateException(exception);
            var contextMock = new Mock<ServerCallContext>();
            var streamRequestMock = new Mock<IAsyncStreamReader<Request>>();
            var streamResponseMock = new Mock<IServerStreamWriter<Response>>();
            var ex = await Assert.ThrowsAsync<RpcException>(() => _interceptor.DuplexStreamingServerHandler<Request, Response>(streamRequestMock.Object, streamResponseMock.Object, contextMock.Object,
            (IAsyncStreamReader<Request> requestStream, IServerStreamWriter<Response> resposeStream, ServerCallContext context) =>
            {
                throw CreateException(exception);
            }));

            Assert.NotNull(ex);
            Assert.Equal(statusCode, ex.StatusCode);
        }

        [Fact]
        public void ShouldCreateServiceFromProvider()
        {
            var services = new ServiceCollection();

            services.AddLogging();
            
            services.AddSingleton<ExceptionHandlerInterceptor>();

            var sp = services.BuildServiceProvider();
            var interceptor = sp.GetService<ExceptionHandlerInterceptor>();

            Assert.NotNull(interceptor);
        }
    }
}

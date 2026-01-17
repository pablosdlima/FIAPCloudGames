using FIAPCloudGames.Api.Middleware;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.InMemory;

namespace FIAPCloudGames.Presentation.Tests.Middlewares
{
    public class LoggingMiddlewareTests : IDisposable
    {
        private readonly RequestDelegate _proximo;
        private readonly ILogger<LoggingMiddleware> _logger;
        private readonly LoggingMiddleware _middleware;
        private readonly ILoggerFactory _loggerFactory;

        public LoggingMiddlewareTests()
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Verbose()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                .MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Warning)
                .Enrich.FromLogContext()
                .WriteTo.InMemory()
                .CreateLogger();

            _loggerFactory = LoggerFactory.Create(builder =>
            {
                builder.AddSerilog(Log.Logger);
            });

            _proximo = Substitute.For<RequestDelegate>();
            _logger = _loggerFactory.CreateLogger<LoggingMiddleware>();
            _middleware = new LoggingMiddleware(_proximo, _logger);
        }

        public void Dispose()
        {
            _loggerFactory.Dispose();
            Log.CloseAndFlush();
        }

        [Fact]
        public async Task InvocarAssincrono_QuandoRequisicaoBemSucedida_DeveLogarInicioEFim()
        {
            var contexto = new DefaultHttpContext();
            contexto.Request.Method = "GET";
            contexto.Request.Path = "/api/recursos";
            contexto.Connection.RemoteIpAddress = System.Net.IPAddress.Parse("127.0.0.1");
            contexto.Request.Headers["User-Agent"] = "TestAgent";
            contexto.TraceIdentifier = "req-123";
            contexto.Response.StatusCode = StatusCodes.Status200OK;

            await _middleware.InvokeAsync(contexto);

            InMemorySink.Instance.LogEvents.Should().HaveCount(2);

            var logInicio = InMemorySink.Instance.LogEvents.ElementAt(0);
            logInicio.Level.Should().Be(LogEventLevel.Information);
            logInicio.MessageTemplate.Text.Should().Be("Iniciando requisição {RequestMethod} {RequestPath} de {RemoteIP}");

            logInicio.Properties.Should().ContainKey("RemoteIP");
            logInicio.Properties["RemoteIP"].Should().BeEquivalentTo(new ScalarValue("127.0.0.1"));
            logInicio.Properties.Should().ContainKey("RequestMethod");
            logInicio.Properties["RequestMethod"].Should().BeEquivalentTo(new ScalarValue("GET"));
            logInicio.Properties.Should().ContainKey("RequestPath");
            logInicio.Properties["RequestPath"].Should().BeEquivalentTo(new ScalarValue("/api/recursos"));
            logInicio.Properties.Should().ContainKey("CorrelationId");
            logInicio.Properties["CorrelationId"].Should().BeEquivalentTo(new ScalarValue("req-123"));
            logInicio.Properties.Should().ContainKey("UserAgent");
            logInicio.Properties["UserAgent"].Should().BeEquivalentTo(new ScalarValue("TestAgent"));
            logInicio.Properties.Should().ContainKey("RequestId");

            var logFim = InMemorySink.Instance.LogEvents.ElementAt(1);
            logFim.Level.Should().Be(LogEventLevel.Information);
            logFim.MessageTemplate.Text.Should().Be("Requisição {RequestMethod} {RequestPath} finalizada com status {StatusCode} em {ElapsedMs}ms");

            logFim.Properties.Should().ContainKey("RemoteIP");
            logFim.Properties["RemoteIP"].Should().BeEquivalentTo(new ScalarValue("127.0.0.1"));
            logFim.Properties.Should().ContainKey("RequestMethod");
            logFim.Properties["RequestMethod"].Should().BeEquivalentTo(new ScalarValue("GET"));
            logFim.Properties.Should().ContainKey("RequestPath");
            logFim.Properties["RequestPath"].Should().BeEquivalentTo(new ScalarValue("/api/recursos"));
            logFim.Properties.Should().ContainKey("StatusCode");
            logFim.Properties["StatusCode"].Should().BeEquivalentTo(new ScalarValue(StatusCodes.Status200OK));

            var elapsedMsFim = (long)((ScalarValue)logFim.Properties["ElapsedMs"]).Value;
            elapsedMsFim.Should().BeGreaterThanOrEqualTo(0L);

            logFim.Properties.Should().ContainKey("CorrelationId");
            logFim.Properties["CorrelationId"].Should().BeEquivalentTo(new ScalarValue("req-123"));
            logFim.Properties.Should().ContainKey("UserAgent");
            logFim.Properties["UserAgent"].Should().BeEquivalentTo(new ScalarValue("TestAgent"));
            logFim.Properties.Should().ContainKey("RequestId");
        }

        [Fact]
        public async Task InvocarAssincrono_QuandoOcorreExcecao_DeveLogarErro()
        {
            var exception = new Exception("Erro simulado no pipeline.");
            _proximo.Invoke(Arg.Any<HttpContext>()).Returns(Task.FromException(exception));

            var contexto = new DefaultHttpContext();
            contexto.Request.Method = "POST";
            contexto.Request.Path = "/api/erros";
            contexto.Connection.RemoteIpAddress = System.Net.IPAddress.Parse("192.168.1.1");
            contexto.Request.Headers["User-Agent"] = "ErrorAgent";
            contexto.TraceIdentifier = "req-456";

            await Assert.ThrowsAsync<Exception>(() => _middleware.InvokeAsync(contexto));

            InMemorySink.Instance.LogEvents.Should().HaveCount(2);

            var logInicio = InMemorySink.Instance.LogEvents.ElementAt(0);
            logInicio.Level.Should().Be(LogEventLevel.Information);
            logInicio.MessageTemplate.Text.Should().Be("Iniciando requisição {RequestMethod} {RequestPath} de {RemoteIP}");

            logInicio.Properties.Should().ContainKey("RemoteIP");
            logInicio.Properties["RemoteIP"].Should().BeEquivalentTo(new ScalarValue("192.168.1.1"));
            logInicio.Properties.Should().ContainKey("RequestMethod");
            logInicio.Properties["RequestMethod"].Should().BeEquivalentTo(new ScalarValue("POST"));
            logInicio.Properties.Should().ContainKey("RequestPath");
            logInicio.Properties["RequestPath"].Should().BeEquivalentTo(new ScalarValue("/api/erros"));
            logInicio.Properties.Should().ContainKey("CorrelationId");
            logInicio.Properties["CorrelationId"].Should().BeEquivalentTo(new ScalarValue("req-456"));
            logInicio.Properties.Should().ContainKey("UserAgent");
            logInicio.Properties["UserAgent"].Should().BeEquivalentTo(new ScalarValue("ErrorAgent"));
            logInicio.Properties.Should().ContainKey("RequestId");

            var logErro = InMemorySink.Instance.LogEvents.ElementAt(1);
            logErro.Level.Should().Be(LogEventLevel.Error);
            logErro.MessageTemplate.Text.Should().Be("Erro não tratado na requisição {RequestMethod} {RequestPath} após {ElapsedMs}ms");
            logErro.Exception.Should().Be(exception);

            logErro.Properties.Should().ContainKey("RemoteIP");
            logErro.Properties["RemoteIP"].Should().BeEquivalentTo(new ScalarValue("192.168.1.1"));
            logErro.Properties.Should().ContainKey("RequestMethod");
            logErro.Properties["RequestMethod"].Should().BeEquivalentTo(new ScalarValue("POST"));
            logErro.Properties.Should().ContainKey("RequestPath");
            logErro.Properties["RequestPath"].Should().BeEquivalentTo(new ScalarValue("/api/erros"));
            logErro.Properties.Should().ContainKey("UserAgent");
            logErro.Properties["UserAgent"].Should().BeEquivalentTo(new ScalarValue("ErrorAgent"));
            logErro.Properties.Should().ContainKey("RequestId");

            var elapsedMsErro = (long)((ScalarValue)logErro.Properties["ElapsedMs"]).Value;
            elapsedMsErro.Should().BeGreaterThanOrEqualTo(0L);

            logErro.Properties.Should().ContainKey("CorrelationId");
            logErro.Properties["CorrelationId"].Should().BeEquivalentTo(new ScalarValue("req-456"));
        }

        [Fact]
        public async Task InvocarAssincrono_QuandoIPRemotoNulo_DeveLogarCorretamente()
        {
            var contexto = new DefaultHttpContext();
            contexto.Request.Method = "PUT";
            contexto.Request.Path = "/api/configuracoes";
            contexto.Connection.RemoteIpAddress = null;
            contexto.Request.Headers["User-Agent"] = "NoIPAgent";
            contexto.TraceIdentifier = "req-789";
            contexto.Response.StatusCode = StatusCodes.Status200OK;

            await _middleware.InvokeAsync(contexto);

            InMemorySink.Instance.LogEvents.Should().HaveCount(2);

            var logInicio = InMemorySink.Instance.LogEvents.ElementAt(0);
            logInicio.Level.Should().Be(LogEventLevel.Information);
            logInicio.MessageTemplate.Text.Should().Be("Iniciando requisição {RequestMethod} {RequestPath} de {RemoteIP}");

            logInicio.Properties.Should().ContainKey("RemoteIP");
            logInicio.Properties["RemoteIP"].Should().BeEquivalentTo(new ScalarValue(null));
            logInicio.Properties.Should().ContainKey("RequestMethod");
            logInicio.Properties["RequestMethod"].Should().BeEquivalentTo(new ScalarValue("PUT"));
            logInicio.Properties.Should().ContainKey("RequestPath");
            logInicio.Properties["RequestPath"].Should().BeEquivalentTo(new ScalarValue("/api/configuracoes"));
            logInicio.Properties.Should().ContainKey("CorrelationId");
            logInicio.Properties["CorrelationId"].Should().BeEquivalentTo(new ScalarValue("req-789"));
            logInicio.Properties.Should().ContainKey("UserAgent");
            logInicio.Properties["UserAgent"].Should().BeEquivalentTo(new ScalarValue("NoIPAgent"));
            logInicio.Properties.Should().ContainKey("RequestId");

            var logFim = InMemorySink.Instance.LogEvents.ElementAt(1);
            logFim.Level.Should().Be(LogEventLevel.Information);
            logFim.MessageTemplate.Text.Should().Be("Requisição {RequestMethod} {RequestPath} finalizada com status {StatusCode} em {ElapsedMs}ms");

            logFim.Properties.Should().ContainKey("RemoteIP");
            logFim.Properties["RemoteIP"].Should().BeEquivalentTo(new ScalarValue(null));
            logFim.Properties.Should().ContainKey("RequestMethod");
            logFim.Properties["RequestMethod"].Should().BeEquivalentTo(new ScalarValue("PUT"));
            logFim.Properties.Should().ContainKey("RequestPath");
            logFim.Properties["RequestPath"].Should().BeEquivalentTo(new ScalarValue("/api/configuracoes"));
            logFim.Properties.Should().ContainKey("StatusCode");
            logFim.Properties["StatusCode"].Should().BeEquivalentTo(new ScalarValue(StatusCodes.Status200OK));

            var elapsedMsFimNullIp = (long)((ScalarValue)logFim.Properties["ElapsedMs"]).Value;
            elapsedMsFimNullIp.Should().BeGreaterThanOrEqualTo(0L);

            logFim.Properties.Should().ContainKey("CorrelationId");
            logFim.Properties["CorrelationId"].Should().BeEquivalentTo(new ScalarValue("req-789"));
            logFim.Properties.Should().ContainKey("UserAgent");
            logFim.Properties["UserAgent"].Should().BeEquivalentTo(new ScalarValue("NoIPAgent"));
            logFim.Properties.Should().ContainKey("RequestId");
        }
    }
}

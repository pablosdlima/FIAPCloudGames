using FIAPCloudGames.Api.Middlewares;
using FIAPCloudGames.Application.Common.Models;
using FIAPCloudGames.Domain.Exceptions;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using NSubstitute;
using System.Text.Json;

namespace FIAPCloudGames.Presentation.Tests.Middleware
{
    public class ExceptionHandlingMiddlewareTests
    {
        private readonly RequestDelegate _proximo;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;
        private readonly ExceptionHandlingMiddleware _middleware;

        public ExceptionHandlingMiddlewareTests()
        {
            _proximo = Substitute.For<RequestDelegate>();
            _logger = Substitute.For<ILogger<ExceptionHandlingMiddleware>>();
            _middleware = new ExceptionHandlingMiddleware(_proximo, _logger);
        }

        private async Task<ErrorDetails> LerDetalhesErroDaResposta(HttpContext contexto)
        {
            contexto.Response.Body.Seek(0, SeekOrigin.Begin);
            using var leitor = new StreamReader(contexto.Response.Body);
            var corpoResposta = await leitor.ReadToEndAsync();
            return JsonSerializer.Deserialize<ErrorDetails>(corpoResposta, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }

        [Fact]
        public async Task InvocarAssincrono_QuandoNaoHaExcecao_DeveChamarProximoDelegate()
        {
            var contexto = new DefaultHttpContext();

            await _middleware.InvokeAsync(contexto);

            await _proximo.Received(1).Invoke(contexto);
            _logger.DidNotReceive().Log(
                LogLevel.Error,
                Arg.Any<EventId>(),
                Arg.Any<object>(),
                Arg.Any<Exception>(),
                Arg.Any<Func<object, Exception, string>>()
            );
        }

        [Fact]
        public async Task InvocarAssincrono_QuandoOcorreValidationException_DeveRetornarBadRequest()
        {
            var errosValidacao = new Dictionary<string, string[]>
            {
                { "Campo1", new[] { "Mensagem de erro 1" } },
                { "Campo2", new[] { "Mensagem de erro 2" } }
            };
            var excecao = new ValidationException("Falha na validação", errosValidacao);
            _proximo.Invoke(Arg.Any<HttpContext>()).Returns(Task.FromException(excecao));

            var contexto = new DefaultHttpContext();
            contexto.Response.Body = new MemoryStream();
            contexto.TraceIdentifier = "id-rastreamento-teste";

            await _middleware.InvokeAsync(contexto);

            contexto.Response.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
            contexto.Response.ContentType.Should().StartWith("application/json");

            var detalhesErro = await LerDetalhesErroDaResposta(contexto);
            detalhesErro.Should().NotBeNull();
            detalhesErro.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
            detalhesErro.Message.Should().Be("Falha na validação");
            detalhesErro.Errors.Should().BeEquivalentTo(errosValidacao);
            detalhesErro.TraceId.Should().Be("id-rastreamento-teste");

            _logger.Received(1).Log(
                LogLevel.Error,
                0,
                Arg.Is<object>(estado => estado.ToString() == $"Ocorreu uma exceção não tratada: {excecao.Message}"),
                Arg.Is<Exception>(e => e == excecao),
                Arg.Any<Func<object, Exception, string>>()
            );
        }

        [Fact]
        public async Task InvocarAssincrono_QuandoOcorreNotFoundException_DeveRetornarNotFound()
        {
            var excecao = new NotFoundException("Recurso não encontrado");
            _proximo.Invoke(Arg.Any<HttpContext>()).Returns(Task.FromException(excecao));

            var contexto = new DefaultHttpContext();
            contexto.Response.Body = new MemoryStream();
            contexto.TraceIdentifier = "id-rastreamento-teste";

            await _middleware.InvokeAsync(contexto);

            contexto.Response.StatusCode.Should().Be(StatusCodes.Status404NotFound);
            contexto.Response.ContentType.Should().StartWith("application/json");

            var detalhesErro = await LerDetalhesErroDaResposta(contexto);
            detalhesErro.Should().NotBeNull();
            detalhesErro.StatusCode.Should().Be(StatusCodes.Status404NotFound);
            detalhesErro.Message.Should().Be("Recurso não encontrado");
            detalhesErro.Errors.Should().BeNull();
            detalhesErro.TraceId.Should().Be("id-rastreamento-teste");

            _logger.Received(1).Log(
                LogLevel.Error,
                0,
                Arg.Is<object>(estado => estado.ToString() == $"Ocorreu uma exceção não tratada: {excecao.Message}"),
                Arg.Is<Exception>(e => e == excecao),
                Arg.Any<Func<object, Exception, string>>()
            );
        }

        [Fact]
        public async Task InvocarAssincrono_QuandoOcorreAutenticacaoException_DeveRetornarUnauthorized()
        {
            var excecao = new AutenticacaoException("Credenciais inválidas");
            _proximo.Invoke(Arg.Any<HttpContext>()).Returns(Task.FromException(excecao));

            var contexto = new DefaultHttpContext();
            contexto.Response.Body = new MemoryStream();
            contexto.TraceIdentifier = "id-rastreamento-teste";

            await _middleware.InvokeAsync(contexto);

            contexto.Response.StatusCode.Should().Be(StatusCodes.Status401Unauthorized);
            contexto.Response.ContentType.Should().StartWith("application/json");

            var detalhesErro = await LerDetalhesErroDaResposta(contexto);
            detalhesErro.Should().NotBeNull();
            detalhesErro.StatusCode.Should().Be(StatusCodes.Status401Unauthorized);
            detalhesErro.Message.Should().Be("Credenciais inválidas");
            detalhesErro.Errors.Should().BeNull();
            detalhesErro.TraceId.Should().Be("id-rastreamento-teste");

            _logger.Received(1).Log(
                LogLevel.Error,
                0,
                Arg.Is<object>(estado => estado.ToString() == $"Ocorreu uma exceção não tratada: {excecao.Message}"),
                Arg.Is<Exception>(e => e == excecao),
                Arg.Any<Func<object, Exception, string>>()
            );
        }

        [Fact]
        public async Task InvocarAssincrono_QuandoOcorreUnauthorizedException_DeveRetornarUnauthorized()
        {
            var excecao = new UnauthorizedException("Acesso negado");
            _proximo.Invoke(Arg.Any<HttpContext>()).Returns(Task.FromException(excecao));

            var contexto = new DefaultHttpContext();
            contexto.Response.Body = new MemoryStream();
            contexto.TraceIdentifier = "id-rastreamento-teste";

            await _middleware.InvokeAsync(contexto);

            contexto.Response.StatusCode.Should().Be(StatusCodes.Status401Unauthorized);
            contexto.Response.ContentType.Should().StartWith("application/json");

            var detalhesErro = await LerDetalhesErroDaResposta(contexto);
            detalhesErro.Should().NotBeNull();
            detalhesErro.StatusCode.Should().Be(StatusCodes.Status401Unauthorized);
            detalhesErro.Message.Should().Be("Acesso negado");
            detalhesErro.Errors.Should().BeNull();
            detalhesErro.TraceId.Should().Be("id-rastreamento-teste");

            _logger.Received(1).Log(
                LogLevel.Error,
                0,
                Arg.Is<object>(estado => estado.ToString() == $"Ocorreu uma exceção não tratada: {excecao.Message}"),
                Arg.Is<Exception>(e => e == excecao),
                Arg.Any<Func<object, Exception, string>>()
            );
        }

        [Fact]
        public async Task InvocarAssincrono_QuandoOcorreForbiddenException_DeveRetornarForbidden()
        {
            var excecao = new ForbiddenException("Permissão negada");
            _proximo.Invoke(Arg.Any<HttpContext>()).Returns(Task.FromException(excecao));

            var contexto = new DefaultHttpContext();
            contexto.Response.Body = new MemoryStream();
            contexto.TraceIdentifier = "id-rastreamento-teste";

            await _middleware.InvokeAsync(contexto);

            contexto.Response.StatusCode.Should().Be(StatusCodes.Status403Forbidden);
            contexto.Response.ContentType.Should().StartWith("application/json");

            var detalhesErro = await LerDetalhesErroDaResposta(contexto);
            detalhesErro.Should().NotBeNull();
            detalhesErro.StatusCode.Should().Be(StatusCodes.Status403Forbidden);
            detalhesErro.Message.Should().Be("Permissão negada");
            detalhesErro.Errors.Should().BeNull();
            detalhesErro.TraceId.Should().Be("id-rastreamento-teste");

            _logger.Received(1).Log(
                LogLevel.Error,
                0,
                Arg.Is<object>(estado => estado.ToString() == $"Ocorreu uma exceção não tratada: {excecao.Message}"),
                Arg.Is<Exception>(e => e == excecao),
                Arg.Any<Func<object, Exception, string>>()
            );
        }

        [Fact]
        public async Task InvocarAssincrono_QuandoOcorreDomainException_DeveRetornarBadRequest()
        {
            var excecao = new DomainException("Ocorreu um erro específico do domínio");
            _proximo.Invoke(Arg.Any<HttpContext>()).Returns(Task.FromException(excecao));

            var contexto = new DefaultHttpContext();
            contexto.Response.Body = new MemoryStream();
            contexto.TraceIdentifier = "id-rastreamento-teste";

            await _middleware.InvokeAsync(contexto);

            contexto.Response.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
            contexto.Response.ContentType.Should().StartWith("application/json");

            var detalhesErro = await LerDetalhesErroDaResposta(contexto);
            detalhesErro.Should().NotBeNull();
            detalhesErro.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
            detalhesErro.Message.Should().Be("Ocorreu um erro específico do domínio");
            detalhesErro.Errors.Should().BeNull();
            detalhesErro.TraceId.Should().Be("id-rastreamento-teste");

            _logger.Received(1).Log(
                LogLevel.Error,
                0,
                Arg.Is<object>(estado => estado.ToString() == $"Ocorreu uma exceção não tratada: {excecao.Message}"),
                Arg.Is<Exception>(e => e == excecao),
                Arg.Any<Func<object, Exception, string>>()
            );
        }

        [Fact]
        public async Task InvocarAssincrono_QuandoOcorreExcecaoGenerica_DeveRetornarInternalServerError()
        {
            var excecao = new Exception("Algo inesperado aconteceu");
            _proximo.Invoke(Arg.Any<HttpContext>()).Returns(Task.FromException(excecao));

            var contexto = new DefaultHttpContext();
            contexto.Response.Body = new MemoryStream();
            contexto.TraceIdentifier = "id-rastreamento-teste";

            await _middleware.InvokeAsync(contexto);

            contexto.Response.StatusCode.Should().Be(StatusCodes.Status500InternalServerError);
            contexto.Response.ContentType.Should().StartWith("application/json");

            var detalhesErro = await LerDetalhesErroDaResposta(contexto);
            detalhesErro.Should().NotBeNull();
            detalhesErro.StatusCode.Should().Be(StatusCodes.Status500InternalServerError);
            detalhesErro.Message.Should().Be("Ocorreu um erro interno no servidor");
            detalhesErro.Errors.Should().BeNull();
            detalhesErro.TraceId.Should().Be("id-rastreamento-teste");

            _logger.Received(1).Log(
                LogLevel.Error,
                0,
                Arg.Is<object>(estado => estado.ToString() == $"Ocorreu uma exceção não tratada: {excecao.Message}"),
                Arg.Is<Exception>(e => e == excecao),
                Arg.Any<Func<object, Exception, string>>()
            );
        }
    }
}

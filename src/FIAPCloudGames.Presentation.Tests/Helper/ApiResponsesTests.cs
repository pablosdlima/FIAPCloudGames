using FIAPCloudGames.Api.Helpers;
using FIAPCloudGames.Application.Common.Models;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;

namespace FIAPCloudGames.Presentation.Tests.Helper
{
    public class ApiResponsesTests
    {
        [Fact]
        public void Ok_DeveRetornar200ComApiResponseDeSucesso()
        {
            // Arrange
            var data = "teste";

            // Act
            var result = ApiResponses.Ok(data);

            // Assert
            result.Should().BeOfType<Ok<ApiResponse<string>>>();

            var okResult = result as Ok<ApiResponse<string>>;
            okResult!.StatusCode.Should().Be(StatusCodes.Status200OK);
            okResult.Value!.Success.Should().BeTrue();
            okResult.Value.Data.Should().Be(data);
        }

        [Fact]
        public void OkMessage_DeveRetornar200ComMensagem()
        {
            // Act
            var result = ApiResponses.OkMessage("mensagem ok");

            // Assert
            result.Should().BeOfType<Ok<ApiResponse<object>>>();

            var okResult = result as Ok<ApiResponse<object>>;
            okResult!.Value!.Success.Should().BeTrue();
            okResult.Value.Message.Should().Be("mensagem ok");
        }

        [Fact]
        public void BadRequest_DeveRetornar400ComDetalhesDeErro()
        {
            // Act
            var result = ApiResponses.BadRequest("Name", "Nome é obrigatório");

            // Assert
            result.Should().BeOfType<BadRequest<ErrorDetails>>();

            var badRequest = result as BadRequest<ErrorDetails>;
            badRequest!.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
            badRequest.Value!.Errors.Should().ContainKey("Name");
        }

        [Fact]
        public void BadRequestMultiple_DeveRetornar400ComMultiplosErros()
        {
            // Arrange
            var errors = new Dictionary<string, string[]>
            {
                { "Name", new[] { "Obrigatório" } },
                { "Price", new[] { "Inválido" } }
            };

            // Act
            var result = ApiResponses.BadRequestMultiple("Erro de validação", errors);

            // Assert
            result.Should().BeOfType<BadRequest<ErrorDetails>>();

            var badRequest = result as BadRequest<ErrorDetails>;
            badRequest!.Value!.Errors.Should().HaveCount(2);
        }

        [Fact]
        public void NotFound_DeveRetornar404()
        {
            // Act
            var result = ApiResponses.NotFound("Game", "Jogo não encontrado");

            // Assert
            result.Should().BeOfType<NotFound<ErrorDetails>>();

            var notFound = result as NotFound<ErrorDetails>;
            notFound!.StatusCode.Should().Be(StatusCodes.Status404NotFound);
            notFound.Value!.Message.Should().Be("Jogo não encontrado");
        }

        [Fact]
        public void Unauthorized_DeveRetornar401()
        {
            // Act
            var result = ApiResponses.Unauthorized("Não autorizado");

            // Assert
            result.Should().BeOfType<JsonHttpResult<ErrorDetails>>();

            var unauthorized = result as JsonHttpResult<ErrorDetails>;
            unauthorized!.StatusCode.Should().Be(StatusCodes.Status401Unauthorized);
            unauthorized.Value!.Message.Should().Be("Não autorizado");
        }

        [Fact]
        public void Problem_DeveRetornar500()
        {
            // Act
            var result = ApiResponses.Problem();

            // Assert
            result.Should().BeOfType<ProblemHttpResult>();

            var problem = result as ProblemHttpResult;
            problem!.StatusCode.Should().Be(StatusCodes.Status500InternalServerError);
        }
    }
}

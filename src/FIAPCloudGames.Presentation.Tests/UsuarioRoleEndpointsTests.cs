using FIAPCloudGames.Application.Common.Models;
using FIAPCloudGames.Application.Interfaces;
using FIAPCloudGames.Domain.Dtos.Request.UsuarioRole;
using FIAPCloudGames.Domain.Enums;
using FluentAssertions;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using NSubstitute;
using System.Net;
using System.Net.Http.Json;

namespace FIAPCloudGames.Presentation.Tests.Endpoints
{
    public class UsuarioRoleEndpointsTests : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly HttpClient _client;
        private readonly CustomWebApplicationFactory _factory;
        private readonly IUsuarioRoleAppService _mockAppService;
        private readonly IValidator<ListarRolePorUsuarioRequest> _mockListarRequestValidator;
        private readonly IValidator<AlterarUsuarioRoleRequest> _mockAlterarRequestValidator;

        public UsuarioRoleEndpointsTests(CustomWebApplicationFactory factory)
        {
            _factory = factory;
            _client = _factory.CreateClient();
            _mockAppService = _factory.MockUsuarioRoleAppService;
            _mockListarRequestValidator = _factory.MockListarRolePorUsuarioRequestValidator;
            _mockAlterarRequestValidator = _factory.MockAlterarUsuarioRoleRequestValidator;

            _mockAppService.ClearReceivedCalls();
            _mockListarRequestValidator.ClearReceivedCalls();
            _mockAlterarRequestValidator.ClearReceivedCalls();
        }


        [Fact]
        public async Task AlterarRoleUsuario_DeveRetornarOk_QuandoSucesso()
        {
            var request = new AlterarUsuarioRoleRequest
            {
                IdUsuarioRole = Guid.NewGuid(),
                UsuarioId = Guid.NewGuid(),
                TipoUsuario = TipoUsuario.Administrador
            };

            _mockAlterarRequestValidator
                .ValidateAsync(Arg.Any<AlterarUsuarioRoleRequest>(), Arg.Any<CancellationToken>())
                .Returns(new FluentValidation.Results.ValidationResult());

            _mockAppService.AlterarRoleUsuario(Arg.Is<AlterarUsuarioRoleRequest>(r => r.IdUsuarioRole == request.IdUsuarioRole))
                .Returns(Task.FromResult(true));

            var response = await _client.PutAsJsonAsync("/api/UsuarioRole/AlterarRoleUsuario", request);

            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var apiResponse = await response.Content.ReadFromJsonAsync<ApiResponse<object>>();
            apiResponse.Should().NotBeNull();
            apiResponse!.Success.Should().BeTrue();
            apiResponse.Message.Should().Be("Role do usuário alterada com sucesso.");

            await _mockAlterarRequestValidator.Received(1).ValidateAsync(Arg.Any<AlterarUsuarioRoleRequest>(), Arg.Any<CancellationToken>());
            await _mockAppService.Received(1).AlterarRoleUsuario(Arg.Is<AlterarUsuarioRoleRequest>(r => r.IdUsuarioRole == request.IdUsuarioRole));
        }

        [Fact]
        public async Task AlterarRoleUsuario_DeveRetornarNotFound_QuandoNaoEncontrado()
        {
            var request = new AlterarUsuarioRoleRequest
            {
                IdUsuarioRole = Guid.NewGuid(),
                UsuarioId = Guid.NewGuid(),
                TipoUsuario = TipoUsuario.Administrador
            };

            _mockAlterarRequestValidator
                .ValidateAsync(Arg.Any<AlterarUsuarioRoleRequest>(), Arg.Any<CancellationToken>())
                .Returns(new FluentValidation.Results.ValidationResult());

            _mockAppService.AlterarRoleUsuario(Arg.Is<AlterarUsuarioRoleRequest>(r => r.IdUsuarioRole == request.IdUsuarioRole))
                .Returns(Task.FromResult(false));

            var response = await _client.PutAsJsonAsync("/api/UsuarioRole/AlterarRoleUsuario", request);

            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
            var errorResponse = await response.Content.ReadFromJsonAsync<ErrorDetails>();
            errorResponse.Should().NotBeNull();
            errorResponse!.StatusCode.Should().Be(StatusCodes.Status404NotFound);
            errorResponse.Errors.Should().ContainKey("usuarioRole");
            errorResponse.Errors["usuarioRole"].Should().Contain("Registro não encontrado ou não foi possível atualizar.");

            await _mockAlterarRequestValidator.Received(1).ValidateAsync(Arg.Any<AlterarUsuarioRoleRequest>(), Arg.Any<CancellationToken>());
            await _mockAppService.Received(1).AlterarRoleUsuario(Arg.Is<AlterarUsuarioRoleRequest>(r => r.IdUsuarioRole == request.IdUsuarioRole));
        }

        [Fact]
        public async Task AlterarRoleUsuario_DeveRetornarBadRequest_QuandoValidacaoFalha()
        {
            var request = new AlterarUsuarioRoleRequest
            {
                IdUsuarioRole = Guid.NewGuid(),
                UsuarioId = Guid.NewGuid(),
                TipoUsuario = TipoUsuario.Administrador
            };

            var validationResult = new FluentValidation.Results.ValidationResult(new[]
            {
                new FluentValidation.Results.ValidationFailure("IdUsuarioRole", "IdUsuarioRole é obrigatório.")
            });

            _mockAlterarRequestValidator
                .ValidateAsync(Arg.Any<AlterarUsuarioRoleRequest>(), Arg.Any<CancellationToken>())
                .Returns(validationResult);

            var response = await _client.PutAsJsonAsync("/api/UsuarioRole/AlterarRoleUsuario", request);

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            var errorResponse = await response.Content.ReadFromJsonAsync<ErrorDetails>();
            errorResponse.Should().NotBeNull();
            errorResponse!.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
            errorResponse.Errors.Should().ContainKey("IdUsuarioRole");
            errorResponse.Errors["IdUsuarioRole"].Should().Contain("IdUsuarioRole é obrigatório.");

            await _mockAlterarRequestValidator.Received(1).ValidateAsync(Arg.Any<AlterarUsuarioRoleRequest>(), Arg.Any<CancellationToken>());
            await _mockAppService.DidNotReceive().AlterarRoleUsuario(Arg.Any<AlterarUsuarioRoleRequest>());
        }
    }
}

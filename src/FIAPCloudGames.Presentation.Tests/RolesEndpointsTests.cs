using FIAPCloudGames.Application.Common.Models;
using FIAPCloudGames.Application.Interfaces;
using FIAPCloudGames.Domain.Dtos.Request.Role;
using FIAPCloudGames.Domain.Dtos.Responses.Role;
using FluentAssertions;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc; // Para ProblemDetails
using NSubstitute;
using System.Net;
using System.Text;
using System.Text.Json;

namespace FIAPCloudGames.Presentation.Tests.Endpoints
{
    public class RolesEndpointsTests : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly HttpClient _client;
        private readonly CustomWebApplicationFactory _factory;
        private readonly IRoleAppService _mockRoleAppService;
        private readonly IValidator<CadastrarRoleRequest> _mockCadastrarRoleRequestValidator;
        private readonly IValidator<AtualizarRoleRequest> _mockAtualizarRoleRequestValidator;

        public RolesEndpointsTests(CustomWebApplicationFactory factory)
        {
            _factory = factory;
            _client = _factory.CreateClient();

            _mockRoleAppService = _factory.MockRoleAppService;
            _mockCadastrarRoleRequestValidator = _factory.MockCadastrarRoleRequestValidator;
            _mockAtualizarRoleRequestValidator = _factory.MockAtualizarRoleRequestValidator;

            _mockRoleAppService.ClearReceivedCalls();
            _mockCadastrarRoleRequestValidator.ClearReceivedCalls();
            _mockAtualizarRoleRequestValidator.ClearReceivedCalls();

            Console.WriteLine("RolesEndpointsTests: Construtor executado. Mocks obtidos e limpos.");
        }

        private static StringContent ObterConteudoJson(object obj)
        {
            return new StringContent(JsonSerializer.Serialize(obj), Encoding.UTF8, "application/json");
        }

        private static async Task<ApiResponse<T>?> DeserializarRespostaSucesso<T>(HttpResponseMessage response)
        {
            var json = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"RolesEndpointsTests: Resposta de sucesso JSON: {json}");
            return JsonSerializer.Deserialize<ApiResponse<T>>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }

        private static async Task<ProblemDetails?> DeserializarProblemDetails(HttpResponseMessage response)
        {
            var json = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"RolesEndpointsTests: ProblemDetails JSON: {json}");
            return JsonSerializer.Deserialize<ProblemDetails>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }

        private static async Task<ErrorDetails?> DeserializarRespostaErro(HttpResponseMessage response)
        {
            var json = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"RolesEndpointsTests: ErrorDetails JSON: {json}");
            return JsonSerializer.Deserialize<ErrorDetails>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }

        // --- Testes para POST /api/Roles/Cadastrar/ ---


        [Fact]
        public async Task CadastrarRole_DeveRetornarProblem_QuandoFalhaNoAppService()
        {
            Console.WriteLine("RolesEndpointsTests: Iniciando CadastrarRole_DeveRetornarProblem_QuandoFalhaNoAppService");
            var request = new CadastrarRoleRequest { Id = 1, RoleName = "Admin", Description = "Administrador do sistema" };

            _mockRoleAppService.Cadastrar(request).Returns((RolesResponse?)null);
            _mockCadastrarRoleRequestValidator.ValidateAsync(Arg.Any<CadastrarRoleRequest>(), Arg.Any<CancellationToken>())
                                              .Returns(new FluentValidation.Results.ValidationResult());

            var response = await _client.PostAsync("/api/Roles/Cadastrar/", ObterConteudoJson(request));

            Console.WriteLine($"RolesEndpointsTests: CadastrarRole_DeveRetornarProblem_QuandoFalhaNoAppService - StatusCode: {response.StatusCode}");
            response.StatusCode.Should().Be(HttpStatusCode.InternalServerError);

            var problemDetails = await DeserializarProblemDetails(response);
            problemDetails.Should().NotBeNull();
            problemDetails!.Status.Should().Be(StatusCodes.Status500InternalServerError);
            problemDetails.Title.Should().Be("Erro ao cadastrar a role.");
            problemDetails.Detail.Should().BeNull();

            await _mockRoleAppService.Received(1).Cadastrar(Arg.Is<CadastrarRoleRequest>(r => r.Id == request.Id && r.RoleName == request.RoleName && r.Description == request.Description)); // CORRIGIDO
            await _mockCadastrarRoleRequestValidator.Received(1).ValidateAsync(Arg.Any<CadastrarRoleRequest>(), Arg.Any<CancellationToken>());
            Console.WriteLine("RolesEndpointsTests: CadastrarRole_DeveRetornarProblem_QuandoFalhaNoAppService - Teste concluído.");
        }

        [Fact]
        public async Task CadastrarRole_DeveRetornarBadRequest_QuandoValidacaoFalha()
        {
            Console.WriteLine("RolesEndpointsTests: Iniciando CadastrarRole_DeveRetornarBadRequest_QuandoValidacaoFalha");
            var request = new CadastrarRoleRequest { Id = 0, RoleName = "", Description = "" }; // Requisição inválida
            var validationResult = new FluentValidation.Results.ValidationResult();
            validationResult.Errors.Add(new FluentValidation.Results.ValidationFailure("Id", "Id deve ser maior que 0."));
            validationResult.Errors.Add(new FluentValidation.Results.ValidationFailure("RoleName", "RoleName é obrigatório."));
            validationResult.Errors.Add(new FluentValidation.Results.ValidationFailure("Description", "Description é obrigatória."));

            _mockCadastrarRoleRequestValidator.ValidateAsync(Arg.Any<CadastrarRoleRequest>(), Arg.Any<CancellationToken>())
                                              .Returns(validationResult);

            var response = await _client.PostAsync("/api/Roles/Cadastrar/", ObterConteudoJson(request));

            Console.WriteLine($"RolesEndpointsTests: CadastrarRole_DeveRetornarBadRequest_QuandoValidacaoFalha - StatusCode: {response.StatusCode}");
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

            var errorDetails = await DeserializarRespostaErro(response);
            errorDetails.Should().NotBeNull();
            errorDetails!.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
            errorDetails.Message.Should().Be("Erro de validação");
            errorDetails.Errors.Should().ContainKey("Id");
            errorDetails.Errors!["Id"].Should().Contain("Id deve ser maior que 0.");
            errorDetails.Errors.Should().ContainKey("RoleName");
            errorDetails.Errors!["RoleName"].Should().Contain("RoleName é obrigatório.");
            errorDetails.Errors.Should().ContainKey("Description");
            errorDetails.Errors!["Description"].Should().Contain("Description é obrigatória.");

            await _mockRoleAppService.DidNotReceive().Cadastrar(Arg.Any<CadastrarRoleRequest>());
            await _mockCadastrarRoleRequestValidator.Received(1).ValidateAsync(Arg.Any<CadastrarRoleRequest>(), Arg.Any<CancellationToken>());
            Console.WriteLine("RolesEndpointsTests: CadastrarRole_DeveRetornarBadRequest_QuandoValidacaoFalha - Teste concluído.");
        }

        // --- Testes para GET /api/Roles/ListarRoles/ ---

        [Fact]
        public async Task ListarRoles_DeveRetornarOkComLista_QuandoRolesExistem()
        {
            Console.WriteLine("RolesEndpointsTests: Iniciando ListarRoles_DeveRetornarOkComLista_QuandoRolesExistem");
            var expectedRoles = new List<RolesResponse>
            {
                new RolesResponse { Id = 1, RoleName = "Admin", Description = "Administrador" },
                new RolesResponse { Id = 2, RoleName = "User", Description = "Usuário Padrão" }
            };
            _mockRoleAppService.ListarRoles().Returns(expectedRoles);

            var response = await _client.GetAsync("/api/Roles/ListarRoles/");

            Console.WriteLine($"RolesEndpointsTests: ListarRoles_DeveRetornarOkComLista_QuandoRolesExistem - StatusCode: {response.StatusCode}");
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var apiResponse = await DeserializarRespostaSucesso<List<RolesResponse>>(response);
            apiResponse.Should().NotBeNull();
            apiResponse!.Success.Should().BeTrue();
            apiResponse.Message.Should().Be("Roles listadas com sucesso.");
            apiResponse.Data.Should().BeEquivalentTo(expectedRoles);

            await _mockRoleAppService.Received(1).ListarRoles();
            Console.WriteLine("RolesEndpointsTests: ListarRoles_DeveRetornarOkComLista_QuandoRolesExistem - Teste concluído.");
        }

        [Fact]
        public async Task ListarRoles_DeveRetornarOkComListaVazia_QuandoNaoHaRoles()
        {
            Console.WriteLine("RolesEndpointsTests: Iniciando ListarRoles_DeveRetornarOkComListaVazia_QuandoNaoHaRoles");
            var expectedRoles = new List<RolesResponse>();
            _mockRoleAppService.ListarRoles().Returns(expectedRoles);

            var response = await _client.GetAsync("/api/Roles/ListarRoles/");

            Console.WriteLine($"RolesEndpointsTests: ListarRoles_DeveRetornarOkComListaVazia_QuandoNaoHaRoles - StatusCode: {response.StatusCode}");
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var apiResponse = await DeserializarRespostaSucesso<List<RolesResponse>>(response);
            apiResponse.Should().NotBeNull();
            apiResponse!.Success.Should().BeTrue();
            apiResponse.Message.Should().Be("Roles listadas com sucesso.");
            apiResponse.Data.Should().BeEmpty();

            await _mockRoleAppService.Received(1).ListarRoles();
            Console.WriteLine("RolesEndpointsTests: ListarRoles_DeveRetornarOkComListaVazia_QuandoNaoHaRoles - Teste concluído.");
        }

        // --- Testes para PUT /api/Roles/Atualizar/{id:int} ---

        [Fact]
        public async Task AtualizarRole_DeveRetornarOk_QuandoSucesso()
        {
            Console.WriteLine("RolesEndpointsTests: Iniciando AtualizarRole_DeveRetornarOk_QuandoSucesso");
            var roleId = 1;
            var request = new AtualizarRoleRequest { Id = roleId, RoleName = "Editor", Description = "Editor de conteúdo" };
            var expectedResponse = new RolesResponse { Id = roleId, RoleName = "Editor", Description = "Editor de conteúdo" };

            _mockRoleAppService.AtualizarRole(request).Returns((expectedResponse, true));
            _mockAtualizarRoleRequestValidator.ValidateAsync(Arg.Any<AtualizarRoleRequest>(), Arg.Any<CancellationToken>())
                                              .Returns(new FluentValidation.Results.ValidationResult());

            var response = await _client.PutAsync($"/api/Roles/Atualizar/{roleId}", ObterConteudoJson(request));

            Console.WriteLine($"RolesEndpointsTests: AtualizarRole_DeveRetornarOk_QuandoSucesso - StatusCode: {response.StatusCode}");
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var apiResponse = await DeserializarRespostaSucesso<RolesResponse>(response);
            apiResponse.Should().NotBeNull();
            apiResponse!.Success.Should().BeTrue();
            apiResponse.Message.Should().Be("Role atualizada com sucesso.");
            apiResponse.Data.Should().BeEquivalentTo(expectedResponse);

            await _mockRoleAppService.Received(1).AtualizarRole(Arg.Is<AtualizarRoleRequest>(r => r.Id == request.Id && r.RoleName == request.RoleName && r.Description == request.Description)); // CORRIGIDO
            await _mockAtualizarRoleRequestValidator.Received(1).ValidateAsync(Arg.Any<AtualizarRoleRequest>(), Arg.Any<CancellationToken>());
            Console.WriteLine("RolesEndpointsTests: AtualizarRole_DeveRetornarOk_QuandoSucesso - Teste concluído.");
        }

        [Fact]
        public async Task AtualizarRole_ComIdDaUrlDiferenteDoCorpo_RetornaBadRequest()
        {
            Console.WriteLine("RolesEndpointsTests: Iniciando AtualizarRole_ComIdDaUrlDiferenteDoCorpo_RetornaBadRequest");
            var idUrl = 1;
            var idCorpo = 2;
            var request = new AtualizarRoleRequest { Id = idCorpo, RoleName = "Editor", Description = "Editor de conteúdo" };

            _mockAtualizarRoleRequestValidator.ValidateAsync(Arg.Any<AtualizarRoleRequest>(), Arg.Any<CancellationToken>())
                                              .Returns(new FluentValidation.Results.ValidationResult());

            var response = await _client.PutAsync($"/api/Roles/Atualizar/{idUrl}", ObterConteudoJson(request));

            Console.WriteLine($"RolesEndpointsTests: AtualizarRole_ComIdDaUrlDiferenteDoCorpo_RetornaBadRequest - StatusCode: {response.StatusCode}");
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

            var errorDetails = await DeserializarRespostaErro(response);
            errorDetails.Should().NotBeNull();
            errorDetails!.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
            errorDetails.Message.Should().Be("Id da URL não corresponde ao Id do corpo da requisição.");
            errorDetails.Errors.Should().ContainKey("id");
            errorDetails.Errors!["id"].Should().Contain("Id da URL não corresponde ao Id do corpo da requisição.");

            await _mockRoleAppService.DidNotReceive().AtualizarRole(Arg.Any<AtualizarRoleRequest>());
            await _mockAtualizarRoleRequestValidator.Received(1).ValidateAsync(Arg.Any<AtualizarRoleRequest>(), Arg.Any<CancellationToken>());
            Console.WriteLine("RolesEndpointsTests: AtualizarRole_ComIdDaUrlDiferenteDoCorpo_RetornaBadRequest - Teste concluído.");
        }

        [Fact]
        public async Task AtualizarRole_ComRoleNaoEncontrada_RetornaNotFound()
        {
            Console.WriteLine("RolesEndpointsTests: Iniciando AtualizarRole_ComRoleNaoEncontrada_RetornaNotFound");
            var roleId = 1;
            var request = new AtualizarRoleRequest { Id = roleId, RoleName = "Editor", Description = "Editor de conteúdo" };

            _mockRoleAppService.AtualizarRole(request).Returns((null, false));
            _mockAtualizarRoleRequestValidator.ValidateAsync(Arg.Any<AtualizarRoleRequest>(), Arg.Any<CancellationToken>())
                                              .Returns(new FluentValidation.Results.ValidationResult());

            var response = await _client.PutAsync($"/api/Roles/Atualizar/{roleId}", ObterConteudoJson(request));

            Console.WriteLine($"RolesEndpointsTests: AtualizarRole_ComRoleNaoEncontrada_RetornaNotFound - StatusCode: {response.StatusCode}");
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);

            var errorDetails = await DeserializarRespostaErro(response);
            errorDetails.Should().NotBeNull();
            errorDetails!.StatusCode.Should().Be(StatusCodes.Status404NotFound);
            errorDetails.Message.Should().Be("Role não encontrada ou não foi possível atualizar.");
            errorDetails.Errors.Should().ContainKey("role");
            errorDetails.Errors!["role"].Should().Contain("Role não encontrada ou não foi possível atualizar.");

            await _mockRoleAppService.Received(1).AtualizarRole(Arg.Is<AtualizarRoleRequest>(r => r.Id == request.Id && r.RoleName == request.RoleName && r.Description == request.Description)); // CORRIGIDO
            await _mockAtualizarRoleRequestValidator.Received(1).ValidateAsync(Arg.Any<AtualizarRoleRequest>(), Arg.Any<CancellationToken>());
            Console.WriteLine("RolesEndpointsTests: AtualizarRole_ComRoleNaoEncontrada_RetornaNotFound - Teste concluído.");
        }

        [Fact]
        public async Task AtualizarRole_DeveRetornarBadRequest_QuandoValidacaoFalha()
        {
            Console.WriteLine("RolesEndpointsTests: Iniciando AtualizarRole_DeveRetornarBadRequest_QuandoValidacaoFalha");
            var roleId = 1;
            var request = new AtualizarRoleRequest { Id = roleId, RoleName = "", Description = "" }; // Requisição inválida
            var validationResult = new FluentValidation.Results.ValidationResult();
            validationResult.Errors.Add(new FluentValidation.Results.ValidationFailure("RoleName", "RoleName é obrigatório."));
            validationResult.Errors.Add(new FluentValidation.Results.ValidationFailure("Description", "Description é obrigatória."));

            _mockAtualizarRoleRequestValidator.ValidateAsync(Arg.Any<AtualizarRoleRequest>(), Arg.Any<CancellationToken>())
                                              .Returns(validationResult);

            var response = await _client.PutAsync($"/api/Roles/Atualizar/{roleId}", ObterConteudoJson(request));

            Console.WriteLine($"RolesEndpointsTests: AtualizarRole_DeveRetornarBadRequest_QuandoValidacaoFalha - StatusCode: {response.StatusCode}");
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

            var errorDetails = await DeserializarRespostaErro(response);
            errorDetails.Should().NotBeNull();
            errorDetails!.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
            errorDetails.Message.Should().Be("Erro de validação");
            errorDetails.Errors.Should().ContainKey("RoleName");
            errorDetails.Errors!["RoleName"].Should().Contain("RoleName é obrigatório.");
            errorDetails.Errors.Should().ContainKey("Description");
            errorDetails.Errors!["Description"].Should().Contain("Description é obrigatória.");

            await _mockRoleAppService.DidNotReceive().AtualizarRole(Arg.Any<AtualizarRoleRequest>());
            await _mockAtualizarRoleRequestValidator.Received(1).ValidateAsync(Arg.Any<AtualizarRoleRequest>(), Arg.Any<CancellationToken>());
            Console.WriteLine("RolesEndpointsTests: AtualizarRole_DeveRetornarBadRequest_QuandoValidacaoFalha - Teste concluído.");
        }
    }
}

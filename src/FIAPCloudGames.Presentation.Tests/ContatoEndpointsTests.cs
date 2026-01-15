using Bogus;
using FIAPCloudGames.Application.Common.Models;
using FIAPCloudGames.Application.Interfaces;
using FIAPCloudGames.Domain.Dtos.Request.Contato;
using FIAPCloudGames.Domain.Dtos.Responses.Contato;
using FIAPCloudGames.Domain.Exceptions;
using FluentAssertions;
using FluentValidation;
using NSubstitute;
using NSubstitute.ClearExtensions;
using System.Net;
using System.Text;
using System.Text.Json;

namespace FIAPCloudGames.Presentation.Tests.Endpoints
{
    public class ContatoEndpointsTests : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly CustomWebApplicationFactory _factory;
        private readonly HttpClient _client;
        private readonly IContatoAppService _mockContatoAppService;
        private readonly IValidator<CadastrarContatoRequest> _mockCadastrarContatoRequestValidator;
        private readonly IValidator<AtualizarContatoRequest> _mockAtualizarContatoRequestValidator;

        public ContatoEndpointsTests(CustomWebApplicationFactory factory)
        {
            _factory = factory;
            _client = _factory.CreateClient();

            _mockContatoAppService = _factory.MockContatoAppService;
            _mockCadastrarContatoRequestValidator = _factory.MockCadastrarContatoRequestValidator;
            _mockAtualizarContatoRequestValidator = _factory.MockAtualizarContatoRequestValidator;

            _mockContatoAppService.ClearSubstitute();
            _mockCadastrarContatoRequestValidator.ClearSubstitute();
            _mockAtualizarContatoRequestValidator.ClearSubstitute();
        }

        private static StringContent ObterConteudoJson(object obj)
        {
            return new StringContent(JsonSerializer.Serialize(obj), Encoding.UTF8, "application/json");
        }

        private async Task<ApiResponse<T>?> DeserializarRespostaSucesso<T>(HttpResponseMessage response)
        {
            var jsonString = await response.Content.ReadAsStringAsync();
            if (string.IsNullOrEmpty(jsonString))
            {
                return default;
            }
            return JsonSerializer.Deserialize<ApiResponse<T>>(jsonString, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }

        private async Task<ErrorDetails?> DeserializarRespostaErro(HttpResponseMessage response)
        {
            var jsonString = await response.Content.ReadAsStringAsync();
            if (string.IsNullOrEmpty(jsonString))
            {
                return default;
            }
            return JsonSerializer.Deserialize<ErrorDetails>(jsonString, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }

        [Fact]
        public async Task ListarContatosPorUsuarioId_ComUsuarioExistente_RetornaOkComContatos()
        {
            var usuarioId = Guid.NewGuid();
            var contatosEsperados = new Faker<ContatoResponse>()
                .RuleFor(c => c.Id, f => f.Random.Guid())
                .RuleFor(c => c.UsuarioId, usuarioId)
                .RuleFor(c => c.Celular, f => f.Phone.PhoneNumber("###########"))
                .RuleFor(c => c.Email, f => f.Internet.Email())
                .Generate(3);
            var mensagemSucesso = "Contatos listados com sucesso.";

            _mockContatoAppService.ListarPorUsuario(usuarioId)
                .Returns(contatosEsperados);

            var resposta = await _client.GetAsync($"/api/usuarios/{usuarioId}/contatos/BuscarPorUsuarioId/");

            resposta.StatusCode.Should().Be(HttpStatusCode.OK);

            var conteudoResposta = await DeserializarRespostaSucesso<List<ContatoResponse>>(resposta);

            conteudoResposta.Should().NotBeNull();
            conteudoResposta!.Success.Should().BeTrue();
            conteudoResposta.Message.Should().Be(mensagemSucesso);
            conteudoResposta.Data.Should().NotBeNull();
            conteudoResposta.Data.Should().HaveCount(3);
            conteudoResposta.Data.Should().BeEquivalentTo(contatosEsperados);
            conteudoResposta.Errors.Should().BeNull();

            await _mockContatoAppService.Received(1).ListarPorUsuario(usuarioId);
        }

        [Fact]
        public async Task ListarContatosPorUsuarioId_ComUsuarioInexistente_RetornaNotFound()
        {
            var usuarioId = Guid.NewGuid();
            var mensagemErro = $"Usuário com ID {usuarioId} não encontrado.";

            _mockContatoAppService.ListarPorUsuario(usuarioId)
                .Returns(Task.FromException<List<ContatoResponse>>(new NotFoundException(mensagemErro)));

            var resposta = await _client.GetAsync($"/api/usuarios/{usuarioId}/contatos/BuscarPorUsuarioId/");

            resposta.StatusCode.Should().Be(HttpStatusCode.NotFound);

            var conteudoRespostaErro = await DeserializarRespostaErro(resposta);

            conteudoRespostaErro.Should().NotBeNull();
            conteudoRespostaErro!.StatusCode.Should().Be(404);
            conteudoRespostaErro.Message.Should().Be(mensagemErro);
            conteudoRespostaErro.Errors.Should().BeNull();

            await _mockContatoAppService.Received(1).ListarPorUsuario(usuarioId);
        }

        [Fact]
        public async Task CadastrarContato_ComDadosValidos_RetornaCreated()
        {
            var usuarioId = Guid.NewGuid();
            var requisicaoCadastro = new Faker<CadastrarContatoRequest>()
                .RuleFor(r => r.UsuarioId, usuarioId)
                .RuleFor(r => r.Celular, f => f.Phone.PhoneNumber("###########"))
                .RuleFor(r => r.Email, f => f.Internet.Email())
                .Generate();

            var contatoCadastrado = new ContatoResponse
            {
                Id = Guid.NewGuid(),
                UsuarioId = usuarioId,
                Celular = requisicaoCadastro.Celular,
                Email = requisicaoCadastro.Email
            };
            var mensagemSucesso = "Contato cadastrado com sucesso.";

            _mockCadastrarContatoRequestValidator
                .ValidateAsync(Arg.Any<CadastrarContatoRequest>(), Arg.Any<CancellationToken>())
                .Returns(new FluentValidation.Results.ValidationResult());

            _mockContatoAppService.Cadastrar(Arg.Is<CadastrarContatoRequest>(r => r.UsuarioId == usuarioId))
                .Returns(contatoCadastrado);

            var resposta = await _client.PostAsync($"/api/usuarios/{usuarioId}/contatos/Cadastrar/", ObterConteudoJson(requisicaoCadastro));

            resposta.StatusCode.Should().Be(HttpStatusCode.Created);
            resposta.Headers.Location.Should().NotBeNull();
            resposta.Headers.Location!.ToString().Should().Contain($"/api/usuarios/{usuarioId}/contatos/{contatoCadastrado.Id}");

            var conteudoResposta = await DeserializarRespostaSucesso<ContatoResponse>(resposta);

            conteudoResposta.Should().NotBeNull();
            conteudoResposta!.Success.Should().BeTrue();
            conteudoResposta.Message.Should().Be(mensagemSucesso);
            conteudoResposta.Data.Should().NotBeNull();
            conteudoResposta.Data!.Id.Should().Be(contatoCadastrado.Id);
            conteudoResposta.Data!.UsuarioId.Should().Be(contatoCadastrado.UsuarioId);

            await _mockContatoAppService.Received(1).Cadastrar(Arg.Is<CadastrarContatoRequest>(r => r.UsuarioId == usuarioId));
        }

        [Fact]
        public async Task CadastrarContato_ComUsuarioInexistente_RetornaNotFound()
        {
            var usuarioId = Guid.NewGuid();
            var requisicaoCadastro = new Faker<CadastrarContatoRequest>()
                .RuleFor(r => r.UsuarioId, usuarioId)
                .RuleFor(r => r.Celular, f => f.Phone.PhoneNumber("###########"))
                .RuleFor(r => r.Email, f => f.Internet.Email())
                .Generate();
            var mensagemErro = $"Usuário com ID {usuarioId} não encontrado para cadastrar o contato.";

            _mockCadastrarContatoRequestValidator
                .ValidateAsync(Arg.Any<CadastrarContatoRequest>(), Arg.Any<CancellationToken>())
                .Returns(new FluentValidation.Results.ValidationResult());

            _mockContatoAppService.Cadastrar(Arg.Is<CadastrarContatoRequest>(r => r.UsuarioId == usuarioId))
                .Returns(Task.FromException<ContatoResponse>(new NotFoundException(mensagemErro)));

            var resposta = await _client.PostAsync($"/api/usuarios/{usuarioId}/contatos/Cadastrar/", ObterConteudoJson(requisicaoCadastro));

            resposta.StatusCode.Should().Be(HttpStatusCode.NotFound);

            var conteudoRespostaErro = await DeserializarRespostaErro(resposta);

            conteudoRespostaErro.Should().NotBeNull();
            conteudoRespostaErro!.StatusCode.Should().Be(404);
            conteudoRespostaErro.Message.Should().Be(mensagemErro);
            conteudoRespostaErro.Errors.Should().BeNull();

            await _mockContatoAppService.Received(1).Cadastrar(Arg.Is<CadastrarContatoRequest>(r => r.UsuarioId == usuarioId));
        }

        [Fact]
        public async Task AtualizarContato_ComDadosValidos_RetornaOk()
        {
            var usuarioId = Guid.NewGuid();
            var contatoId = Guid.NewGuid();
            var requisicaoAtualizacao = new Faker<AtualizarContatoRequest>()
                .RuleFor(r => r.Id, contatoId)
                .RuleFor(r => r.UsuarioId, usuarioId)
                .RuleFor(r => r.Celular, f => f.Phone.PhoneNumber("###########"))
                .RuleFor(r => r.Email, f => f.Internet.Email())
                .Generate();

            var contatoAtualizado = new ContatoResponse
            {
                Id = contatoId,
                UsuarioId = usuarioId,
                Celular = requisicaoAtualizacao.Celular,
                Email = requisicaoAtualizacao.Email
            };
            var mensagemSucesso = "Contato atualizado com sucesso.";

            _mockAtualizarContatoRequestValidator
                .ValidateAsync(Arg.Any<AtualizarContatoRequest>(), Arg.Any<CancellationToken>())
                .Returns(new FluentValidation.Results.ValidationResult());

            _mockContatoAppService.Atualizar(Arg.Is<AtualizarContatoRequest>(r => r.Id == contatoId && r.UsuarioId == usuarioId))
                .Returns((contatoAtualizado, true));

            var resposta = await _client.PutAsync($"/api/usuarios/{usuarioId}/contatos/Atualizar/{contatoId}", ObterConteudoJson(requisicaoAtualizacao));

            resposta.StatusCode.Should().Be(HttpStatusCode.OK);

            var conteudoResposta = await DeserializarRespostaSucesso<ContatoResponse>(resposta);

            conteudoResposta.Should().NotBeNull();
            conteudoResposta!.Success.Should().BeTrue();
            conteudoResposta.Message.Should().Be(mensagemSucesso);
            conteudoResposta.Data.Should().NotBeNull();
            conteudoResposta.Data!.Id.Should().Be(contatoAtualizado.Id);
            conteudoResposta.Data!.UsuarioId.Should().Be(contatoAtualizado.UsuarioId);

            await _mockContatoAppService.Received(1).Atualizar(Arg.Is<AtualizarContatoRequest>(r => r.Id == contatoId && r.UsuarioId == usuarioId));
        }

        [Fact]
        public async Task AtualizarContato_ComIdDaUrlDiferenteDoCorpo_RetornaBadRequest()
        {
            var usuarioId = Guid.NewGuid();
            var idUrl = Guid.NewGuid();
            var idCorpo = Guid.NewGuid();
            var requisicaoAtualizacao = new Faker<AtualizarContatoRequest>()
                .RuleFor(r => r.Id, idCorpo)
                .RuleFor(r => r.UsuarioId, usuarioId)
                .RuleFor(r => r.Celular, f => f.Phone.PhoneNumber("###########"))
                .RuleFor(r => r.Email, f => f.Internet.Email())
                .Generate();
            var mensagemErro = "Id da URL não corresponde ao Id do corpo da requisição.";

            _mockAtualizarContatoRequestValidator
                .ValidateAsync(Arg.Any<AtualizarContatoRequest>(), Arg.Any<CancellationToken>())
                .Returns(new FluentValidation.Results.ValidationResult());

            var resposta = await _client.PutAsync($"/api/usuarios/{usuarioId}/contatos/Atualizar/{idUrl}", ObterConteudoJson(requisicaoAtualizacao));

            resposta.StatusCode.Should().Be(HttpStatusCode.BadRequest);

            var conteudoRespostaErro = await DeserializarRespostaErro(resposta);

            conteudoRespostaErro.Should().NotBeNull();
            conteudoRespostaErro!.StatusCode.Should().Be(400);
            conteudoRespostaErro.Message.Should().Be(mensagemErro);
            conteudoRespostaErro.Errors.Should().NotBeNull();
            conteudoRespostaErro.Errors!.Should().ContainKey("id");
            conteudoRespostaErro.Errors!["id"].Should().Contain(mensagemErro);

            await _mockContatoAppService.DidNotReceive().Atualizar(Arg.Any<AtualizarContatoRequest>());
        }

        [Fact]
        public async Task AtualizarContato_ComContatoNaoEncontrado_RetornaNotFound()
        {
            var usuarioId = Guid.NewGuid();
            var contatoId = Guid.NewGuid();
            var requisicaoAtualizacao = new Faker<AtualizarContatoRequest>()
                .RuleFor(r => r.Id, contatoId)
                .RuleFor(r => r.UsuarioId, usuarioId)
                .RuleFor(r => r.Celular, f => f.Phone.PhoneNumber("###########"))
                .RuleFor(r => r.Email, f => f.Internet.Email())
                .Generate();
            var mensagemErro = "Contato não encontrado ou não pertence ao usuário.";

            _mockAtualizarContatoRequestValidator
                .ValidateAsync(Arg.Any<AtualizarContatoRequest>(), Arg.Any<CancellationToken>())
                .Returns(new FluentValidation.Results.ValidationResult());

            _mockContatoAppService.Atualizar(Arg.Is<AtualizarContatoRequest>(r => r.Id == contatoId && r.UsuarioId == usuarioId))
                .Returns((null, false));

            var resposta = await _client.PutAsync($"/api/usuarios/{usuarioId}/contatos/Atualizar/{contatoId}", ObterConteudoJson(requisicaoAtualizacao));

            resposta.StatusCode.Should().Be(HttpStatusCode.NotFound);

            var conteudoRespostaErro = await DeserializarRespostaErro(resposta);

            conteudoRespostaErro.Should().NotBeNull();
            conteudoRespostaErro!.StatusCode.Should().Be(404);
            conteudoRespostaErro.Message.Should().Be(mensagemErro);
            conteudoRespostaErro.Errors.Should().NotBeNull();
            conteudoRespostaErro.Errors!.Should().ContainKey("contato");
            conteudoRespostaErro.Errors!["contato"].Should().Contain(mensagemErro);

            await _mockContatoAppService.Received(1).Atualizar(Arg.Is<AtualizarContatoRequest>(r => r.Id == contatoId && r.UsuarioId == usuarioId));
        }

        [Fact]
        public async Task DeletarContato_ComContatoExistente_RetornaOk()
        {
            var usuarioId = Guid.NewGuid();
            var contatoId = Guid.NewGuid();
            var mensagemSucesso = "Contato removido com sucesso.";

            _mockContatoAppService.Deletar(contatoId, usuarioId)
                .Returns(true);

            var resposta = await _client.DeleteAsync($"/api/usuarios/{usuarioId}/contatos/Deletar/{contatoId}");

            resposta.StatusCode.Should().Be(HttpStatusCode.OK);

            var conteudoResposta = await DeserializarRespostaSucesso<object>(resposta);

            conteudoResposta.Should().NotBeNull();
            conteudoResposta!.Success.Should().BeTrue();
            conteudoResposta.Message.Should().Be(mensagemSucesso);
            conteudoResposta.Data.Should().BeNull();
            conteudoResposta.Errors.Should().BeNull();

            await _mockContatoAppService.Received(1).Deletar(contatoId, usuarioId);
        }

        [Fact]
        public async Task DeletarContato_ComContatoNaoEncontrado_RetornaNotFound()
        {
            var usuarioId = Guid.NewGuid();
            var contatoId = Guid.NewGuid();
            var mensagemErro = "Contato não encontrado ou não pertence ao usuário.";

            _mockContatoAppService.Deletar(contatoId, usuarioId)
                .Returns(false);

            var resposta = await _client.DeleteAsync($"/api/usuarios/{usuarioId}/contatos/Deletar/{contatoId}");

            resposta.StatusCode.Should().Be(HttpStatusCode.NotFound);

            var conteudoRespostaErro = await DeserializarRespostaErro(resposta);

            conteudoRespostaErro.Should().NotBeNull();
            conteudoRespostaErro!.StatusCode.Should().Be(404);
            conteudoRespostaErro.Message.Should().Be(mensagemErro);
            conteudoRespostaErro.Errors.Should().NotBeNull();
            conteudoRespostaErro.Errors!.Should().ContainKey("contato");
            conteudoRespostaErro.Errors!["contato"].Should().Contain(mensagemErro);

            await _mockContatoAppService.Received(1).Deletar(contatoId, usuarioId);
        }
    }
}

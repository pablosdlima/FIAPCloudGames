using Bogus;
using FIAPCloudGames.Application.Common.Models;
using FIAPCloudGames.Application.Interfaces;
using FIAPCloudGames.Domain.Dtos.Request.Enderecos;
using FIAPCloudGames.Domain.Dtos.Responses.Endereco;
using FluentAssertions;
using FluentValidation;
using NSubstitute;
using NSubstitute.ClearExtensions;
using System.Net;
using System.Text;
using System.Text.Json;

namespace FIAPCloudGames.Presentation.Tests.Endpoints
{
    public class EnderecoEndpointsTests : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly CustomWebApplicationFactory _factory;
        private readonly HttpClient _client;
        private readonly IEnderecoAppService _mockEnderecoAppService;
        private readonly IValidator<CadastrarEnderecoRequest> _mockCadastrarEnderecoRequestValidator;
        private readonly IValidator<AtualizarEnderecoRequest> _mockAtualizarEnderecoRequestValidator;

        public EnderecoEndpointsTests(CustomWebApplicationFactory factory)
        {
            _factory = factory;
            _client = _factory.CreateClient();
            _mockEnderecoAppService = _factory.MockEnderecoAppService;
            _mockCadastrarEnderecoRequestValidator = _factory.MockCadastrarEnderecoRequestValidator;
            _mockAtualizarEnderecoRequestValidator = _factory.MockAtualizarEnderecoRequestValidator;

            // Limpar mocks antes de cada teste
            _mockEnderecoAppService.ClearSubstitute();
            _mockCadastrarEnderecoRequestValidator.ClearSubstitute();
            _mockAtualizarEnderecoRequestValidator.ClearSubstitute();
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
        public async Task ListarEnderecosPorUsuarioId_ComUsuarioExistente_RetornaOkComEnderecos()
        {
            var usuarioId = Guid.NewGuid();
            var enderecosEsperados = new Faker<EnderecoResponse>()
                .RuleFor(e => e.Id, f => f.Random.Guid())
                .RuleFor(e => e.UsuarioId, usuarioId)
                .RuleFor(e => e.Rua, f => f.Address.StreetName())
                .RuleFor(e => e.Numero, f => f.Address.BuildingNumber())
                .RuleFor(e => e.Complemento, f => f.Address.SecondaryAddress())
                .RuleFor(e => e.Bairro, f => f.Address.County())
                .RuleFor(e => e.Cidade, f => f.Address.City())
                .RuleFor(e => e.Estado, f => f.Address.StateAbbr())
                .RuleFor(e => e.Cep, f => f.Address.ZipCode())
                .Generate(3);
            var mensagemSucesso = "Endereços listados com sucesso.";

            _mockEnderecoAppService.ListarPorUsuario(usuarioId)
                .Returns(enderecosEsperados);

            var resposta = await _client.GetAsync($"/api/usuarios/{usuarioId}/enderecos/BuscarPorUsuarioId/");

            resposta.StatusCode.Should().Be(HttpStatusCode.OK);

            var conteudoResposta = await DeserializarRespostaSucesso<List<EnderecoResponse>>(resposta);

            conteudoResposta.Should().NotBeNull();
            conteudoResposta!.Success.Should().BeTrue();
            conteudoResposta.Message.Should().Be(mensagemSucesso);
            conteudoResposta.Data.Should().NotBeNull();
            conteudoResposta.Data.Should().BeEquivalentTo(enderecosEsperados);
            conteudoResposta.Errors.Should().BeNull();

            await _mockEnderecoAppService.Received(1).ListarPorUsuario(usuarioId);
        }

        [Fact]
        public async Task ListarEnderecosPorUsuarioId_ComUsuarioInexistente_RetornaOkComListaVazia()
        {
            var usuarioId = Guid.NewGuid();
            var enderecosEsperados = new List<EnderecoResponse>();
            var mensagemSucesso = "Endereços listados com sucesso.";

            _mockEnderecoAppService.ListarPorUsuario(usuarioId)
                .Returns(enderecosEsperados);

            var resposta = await _client.GetAsync($"/api/usuarios/{usuarioId}/enderecos/BuscarPorUsuarioId/");

            resposta.StatusCode.Should().Be(HttpStatusCode.OK);

            var conteudoResposta = await DeserializarRespostaSucesso<List<EnderecoResponse>>(resposta);

            conteudoResposta.Should().NotBeNull();
            conteudoResposta!.Success.Should().BeTrue();
            conteudoResposta.Message.Should().Be(mensagemSucesso);
            conteudoResposta.Data.Should().NotBeNull();
            conteudoResposta.Data.Should().BeEmpty();
            conteudoResposta.Errors.Should().BeNull();

            await _mockEnderecoAppService.Received(1).ListarPorUsuario(usuarioId);
        }

        [Fact]
        public async Task CadastrarEndereco_ComDadosValidos_RetornaCreated()
        {
            var usuarioId = Guid.NewGuid();
            var requisicaoCadastro = new Faker<CadastrarEnderecoRequest>()
                .RuleFor(r => r.Rua, f => f.Address.StreetName())
                .RuleFor(r => r.Numero, f => f.Address.BuildingNumber())
                .RuleFor(r => r.Complemento, f => f.Address.SecondaryAddress())
                .RuleFor(r => r.Bairro, f => f.Address.County())
                .RuleFor(r => r.Cidade, f => f.Address.City())
                .RuleFor(r => r.Estado, f => f.Address.StateAbbr())
                .RuleFor(r => r.Cep, f => f.Address.ZipCode())
                .Generate();
            var enderecoCadastrado = new EnderecoResponse
            {
                Id = Guid.NewGuid(),
                UsuarioId = usuarioId,
                Rua = requisicaoCadastro.Rua,
                Numero = requisicaoCadastro.Numero,
                Complemento = requisicaoCadastro.Complemento,
                Bairro = requisicaoCadastro.Bairro,
                Cidade = requisicaoCadastro.Cidade,
                Estado = requisicaoCadastro.Estado,
                Cep = requisicaoCadastro.Cep
            };
            var mensagemSucesso = "Endereço cadastrado com sucesso.";

            _mockCadastrarEnderecoRequestValidator
                .ValidateAsync(Arg.Any<CadastrarEnderecoRequest>(), Arg.Any<CancellationToken>())
                .Returns(new FluentValidation.Results.ValidationResult());

            _mockEnderecoAppService.Cadastrar(Arg.Is<CadastrarEnderecoRequest>(r => r.UsuarioId == usuarioId))
                .Returns(enderecoCadastrado);

            var resposta = await _client.PostAsync($"/api/usuarios/{usuarioId}/enderecos/Cadastrar/", ObterConteudoJson(requisicaoCadastro));

            resposta.StatusCode.Should().Be(HttpStatusCode.Created);
            resposta.Headers.Location.Should().Be($"/api/usuarios/{usuarioId}/enderecos/{enderecoCadastrado.Id}");

            var conteudoResposta = await DeserializarRespostaSucesso<EnderecoResponse>(resposta);

            conteudoResposta.Should().NotBeNull();
            conteudoResposta!.Success.Should().BeTrue();
            conteudoResposta.Message.Should().Be(mensagemSucesso);
            conteudoResposta.Data.Should().NotBeNull();
            conteudoResposta.Data.Should().BeEquivalentTo(enderecoCadastrado);
            conteudoResposta.Errors.Should().BeNull();

            await _mockEnderecoAppService.Received(1).Cadastrar(Arg.Is<CadastrarEnderecoRequest>(r => r.UsuarioId == usuarioId));
        }

        [Fact]
        public async Task CadastrarEndereco_ComDadosInvalidos_RetornaBadRequestDoFiltroDeValidacao()
        {
            var usuarioId = Guid.NewGuid();
            var invalidRequest = new CadastrarEnderecoRequest
            {
                Rua = string.Empty,
                Numero = string.Empty,
                Complemento = null,
                Bairro = string.Empty,
                Cidade = string.Empty,
                Estado = string.Empty,
                Cep = string.Empty
            };
            var mensagemErroValidacao = "Erro de validação";
            var validationFailures = new List<FluentValidation.Results.ValidationFailure>
            {
                new FluentValidation.Results.ValidationFailure("Rua", "Rua é obrigatória"),
                new FluentValidation.Results.ValidationFailure("Numero", "Número é obrigatório"),
                new FluentValidation.Results.ValidationFailure("Bairro", "Bairro é obrigatório"),
                new FluentValidation.Results.ValidationFailure("Cidade", "Cidade é obrigatória"),
                new FluentValidation.Results.ValidationFailure("Estado", "Estado é obrigatório"),
                new FluentValidation.Results.ValidationFailure("Cep", "CEP é obrigatório")
            };

            _mockCadastrarEnderecoRequestValidator
                .ValidateAsync(Arg.Any<CadastrarEnderecoRequest>(), Arg.Any<CancellationToken>())
                .Returns(new FluentValidation.Results.ValidationResult(validationFailures));

            var resposta = await _client.PostAsync($"/api/usuarios/{usuarioId}/enderecos/Cadastrar/", ObterConteudoJson(invalidRequest));

            resposta.StatusCode.Should().Be(HttpStatusCode.BadRequest);

            var conteudoRespostaErro = await DeserializarRespostaErro(resposta);

            conteudoRespostaErro.Should().NotBeNull();
            conteudoRespostaErro!.StatusCode.Should().Be(400);
            conteudoRespostaErro.Message.Should().Be(mensagemErroValidacao);
            conteudoRespostaErro.Errors.Should().NotBeNull();
            conteudoRespostaErro.Errors!.Should().ContainKey("Rua");
            conteudoRespostaErro.Errors!["Rua"].Should().Contain("Rua é obrigatória");
            conteudoRespostaErro.Errors!.Should().ContainKey("Numero");
            conteudoRespostaErro.Errors!["Numero"].Should().Contain("Número é obrigatório");
            conteudoRespostaErro.Errors!.Should().ContainKey("Bairro");
            conteudoRespostaErro.Errors!["Bairro"].Should().Contain("Bairro é obrigatório");
            conteudoRespostaErro.Errors!.Should().ContainKey("Cidade");
            conteudoRespostaErro.Errors!["Cidade"].Should().Contain("Cidade é obrigatória");
            conteudoRespostaErro.Errors!.Should().ContainKey("Estado");
            conteudoRespostaErro.Errors!["Estado"].Should().Contain("Estado é obrigatório");
            conteudoRespostaErro.Errors!.Should().ContainKey("Cep");
            conteudoRespostaErro.Errors!["Cep"].Should().Contain("CEP é obrigatório");

            await _mockEnderecoAppService.DidNotReceive().Cadastrar(Arg.Any<CadastrarEnderecoRequest>());
        }

        [Fact]
        public async Task AtualizarEndereco_ComDadosValidos_RetornaOk()
        {
            var usuarioId = Guid.NewGuid();
            var enderecoId = Guid.NewGuid();
            var requisicaoAtualizacao = new Faker<AtualizarEnderecoRequest>()
                .RuleFor(r => r.Id, enderecoId)
                .RuleFor(r => r.UsuarioId, usuarioId)
                .RuleFor(r => r.Rua, f => f.Address.StreetName())
                .RuleFor(r => r.Numero, f => f.Address.BuildingNumber())
                .RuleFor(r => r.Complemento, f => f.Address.SecondaryAddress())
                .RuleFor(r => r.Bairro, f => f.Address.County())
                .RuleFor(r => r.Cidade, f => f.Address.City())
                .RuleFor(r => r.Estado, f => f.Address.StateAbbr())
                .RuleFor(r => r.Cep, f => f.Address.ZipCode())
                .Generate();
            var enderecoAtualizado = new EnderecoResponse
            {
                Id = enderecoId,
                UsuarioId = usuarioId,
                Rua = requisicaoAtualizacao.Rua,
                Numero = requisicaoAtualizacao.Numero,
                Complemento = requisicaoAtualizacao.Complemento,
                Bairro = requisicaoAtualizacao.Bairro,
                Cidade = requisicaoAtualizacao.Cidade,
                Estado = requisicaoAtualizacao.Estado,
                Cep = requisicaoAtualizacao.Cep
            };
            var mensagemSucesso = "Endereço atualizado com sucesso.";

            _mockAtualizarEnderecoRequestValidator
                .ValidateAsync(Arg.Any<AtualizarEnderecoRequest>(), Arg.Any<CancellationToken>())
                .Returns(new FluentValidation.Results.ValidationResult());

            _mockEnderecoAppService.Atualizar(Arg.Is<AtualizarEnderecoRequest>(r => r.Id == enderecoId && r.UsuarioId == usuarioId))
                .Returns((enderecoAtualizado, true));

            var resposta = await _client.PutAsync($"/api/usuarios/{usuarioId}/enderecos/Atualizar/{enderecoId}", ObterConteudoJson(requisicaoAtualizacao));

            resposta.StatusCode.Should().Be(HttpStatusCode.OK);

            var conteudoResposta = await DeserializarRespostaSucesso<EnderecoResponse>(resposta);

            conteudoResposta.Should().NotBeNull();
            conteudoResposta!.Success.Should().BeTrue();
            conteudoResposta.Message.Should().Be(mensagemSucesso);
            conteudoResposta.Data.Should().NotBeNull();
            conteudoResposta.Data.Should().BeEquivalentTo(enderecoAtualizado);
            conteudoResposta.Errors.Should().BeNull();

            await _mockEnderecoAppService.Received(1).Atualizar(Arg.Is<AtualizarEnderecoRequest>(r => r.Id == enderecoId && r.UsuarioId == usuarioId));
        }

        [Fact]
        public async Task AtualizarEndereco_ComIdDaUrlDiferenteDoCorpo_RetornaBadRequest()
        {
            var usuarioId = Guid.NewGuid();
            var idUrl = Guid.NewGuid();
            var idCorpo = Guid.NewGuid();
            var requisicaoAtualizacao = new Faker<AtualizarEnderecoRequest>()
                .RuleFor(r => r.Id, idCorpo)
                .RuleFor(r => r.UsuarioId, usuarioId)
                .RuleFor(r => r.Rua, f => f.Address.StreetName())
                .RuleFor(r => r.Numero, f => f.Address.BuildingNumber())
                .RuleFor(r => r.Complemento, f => f.Address.SecondaryAddress())
                .RuleFor(r => r.Bairro, f => f.Address.County())
                .RuleFor(r => r.Cidade, f => f.Address.City())
                .RuleFor(r => r.Estado, f => f.Address.StateAbbr())
                .RuleFor(r => r.Cep, f => f.Address.ZipCode())
                .Generate();
            var mensagemErro = "Id da URL não corresponde ao Id do corpo da requisição.";

            _mockAtualizarEnderecoRequestValidator
                .ValidateAsync(Arg.Any<AtualizarEnderecoRequest>(), Arg.Any<CancellationToken>())
                .Returns(new FluentValidation.Results.ValidationResult());

            var resposta = await _client.PutAsync($"/api/usuarios/{usuarioId}/enderecos/Atualizar/{idUrl}", ObterConteudoJson(requisicaoAtualizacao));

            resposta.StatusCode.Should().Be(HttpStatusCode.BadRequest);

            var conteudoRespostaErro = await DeserializarRespostaErro(resposta);

            conteudoRespostaErro.Should().NotBeNull();
            conteudoRespostaErro!.StatusCode.Should().Be(400);
            conteudoRespostaErro.Message.Should().Be(mensagemErro);
            conteudoRespostaErro.Errors.Should().NotBeNull();
            conteudoRespostaErro.Errors!.Should().ContainKey("id");
            conteudoRespostaErro.Errors!["id"].Should().Contain(mensagemErro);

            await _mockEnderecoAppService.DidNotReceive().Atualizar(Arg.Any<AtualizarEnderecoRequest>());
        }

        [Fact]
        public async Task AtualizarEndereco_ComEnderecoNaoEncontrado_RetornaNotFound()
        {
            var usuarioId = Guid.NewGuid();
            var enderecoId = Guid.NewGuid();
            var requisicaoAtualizacao = new Faker<AtualizarEnderecoRequest>()
                .RuleFor(r => r.Id, enderecoId)
                .RuleFor(r => r.UsuarioId, usuarioId)
                .RuleFor(r => r.Rua, f => f.Address.StreetName())
                .RuleFor(r => r.Numero, f => f.Address.BuildingNumber())
                .RuleFor(r => r.Complemento, f => f.Address.SecondaryAddress())
                .RuleFor(r => r.Bairro, f => f.Address.County())
                .RuleFor(r => r.Cidade, f => f.Address.City())
                .RuleFor(r => r.Estado, f => f.Address.StateAbbr())
                .RuleFor(r => r.Cep, f => f.Address.ZipCode())
                .Generate();
            var mensagemErro = "Endereço não encontrado ou não pertence ao usuário.";

            _mockAtualizarEnderecoRequestValidator
                .ValidateAsync(Arg.Any<AtualizarEnderecoRequest>(), Arg.Any<CancellationToken>())
                .Returns(new FluentValidation.Results.ValidationResult());

            _mockEnderecoAppService.Atualizar(Arg.Is<AtualizarEnderecoRequest>(r => r.Id == enderecoId && r.UsuarioId == usuarioId))
                .Returns((null, false));

            var resposta = await _client.PutAsync($"/api/usuarios/{usuarioId}/enderecos/Atualizar/{enderecoId}", ObterConteudoJson(requisicaoAtualizacao));

            resposta.StatusCode.Should().Be(HttpStatusCode.NotFound);

            var conteudoRespostaErro = await DeserializarRespostaErro(resposta);

            conteudoRespostaErro.Should().NotBeNull();
            conteudoRespostaErro!.StatusCode.Should().Be(404);
            conteudoRespostaErro.Message.Should().Be(mensagemErro);
            conteudoRespostaErro.Errors.Should().NotBeNull();
            conteudoRespostaErro.Errors!.Should().ContainKey("endereco");
            conteudoRespostaErro.Errors!["endereco"].Should().Contain(mensagemErro);

            await _mockEnderecoAppService.Received(1).Atualizar(Arg.Is<AtualizarEnderecoRequest>(r => r.Id == enderecoId && r.UsuarioId == usuarioId));
        }

        [Fact]
        public async Task AtualizarEndereco_ComDadosInvalidos_RetornaBadRequestDoFiltroDeValidacao()
        {
            var usuarioId = Guid.NewGuid();
            var enderecoId = Guid.NewGuid();
            var invalidRequest = new AtualizarEnderecoRequest
            {
                Id = enderecoId,
                UsuarioId = usuarioId,
                Rua = string.Empty,
                Numero = string.Empty,
                Complemento = null,
                Bairro = string.Empty,
                Cidade = string.Empty,
                Estado = string.Empty,
                Cep = string.Empty
            };
            var mensagemErroValidacao = "Erro de validação";
            var validationFailures = new List<FluentValidation.Results.ValidationFailure>
            {
                new FluentValidation.Results.ValidationFailure("Rua", "Rua é obrigatória"),
                new FluentValidation.Results.ValidationFailure("Numero", "Número é obrigatório"),
                new FluentValidation.Results.ValidationFailure("Bairro", "Bairro é obrigatório"),
                new FluentValidation.Results.ValidationFailure("Cidade", "Cidade é obrigatória"),
                new FluentValidation.Results.ValidationFailure("Estado", "Estado é obrigatório"),
                new FluentValidation.Results.ValidationFailure("Cep", "CEP é obrigatório")
            };

            _mockAtualizarEnderecoRequestValidator
                .ValidateAsync(Arg.Is<AtualizarEnderecoRequest>(r => r.Id == enderecoId && r.UsuarioId == usuarioId), Arg.Any<CancellationToken>())
                .Returns(new FluentValidation.Results.ValidationResult(validationFailures));

            var resposta = await _client.PutAsync($"/api/usuarios/{usuarioId}/enderecos/Atualizar/{enderecoId}", ObterConteudoJson(invalidRequest));

            resposta.StatusCode.Should().Be(HttpStatusCode.BadRequest);

            var conteudoRespostaErro = await DeserializarRespostaErro(resposta);

            conteudoRespostaErro.Should().NotBeNull();
            conteudoRespostaErro!.StatusCode.Should().Be(400);
            conteudoRespostaErro.Message.Should().Be(mensagemErroValidacao);
            conteudoRespostaErro.Errors.Should().NotBeNull();
            conteudoRespostaErro.Errors!.Should().ContainKey("Rua");
            conteudoRespostaErro.Errors!["Rua"].Should().Contain("Rua é obrigatória");
            conteudoRespostaErro.Errors!.Should().ContainKey("Numero");
            conteudoRespostaErro.Errors!["Numero"].Should().Contain("Número é obrigatório");
            conteudoRespostaErro.Errors!.Should().ContainKey("Bairro");
            conteudoRespostaErro.Errors!["Bairro"].Should().Contain("Bairro é obrigatório");
            conteudoRespostaErro.Errors!.Should().ContainKey("Cidade");
            conteudoRespostaErro.Errors!["Cidade"].Should().Contain("Cidade é obrigatória");
            conteudoRespostaErro.Errors!["Estado"].Should().Contain("Estado é obrigatório");
            conteudoRespostaErro.Errors!.Should().ContainKey("Cep");
            conteudoRespostaErro.Errors!["Cep"].Should().Contain("CEP é obrigatório");

            await _mockEnderecoAppService.DidNotReceive().Atualizar(Arg.Any<AtualizarEnderecoRequest>());
        }

        [Fact]
        public async Task DeletarEndereco_ComEnderecoExistente_RetornaOk()
        {
            var usuarioId = Guid.NewGuid();
            var enderecoId = Guid.NewGuid();
            var mensagemSucesso = "Endereço removido com sucesso.";

            _mockEnderecoAppService.Deletar(enderecoId, usuarioId)
                .Returns(true);

            var resposta = await _client.DeleteAsync($"/api/usuarios/{usuarioId}/enderecos/Deletar/{enderecoId}");

            resposta.StatusCode.Should().Be(HttpStatusCode.OK);

            var conteudoResposta = await DeserializarRespostaSucesso<object>(resposta);

            conteudoResposta.Should().NotBeNull();
            conteudoResposta!.Success.Should().BeTrue();
            conteudoResposta.Message.Should().Be(mensagemSucesso);
            conteudoResposta.Data.Should().BeNull();
            conteudoResposta.Errors.Should().BeNull();

            await _mockEnderecoAppService.Received(1).Deletar(enderecoId, usuarioId);
        }

        [Fact]
        public async Task DeletarEndereco_ComEnderecoNaoEncontrado_RetornaNotFound()
        {
            var usuarioId = Guid.NewGuid();
            var enderecoId = Guid.NewGuid();
            var mensagemErro = "Endereço não encontrado ou não pertence ao usuário.";

            _mockEnderecoAppService.Deletar(enderecoId, usuarioId)
                .Returns(false);

            var resposta = await _client.DeleteAsync($"/api/usuarios/{usuarioId}/enderecos/Deletar/{enderecoId}");

            resposta.StatusCode.Should().Be(HttpStatusCode.NotFound);

            var conteudoRespostaErro = await DeserializarRespostaErro(resposta);

            conteudoRespostaErro.Should().NotBeNull();
            conteudoRespostaErro!.StatusCode.Should().Be(404);
            conteudoRespostaErro.Message.Should().Be(mensagemErro);
            conteudoRespostaErro.Errors.Should().NotBeNull();
            conteudoRespostaErro.Errors!.Should().ContainKey("endereco");
            conteudoRespostaErro.Errors!["endereco"].Should().Contain(mensagemErro);

            await _mockEnderecoAppService.Received(1).Deletar(enderecoId, usuarioId);
        }
    }
}

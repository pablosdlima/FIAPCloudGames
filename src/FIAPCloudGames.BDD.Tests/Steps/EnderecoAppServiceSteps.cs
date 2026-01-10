using FIAPCloudGames.Application.AppServices;
using FIAPCloudGames.Application.Interfaces;
using FIAPCloudGames.Domain.Dtos.Request.Enderecos;
using FIAPCloudGames.Domain.Dtos.Responses.Endereco;
using FIAPCloudGames.Domain.Interfaces.Services;
using FIAPCloudGames.Domain.Models;
using Moq;
using TechTalk.SpecFlow;

namespace FIAPCloudGames.BDD.Tests.Steps
{
    [Binding]
    public class EnderecoAppServiceSteps
    {
        private readonly ScenarioContext _scenarioContext;
        private readonly Mock<IEnderecoService> _mockEnderecoService;
        private readonly Mock<IUsuarioService> _mockUsuarioService;
        private readonly IEnderecoAppService _enderecoAppService;

        private Guid _enderecoId;
        private List<Endereco> _enderecosUsuario;

        private string _rua;
        private string _numero;
        private string? _complemento;
        private string _bairro;
        private string _cidade;
        private string _estado;
        private string _cep;

        private bool _cadastroDeveRetornarNull;
        private bool _deletarDeveRetornarFalse;

        private List<EnderecoResponse>? _listagemResult;
        private EnderecoResponse? _enderecoResult;
        private (EnderecoResponse? Endereco, bool Success)? _atualizacaoResult;
        private bool? _deletarResult;
        private Exception? _exception;

        public EnderecoAppServiceSteps(ScenarioContext scenarioContext)
        {
            _scenarioContext = scenarioContext;
            _mockEnderecoService = new Mock<IEnderecoService>();
            _mockUsuarioService = new Mock<IUsuarioService>();

            // Registra o mock no contexto com chave única
            _scenarioContext["MockUsuarioService_Endereco"] = _mockUsuarioService;

            _enderecoAppService = new EnderecoAppService(
                _mockEnderecoService.Object,
                _mockUsuarioService.Object
            );

            _enderecosUsuario = [];
            _rua = string.Empty;
            _numero = string.Empty;
            _bairro = string.Empty;
            _cidade = string.Empty;
            _estado = string.Empty;
            _cep = string.Empty;
        }

        [Given(@"que o serviço de endereços está configurado")]
        public void DadoQueOServicoDeEnderecosEstaConfigurado()
        {
            // O serviço já está configurado no construtor
        }

        [Given(@"o usuário possui (.*) endereços cadastrados")]
        public void DadoQueOUsuarioPossuiEnderecosCadastrados(int quantidade)
        {
            var usuarioId = _scenarioContext.Get<Guid>("UsuarioId");
            _enderecosUsuario = [];

            for (var i = 0; i < quantidade; i++)
            {
                var endereco = new Endereco
                {
                    Id = Guid.NewGuid(),
                    UsuarioId = usuarioId,
                    Rua = $"Rua Teste {i}",
                    Numero = $"{100 + i}",
                    Complemento = $"Apto {i}",
                    Bairro = "Centro",
                    Cidade = "São Paulo",
                    Estado = "SP",
                    Cep = $"0131{i}-100"
                };
                _enderecosUsuario.Add(endereco);
            }

            _mockEnderecoService
                .Setup(s => s.ListarPorUsuario(usuarioId))
                .Returns(_enderecosUsuario);
        }

        [Given(@"o usuário não possui endereços cadastrados")]
        public void DadoQueOUsuarioNaoPossuiEnderecosCadastrados()
        {
            var usuarioId = _scenarioContext.Get<Guid>("UsuarioId");
            _enderecosUsuario = [];

            _mockEnderecoService
                .Setup(s => s.ListarPorUsuario(usuarioId))
                .Returns(_enderecosUsuario);
        }

        [Given(@"tenho os dados do endereço:")]
        [Given(@"tenho os dados atualizados do endereço:")]
        public void DadoQueTenhoOsDadosDoEndereco(Table table)
        {
            var dados = table.Rows.ToDictionary(r => r["Campo"], r => r["Valor"]);
            _rua = dados["Rua"];
            _numero = dados["Numero"];
            _complemento = dados.ContainsKey("Complemento") ? dados["Complemento"] : null;
            _bairro = dados["Bairro"];
            _cidade = dados["Cidade"];
            _estado = dados["Estado"];
            _cep = dados["Cep"];
        }

        [Given(@"tenho os dados do endereço sem complemento:")]
        public void DadoQueTenhoOsDadosDoEnderecoSemComplemento(Table table)
        {
            var dados = table.Rows.ToDictionary(r => r["Campo"], r => r["Valor"]);
            _rua = dados["Rua"];
            _numero = dados["Numero"];
            _complemento = null;
            _bairro = dados["Bairro"];
            _cidade = dados["Cidade"];
            _estado = dados["Estado"];
            _cep = dados["Cep"];
        }

        [Given(@"existe um endereço com ID ""(.*)""")]
        public void DadoQueExisteUmEnderecoComID(string enderecoId)
        {
            _enderecoId = Guid.Parse(enderecoId);

            _mockEnderecoService
                .Setup(s => s.Atualizar(It.Is<Endereco>(e => e.Id == _enderecoId)))
                .ReturnsAsync((Endereco enderecoAtualizado) => (enderecoAtualizado, true));
        }

        [Given(@"o endereço ""(.*)"" não existe")]
        public void DadoQueOEnderecoNaoExiste(string enderecoId)
        {
            _enderecoId = Guid.Parse(enderecoId);

            _mockEnderecoService
                .Setup(s => s.Atualizar(It.Is<Endereco>(e => e.Id == _enderecoId)))
                .ReturnsAsync(((Endereco?)null, false));
        }

        [Given(@"o endereço ""(.*)"" não pode ser deletado")]
        public void DadoQueOEnderecoNaoPodeSerDeletado(string enderecoId)
        {
            _enderecoId = Guid.Parse(enderecoId);
            _deletarDeveRetornarFalse = true;

            var usuarioId = _scenarioContext.Get<Guid>("UsuarioId");
            _mockEnderecoService
                .Setup(s => s.Deletar(_enderecoId, usuarioId))
                .ReturnsAsync(false);
        }

        [Given(@"o serviço de endereço não consegue cadastrar")]
        public void DadoQueOServicoDeEnderecoNaoConsegueCadastrar()
        {
            _cadastroDeveRetornarNull = true;

            _mockEnderecoService
                .Setup(s => s.Cadastrar(It.IsAny<Endereco>()))
                .ReturnsAsync((Endereco?)null);
        }

        [When(@"eu listar os endereços do usuário ""(.*)""")]
        public async Task QuandoEuListarOsEnderecosDoUsuario(string usuarioId)
        {
            try
            {
                var id = Guid.Parse(usuarioId);
                _listagemResult = await _enderecoAppService.ListarPorUsuario(id);
                _exception = null;
            }
            catch (Exception ex)
            {
                _exception = ex;
                _enderecoResult = null;


                _scenarioContext["Exception"] = ex;
            }

        }

        [When(@"eu cadastrar o endereço para o usuário ""(.*)""")]
        public async Task QuandoEuCadastrarOEnderecoParaOUsuario(string usuarioId)
        {
            try
            {
                var id = Guid.Parse(usuarioId);
                var request = new CadastrarEnderecoRequest
                {
                    UsuarioId = id,
                    Rua = _rua,
                    Numero = _numero,
                    Complemento = _complemento,
                    Bairro = _bairro,
                    Cidade = _cidade,
                    Estado = _estado,
                    Cep = _cep
                };

                if (!_cadastroDeveRetornarNull)
                {
                    var enderecoCadastrado = new Endereco
                    {
                        Id = Guid.NewGuid(),
                        UsuarioId = id,
                        Rua = _rua,
                        Numero = _numero,
                        Complemento = _complemento,
                        Bairro = _bairro,
                        Cidade = _cidade,
                        Estado = _estado,
                        Cep = _cep
                    };

                    _mockEnderecoService
                        .Setup(s => s.Cadastrar(It.IsAny<Endereco>()))
                        .ReturnsAsync(enderecoCadastrado);
                }

                _enderecoResult = await _enderecoAppService.Cadastrar(request);
                _exception = null;
            }
            catch (Exception ex)
            {
                _exception = ex;
                _enderecoResult = null;


                _scenarioContext["Exception"] = ex;
            }

        }

        [When(@"eu atualizar o endereço ""(.*)"" do usuário ""(.*)""")]
        public async Task QuandoEuAtualizarOEnderecoDoUsuario(string enderecoId, string usuarioId)
        {
            try
            {
                _enderecoId = Guid.Parse(enderecoId);
                var id = Guid.Parse(usuarioId);

                var request = new AtualizarEnderecoRequest
                {
                    Id = _enderecoId,
                    UsuarioId = id,
                    Rua = _rua,
                    Numero = _numero,
                    Complemento = _complemento,
                    Bairro = _bairro,
                    Cidade = _cidade,
                    Estado = _estado,
                    Cep = _cep
                };

                _atualizacaoResult = await _enderecoAppService.Atualizar(request);
                _exception = null;
            }
            catch (Exception ex)
            {
                _exception = ex;
                _enderecoResult = null;


                _scenarioContext["Exception"] = ex;
            }

        }

        [When(@"eu deletar o endereço ""(.*)"" do usuário ""(.*)""")]
        public async Task QuandoEuDeletarOEnderecoDoUsuario(string enderecoId, string usuarioId)
        {
            try
            {
                _enderecoId = Guid.Parse(enderecoId);
                var id = Guid.Parse(usuarioId);

                if (!_deletarDeveRetornarFalse)
                {
                    _mockEnderecoService
                        .Setup(s => s.Deletar(_enderecoId, id))
                        .ReturnsAsync(true);
                }

                _deletarResult = await _enderecoAppService.Deletar(_enderecoId, id);
                _exception = null;
            }
            catch (Exception ex)
            {
                _exception = ex;
                _enderecoResult = null;


                _scenarioContext["Exception"] = ex;
            }
        }

        [Then(@"a listagem de endereços deve retornar (.*) endereços")]
        public void EntaoAListagemDeEnderecosDeveRetornarEnderecos(int quantidade)
        {
            Assert.NotNull(_listagemResult);
            Assert.Equal(quantidade, _listagemResult.Count);
        }

        [Then(@"todos os endereços devem pertencer ao usuário")]
        public void EntaoTodosOsEnderecosDevemPertencerAoUsuario()
        {
            var usuarioId = _scenarioContext.Get<Guid>("UsuarioId");
            Assert.NotNull(_listagemResult);
            Assert.All(_listagemResult, e => Assert.Equal(usuarioId, e.UsuarioId));
        }

        [Then(@"o endereço deve ser cadastrado com sucesso")]
        public void EntaoOEnderecoDeveSerCadastradoComSucesso()
        {
            Assert.NotNull(_enderecoResult);
            Assert.Null(_exception);
        }

        [Then(@"o endereço retornado deve ter um ID válido")]
        public void EntaoOEnderecoRetornadoDeveTerUmIDValido()
        {
            Assert.NotNull(_enderecoResult);
            Assert.NotEqual(Guid.Empty, _enderecoResult.Id);
        }

        [Then(@"o endereço deve conter os dados informados")]
        public void EntaoOEnderecoDeveConterOsDadosInformados()
        {
            var usuarioId = _scenarioContext.Get<Guid>("UsuarioId");
            Assert.NotNull(_enderecoResult);
            Assert.Equal(_rua, _enderecoResult.Rua);
            Assert.Equal(_numero, _enderecoResult.Numero);
            Assert.Equal(_complemento, _enderecoResult.Complemento);
            Assert.Equal(_bairro, _enderecoResult.Bairro);
            Assert.Equal(_cidade, _enderecoResult.Cidade);
            Assert.Equal(_estado, _enderecoResult.Estado);
            Assert.Equal(_cep, _enderecoResult.Cep);
            Assert.Equal(usuarioId, _enderecoResult.UsuarioId);
        }

        [Then(@"o complemento deve ser nulo ou vazio")]
        public void EntaoOComplementoDeveSerNuloOuVazio()
        {
            Assert.NotNull(_enderecoResult);
            Assert.True(string.IsNullOrEmpty(_enderecoResult.Complemento));
        }

        [Then(@"o endereço deve ser atualizado com sucesso")]
        public void EntaoOEnderecoDeveSerAtualizadoComSucesso()
        {
            Assert.NotNull(_atualizacaoResult);
            Assert.True(_atualizacaoResult.Value.Success);
            Assert.NotNull(_atualizacaoResult.Value.Endereco);
        }

        [Then(@"o endereço retornado deve ter os novos dados")]
        public void EntaoOEnderecoRetornadoDeveTerOsNovosDados()
        {
            Assert.NotNull(_atualizacaoResult);
            Assert.NotNull(_atualizacaoResult.Value.Endereco);
            Assert.Equal(_rua, _atualizacaoResult.Value.Endereco.Rua);
            Assert.Equal(_numero, _atualizacaoResult.Value.Endereco.Numero);
            Assert.Equal(_complemento, _atualizacaoResult.Value.Endereco.Complemento);
            Assert.Equal(_bairro, _atualizacaoResult.Value.Endereco.Bairro);
            Assert.Equal(_cidade, _atualizacaoResult.Value.Endereco.Cidade);
            Assert.Equal(_estado, _atualizacaoResult.Value.Endereco.Estado);
            Assert.Equal(_cep, _atualizacaoResult.Value.Endereco.Cep);
        }

        [Then(@"a atualização do endereço deve falhar")]
        public void EntaoAAtualizacaoDoEnderecoDeveFalhar()
        {
            Assert.NotNull(_atualizacaoResult);
            Assert.False(_atualizacaoResult.Value.Success);
        }

        [Then(@"o resultado do endereço deve indicar falha")]
        public void EntaoOResultadoDoEnderecoDeveIndicarFalha()
        {
            Assert.NotNull(_atualizacaoResult);
            Assert.Null(_atualizacaoResult.Value.Endereco);
        }

        [Then(@"o endereço deve ser deletado com sucesso")]
        public void EntaoOEnderecoDeveSerDeletadoComSucesso()
        {
            Assert.NotNull(_deletarResult);
            Assert.True(_deletarResult.Value);
            Assert.Null(_exception);
        }

        [Then(@"a exclusão do endereço deve falhar")]
        public void EntaoAExclusaoDoEnderecoDeveFalhar()
        {
            Assert.NotNull(_deletarResult);
            Assert.False(_deletarResult.Value);
        }

        [AfterScenario(Order = 1)]
        public void LimparCenario()
        {
            _enderecosUsuario.Clear();
            _listagemResult = null;
            _enderecoResult = null;
            _atualizacaoResult = null;
            _deletarResult = null;
            _exception = null;
            _rua = string.Empty;
            _numero = string.Empty;
            _complemento = null;
            _bairro = string.Empty;
            _cidade = string.Empty;
            _estado = string.Empty;
            _cep = string.Empty;
            _cadastroDeveRetornarNull = false;
            _deletarDeveRetornarFalse = false;
        }
    }
}

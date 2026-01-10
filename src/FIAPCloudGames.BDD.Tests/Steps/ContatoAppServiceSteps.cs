using FIAPCloudGames.Application.AppServices;
using FIAPCloudGames.Application.Interfaces;
using FIAPCloudGames.Domain.Dtos.Request.Contato;
using FIAPCloudGames.Domain.Dtos.Responses.Contato;
using FIAPCloudGames.Domain.Interfaces.Services;
using FIAPCloudGames.Domain.Models;
using Moq;
using TechTalk.SpecFlow;

namespace FIAPCloudGames.BDD.Tests.Steps
{
    [Binding]
    public class ContatoAppServiceSteps
    {
        private readonly ScenarioContext _scenarioContext;
        private readonly Mock<IContatoService> _mockContatoService;
        private readonly Mock<IUsuarioService> _mockUsuarioService;
        private readonly IContatoAppService _contatoAppService;

        private Guid _contatoId;
        private List<Contato> _contatosUsuario;
        private string _celular;
        private string _email;
        private bool _cadastroDeveRetornarNull;
        private bool _deletarDeveRetornarFalse;
        private List<ContatoResponse>? _listagemResult;
        private ContatoResponse? _contatoResult;
        private (ContatoResponse? Contato, bool Success)? _atualizacaoResult;
        private bool? _deletarResult;
        private Exception? _exception;

        public ContatoAppServiceSteps(ScenarioContext scenarioContext)
        {
            _scenarioContext = scenarioContext;
            _mockContatoService = new Mock<IContatoService>();
            _mockUsuarioService = new Mock<IUsuarioService>();
            _scenarioContext["MockUsuarioService_Contato"] = _mockUsuarioService;
            _contatoAppService = new ContatoAppService(
                _mockContatoService.Object,
                _mockUsuarioService.Object
            );
            _contatosUsuario = [];
            _celular = string.Empty;
            _email = string.Empty;
        }

        [Given(@"que o serviço de contatos está configurado")]
        public void DadoQueOServicoDeContatosEstaConfigurado()
        {
        }

        [Given(@"o usuário possui (.*) contatos cadastrados")]
        public void DadoQueOUsuarioPossuiContatosCadastrados(int quantidade)
        {
            var usuarioId = _scenarioContext.Get<Guid>("UsuarioId");
            _contatosUsuario = [];

            for (var i = 0; i < quantidade; i++)
            {
                var contato = new Contato($"(11) 9876{i}-4321", $"contato{i}@email.com")
                {
                    Id = Guid.NewGuid(),
                    UsuarioId = usuarioId
                };
                _contatosUsuario.Add(contato);
            }

            _mockContatoService.Setup(s => s.ListarPorUsuario(usuarioId)).Returns(_contatosUsuario);
        }

        [Given(@"o usuário não possui contatos cadastrados")]
        public void DadoQueOUsuarioNaoPossuiContatosCadastrados()
        {
            var usuarioId = _scenarioContext.Get<Guid>("UsuarioId");
            _contatosUsuario = [];

            _mockContatoService.Setup(s => s.ListarPorUsuario(usuarioId)).Returns(_contatosUsuario);
        }

        [Given(@"tenho os dados do contato:")]
        [Given(@"tenho os dados atualizados do contato:")]
        public void DadoQueTenhoOsDadosDoContato(Table table)
        {
            var dados = table.Rows.ToDictionary(r => r["Campo"], r => r["Valor"]);
            _celular = dados["Celular"];
            _email = dados["Email"];
        }

        [Given(@"existe um contato com ID ""(.*)""")]
        public void DadoQueExisteUmContatoComID(string contatoId)
        {
            _contatoId = Guid.Parse(contatoId);

            _mockContatoService
                .Setup(s => s.Atualizar(It.Is<Contato>(c => c.Id == _contatoId)))
                .ReturnsAsync((Contato contatoAtualizado) => (contatoAtualizado, true));
        }

        [Given(@"o contato ""(.*)"" não existe")]
        public void DadoQueOContatoNaoExiste(string contatoId)
        {
            _contatoId = Guid.Parse(contatoId);
            _mockContatoService.Setup(s => s.Atualizar(It.Is<Contato>(c => c.Id == _contatoId))).ReturnsAsync(((Contato?)null, false));
        }

        [Given(@"o contato ""(.*)"" não pode ser deletado")]
        public void DadoQueOContatoNaoPodeSerDeletado(string contatoId)
        {
            _contatoId = Guid.Parse(contatoId);
            _deletarDeveRetornarFalse = true;

            var usuarioId = _scenarioContext.Get<Guid>("UsuarioId");

            _mockContatoService.Setup(s => s.Deletar(_contatoId, usuarioId)).ReturnsAsync(false);
        }

        [Given(@"o serviço de contato não consegue cadastrar")]
        public void DadoQueOServicoDeContatoNaoConsegueCadastrar()
        {
            _cadastroDeveRetornarNull = true;
            _mockContatoService.Setup(s => s.Cadastrar(It.IsAny<Contato>())).ReturnsAsync((Contato?)null);
        }

        [When(@"eu listar os contatos do usuário ""(.*)""")]
        public async Task QuandoEuListarOsContatosDoUsuario(string usuarioId)
        {
            try
            {
                var id = Guid.Parse(usuarioId);
                _listagemResult = await _contatoAppService.ListarPorUsuario(id);
                _exception = null;
            }
            catch (Exception ex)
            {
                _exception = ex;
                _contatoResult = null;
                _scenarioContext["Exception"] = ex;
            }
        }

        [When(@"eu cadastrar o contato para o usuário ""(.*)""")]
        public async Task QuandoEuCadastrarOContatoParaOUsuario(string usuarioId)
        {
            try
            {
                var id = Guid.Parse(usuarioId);
                var request = new CadastrarContatoRequest
                {
                    UsuarioId = id,
                    Celular = _celular,
                    Email = _email
                };

                if (!_cadastroDeveRetornarNull)
                {
                    var contatoCadastrado = new Contato(_celular, _email)
                    {
                        Id = Guid.NewGuid(),
                        UsuarioId = id
                    };

                    _mockContatoService
                        .Setup(s => s.Cadastrar(It.IsAny<Contato>()))
                        .ReturnsAsync(contatoCadastrado);
                }

                _contatoResult = await _contatoAppService.Cadastrar(request);
                _exception = null;
            }
            catch (Exception ex)
            {
                _exception = ex;
                _contatoResult = null;
                _scenarioContext["Exception"] = ex;
            }
        }

        [When(@"eu atualizar o contato ""(.*)"" do usuário ""(.*)""")]
        public async Task QuandoEuAtualizarOContatoDoUsuario(string contatoId, string usuarioId)
        {
            try
            {
                _contatoId = Guid.Parse(contatoId);
                var id = Guid.Parse(usuarioId);

                var request = new AtualizarContatoRequest
                {
                    Id = _contatoId,
                    UsuarioId = id,
                    Celular = _celular,
                    Email = _email
                };

                _atualizacaoResult = await _contatoAppService.Atualizar(request);
                _exception = null;
            }
            catch (Exception ex)
            {
                _exception = ex;
                _contatoResult = null;
                _scenarioContext["Exception"] = ex;
            }
        }

        [When(@"eu deletar o contato ""(.*)"" do usuário ""(.*)""")]
        public async Task QuandoEuDeletarOContatoDoUsuario(string contatoId, string usuarioId)
        {
            try
            {
                _contatoId = Guid.Parse(contatoId);
                var id = Guid.Parse(usuarioId);

                if (!_deletarDeveRetornarFalse)
                {
                    _mockContatoService
                        .Setup(s => s.Deletar(_contatoId, id))
                        .ReturnsAsync(true);
                }

                _deletarResult = await _contatoAppService.Deletar(_contatoId, id);
                _exception = null;
            }
            catch (Exception ex)
            {
                _exception = ex;
                _contatoResult = null;
                _scenarioContext["Exception"] = ex;
            }
        }

        [Then(@"a listagem deve retornar (.*) contatos")]
        public void EntaoAListagemDeveRetornarContatos(int quantidade)
        {
            Assert.NotNull(_listagemResult);
            Assert.Equal(quantidade, _listagemResult.Count);
        }

        [Then(@"todos os contatos devem pertencer ao usuário")]
        public void EntaoTodosOsContatosDevemPertencerAoUsuario()
        {
            var usuarioId = _scenarioContext.Get<Guid>("UsuarioId");
            Assert.NotNull(_listagemResult);
            Assert.All(_listagemResult, c => Assert.Equal(usuarioId, c.UsuarioId));
        }

        [Then(@"o contato deve ser cadastrado com sucesso")]
        public void EntaoOContatoDeveSerCadastradoComSucesso()
        {
            Assert.NotNull(_contatoResult);
            Assert.Null(_exception);
        }

        [Then(@"o contato retornado deve ter um ID válido")]
        public void EntaoOContatoRetornadoDeveTerUmIDValido()
        {
            Assert.NotNull(_contatoResult);
            Assert.NotEqual(Guid.Empty, _contatoResult.Id);
        }

        [Then(@"o contato deve conter os dados informados")]
        public void EntaoOContatoDeveConterOsDadosInformados()
        {
            var usuarioId = _scenarioContext.Get<Guid>("UsuarioId");
            Assert.NotNull(_contatoResult);
            Assert.Equal(_celular, _contatoResult.Celular);
            Assert.Equal(_email, _contatoResult.Email);
            Assert.Equal(usuarioId, _contatoResult.UsuarioId);
        }

        [Then(@"o contato deve ser atualizado com sucesso")]
        public void EntaoOContatoDeveSerAtualizadoComSucesso()
        {
            Assert.NotNull(_atualizacaoResult);
            Assert.True(_atualizacaoResult.Value.Success);
            Assert.NotNull(_atualizacaoResult.Value.Contato);
        }

        [Then(@"o contato retornado deve ter os novos dados")]
        public void EntaoOContatoRetornadoDeveTerOsNovosDados()
        {
            Assert.NotNull(_atualizacaoResult);
            Assert.NotNull(_atualizacaoResult.Value.Contato);
            Assert.Equal(_celular, _atualizacaoResult.Value.Contato.Celular);
            Assert.Equal(_email, _atualizacaoResult.Value.Contato.Email);
        }

        [Then(@"a atualização deve falhar")]
        public void EntaoAAtualizacaoDeveFalhar()
        {
            Assert.NotNull(_atualizacaoResult);
            Assert.False(_atualizacaoResult.Value.Success);
        }

        [Then(@"o resultado deve indicar falha")]
        public void EntaoOResultadoDeveIndicarFalha()
        {
            Assert.NotNull(_atualizacaoResult);
            Assert.Null(_atualizacaoResult.Value.Contato);
        }

        [Then(@"o contato deve ser deletado com sucesso")]
        public void EntaoOContatoDeveSerDeletadoComSucesso()
        {
            Assert.NotNull(_deletarResult);
            Assert.True(_deletarResult.Value);
            Assert.Null(_exception);
        }

        [Then(@"a exclusão deve falhar")]
        public void EntaoAExclusaoDeveFalhar()
        {
            Assert.NotNull(_deletarResult);
            Assert.False(_deletarResult.Value);
        }

        [AfterScenario(Order = 1)]
        public void LimparCenario()
        {
            _contatosUsuario.Clear();
            _listagemResult = null;
            _contatoResult = null;
            _atualizacaoResult = null;
            _deletarResult = null;
            _exception = null;
            _celular = string.Empty;
            _email = string.Empty;
            _cadastroDeveRetornarNull = false;
            _deletarDeveRetornarFalse = false;
        }
    }
}

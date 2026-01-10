using FIAPCloudGames.Application.AppServices;
using FIAPCloudGames.Application.Interfaces;
using FIAPCloudGames.Domain.Dtos.Request.UsuarioGameBiblioteca;
using FIAPCloudGames.Domain.Dtos.Responses.UsuarioGameBiblioteca;
using FIAPCloudGames.Domain.Interfaces.Services;
using FIAPCloudGames.Domain.Models;
using Moq;
using TechTalk.SpecFlow;

namespace FIAPCloudGames.BDD.Tests.Steps
{
    [Binding]
    public class UsuarioGameBibliotecaAppServiceSteps
    {
        private readonly ScenarioContext _scenarioContext;
        private readonly Mock<IUsuarioGameBibliotecaService> _mockBibliotecaService;
        private readonly Mock<IUsuarioService> _mockUsuarioService;
        private readonly Mock<IGameService> _mockGameService;
        private readonly IUsuarioGameBibliotecaAppService _bibliotecaAppService;

        private Guid _usuarioId;
        private Guid _gameId;
        private Guid _bibliotecaId;
        private List<UsuarioGameBiblioteca> _biblioteca;
        private string _tipoAquisicao;
        private decimal _precoAquisicao;
        private DateTimeOffset? _dataAquisicao;
        private bool _compraDeveFalhar;
        private bool _atualizacaoDeveFalhar;
        private bool _deletarDeveFalhar;
        private List<BibliotecaResponse>? _listagemResult;
        private (BibliotecaResponse? Biblioteca, bool Success, string? ErrorMessage)? _compraResult;
        private (BibliotecaResponse? Biblioteca, bool Success)? _atualizacaoResult;
        private bool? _deletarResult;
        private Exception? _exception;

        public UsuarioGameBibliotecaAppServiceSteps(ScenarioContext scenarioContext)
        {
            _scenarioContext = scenarioContext;
            _mockBibliotecaService = new Mock<IUsuarioGameBibliotecaService>();
            _mockUsuarioService = new Mock<IUsuarioService>();
            _mockGameService = new Mock<IGameService>();

            _scenarioContext["MockUsuarioService_Biblioteca"] = _mockUsuarioService;
            _scenarioContext["MockGameService_Biblioteca"] = _mockGameService;

            _bibliotecaAppService = new UsuarioGameBibliotecaAppService(
                _mockBibliotecaService.Object,
                _mockUsuarioService.Object,
                _mockGameService.Object
            );

            _biblioteca = [];
            _tipoAquisicao = string.Empty;
        }

        [Given(@"que o serviço de biblioteca está configurado")]
        public void DadoQueOServicoDeBibliotecaEstaConfigurado()
        {
        }

        [Given(@"o usuário possui (.*) jogos na biblioteca")]
        [Given(@"o usuário possui (.*) jogo na biblioteca")]
        public void DadoQueOUsuarioPossuiJogosNaBiblioteca(int quantidade)
        {
            _usuarioId = _scenarioContext.Get<Guid>("UsuarioId");
            _biblioteca = [];

            for (var i = 0; i < quantidade; i++)
            {
                var game = Game.Criar(
                    $"Game {i + 1}",
                    $"Descrição {i + 1}",
                    "Ação",
                    "Developer",
                    199.90m,
                    new DateTimeOffset(new DateTime(2020, 1, 1), TimeSpan.Zero)
                );
                game.Id = Guid.NewGuid();

                var item = new UsuarioGameBiblioteca
                {
                    Id = Guid.NewGuid(),
                    UsuarioId = _usuarioId,
                    GameId = game.Id,
                    TipoAquisicao = "Compra",
                    PrecoAquisicao = 199.90m,
                    DataAquisicao = DateTimeOffset.Now,
                    Game = game
                };
                _biblioteca.Add(item);
            }

            _mockBibliotecaService.Setup(s => s.ListarPorUsuario(_usuarioId)).Returns(_biblioteca);
        }

        [Given(@"o usuário não possui jogos na biblioteca")]
        public void DadoQueOUsuarioNaoPossuiJogosNaBiblioteca()
        {
            _usuarioId = _scenarioContext.Get<Guid>("UsuarioId");
            _biblioteca = [];

            _mockBibliotecaService.Setup(s => s.ListarPorUsuario(_usuarioId)).Returns(_biblioteca);
        }


        [Given(@"existe um game com ID ""(.*)""")]
        public void DadoQueExisteUmGameComID(string gameId)
        {
            _gameId = Guid.Parse(gameId);

            var game = Game.Criar(
                "The Last of Us",
                "Jogo de ação",
                "Ação",
                "Naughty Dog",
                299.90m,
                new DateTimeOffset(new DateTime(2013, 6, 14), TimeSpan.Zero)
            );
            game.Id = _gameId;

            _mockGameService.Setup(s => s.GetById(_gameId)).Returns(game);
        }

        [Given(@"não existe um game com ID ""(.*)""")]
        public void DadoQueNaoExisteUmGameComID(string gameId)
        {
            _gameId = Guid.Parse(gameId);
            _mockGameService.Setup(s => s.GetById(_gameId)).Returns((Game?)null);
        }


        [Given(@"existe um item na biblioteca com ID ""(.*)""")]
        public void DadoQueExisteUmItemNaBibliotecaComID(string bibliotecaId)
        {
            _bibliotecaId = Guid.Parse(bibliotecaId);

            _mockBibliotecaService
                .Setup(s => s.Atualizar(It.Is<UsuarioGameBiblioteca>(b => b.Id == _bibliotecaId)))
                .ReturnsAsync((UsuarioGameBiblioteca bibliotecaAtualizada) => (bibliotecaAtualizada, true));

            _mockBibliotecaService.Setup(s => s.Deletar(_bibliotecaId, It.IsAny<Guid>())).ReturnsAsync(true);
        }

        [Given(@"o item da biblioteca não existe")]
        public void DadoQueOItemDaBibliotecaNaoExiste()
        {
            _atualizacaoDeveFalhar = true;

            _mockBibliotecaService
                .Setup(s => s.Atualizar(It.IsAny<UsuarioGameBiblioteca>()))
                .ReturnsAsync(((UsuarioGameBiblioteca?)null, false));
        }

        [Given(@"o item da biblioteca não pode ser deletado")]
        public void DadoQueOItemDaBibliotecaNaoPodeSerDeletado()
        {
            _deletarDeveFalhar = true;
            _mockBibliotecaService.Setup(s => s.Deletar(It.IsAny<Guid>(), It.IsAny<Guid>())).ReturnsAsync(false);
        }


        [Given(@"tenho os dados da compra:")]
        [Given(@"tenho os dados atualizados da biblioteca:")]
        public void DadoQueTenhoOsDadosDaCompra(Table table)
        {
            var dados = table.Rows.ToDictionary(r => r["Campo"], r => r["Valor"]);
            _tipoAquisicao = dados["TipoAquisicao"];
            _precoAquisicao = decimal.Parse(dados["PrecoAquisicao"]);
            _dataAquisicao = DateTimeOffset.Parse(dados["DataAquisicao"]);
        }


        [Given(@"o serviço de biblioteca não consegue comprar o game")]
        public void DadoQueOServicoDeBibliotecaNaoConsegueComprarOGame()
        {
            _compraDeveFalhar = true;

            _mockBibliotecaService
                .Setup(s => s.ComprarGame(It.IsAny<UsuarioGameBiblioteca>()))
                .ReturnsAsync(((UsuarioGameBiblioteca?)null, false, "Erro ao comprar o game"));
        }


        [When(@"eu listar a biblioteca do usuário ""(.*)""")]
        public async Task QuandoEuListarABibliotecaDoUsuario(string usuarioId)
        {
            try
            {
                _usuarioId = Guid.Parse(usuarioId);
                _listagemResult = await _bibliotecaAppService.ListarPorUsuario(_usuarioId);
                _exception = null;
            }
            catch (Exception ex)
            {
                _exception = ex;
                _listagemResult = null;
                _scenarioContext["Exception"] = ex;
            }
        }

        [When(@"eu comprar o game ""(.*)"" para o usuário ""(.*)""")]
        public async Task QuandoEuComprarOGameParaOUsuario(string gameId, string usuarioId)
        {
            try
            {
                _gameId = Guid.Parse(gameId);
                _usuarioId = Guid.Parse(usuarioId);

                var request = new ComprarGameRequest
                {
                    UsuarioId = _usuarioId,
                    GameId = _gameId,
                    TipoAquisicao = _tipoAquisicao,
                    PrecoAquisicao = _precoAquisicao,
                    DataAquisicao = _dataAquisicao
                };

                if (!_compraDeveFalhar)
                {
                    var game = _mockGameService.Object.GetById(_gameId);
                    var bibliotecaComprada = new UsuarioGameBiblioteca
                    {
                        Id = Guid.NewGuid(),
                        UsuarioId = _usuarioId,
                        GameId = _gameId,
                        TipoAquisicao = _tipoAquisicao,
                        PrecoAquisicao = _precoAquisicao,
                        DataAquisicao = _dataAquisicao,
                        Game = game
                    };

                    _mockBibliotecaService
                        .Setup(s => s.ComprarGame(It.IsAny<UsuarioGameBiblioteca>()))
                        .ReturnsAsync((bibliotecaComprada, true, (string?)null));
                }

                _compraResult = await _bibliotecaAppService.ComprarGame(request);
                _exception = null;
            }
            catch (Exception ex)
            {
                _exception = ex;
                _compraResult = null;
                _scenarioContext["Exception"] = ex;
            }
        }

        [When(@"eu atualizar o item ""(.*)"" da biblioteca")]
        public async Task QuandoEuAtualizarOItemDaBiblioteca(string bibliotecaId)
        {
            try
            {
                _bibliotecaId = Guid.Parse(bibliotecaId);
                _usuarioId = _scenarioContext.Get<Guid>("UsuarioId");

                var request = new AtualizarBibliotecaRequest
                {
                    Id = _bibliotecaId,
                    UsuarioId = _usuarioId,
                    GameId = _gameId,
                    TipoAquisicao = _tipoAquisicao,
                    PrecoAquisicao = _precoAquisicao,
                    DataAquisicao = _dataAquisicao
                };

                _atualizacaoResult = await _bibliotecaAppService.Atualizar(request);
                _exception = null;
            }
            catch (Exception ex)
            {
                _exception = ex;
                _atualizacaoResult = null;
                _scenarioContext["Exception"] = ex;
            }
        }


        [When(@"eu deletar o item ""(.*)"" do usuário ""(.*)""")]
        public async Task QuandoEuDeletarOItemDoUsuario(string bibliotecaId, string usuarioId)
        {
            try
            {
                _bibliotecaId = Guid.Parse(bibliotecaId);
                _usuarioId = Guid.Parse(usuarioId);

                _deletarResult = await _bibliotecaAppService.Deletar(_bibliotecaId, _usuarioId);
                _exception = null;
            }
            catch (Exception ex)
            {
                _exception = ex;
                _deletarResult = null;
                _scenarioContext["Exception"] = ex;
            }
        }


        [Then(@"a listagem deve retornar (.*) jogos")]
        [Then(@"a listagem deve retornar (.*) jogo")]
        public void EntaoAListagemDeveRetornarJogos(int quantidade)
        {
            Assert.NotNull(_listagemResult);
            Assert.Equal(quantidade, _listagemResult.Count);
        }

        [Then(@"todos os jogos devem pertencer ao usuário")]
        public void EntaoTodosOsJogosDevemPertencerAoUsuario()
        {
            Assert.NotNull(_listagemResult);
            Assert.All(_listagemResult, b => Assert.Equal(_usuarioId, b.UsuarioId));
        }


        [Then(@"a compra deve ser realizada com sucesso")]
        public void EntaoACompraDeveSerRealizadaComSucesso()
        {
            Assert.NotNull(_compraResult);
            Assert.True(_compraResult.Value.Success);
            Assert.NotNull(_compraResult.Value.Biblioteca);
        }

        [Then(@"o jogo deve estar na biblioteca do usuário")]
        public void EntaoOJogoDeveEstarNaBibliotecaDoUsuario()
        {
            Assert.NotNull(_compraResult);
            Assert.NotNull(_compraResult.Value.Biblioteca);
            Assert.Equal(_usuarioId, _compraResult.Value.Biblioteca.UsuarioId);
            Assert.Equal(_gameId, _compraResult.Value.Biblioteca.GameId);
        }

        [Then(@"a compra deve falhar")]
        public void EntaoACompraDeveFalhar()
        {
            Assert.NotNull(_compraResult);
            Assert.False(_compraResult.Value.Success);
        }


        [Then(@"a atualização deve ser realizada com sucesso")]
        public void EntaoAAtualizacaoDeveSerRealizadaComSucesso()
        {
            Assert.NotNull(_atualizacaoResult);
            Assert.True(_atualizacaoResult.Value.Success);
            Assert.NotNull(_atualizacaoResult.Value.Biblioteca);
        }

        [Then(@"o item deve ter os dados atualizados")]
        public void EntaoOItemDeveTerOsDadosAtualizados()
        {
            Assert.NotNull(_atualizacaoResult);
            Assert.NotNull(_atualizacaoResult.Value.Biblioteca);
            Assert.Equal(_tipoAquisicao, _atualizacaoResult.Value.Biblioteca.TipoAquisicao);
            Assert.Equal(_precoAquisicao, _atualizacaoResult.Value.Biblioteca.PrecoAquisicao);
        }

        [Then(@"a atualização da biblioteca deve falhar")]
        public void EntaoAAtualizacaoDaBibliotecaDeveFalhar()
        {
            Assert.NotNull(_atualizacaoResult);
            Assert.False(_atualizacaoResult.Value.Success);
        }


        [Then(@"o item deve ser deletado com sucesso")]
        public void EntaoOItemDeveSerDeletadoComSucesso()
        {
            Assert.NotNull(_deletarResult);
            Assert.True(_deletarResult.Value);
        }

        [Then(@"a exclusão da biblioteca deve falhar")]
        public void EntaoAExclusaoDaBibliotecaDeveFalhar()
        {
            Assert.NotNull(_deletarResult);
            Assert.False(_deletarResult.Value);
        }


        [AfterScenario(Order = 1)]
        public void LimparCenario()
        {
            _biblioteca.Clear();
            _listagemResult = null;
            _compraResult = null;
            _atualizacaoResult = null;
            _deletarResult = null;
            _exception = null;
            _tipoAquisicao = string.Empty;
            _precoAquisicao = 0;
            _dataAquisicao = null;
            _compraDeveFalhar = false;
            _atualizacaoDeveFalhar = false;
            _deletarDeveFalhar = false;
        }
    }
}

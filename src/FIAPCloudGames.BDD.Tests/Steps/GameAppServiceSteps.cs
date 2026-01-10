using FIAPCloudGames.Application.AppServices;
using FIAPCloudGames.Application.Interfaces;
using FIAPCloudGames.Domain.Dtos.Request.Game;
using FIAPCloudGames.Domain.Dtos.Responses.Game;
using FIAPCloudGames.Domain.Interfaces.Services;
using FIAPCloudGames.Domain.Models;
using Moq;
using TechTalk.SpecFlow;

namespace FIAPCloudGames.BDD.Tests.Steps
{
    [Binding]
    public class GameAppServiceSteps
    {
        private readonly ScenarioContext _scenarioContext;
        private readonly Mock<IGameService> _mockGameService;
        private readonly IGameAppService _gameAppService;

        private Guid _gameId;
        private List<Game> _gamesCadastrados;
        private string _nome;
        private string _descricao;
        private string _genero;
        private string _desenvolvedor;
        private decimal _preco;
        private DateTime? _dataRelease;
        private int _numeroPagina;
        private int _tamanhoPagina;
        private string? _filtro;
        private string? _generoFiltro;
        private Game? _gameResult;
        private ListarGamesPaginadoResponse? _listagemResult;
        private (AtualizarGameResponse? Game, bool Success)? _atualizacaoResult;
        private Exception? _exception;

        public GameAppServiceSteps(ScenarioContext scenarioContext)
        {
            _scenarioContext = scenarioContext;
            _mockGameService = new Mock<IGameService>();

            _gameAppService = new GameAppService(_mockGameService.Object);

            _gamesCadastrados = [];
            _nome = string.Empty;
            _descricao = string.Empty;
            _genero = string.Empty;
            _desenvolvedor = string.Empty;
        }


        [Given(@"que o serviço de games está configurado")]
        public void DadoQueOServicoDeGamesEstaConfigurado()
        {
        }


        [Given(@"que tenho os dados do game:")]
        [Given(@"tenho os dados atualizados do game:")]
        public void DadoQueTenhoOsDadosDoGame(Table table)
        {
            var dados = table.Rows.ToDictionary(r => r["Campo"], r => r["Valor"]);
            _nome = dados["Nome"];
            _descricao = dados["Descricao"];
            _genero = dados["Genero"];
            _desenvolvedor = dados["Desenvolvedor"];
            _preco = decimal.Parse(dados["Preco"]);
            _dataRelease = DateTime.Parse(dados["DataRelease"]);
        }


        [Given(@"que existe um game com ID ""(.*)""")]
        public void DadoQueExisteUmGameComID(string gameId)
        {
            _gameId = Guid.Parse(gameId);

            var dataRelease = new DateTimeOffset(new DateTime(2013, 6, 14), TimeSpan.Zero);
            var game = Game.Criar(
                "The Last of Us",
                "Jogo de ação",
                "Ação",
                "Naughty Dog",
                199.90m,
                dataRelease
            );
            game.Id = _gameId;

            _mockGameService.Setup(s => s.GetById(_gameId)).Returns(game);

            _mockGameService
                .Setup(s => s.AtualizarGame(It.Is<Game>(g => g.Id == _gameId)))
                .ReturnsAsync((Game gameAtualizado) => (gameAtualizado, true));
        }

        [Given(@"que não existe um game com ID ""(.*)""")]
        public void DadoQueNaoExisteUmGameComID(string gameId)
        {
            _gameId = Guid.Parse(gameId);

            _mockGameService.Setup(s => s.GetById(_gameId)).Returns((Game?)null);

            _mockGameService
                .Setup(s => s.AtualizarGame(It.Is<Game>(g => g.Id == _gameId)))
                .ReturnsAsync(((Game?)null, false));
        }


        [Given(@"que existem (.*) games cadastrados")]
        public void DadoQueExistemGamesCadastrados(int quantidade)
        {
            _gamesCadastrados = [];

            for (var i = 1; i <= quantidade; i++)
            {
                var dataRelease = new DateTimeOffset(new DateTime(2020, 1, i), TimeSpan.Zero);
                var game = Game.Criar(
                    $"Game {i}",
                    $"Descrição do Game {i}",
                    "Ação",
                    "Developer",
                    99.90m,
                    dataRelease
                );
                game.Id = Guid.NewGuid();
                _gamesCadastrados.Add(game);
            }

            _mockGameService
                .Setup(s => s.ListarPaginado(It.IsAny<int>(), It.IsAny<int>(), null, null))
                .ReturnsAsync((int numeroPagina, int tamanhoPagina, string? filtro, string? genero) =>
                {
                    var inicio = (numeroPagina - 1) * tamanhoPagina;
                    var gamesPagina = _gamesCadastrados.Skip(inicio).Take(tamanhoPagina).ToList();
                    return (gamesPagina, _gamesCadastrados.Count);
                });
        }

        [Given(@"que não existem games cadastrados")]
        public void DadoQueNaoExistemGamesCadastrados()
        {
            _gamesCadastrados = [];

            _mockGameService
                .Setup(s => s.ListarPaginado(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(([], 0));
        }

        [Given(@"que existem games cadastrados com diferentes nomes")]
        public void DadoQueExistemGamesCadastradosComDiferentesNomes()
        {
            var data1 = new DateTimeOffset(new DateTime(2013, 6, 14), TimeSpan.Zero);
            var data2 = new DateTimeOffset(new DateTime(2020, 6, 19), TimeSpan.Zero);
            var data3 = new DateTimeOffset(new DateTime(2018, 4, 20), TimeSpan.Zero);
            var data4 = new DateTimeOffset(new DateTime(2017, 2, 28), TimeSpan.Zero);

            _gamesCadastrados =
            [
                Game.Criar("The Last of Us", "Desc", "Ação", "Dev", 199m, data1),
                Game.Criar("The Last of Us Part II", "Desc", "Ação", "Dev", 299m, data2),
                Game.Criar("God of War", "Desc", "Ação", "Dev", 249m, data3),
                Game.Criar("Horizon Zero Dawn", "Desc", "RPG", "Dev", 199m, data4)
            ];

            foreach (var game in _gamesCadastrados)
            {
                game.Id = Guid.NewGuid();
            }

            _mockGameService
                .Setup(s => s.ListarPaginado(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), null))
                .ReturnsAsync((int numeroPagina, int tamanhoPagina, string? filtro, string? genero) =>
                {
                    var gamesFiltrados = string.IsNullOrEmpty(filtro)
                        ? _gamesCadastrados
                        : _gamesCadastrados.Where(g => g.Nome.Contains(filtro, StringComparison.OrdinalIgnoreCase)).ToList();

                    var inicio = (numeroPagina - 1) * tamanhoPagina;
                    var gamesPagina = gamesFiltrados.Skip(inicio).Take(tamanhoPagina).ToList();
                    return (gamesPagina, gamesFiltrados.Count);
                });
        }

        [Given(@"que existem games cadastrados com diferentes gêneros")]
        public void DadoQueExistemGamesCadastradosComDiferentesGeneros()
        {
            var dataRelease = new DateTimeOffset(new DateTime(2020, 1, 1), TimeSpan.Zero);

            _gamesCadastrados =
            [
                Game.Criar("Game Ação 1", "Desc", "Ação", "Dev", 199m, dataRelease),
                Game.Criar("Game Ação 2", "Desc", "Ação", "Dev", 199m, dataRelease),
                Game.Criar("Game RPG 1", "Desc", "RPG", "Dev", 199m, dataRelease),
                Game.Criar("Game Aventura 1", "Desc", "Aventura", "Dev", 199m, dataRelease)
            ];

            foreach (var game in _gamesCadastrados)
            {
                game.Id = Guid.NewGuid();
            }

            _mockGameService
                .Setup(s => s.ListarPaginado(It.IsAny<int>(), It.IsAny<int>(), null, It.IsAny<string>()))
                .ReturnsAsync((int numeroPagina, int tamanhoPagina, string? filtro, string? genero) =>
                {
                    var gamesFiltrados = string.IsNullOrEmpty(genero)
                        ? _gamesCadastrados
                        : _gamesCadastrados.Where(g => g.Genero.Equals(genero, StringComparison.OrdinalIgnoreCase)).ToList();

                    var inicio = (numeroPagina - 1) * tamanhoPagina;
                    var gamesPagina = gamesFiltrados.Skip(inicio).Take(tamanhoPagina).ToList();
                    return (gamesPagina, gamesFiltrados.Count);
                });
        }


        [When(@"eu cadastrar o game")]
        public async Task QuandoEuCadastrarOGame()
        {
            try
            {
                var request = new CadastrarGameRequest
                {
                    Nome = _nome,
                    Descricao = _descricao,
                    Genero = _genero,
                    Desenvolvedor = _desenvolvedor,
                    Preco = _preco,
                    DataRelease = _dataRelease!.Value
                };

                var dataReleaseOffset = new DateTimeOffset(_dataRelease.Value, TimeSpan.Zero);
                var gameCadastrado = Game.Criar(_nome, _descricao, _genero, _desenvolvedor, _preco, dataReleaseOffset);
                gameCadastrado.Id = Guid.NewGuid();

                _mockGameService
                    .Setup(s => s.Insert(It.IsAny<Game>()))
                    .ReturnsAsync(gameCadastrado);

                _gameResult = await _gameAppService.Cadastrar(request);
                _exception = null;
            }
            catch (Exception ex)
            {
                _exception = ex;
                _gameResult = null;
                _scenarioContext["Exception"] = ex;
            }
        }

        [When(@"eu buscar o game por ID ""(.*)""")]
        public void QuandoEuBuscarOGamePorID(string gameId)
        {
            try
            {
                _gameId = Guid.Parse(gameId);
                _gameResult = _gameAppService.BuscarPorId(_gameId);
                _exception = null;
            }
            catch (Exception ex)
            {
                _exception = ex;
                _gameResult = null;
                _scenarioContext["Exception"] = ex;
            }
        }

        [When(@"eu listar os games com página (.*) e tamanho (.*)")]
        public async Task QuandoEuListarOsGamesComPaginaETamanho(int numeroPagina, int tamanhoPagina)
        {
            try
            {
                _numeroPagina = numeroPagina;
                _tamanhoPagina = tamanhoPagina;

                var request = new ListarGamesPaginadoRequest
                {
                    NumeroPagina = numeroPagina,
                    TamanhoPagina = tamanhoPagina
                };

                _listagemResult = await _gameAppService.ListarGamesPaginado(request);
                _exception = null;
            }
            catch (Exception ex)
            {
                _exception = ex;
                _listagemResult = null;
                _scenarioContext["Exception"] = ex;
            }
        }

        [When(@"eu listar os games com filtro ""(.*)"" na página (.*) e tamanho (.*)")]
        public async Task QuandoEuListarOsGamesComFiltroNaPaginaETamanho(string filtro, int numeroPagina, int tamanhoPagina)
        {
            try
            {
                _numeroPagina = numeroPagina;
                _tamanhoPagina = tamanhoPagina;
                _filtro = filtro;

                var request = new ListarGamesPaginadoRequest
                {
                    NumeroPagina = numeroPagina,
                    TamanhoPagina = tamanhoPagina,
                    Filtro = filtro
                };

                _listagemResult = await _gameAppService.ListarGamesPaginado(request);
                _exception = null;
            }
            catch (Exception ex)
            {
                _exception = ex;
                _listagemResult = null;
                _scenarioContext["Exception"] = ex;
            }
        }

        [When(@"eu listar os games com gênero ""(.*)"" na página (.*) e tamanho (.*)")]
        public async Task QuandoEuListarOsGamesComGeneroNaPaginaETamanho(string genero, int numeroPagina, int tamanhoPagina)
        {
            try
            {
                _numeroPagina = numeroPagina;
                _tamanhoPagina = tamanhoPagina;
                _generoFiltro = genero;

                var request = new ListarGamesPaginadoRequest
                {
                    NumeroPagina = numeroPagina,
                    TamanhoPagina = tamanhoPagina,
                    Genero = genero
                };

                _listagemResult = await _gameAppService.ListarGamesPaginado(request);
                _exception = null;
            }
            catch (Exception ex)
            {
                _exception = ex;
                _listagemResult = null;
                _scenarioContext["Exception"] = ex;
            }
        }

        [When(@"eu atualizar o game ""(.*)""")]
        public async Task QuandoEuAtualizarOGame(string gameId)
        {
            try
            {
                _gameId = Guid.Parse(gameId);

                DateOnly? dataReleaseOnly = _dataRelease.HasValue ? DateOnly.FromDateTime(_dataRelease.Value) : null;

                var request = new AtualizarGameRequest
                {
                    Id = _gameId,
                    Nome = _nome,
                    Descricao = _descricao,
                    Genero = _genero,
                    Desenvolvedor = _desenvolvedor,
                    Preco = _preco,
                    DataRelease = dataReleaseOnly
                };

                _atualizacaoResult = await _gameAppService.AtualizarGame(request);
                _exception = null;
            }
            catch (Exception ex)
            {
                _exception = ex;
                _atualizacaoResult = null;
                _scenarioContext["Exception"] = ex;
            }
        }


        [Then(@"o game deve ser cadastrado com sucesso")]
        public void EntaoOGameDeveSerCadastradoComSucesso()
        {
            Assert.NotNull(_gameResult);
            Assert.Null(_exception);
        }

        [Then(@"o game retornado deve ter um ID válido")]
        public void EntaoOGameRetornadoDeveTerUmIDValido()
        {
            Assert.NotNull(_gameResult);
            Assert.NotEqual(Guid.Empty, _gameResult.Id);
        }

        [Then(@"o game deve conter os dados informados")]
        public void EntaoOGameDeveConterOsDadosInformados()
        {
            Assert.NotNull(_gameResult);
            Assert.Equal(_nome, _gameResult.Nome);
            Assert.Equal(_descricao, _gameResult.Descricao);
            Assert.Equal(_genero, _gameResult.Genero);
            Assert.Equal(_desenvolvedor, _gameResult.Desenvolvedor);
            Assert.Equal(_preco, _gameResult.Preco);
        }


        [Then(@"o game deve ser retornado com sucesso")]
        public void EntaoOGameDeveSerRetornadoComSucesso()
        {
            Assert.NotNull(_gameResult);
            Assert.Null(_exception);
        }

        [Then(@"o game deve ter o ID ""(.*)""")]
        public void EntaoOGameDeveTerOID(string gameId)
        {
            Assert.NotNull(_gameResult);
            Assert.Equal(Guid.Parse(gameId), _gameResult.Id);
        }


        [Then(@"a listagem deve retornar (.*) games")]
        public void EntaoAListagemDeveRetornarGames(int quantidade)
        {
            Assert.NotNull(_listagemResult);
            Assert.Equal(quantidade, _listagemResult.Jogos.Count);
        }

        [Then(@"deve indicar que existe próxima página")]
        public void EntaoDeveIndicarQueExisteProximaPagina()
        {
            Assert.NotNull(_listagemResult);
            Assert.True(_listagemResult.TemProximaPagina);
        }

        [Then(@"deve indicar que não existe próxima página")]
        public void EntaoDeveIndicarQueNaoExisteProximaPagina()
        {
            Assert.NotNull(_listagemResult);
            Assert.False(_listagemResult.TemProximaPagina);
        }

        [Then(@"deve indicar que existe página anterior")]
        public void EntaoDeveIndicarQueExistePaginaAnterior()
        {
            Assert.NotNull(_listagemResult);
            Assert.True(_listagemResult.TemPaginaAnterior);
        }

        [Then(@"deve indicar que não existe página anterior")]
        public void EntaoDeveIndicarQueNaoExistePaginaAnterior()
        {
            Assert.NotNull(_listagemResult);
            Assert.False(_listagemResult.TemPaginaAnterior);
        }

        [Then(@"o total de páginas deve ser (.*)")]
        public void EntaoOTotalDePaginasDeveSer(int totalPaginas)
        {
            Assert.NotNull(_listagemResult);
            Assert.Equal(totalPaginas, _listagemResult.TotalPaginas);
        }

        [Then(@"a listagem deve retornar apenas games filtrados")]
        public void EntaoAListagemDeveRetornarApenasGamesFiltrados()
        {
            Assert.NotNull(_listagemResult);
            Assert.All(_listagemResult.Jogos, g =>
                Assert.Contains(_filtro!, g.Nome, StringComparison.OrdinalIgnoreCase));
        }

        [Then(@"a listagem deve retornar apenas games do gênero ""(.*)""")]
        public void EntaoAListagemDeveRetornarApenasGamesDoGenero(string genero)
        {
            Assert.NotNull(_listagemResult);
            Assert.All(_listagemResult.Jogos, g => Assert.Equal(genero, g.Genero, StringComparer.OrdinalIgnoreCase));
        }


        [Then(@"o game deve ser atualizado com sucesso")]
        public void EntaoOGameDeveSerAtualizadoComSucesso()
        {
            Assert.NotNull(_atualizacaoResult);
            Assert.True(_atualizacaoResult.Value.Success);
            Assert.NotNull(_atualizacaoResult.Value.Game);
        }

        [Then(@"o game retornado deve ter os novos dados")]
        public void EntaoOGameRetornadoDeveTerOsNovosDados()
        {
            Assert.NotNull(_atualizacaoResult);
            Assert.NotNull(_atualizacaoResult.Value.Game);
            Assert.Equal(_nome, _atualizacaoResult.Value.Game.Nome);
            Assert.Equal(_descricao, _atualizacaoResult.Value.Game.Descricao);
            Assert.Equal(_genero, _atualizacaoResult.Value.Game.Genero);
            Assert.Equal(_desenvolvedor, _atualizacaoResult.Value.Game.Desenvolvedor);
            Assert.Equal(_preco, _atualizacaoResult.Value.Game.Preco);
        }

        [Then(@"a atualização do game deve falhar")]
        public void EntaoAAtualizacaoDoGameDeveFalhar()
        {
            Assert.NotNull(_atualizacaoResult);
            Assert.False(_atualizacaoResult.Value.Success);
        }

        [Then(@"o resultado do game deve indicar falha")]
        public void EntaoOResultadoDoGameDeveIndicarFalha()
        {
            Assert.NotNull(_atualizacaoResult);
            Assert.Null(_atualizacaoResult.Value.Game);
        }


        [AfterScenario(Order = 1)]
        public void LimparCenario()
        {
            _gamesCadastrados.Clear();
            _gameResult = null;
            _listagemResult = null;
            _atualizacaoResult = null;
            _exception = null;
            _nome = string.Empty;
            _descricao = string.Empty;
            _genero = string.Empty;
            _desenvolvedor = string.Empty;
            _preco = 0;
            _dataRelease = null;
            _filtro = null;
            _generoFiltro = null;
        }
    }
}

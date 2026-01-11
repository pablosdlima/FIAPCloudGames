using FIAPCloudGames.Application.AppServices;
using FIAPCloudGames.Application.Interfaces;
using FIAPCloudGames.Domain.Dtos.Request.UsuarioPerfil;
using FIAPCloudGames.Domain.Dtos.Responses.UsuarioPerfil;
using FIAPCloudGames.Domain.Interfaces.Services;
using FIAPCloudGames.Domain.Models;
using Microsoft.Extensions.Logging;
using Moq;
using TechTalk.SpecFlow;

namespace FIAPCloudGames.BDD.Tests.Steps
{
    [Binding]
    public class UsuarioPerfilAppServiceSteps
    {
        private readonly ScenarioContext _scenarioContext;
        private readonly Mock<IUsuarioPerfilService> _mockPerfilService;
        private readonly Mock<IUsuarioService> _mockUsuarioService;
        private readonly Mock<ILogger<UsuarioPerfilAppService>> _mockLogger;
        private readonly IUsuarioPerfilAppService _perfilAppService;
        private Guid _usuarioId;
        private Guid _perfilId;
        private string _nomeCompleto;
        private DateTimeOffset? _dataNascimento;
        private string _pais;
        private string _avatarUrl;
        private bool _cadastroDeveRetornarNull;
        private bool _atualizacaoDeveFalhar;
        private bool _deletarDeveFalhar;
        private BuscarUsuarioPerfilResponse? _perfilResult;
        private (BuscarUsuarioPerfilResponse? Perfil, bool Success)? _atualizacaoResult;
        private bool? _deletarResult;
        private Exception? _exception;

        public UsuarioPerfilAppServiceSteps(ScenarioContext scenarioContext)
        {
            _scenarioContext = scenarioContext;
            _mockPerfilService = new Mock<IUsuarioPerfilService>();
            _mockUsuarioService = new Mock<IUsuarioService>();
            _mockLogger = new Mock<ILogger<UsuarioPerfilAppService>>();
            _scenarioContext["MockUsuarioService_Perfil"] = _mockUsuarioService;
            _perfilAppService = new UsuarioPerfilAppService(
                _mockPerfilService.Object,
                _mockUsuarioService.Object,
                _mockLogger.Object
            );

            _nomeCompleto = string.Empty;
            _pais = string.Empty;
            _avatarUrl = string.Empty;
        }

        [Given(@"que o serviço de perfil está configurado")]
        public void DadoQueOServicoDePerfilEstaConfigurado()
        {
        }


        [Given(@"o usuário possui um perfil cadastrado")]
        public void DadoQueOUsuarioPossuiUmPerfilCadastrado()
        {
            _usuarioId = _scenarioContext.Get<Guid>("UsuarioId");

            var perfil = new UsuarioPerfil(
                "Marcio Silva",
                new DateTimeOffset(new DateTime(1990, 5, 15), TimeSpan.Zero),
                "Brasil",
                "https://avatar.url"
            )
            {
                Id = Guid.NewGuid(),
                UsuarioId = _usuarioId
            };

            _mockPerfilService.Setup(s => s.BuscarPorUsuarioId(_usuarioId)).Returns(perfil);
        }

        [Given(@"o usuário não possui perfil cadastrado")]
        public void DadoQueOUsuarioNaoPossuiPerfilCadastrado()
        {
            _usuarioId = _scenarioContext.Get<Guid>("UsuarioId");

            _mockPerfilService.Setup(s => s.BuscarPorUsuarioId(_usuarioId)).Returns((UsuarioPerfil?)null);
        }

        [Given(@"existe um perfil com ID ""(.*)""")]
        public void DadoQueExisteUmPerfilComID(string perfilId)
        {
            _perfilId = Guid.Parse(perfilId);
            _usuarioId = _scenarioContext.Get<Guid>("UsuarioId");

            _mockPerfilService
                .Setup(s => s.Atualizar(It.Is<UsuarioPerfil>(p => p.Id == _perfilId)))
                .ReturnsAsync((UsuarioPerfil perfilAtualizado) => (perfilAtualizado, true));

            _mockPerfilService.Setup(s => s.Deletar(_perfilId, _usuarioId)).ReturnsAsync(true);
        }

        [Given(@"o perfil não existe")]
        public void DadoQueOPerfilNaoExiste()
        {
            _atualizacaoDeveFalhar = true;

            _mockPerfilService.Setup(s => s.Atualizar(It.IsAny<UsuarioPerfil>())).ReturnsAsync(((UsuarioPerfil?)null, false));
        }

        [Given(@"o perfil não pode ser deletado")]
        public void DadoQueOPerfilNaoPodeSerDeletado()
        {
            _deletarDeveFalhar = true;

            _mockPerfilService
                .Setup(s => s.Deletar(It.IsAny<Guid>(), It.IsAny<Guid>()))
                .ReturnsAsync(false);
        }


        [Given(@"tenho os dados do perfil:")]
        [Given(@"tenho os dados atualizados do perfil:")]
        public void DadoQueTenhoOsDadosDoPerfil(Table table)
        {
            var dados = table.Rows.ToDictionary(r => r["Campo"], r => r["Valor"]);
            _nomeCompleto = dados["NomeCompleto"];
            _dataNascimento = DateTimeOffset.Parse(dados["DataNascimento"]);
            _pais = dados["Pais"];
            _avatarUrl = dados["AvatarUrl"];
        }


        [Given(@"o serviço de perfil não consegue cadastrar")]
        public void DadoQueOServicoDePerfilNaoConsegueCadastrar()
        {
            _cadastroDeveRetornarNull = true;

            _mockPerfilService.Setup(s => s.Cadastrar(It.IsAny<UsuarioPerfil>())).ReturnsAsync((UsuarioPerfil?)null);
        }

        [When(@"eu buscar o perfil do usuário ""(.*)""")]
        public async Task QuandoEuBuscarOPerfilDoUsuario(string usuarioId)
        {
            try
            {
                _usuarioId = Guid.Parse(usuarioId);
                _perfilResult = await _perfilAppService.BuscarPorUsuarioId(_usuarioId);
                _exception = null;
            }
            catch (Exception ex)
            {
                _exception = ex;
                _perfilResult = null;
                _scenarioContext["Exception"] = ex;
            }
        }

        [When(@"eu cadastrar o perfil para o usuário ""(.*)""")]
        public async Task QuandoEuCadastrarOPerfilParaOUsuario(string usuarioId)
        {
            try
            {
                _usuarioId = Guid.Parse(usuarioId);

                var request = new CadastrarUsuarioPerfilRequest
                {
                    UsuarioId = _usuarioId,
                    NomeCompleto = _nomeCompleto,
                    DataNascimento = _dataNascimento,
                    Pais = _pais,
                    AvatarUrl = _avatarUrl
                };

                if (!_cadastroDeveRetornarNull)
                {
                    var perfilCadastrado = new UsuarioPerfil(
                        _nomeCompleto,
                        _dataNascimento,
                        _pais,
                        _avatarUrl
                    )
                    {
                        Id = Guid.NewGuid(),
                        UsuarioId = _usuarioId
                    };

                    _mockPerfilService
                        .Setup(s => s.Cadastrar(It.IsAny<UsuarioPerfil>()))
                        .ReturnsAsync(perfilCadastrado);
                }

                _perfilResult = await _perfilAppService.Cadastrar(request);
                _exception = null;
            }
            catch (Exception ex)
            {
                _exception = ex;
                _perfilResult = null;
                _scenarioContext["Exception"] = ex;
            }
        }

        [When(@"eu atualizar o perfil ""(.*)""")]
        public async Task QuandoEuAtualizarOPerfil(string perfilId)
        {
            try
            {
                _perfilId = Guid.Parse(perfilId);
                _usuarioId = _scenarioContext.Get<Guid>("UsuarioId");

                var request = new AtualizarUsuarioPerfilRequest
                {
                    Id = _perfilId,
                    UsuarioId = _usuarioId,
                    NomeCompleto = _nomeCompleto,
                    DataNascimento = _dataNascimento,
                    Pais = _pais,
                    AvatarUrl = _avatarUrl
                };

                _atualizacaoResult = await _perfilAppService.Atualizar(request);
                _exception = null;
            }
            catch (Exception ex)
            {
                _exception = ex;
                _atualizacaoResult = null;
                _scenarioContext["Exception"] = ex;
            }
        }

        [When(@"eu deletar o perfil ""(.*)"" do usuário ""(.*)""")]
        public async Task QuandoEuDeletarOPerfilDoUsuario(string perfilId, string usuarioId)
        {
            try
            {
                _perfilId = Guid.Parse(perfilId);
                _usuarioId = Guid.Parse(usuarioId);

                _deletarResult = await _perfilAppService.Deletar(_perfilId, _usuarioId);
                _exception = null;
            }
            catch (Exception ex)
            {
                _exception = ex;
                _deletarResult = null;
                _scenarioContext["Exception"] = ex;
            }
        }

        [Then(@"o perfil deve ser retornado com sucesso")]
        public void EntaoOPerfilDeveSerRetornadoComSucesso()
        {
            Assert.NotNull(_perfilResult);
            Assert.Null(_exception);
        }

        [Then(@"o perfil deve conter as informações do usuário")]
        public void EntaoOPerfilDeveConterAsInformacoesDoUsuario()
        {
            Assert.NotNull(_perfilResult);
            Assert.NotNull(_perfilResult.NomeCompleto);
            Assert.Equal(_usuarioId, _perfilResult.UsuarioId);
        }

        [Then(@"o perfil deve ser cadastrado com sucesso")]
        public void EntaoOPerfilDeveSerCadastradoComSucesso()
        {
            Assert.NotNull(_perfilResult);
            Assert.Null(_exception);
        }

        [Then(@"o perfil retornado deve ter um ID válido")]
        public void EntaoOPerfilRetornadoDeveTerUmIDValido()
        {
            Assert.NotNull(_perfilResult);
            Assert.NotEqual(Guid.Empty, _perfilResult.Id);
        }

        [Then(@"o perfil deve conter os dados informados")]
        public void EntaoOPerfilDeveConterOsDadosInformados()
        {
            Assert.NotNull(_perfilResult);
            Assert.Equal(_nomeCompleto, _perfilResult.NomeCompleto);
            Assert.Equal(_dataNascimento, _perfilResult.DataNascimento);
            Assert.Equal(_pais, _perfilResult.Pais);
            Assert.Equal(_avatarUrl, _perfilResult.AvatarUrl);
        }

        [Then(@"o perfil deve ser atualizado com sucesso")]
        public void EntaoOPerfilDeveSerAtualizadoComSucesso()
        {
            Assert.NotNull(_atualizacaoResult);
            Assert.True(_atualizacaoResult.Value.Success);
            Assert.NotNull(_atualizacaoResult.Value.Perfil);
        }

        [Then(@"o perfil retornado deve ter os novos dados")]
        public void EntaoOPerfilRetornadoDeveTerOsNovosDados()
        {
            Assert.NotNull(_atualizacaoResult);
            Assert.NotNull(_atualizacaoResult.Value.Perfil);
            Assert.Equal(_nomeCompleto, _atualizacaoResult.Value.Perfil.NomeCompleto);
            Assert.Equal(_dataNascimento, _atualizacaoResult.Value.Perfil.DataNascimento);
            Assert.Equal(_pais, _atualizacaoResult.Value.Perfil.Pais);
            Assert.Equal(_avatarUrl, _atualizacaoResult.Value.Perfil.AvatarUrl);
        }

        [Then(@"a atualização do perfil deve falhar")]
        public void EntaoAAtualizacaoDoPerfilDeveFalhar()
        {
            Assert.NotNull(_atualizacaoResult);
            Assert.False(_atualizacaoResult.Value.Success);
        }

        [Then(@"o perfil deve ser deletado com sucesso")]
        public void EntaoOPerfilDeveSerDeletadoComSucesso()
        {
            Assert.NotNull(_deletarResult);
            Assert.True(_deletarResult.Value);
        }

        [Then(@"a exclusão do perfil deve falhar")]
        public void EntaoAExclusaoDoPerfilDeveFalhar()
        {
            Assert.NotNull(_deletarResult);
            Assert.False(_deletarResult.Value);
        }

        [AfterScenario(Order = 1)]
        public void LimparCenario()
        {
            _perfilResult = null;
            _atualizacaoResult = null;
            _deletarResult = null;
            _exception = null;
            _nomeCompleto = string.Empty;
            _dataNascimento = null;
            _pais = string.Empty;
            _avatarUrl = string.Empty;
            _cadastroDeveRetornarNull = false;
            _atualizacaoDeveFalhar = false;
            _deletarDeveFalhar = false;
        }
    }
}

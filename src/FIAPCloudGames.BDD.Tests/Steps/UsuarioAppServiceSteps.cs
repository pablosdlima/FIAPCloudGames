using FIAPCloudGames.Application.AppServices;
using FIAPCloudGames.Application.Interfaces;
using FIAPCloudGames.Domain.Dtos.Request.Usuario;
using FIAPCloudGames.Domain.Dtos.Responses.Usuario;
using FIAPCloudGames.Domain.Dtos.Responses.Usuarios;
using FIAPCloudGames.Domain.Enums;
using FIAPCloudGames.Domain.Interfaces.Services;
using FIAPCloudGames.Domain.Models;
using Microsoft.Extensions.Logging;
using Moq;
using TechTalk.SpecFlow;

namespace FIAPCloudGames.BDD.Tests.Steps
{
    [Binding]
    public class UsuarioAppServiceSteps
    {
        private readonly ScenarioContext _scenarioContext;
        private readonly Mock<IUsuarioService> _mockUsuarioService;
        private readonly Mock<ILogger<UsuarioAppService>> _mockLogger;
        private readonly IUsuarioAppService _usuarioAppService;

        private Guid _usuarioId;
        private string _nome;
        private string _senha;
        private string _celular;
        private string _email;
        private string _nomeCompleto;
        private DateTimeOffset? _dataNascimento;
        private string _pais;
        private string _avatarUrl;
        private bool _cadastroDeveRetornarNull;
        private bool _alterarSenhaDeveFalhar;
        private CadastrarUsuarioResponse? _cadastroResult;
        private BuscarPorIdResponse? _usuarioResult;
        private bool? _alterarSenhaResult;
        private AlterarStatusResponse? _alterarStatusResult;
        private Exception? _exception;

        public UsuarioAppServiceSteps(ScenarioContext scenarioContext)
        {
            _scenarioContext = scenarioContext;
            _mockUsuarioService = new Mock<IUsuarioService>();
            _mockLogger = new Mock<ILogger<UsuarioAppService>>();
            _scenarioContext["MockUsuarioService_Usuario"] = _mockUsuarioService;
            _usuarioAppService = new UsuarioAppService(_mockUsuarioService.Object, _mockLogger.Object);
            _nome = string.Empty;
            _senha = string.Empty;
            _celular = string.Empty;
            _email = string.Empty;
            _nomeCompleto = string.Empty;
            _pais = string.Empty;
            _avatarUrl = string.Empty;
        }


        [Given(@"que o serviço de usuários está configurado")]
        public void DadoQueOServicoDeUsuariosEstaConfigurado()
        {
        }


        [Given(@"que tenho os dados do usuário:")]
        [Given(@"tenho os dados do usuário:")]
        public void DadoQueTenhoOsDadosDoUsuario(Table table)
        {
            var dados = table.Rows.ToDictionary(r => r["Campo"], r => r["Valor"]);
            _nome = dados["Nome"];
            _senha = dados["Senha"];
            _celular = dados["Celular"];
            _email = dados["Email"];
            _nomeCompleto = dados["NomeCompleto"];
            _dataNascimento = DateTimeOffset.Parse(dados["DataNascimento"]);
            _pais = dados["Pais"];
            _avatarUrl = dados["AvatarUrl"];
        }


        [Given(@"que existe um usuário completo com perfil e roles com ID ""(.*)""")]
        public void DadoQueExisteUmUsuarioCompletoComPerfilERolesComID(string usuarioId)
        {
            _usuarioId = Guid.Parse(usuarioId);

            var usuario = Usuario.Criar("Marcio Silva", "Senha@123", true);
            usuario.Id = _usuarioId;

            var perfil = new UsuarioPerfil(
                "Marcio Silva",
                new DateTimeOffset(new DateTime(1990, 5, 15), TimeSpan.Zero),
                "Brasil",
                "https://avatar.url"
            )
            {
                UsuarioId = _usuarioId
            };
            usuario.Perfil = perfil;

            usuario.UsuarioRoles =
            [
                new UsuarioRole
                {
                    Id = Guid.NewGuid(),
                    UsuarioId = _usuarioId,
                    RoleId = 1,
                    Role = new Role
                    {
                        Id = 1,
                        RoleName = "Admin",
                        Description = "Administrador"
                    }
                }
            ];

            _mockUsuarioService.Setup(s => s.GetById(_usuarioId)).Returns(usuario);
        }

        [Given(@"que existe um usuário ativo para alteração de status com ID ""(.*)""")]
        public void DadoQueExisteUmUsuarioAtivoParaAlteracaoDeStatusComID(string usuarioId)
        {
            _usuarioId = Guid.Parse(usuarioId);

            var usuario = Usuario.Criar("Marcio Silva", "Senha@123", true);
            usuario.Id = _usuarioId;

            _mockUsuarioService.Setup(s => s.GetById(_usuarioId)).Returns(usuario);

            _mockUsuarioService.Setup(s => s.AlterarStatus(_usuarioId))
                .ReturnsAsync(new AlterarStatusResponse("Inativo"));
        }

        [Given(@"que existe um usuário inativo para alteração de status com ID ""(.*)""")]
        public void DadoQueExisteUmUsuarioInativoParaAlteracaoDeStatusComID(string usuarioId)
        {
            _usuarioId = Guid.Parse(usuarioId);

            var usuario = Usuario.Criar("Marcio Silva", "Senha@123", false);
            usuario.Id = _usuarioId;

            _mockUsuarioService.Setup(s => s.GetById(_usuarioId)).Returns(usuario);

            _mockUsuarioService.Setup(s => s.AlterarStatus(_usuarioId))
                .ReturnsAsync(new AlterarStatusResponse("Ativo"));
        }

        [Given(@"que existe um usuário para alterar senha com ID ""(.*)""")]
        public void DadoQueExisteUmUsuarioParaAlterarSenhaComID(string usuarioId)
        {
            _usuarioId = Guid.Parse(usuarioId);

            var usuario = Usuario.Criar("Marcio Silva", "Senha@123", true);
            usuario.Id = _usuarioId;

            _mockUsuarioService.Setup(s => s.GetById(_usuarioId)).Returns(usuario);

            _mockUsuarioService.Setup(s => s.AlterarSenha(It.IsAny<AlterarSenhaRequest>())).ReturnsAsync(true);
        }


        [Given(@"que o serviço de usuário não consegue cadastrar")]
        public void DadoQueOServicoDeUsuarioNaoConsegueCadastrar()
        {
            _cadastroDeveRetornarNull = true;
            _mockUsuarioService.Setup(s => s.CadastrarUsuario(It.IsAny<CadastrarUsuarioRequest>())).ReturnsAsync((Usuario?)null);
        }

        [Given(@"que a alteração de senha vai falhar")]
        public void DadoQueAAlteracaoDeSenhaVaiFalhar()
        {
            _alterarSenhaDeveFalhar = true;
            _mockUsuarioService.Setup(s => s.AlterarSenha(It.IsAny<AlterarSenhaRequest>())).ReturnsAsync(false);
        }


        [When(@"eu cadastrar o usuário")]
        public async Task QuandoEuCadastrarOUsuario()
        {
            try
            {
                var request = new CadastrarUsuarioRequest
                {
                    Nome = _nome,
                    Senha = _senha,
                    Celular = _celular,
                    Email = _email,
                    TipoUsuario = TipoUsuario.Usuario,
                    NomeCompleto = _nomeCompleto,
                    DataNascimento = _dataNascimento,
                    Pais = _pais,
                    AvatarUrl = _avatarUrl
                };

                if (!_cadastroDeveRetornarNull)
                {
                    var usuarioCadastrado = Usuario.Criar(_nome, _senha, true);
                    usuarioCadastrado.Id = Guid.NewGuid();

                    _mockUsuarioService
                        .Setup(s => s.CadastrarUsuario(It.IsAny<CadastrarUsuarioRequest>()))
                        .ReturnsAsync(usuarioCadastrado);
                }

                _cadastroResult = await _usuarioAppService.Cadastrar(request);
                _exception = null;
            }
            catch (Exception ex)
            {
                _exception = ex;
                _cadastroResult = null;
                _scenarioContext["Exception"] = ex;
            }
        }

        [When(@"eu buscar o usuário por ID ""(.*)""")]
        public void QuandoEuBuscarOUsuarioPorID(string usuarioId)
        {
            try
            {
                _usuarioId = Guid.Parse(usuarioId);
                _usuarioResult = _usuarioAppService.BuscarPorId(_usuarioId);
                _exception = null;
            }
            catch (Exception ex)
            {
                _exception = ex;
                _usuarioResult = null;
                _scenarioContext["Exception"] = ex;
            }
        }

        [When(@"eu alterar a senha do usuário ""(.*)"" para ""(.*)""")]
        public async Task QuandoEuAlterarASenhaDoUsuario(string usuarioId, string novaSenha)
        {
            try
            {
                _usuarioId = Guid.Parse(usuarioId);
                var request = new AlterarSenhaRequest(_usuarioId, novaSenha);

                _alterarSenhaResult = await _usuarioAppService.AlterarSenha(request);
                _exception = null;
            }
            catch (Exception ex)
            {
                _exception = ex;
                _alterarSenhaResult = null;
                _scenarioContext["Exception"] = ex;
            }
        }

        [When(@"eu alterar o status do usuário ""(.*)""")]
        public async Task QuandoEuAlterarOStatusDoUsuario(string usuarioId)
        {
            try
            {
                _usuarioId = Guid.Parse(usuarioId);
                _alterarStatusResult = await _usuarioAppService.AlterarStatus(_usuarioId);
                _exception = null;
            }
            catch (Exception ex)
            {
                _exception = ex;
                _alterarStatusResult = null;
                _scenarioContext["Exception"] = ex;
            }
        }


        [Then(@"o usuário deve ser cadastrado com sucesso")]
        public void EntaoOUsuarioDeveSerCadastradoComSucesso()
        {
            Assert.NotNull(_cadastroResult);
            Assert.Null(_exception);
        }

        [Then(@"o usuário retornado deve ter um ID válido")]
        public void EntaoOUsuarioRetornadoDeveTerUmIDValido()
        {
            Assert.NotNull(_cadastroResult);
            Assert.NotEqual(Guid.Empty, _cadastroResult.IdUsuario);
        }


        [Then(@"o usuário deve ser retornado com sucesso")]
        public void EntaoOUsuarioDeveSerRetornadoComSucesso()
        {
            Assert.NotNull(_usuarioResult);
            Assert.Null(_exception);
        }

        [Then(@"o usuário deve ter o ID ""(.*)""")]
        public void EntaoOUsuarioDeveTerOID(string usuarioId)
        {
            Assert.NotNull(_usuarioResult);
            Assert.Equal(Guid.Parse(usuarioId), _usuarioResult.Id);
        }

        [Then(@"o usuário deve ter perfil preenchido")]
        public void EntaoOUsuarioDeveTerPerfilPreenchido()
        {
            Assert.NotNull(_usuarioResult);
            Assert.NotNull(_usuarioResult.Perfil);
            Assert.NotNull(_usuarioResult.Perfil.NomeCompleto);
        }

        [Then(@"o usuário deve ter roles associadas")]
        public void EntaoOUsuarioDeveTerRolesAssociadas()
        {
            Assert.NotNull(_usuarioResult);
            Assert.NotNull(_usuarioResult.Roles);
            Assert.True(_usuarioResult.Roles.Count > 0);
        }


        [Then(@"a alteração de senha deve ser bem-sucedida")]
        public void EntaoAAlteracaoDeSenhaDeveSerBemSucedida()
        {
            Assert.NotNull(_alterarSenhaResult);
            Assert.True(_alterarSenhaResult.Value);
            Assert.Null(_exception);
        }

        [Then(@"a alteração de senha deve falhar")]
        public void EntaoAAlteracaoDeSenhaDeveFalhar()
        {
            Assert.NotNull(_alterarSenhaResult);
            Assert.False(_alterarSenhaResult.Value);
        }


        [Then(@"o status deve ser alterado com sucesso")]
        public void EntaoOStatusDeveSerAlteradoComSucesso()
        {
            Assert.NotNull(_alterarStatusResult);
            Assert.Null(_exception);
        }

        [Then(@"o status retornado deve ser ""(.*)""")]
        public void EntaoOStatusRetornadoDeveSer(string statusEsperado)
        {
            Assert.NotNull(_alterarStatusResult);
            Assert.Equal(statusEsperado, _alterarStatusResult.StatusAtual);
        }


        [AfterScenario(Order = 1)]
        public void LimparCenario()
        {
            _cadastroResult = null;
            _usuarioResult = null;
            _alterarSenhaResult = null;
            _alterarStatusResult = null;
            _exception = null;
            _nome = string.Empty;
            _senha = string.Empty;
            _celular = string.Empty;
            _email = string.Empty;
            _nomeCompleto = string.Empty;
            _dataNascimento = null;
            _pais = string.Empty;
            _avatarUrl = string.Empty;
            _cadastroDeveRetornarNull = false;
            _alterarSenhaDeveFalhar = false;
        }
    }
}

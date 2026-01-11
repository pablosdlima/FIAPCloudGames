using FIAPCloudGames.Application.AppServices;
using FIAPCloudGames.Application.Interfaces;
using FIAPCloudGames.Domain.Dtos.Request.UsuarioRole;
using FIAPCloudGames.Domain.Dtos.Responses.UsuarioRole;
using FIAPCloudGames.Domain.Enums;
using FIAPCloudGames.Domain.Interfaces.Services;
using FIAPCloudGames.Domain.Models;
using Microsoft.Extensions.Logging;
using MockQueryable.Moq;
using Moq;
using TechTalk.SpecFlow;

namespace FIAPCloudGames.BDD.Tests.Steps
{
    [Binding]
    public class UsuarioRoleAppServiceSteps
    {
        private readonly ScenarioContext _scenarioContext;
        private readonly Mock<IUsuarioRoleServices> _mockUsuarioRoleService;
        private readonly Mock<IUsuarioService> _mockUsuarioService;
        private readonly Mock<IRoleServices> _mockRoleService;
        private readonly Mock<ILogger<UsuarioRoleAppService>> _mockLogger;
        private readonly IUsuarioRoleAppService _usuarioRoleAppService;

        private Guid _usuarioId;
        private Guid _usuarioRoleId;
        private int _roleId;
        private TipoUsuario _tipoUsuario;
        private List<UsuarioRole> _usuarioRoles;
        private bool _atualizacaoDeveFalhar;
        private bool? _alterarResult;
        private IEnumerable<ListarRolesPorUsuarioResponse>? _listagemResult;
        private Exception? _exception;

        public UsuarioRoleAppServiceSteps(ScenarioContext scenarioContext)
        {
            _scenarioContext = scenarioContext;
            _mockUsuarioRoleService = new Mock<IUsuarioRoleServices>();
            _mockUsuarioService = new Mock<IUsuarioService>();
            _mockRoleService = new Mock<IRoleServices>();
            _mockLogger = new Mock<ILogger<UsuarioRoleAppService>>();
            _scenarioContext["MockUsuarioService_UsuarioRole"] = _mockUsuarioService;
            _scenarioContext["MockRoleService_UsuarioRole"] = _mockRoleService;

            _usuarioRoleAppService = new UsuarioRoleAppService(
                _mockUsuarioRoleService.Object,
                _mockUsuarioService.Object,
                _mockRoleService.Object,
                _mockLogger.Object
            );

            _usuarioRoles = [];
        }

        [Given(@"que o serviço de usuario role está configurado")]
        public void DadoQueOServicoDeUsuarioRoleEstaConfigurado()
        {
        }

        [Given(@"existe uma role com ID (.*)")]
        public void DadoQueExisteUmaRoleComID(int roleId)
        {
            _roleId = roleId;

            var role = new Role
            {
                Id = roleId,
                RoleName = roleId == 1 ? "Usuario" : "Administrador",
                Description = roleId == 1 ? "Usuário comum" : "Administrador do sistema"
            };

            _mockRoleService.Setup(s => s.GetByIdInt(roleId)).Returns(role);
        }

        [Given(@"não existe uma role com ID (.*)")]
        public void DadoQueNaoExisteUmaRoleComID(int roleId)
        {
            _roleId = roleId;

            _mockRoleService.Setup(s => s.GetByIdInt(roleId)).Returns((Role?)null);
        }

        [Given(@"existe uma associação usuario-role com ID ""(.*)""")]
        public void DadoQueExisteUmaAssociacaoUsuarioRoleComID(string usuarioRoleId)
        {
            _usuarioRoleId = Guid.Parse(usuarioRoleId);
            _usuarioId = _scenarioContext.Get<Guid>("UsuarioId");

            var usuarioRole = new UsuarioRole(1)
            {
                Id = _usuarioRoleId,
                UsuarioId = _usuarioId,
                RoleId = 1
            };

            _mockUsuarioRoleService.Setup(s => s.GetById(_usuarioRoleId)).Returns(usuarioRole);

            _mockUsuarioRoleService.Setup(s => s.Update(It.Is<UsuarioRole>(ur => ur.Id == _usuarioRoleId)))
                .ReturnsAsync((UsuarioRole ur) => (ur, true));
        }

        [Given(@"não existe uma associação usuario-role com ID ""(.*)""")]
        public void DadoQueNaoExisteUmaAssociacaoUsuarioRoleComID(string usuarioRoleId)
        {
            _usuarioRoleId = Guid.Parse(usuarioRoleId);

            _mockUsuarioRoleService.Setup(s => s.GetById(_usuarioRoleId)).Returns((UsuarioRole?)null);
        }

        [Given(@"o usuário possui (.*) roles associadas")]
        [Given(@"o usuário possui (.*) role associada")]
        public void DadoQueOUsuarioPossuiRolesAssociadas(int quantidade)
        {
            _usuarioId = _scenarioContext.Get<Guid>("UsuarioId");
            _usuarioRoles = [];

            for (var i = 0; i < quantidade; i++)
            {
                var role = new Role
                {
                    Id = i + 1,
                    RoleName = $"Role {i + 1}",
                    Description = $"Descrição Role {i + 1}"
                };

                var usuarioRole = new UsuarioRole(role.Id)
                {
                    Id = Guid.NewGuid(),
                    UsuarioId = _usuarioId,
                    RoleId = role.Id,
                    Role = role,
                };

                _usuarioRoles.Add(usuarioRole);
            }

            var mockDbSet = _usuarioRoles.AsQueryable().BuildMockDbSet();

            _mockUsuarioRoleService.Setup(s => s.Get()).Returns(mockDbSet.Object);
        }

        [Given(@"o usuário não possui roles associadas")]
        public void DadoQueOUsuarioNaoPossuiRolesAssociadas()
        {
            _usuarioId = _scenarioContext.Get<Guid>("UsuarioId");
            _usuarioRoles = [];

            var mockDbSet = _usuarioRoles.AsQueryable().BuildMockDbSet();

            _mockUsuarioRoleService.Setup(s => s.Get()).Returns(mockDbSet.Object);
        }


        [Given(@"tenho os dados para alterar a role:")]
        public void DadoQueTenhoOsDadosParaAlterarARole(Table table)
        {
            var dados = table.Rows.ToDictionary(r => r["Campo"], r => r["Valor"]);
            _tipoUsuario = Enum.Parse<TipoUsuario>(dados["TipoUsuario"]);
        }


        [Given(@"o serviço de usuario role não consegue atualizar")]
        public void DadoQueOServicoDeUsuarioRoleNaoConsegueAtualizar()
        {
            _atualizacaoDeveFalhar = true;

            _mockUsuarioRoleService.Setup(s => s.Update(It.IsAny<UsuarioRole>()))
                .ReturnsAsync((UsuarioRole ur) => ((UsuarioRole?)null, false));
        }


        [When(@"eu alterar a role da associação ""(.*)"" do usuário ""(.*)""")]
        public async Task QuandoEuAlterarARoleDaAssociacaoDoUsuario(string usuarioRoleId, string usuarioId)
        {
            try
            {
                _usuarioRoleId = Guid.Parse(usuarioRoleId);
                _usuarioId = Guid.Parse(usuarioId);

                var request = new AlterarUsuarioRoleRequest
                {
                    IdUsuarioRole = _usuarioRoleId,
                    UsuarioId = _usuarioId,
                    TipoUsuario = _tipoUsuario
                };

                _alterarResult = await _usuarioRoleAppService.AlterarRoleUsuario(request);
                _exception = null;
            }
            catch (Exception ex)
            {
                _exception = ex;
                _alterarResult = null;
                _scenarioContext["Exception"] = ex;
            }
        }

        [When(@"eu listar as roles do usuário ""(.*)""")]
        public async Task QuandoEuListarAsRolesDoUsuario(string usuarioId)
        {
            try
            {
                _usuarioId = Guid.Parse(usuarioId);
                var request = new ListarRolePorUsuarioRequest(_usuarioId);

                _listagemResult = await _usuarioRoleAppService.ListarRolesPorUsuario(request);
                _exception = null;
            }
            catch (Exception ex)
            {
                _exception = ex;
                _listagemResult = null;
                _scenarioContext["Exception"] = ex;
            }
        }


        [Then(@"a alteração da role deve ser bem-sucedida")]
        public void EntaoAAlteracaoDaRoleDeveSerBemSucedida()
        {
            Assert.NotNull(_alterarResult);
            Assert.True(_alterarResult.Value);
            Assert.Null(_exception);
        }


        [Then(@"a listagem de roles do usuário deve retornar (.*) roles")]
        [Then(@"a listagem de roles do usuário deve retornar (.*) role")]
        public void EntaoAListagemDeRolesDoUsuarioDeveRetornarRoles(int quantidade)
        {
            Assert.NotNull(_listagemResult);
            Assert.Equal(quantidade, _listagemResult.Count());
        }

        [Then(@"todas as roles devem pertencer ao usuário")]
        public void EntaoTodasAsRolesDevemPertencerAoUsuario()
        {
            Assert.NotNull(_listagemResult);
            Assert.All(_listagemResult, r => Assert.Equal(_usuarioId, r.UsuarioId));
        }


        [AfterScenario(Order = 1)]
        public void LimparCenario()
        {
            _usuarioRoles.Clear();
            _alterarResult = null;
            _listagemResult = null;
            _exception = null;
            _atualizacaoDeveFalhar = false;
        }
    }
}

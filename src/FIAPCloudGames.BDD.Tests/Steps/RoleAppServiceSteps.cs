using FIAPCloudGames.Application.AppServices;
using FIAPCloudGames.Application.Interfaces;
using FIAPCloudGames.Domain.Dtos.Request.Role;
using FIAPCloudGames.Domain.Dtos.Responses.Role;
using FIAPCloudGames.Domain.Interfaces.Services;
using FIAPCloudGames.Domain.Models;
using Moq;
using TechTalk.SpecFlow;

namespace FIAPCloudGames.BDD.Tests.Steps
{
    [Binding]
    public class RoleAppServiceSteps
    {
        private readonly ScenarioContext _scenarioContext;
        private readonly Mock<IRoleServices> _mockRoleService;
        private readonly IRoleAppService _roleAppService;

        private int _roleId;
        private List<Role> _rolesCadastradas;
        private int _id;
        private string _roleName;
        private string? _description;
        private bool _cadastroDeveRetornarNull;
        private RolesResponse? _roleResult;
        private List<RolesResponse>? _listagemResult;
        private (RolesResponse? Role, bool Success)? _atualizacaoResult;
        private Exception? _exception;

        public RoleAppServiceSteps(ScenarioContext scenarioContext)
        {
            _scenarioContext = scenarioContext;
            _mockRoleService = new Mock<IRoleServices>();
            _roleAppService = new RoleAppService(_mockRoleService.Object);
            _rolesCadastradas = [];
            _roleName = string.Empty;
        }

        [Given(@"que o serviço de roles está configurado")]
        public void DadoQueOServicoDeRolesEstaConfigurado()
        {
        }


        [Given(@"que tenho os dados da role:")]
        [Given(@"tenho os dados da role:")]
        [Given(@"tenho os dados atualizados da role:")]
        public void DadoQueTenhoOsDadosDaRole(Table table)
        {
            var dados = table.Rows.ToDictionary(r => r["Campo"], r => r["Valor"]);

            if (dados.ContainsKey("Id"))
            {
                _id = int.Parse(dados["Id"]);
            }

            _roleName = dados["RoleName"];
            _description = dados.ContainsKey("Description") ? dados["Description"] : null;
        }


        [Given(@"que existe uma role com ID (.*)")]
        public void DadoQueExisteUmaRoleComID(int roleId)
        {
            _roleId = roleId;

            var role = new Role
            {
                Id = roleId,
                RoleName = "Administrador",
                Description = "Role administrativa"
            };

            _mockRoleService
                .Setup(s => s.AtualizarRole(It.Is<Role>(r => r.Id == roleId)))
                .ReturnsAsync((Role roleAtualizada) => (roleAtualizada, true));
        }

        [Given(@"que não existe uma role com ID (.*)")]
        public void DadoQueNaoExisteUmaRoleComID(int roleId)
        {
            _roleId = roleId;

            _mockRoleService
                .Setup(s => s.AtualizarRole(It.Is<Role>(r => r.Id == roleId)))
                .ReturnsAsync(((Role?)null, false));
        }

        [Given(@"que existem (.*) roles cadastradas")]
        public void DadoQueExistemRolesCadastradas(int quantidade)
        {
            _rolesCadastradas = [];

            for (var i = 1; i <= quantidade; i++)
            {
                var role = new Role
                {
                    Id = i,
                    RoleName = $"Role {i}",
                    Description = $"Descrição da Role {i}"
                };
                _rolesCadastradas.Add(role);
            }

            _mockRoleService.Setup(s => s.ListarRoles()).Returns(_rolesCadastradas);
        }

        [Given(@"que não existem roles cadastradas")]
        public void DadoQueNaoExistemRolesCadastradas()
        {
            _rolesCadastradas = [];

            _mockRoleService.Setup(s => s.ListarRoles()).Returns(_rolesCadastradas);
        }

        [Given(@"que existem roles cadastradas com diferentes nomes")]
        public void DadoQueExistemRolesCadastradasComDiferentesNomes()
        {
            _rolesCadastradas =
            [
                new Role { Id = 1, RoleName = "Administrador", Description = "Acesso total" },
                new Role { Id = 2, RoleName = "Gerente", Description = "Acesso gerencial" },
                new Role { Id = 3, RoleName = "Usuário", Description = "Acesso básico" }
            ];

            _mockRoleService.Setup(s => s.ListarRoles()).Returns(_rolesCadastradas);
        }


        [Given(@"que o serviço de role não consegue cadastrar")]
        public void DadoQueOServicoDeRoleNaoConsegueCadastrar()
        {
            _cadastroDeveRetornarNull = true;
            _mockRoleService.Setup(s => s.Insert(It.IsAny<Role>())).ReturnsAsync((Role?)null);
        }


        [When(@"eu cadastrar a role")]
        public async Task QuandoEuCadastrarARole()
        {
            try
            {
                var request = new CadastrarRoleRequest
                {
                    Id = _id,
                    RoleName = _roleName,
                    Description = _description ?? string.Empty
                };

                if (!_cadastroDeveRetornarNull)
                {
                    var roleCadastrada = new Role
                    {
                        Id = _id,
                        RoleName = _roleName,
                        Description = _description
                    };

                    _mockRoleService.Setup(s => s.Insert(It.IsAny<Role>())).ReturnsAsync(roleCadastrada);
                }

                _roleResult = await _roleAppService.Cadastrar(request);
                _exception = null;
            }
            catch (Exception ex)
            {
                _exception = ex;
                _roleResult = null;
                _scenarioContext["Exception"] = ex;
            }
        }

        [When(@"eu listar as roles")]
        public async Task QuandoEuListarAsRoles()
        {
            try
            {
                _listagemResult = await _roleAppService.ListarRoles();
                _exception = null;
            }
            catch (Exception ex)
            {
                _exception = ex;
                _listagemResult = null;
                _scenarioContext["Exception"] = ex;
            }
        }

        [When(@"eu atualizar a role")]
        public async Task QuandoEuAtualizarARole()
        {
            try
            {
                var request = new AtualizarRoleRequest
                {
                    Id = _roleId,
                    RoleName = _roleName,
                    Description = _description
                };

                _atualizacaoResult = await _roleAppService.AtualizarRole(request);
                _exception = null;
            }
            catch (Exception ex)
            {
                _exception = ex;
                _atualizacaoResult = null;
                _scenarioContext["Exception"] = ex;
            }
        }


        [Then(@"a role deve ser cadastrada com sucesso")]
        public void EntaoARoleDeveSerCadastradaComSucesso()
        {
            Assert.NotNull(_roleResult);
            Assert.Null(_exception);
        }

        [Then(@"a role retornada deve ter um ID válido")]
        public void EntaoARoleRetornadaDeveTerUmIDValido()
        {
            Assert.NotNull(_roleResult);
            Assert.True(_roleResult.Id > 0);
        }

        [Then(@"a role deve conter os dados informados")]
        public void EntaoARoleDeveConterOsDadosInformados()
        {
            Assert.NotNull(_roleResult);

            if (_id > 0)
            {
                Assert.Equal(_id, _roleResult.Id);
            }

            Assert.Equal(_roleName, _roleResult.RoleName);
            Assert.Equal(_description, _roleResult.Description);
        }


        [Then(@"a listagem deve retornar (.*) roles")]
        public void EntaoAListagemDeveRetornarRoles(int quantidade)
        {
            Assert.NotNull(_listagemResult);
            Assert.Equal(quantidade, _listagemResult.Count);
        }

        [Then(@"a listagem deve conter roles com nomes distintos")]
        public void EntaoAListagemDeveConterRolesComNomesDistintos()
        {
            Assert.NotNull(_listagemResult);
            Assert.True(_listagemResult.Count > 0);

            var nomesDistintos = _listagemResult.Select(r => r.RoleName).Distinct().Count();
            Assert.True(nomesDistintos > 1, "Deve haver roles com nomes distintos");
        }


        [Then(@"a role deve ser atualizada com sucesso")]
        public void EntaoARoleDeveSerAtualizadaComSucesso()
        {
            Assert.NotNull(_atualizacaoResult);
            Assert.True(_atualizacaoResult.Value.Success);
            Assert.NotNull(_atualizacaoResult.Value.Role);
        }

        [Then(@"a role retornada deve ter os novos dados")]
        public void EntaoARoleRetornadaDeveTerOsNovosDados()
        {
            Assert.NotNull(_atualizacaoResult);
            Assert.NotNull(_atualizacaoResult.Value.Role);
            Assert.Equal(_roleName, _atualizacaoResult.Value.Role.RoleName);
            Assert.Equal(_description, _atualizacaoResult.Value.Role.Description);
        }

        [Then(@"a atualização da role deve falhar")]
        public void EntaoAAtualizacaoDaRoleDeveFalhar()
        {
            Assert.NotNull(_atualizacaoResult);
            Assert.False(_atualizacaoResult.Value.Success);
        }

        [Then(@"o resultado da role deve indicar falha")]
        public void EntaoOResultadoDaRoleDeveIndicarFalha()
        {
            Assert.NotNull(_atualizacaoResult);
            Assert.Null(_atualizacaoResult.Value.Role);
        }


        [AfterScenario(Order = 1)]
        public void LimparCenario()
        {
            _rolesCadastradas.Clear();
            _roleResult = null;
            _listagemResult = null;
            _atualizacaoResult = null;
            _exception = null;
            _id = 0;
            _roleName = string.Empty;
            _description = null;
            _cadastroDeveRetornarNull = false;
        }
    }
}

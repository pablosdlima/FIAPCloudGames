using FIAPCloudGames.Application.AppServices;
using FIAPCloudGames.Application.Interfaces;
using FIAPCloudGames.Domain.Dtos.Responses.Authentication;
using FIAPCloudGames.Domain.Exceptions;
using FIAPCloudGames.Domain.Interfaces.Services;
using FIAPCloudGames.Domain.Models;
using Moq;
using TechTalk.SpecFlow;

namespace FIAPCloudGames.BDD.Tests.Steps
{
    [Binding]
    public class AuthenticationAppServiceSteps
    {
        private readonly ScenarioContext _scenarioContext;
        private readonly Mock<IJwtGenerator> _mockJwtGenerator;
        private readonly Mock<IUsuarioService> _mockUsuarioService;
        private readonly IAuthenticationAppService _authenticationAppService;

        private Usuario? _usuarioExistente;
        private LoginResponse? _loginResponse;
        private Exception? _exception;
        private string _tokenGerado;

        public AuthenticationAppServiceSteps(ScenarioContext scenarioContext)
        {
            _scenarioContext = scenarioContext;
            _mockJwtGenerator = new Mock<IJwtGenerator>();
            _mockUsuarioService = new Mock<IUsuarioService>();

            // Registra o mock no contexto com chave única
            _scenarioContext["MockUsuarioService_Authentication"] = _mockUsuarioService;

            _authenticationAppService = new AuthenticationAppService(
                _mockJwtGenerator.Object,
                _mockUsuarioService.Object
            );

            _tokenGerado = string.Empty;
        }

        [Given(@"que o serviço de autenticação está configurado")]
        public void DadoQueOServicoDeAutenticacaoEstaConfigurado()
        {
            // O serviço já está configurado no construtor
        }

        [Given(@"que existe um usuário com login ""(.*)"" e senha ""(.*)""")]
        public void DadoQueExisteUmUsuarioComLoginESenha(string login, string senha)
        {
            // Cria o usuário
            _usuarioExistente = Usuario.Criar(login, senha);

            // Configura o mock para retornar o usuário quando as credenciais corretas forem fornecidas
            _mockUsuarioService
                .Setup(s => s.ValidarLogin(login, senha))
                .ReturnsAsync(_usuarioExistente);

            // Configura o mock do JWT para retornar um token válido
            _tokenGerado = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1c2VySWQiOiJ0ZXN0IiwibmFtZSI6InRlc3QifQ.test";

            _mockJwtGenerator
                .Setup(j => j.GenerateToken(_usuarioExistente))
                .Returns(_tokenGerado);
        }

        [Given(@"que não existe um usuário com login ""(.*)""")]
        public void DadoQueNaoExisteUmUsuarioComLogin(string login)
        {
            // Configura o mock para retornar null para qualquer senha com este login
            _mockUsuarioService
                .Setup(s => s.ValidarLogin(login, It.IsAny<string>()))
                .ReturnsAsync((Usuario?)null);
        }

        [When(@"eu realizar o login com usuário ""(.*)"" e senha ""(.*)""")]
        public async Task QuandoEuRealizarOLoginComUsuarioESenha(string usuario, string senha)
        {
            try
            {
                _loginResponse = await _authenticationAppService.Login(usuario, senha);
                _exception = null;
            }
            catch (Exception ex)
            {
                _exception = ex;
                _loginResponse = null;
            }
        }

        [Then(@"o login deve ser realizado com sucesso")]
        public void EntaoOLoginDeveSerRealizadoComSucesso()
        {
            Assert.NotNull(_loginResponse);
            Assert.Null(_exception);
        }

        [Then(@"um token JWT válido deve ser retornado")]
        public void EntaoUmTokenJWTValidoDeveSerRetornado()
        {
            Assert.NotNull(_loginResponse);
            Assert.NotNull(_loginResponse.Token);
            Assert.NotEmpty(_loginResponse.Token);

            // Verifica se o token tem o formato JWT básico (três partes separadas por pontos)
            var tokenParts = _loginResponse.Token.Split('.');
            Assert.Equal(3, tokenParts.Length);
        }

        [Then(@"o login deve falhar com uma exceção de autenticação")]
        public void EntaoOLoginDeveFalharComUmaExcecaoDeAutenticacao()
        {
            Assert.NotNull(_exception);
            Assert.IsType<AutenticacaoException>(_exception);
            Assert.Null(_loginResponse);
        }

        [Then(@"a mensagem de erro deve ser ""(.*)""")]
        public void EntaoAMensagemDeErroDeveSer(string mensagemEsperada)
        {
            Assert.NotNull(_exception);
            Assert.Equal(mensagemEsperada, _exception.Message);
        }

        [Then(@"o token JWT deve conter as informações do usuário autenticado")]
        public void EntaoOTokenJWTDeveConterAsInformacoesDoUsuarioAutenticado()
        {
            Assert.NotNull(_loginResponse);
            Assert.NotNull(_loginResponse.Token);

            // Verifica se o GenerateToken foi chamado com o usuário correto
            _mockJwtGenerator.Verify(
                j => j.GenerateToken(It.Is<Usuario>(u => u == _usuarioExistente)),
                Times.Once,
                "O token deve ser gerado com as informações do usuário autenticado"
            );
        }

        [AfterScenario]
        public void LimparCenario()
        {
            _usuarioExistente = null;
            _loginResponse = null;
            _exception = null;
            _tokenGerado = string.Empty;
        }
    }
}

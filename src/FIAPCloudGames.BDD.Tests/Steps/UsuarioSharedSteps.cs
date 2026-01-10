using FIAPCloudGames.Domain.Interfaces.Services;
using FIAPCloudGames.Domain.Models;
using Moq;
using TechTalk.SpecFlow;

namespace FIAPCloudGames.BDD.Tests.Steps
{
    [Binding]
    public class UsuarioSharedSteps
    {
        private readonly ScenarioContext _scenarioContext;

        public UsuarioSharedSteps(ScenarioContext scenarioContext)
        {
            _scenarioContext = scenarioContext;
        }

        [Given(@"que existe um usuário com ID ""(.*)""")]
        public void DadoQueExisteUmUsuarioComID(string usuarioId)
        {
            var guid = Guid.Parse(usuarioId);
            var usuario = Usuario.Criar("Marcio", "Senha@123");
            usuario.Id = guid; // Força o ID

            // Armazena no contexto
            _scenarioContext["UsuarioId"] = guid;
            _scenarioContext["Usuario"] = usuario;

            // Configura TODOS os mocks de IUsuarioService que existem no contexto
            ConfigurarMockUsuarioService(guid, usuario);
        }

        [Given(@"que não existe um usuário com ID ""(.*)""")]
        public void DadoQueNaoExisteUmUsuarioComID(string usuarioId)
        {
            var guid = Guid.Parse(usuarioId);

            // Armazena no contexto
            _scenarioContext["UsuarioId"] = guid;
            _scenarioContext["Usuario"] = null;

            // Configura TODOS os mocks de IUsuarioService que existem no contexto
            ConfigurarMockUsuarioService(guid, null);
        }

        private void ConfigurarMockUsuarioService(Guid usuarioId, Usuario? usuario)
        {
            // Procura por todos os mocks de IUsuarioService no contexto e configura
            var keys = _scenarioContext.Keys.Where(k => k.StartsWith("MockUsuarioService_")).ToList();

            foreach (var key in keys)
            {
                if (_scenarioContext.TryGetValue<Mock<IUsuarioService>>(key, out var mock))
                {
                    mock.Setup(s => s.GetById(usuarioId)).Returns(usuario);
                }
            }
        }
    }
}

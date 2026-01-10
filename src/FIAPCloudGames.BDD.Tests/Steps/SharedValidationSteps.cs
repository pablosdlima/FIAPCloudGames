using FIAPCloudGames.Domain.Exceptions;
using TechTalk.SpecFlow;

namespace FIAPCloudGames.BDD.Tests.Steps
{
    [Binding]
    public class SharedValidationSteps
    {
        private readonly ScenarioContext _scenarioContext;

        public SharedValidationSteps(ScenarioContext scenarioContext)
        {
            _scenarioContext = scenarioContext;
        }

        [Then(@"deve lançar uma exceção de não encontrado")]
        public void EntaoDeveLancarUmaExcecaoDeNaoEncontrado()
        {
            var exception = _scenarioContext.Get<Exception>("Exception");
            Assert.NotNull(exception);
            Assert.IsType<NotFoundException>(exception);
        }

        [Then(@"deve lançar uma exceção de domínio")]
        public void EntaoDeveLancarUmaExcecaoDeDominio()
        {
            var exception = _scenarioContext.Get<Exception>("Exception");
            Assert.NotNull(exception);
            Assert.IsType<DomainException>(exception);
        }

        [Then(@"a mensagem deve conter ""(.*)""")]
        public void EntaoAMensagemDeveConter(string mensagemEsperada)
        {
            var exception = _scenarioContext.Get<Exception>("Exception");
            Assert.NotNull(exception);
            Assert.Contains(mensagemEsperada, exception.Message);
        }

        [Then(@"a mensagem deve ser ""(.*)""")]
        public void EntaoAMensagemDeveSer(string mensagemEsperada)
        {
            var exception = _scenarioContext.Get<Exception>("Exception");
            Assert.NotNull(exception);
            Assert.Equal(mensagemEsperada, exception.Message);
        }
    }
}

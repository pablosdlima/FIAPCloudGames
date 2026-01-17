using FIAPCloudGames.Domain.Dtos.Request.Game;
using FIAPCloudGames.Domain.Dtos.Validators.Game;
using FluentAssertions;

namespace FIAPCloudGames.Domain.Tests.Validators
{
    public class ListarGamesPaginadoRequestValidatorTests
    {
        private readonly ListarGamesPaginadoRequestValidator _validator;

        public ListarGamesPaginadoRequestValidatorTests()
        {
            _validator = new ListarGamesPaginadoRequestValidator();
        }

        [Fact]
        public void Deve_ser_valido_quando_request_for_valido()
        {
            var request = new ListarGamesPaginadoRequest
            {
                NumeroPagina = 1,
                TamanhoPagina = 10,
                Filtro = "filtro",
                Genero = "Ação"
            };

            var result = _validator.Validate(request);

            result.IsValid.Should().BeTrue();
            result.Errors.Should().BeEmpty();
        }

        [Fact]
        public void Deve_ter_erro_quando_NumeroPagina_for_menor_que_1()
        {
            var request = new ListarGamesPaginadoRequest
            {
                NumeroPagina = 0,
                TamanhoPagina = 10
            };

            var result = _validator.Validate(request);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(e => e.PropertyName == nameof(request.NumeroPagina) && e.ErrorMessage == "Número da página deve ser maior que zero.");
        }

        [Fact]
        public void Deve_ter_erro_quando_TamanhoPagina_for_menor_que_1()
        {
            var request = new ListarGamesPaginadoRequest
            {
                NumeroPagina = 1,
                TamanhoPagina = 0
            };

            var result = _validator.Validate(request);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(e => e.PropertyName == nameof(request.TamanhoPagina) && e.ErrorMessage == "Tamanho da página deve ser no mínimo 1.");
        }

        [Fact]
        public void Deve_ter_erro_quando_TamanhoPagina_for_maior_que_100()
        {
            var request = new ListarGamesPaginadoRequest
            {
                NumeroPagina = 1,
                TamanhoPagina = 101
            };

            var result = _validator.Validate(request);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(e => e.PropertyName == nameof(request.TamanhoPagina) && e.ErrorMessage == "Tamanho da página deve ser no máximo 100.");
        }

        [Fact]
        public void Deve_ter_erro_quando_Genero_tiver_mais_de_50_caracteres()
        {
            var longGenero = new string('g', 51);
            var request = new ListarGamesPaginadoRequest
            {
                NumeroPagina = 1,
                TamanhoPagina = 10,
                Genero = longGenero
            };

            var result = _validator.Validate(request);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(e => e.PropertyName == nameof(request.Genero) && e.ErrorMessage == "Gênero deve ter no máximo 50 caracteres.");
        }

        [Fact]
        public void Deve_ser_valido_quando_Genero_for_nulo_ou_espaco()
        {
            var requestNull = new ListarGamesPaginadoRequest
            {
                NumeroPagina = 1,
                TamanhoPagina = 10,
                Genero = null
            };

            var resultNull = _validator.Validate(requestNull);
            resultNull.IsValid.Should().BeTrue();

            var requestSpace = new ListarGamesPaginadoRequest
            {
                NumeroPagina = 1,
                TamanhoPagina = 10,
                Genero = "   "
            };

            var resultSpace = _validator.Validate(requestSpace);
            resultSpace.IsValid.Should().BeTrue();
        }

        [Fact]
        public void Deve_ter_erro_quando_Filtro_tiver_mais_de_100_caracteres()
        {
            var longFiltro = new string('f', 101);
            var request = new ListarGamesPaginadoRequest
            {
                NumeroPagina = 1,
                TamanhoPagina = 10,
                Filtro = longFiltro
            };

            var result = _validator.Validate(request);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(e => e.PropertyName == nameof(request.Filtro) && e.ErrorMessage == "Filtro deve ter no máximo 100 caracteres.");
        }

        [Fact]
        public void Deve_ser_valido_quando_Filtro_for_nulo_ou_espaco()
        {
            var requestNull = new ListarGamesPaginadoRequest
            {
                NumeroPagina = 1,
                TamanhoPagina = 10,
                Filtro = null
            };

            var resultNull = _validator.Validate(requestNull);
            resultNull.IsValid.Should().BeTrue();

            var requestSpace = new ListarGamesPaginadoRequest
            {
                NumeroPagina = 1,
                TamanhoPagina = 10,
                Filtro = "   "
            };

            var resultSpace = _validator.Validate(requestSpace);
            resultSpace.IsValid.Should().BeTrue();
        }
    }
}

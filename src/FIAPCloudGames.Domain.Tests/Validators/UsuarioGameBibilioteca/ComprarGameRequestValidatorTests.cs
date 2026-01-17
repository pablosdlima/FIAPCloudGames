using FIAPCloudGames.Domain.Dtos.Request.UsuarioGameBiblioteca;
using FIAPCloudGames.Domain.Dtos.Validators.UsuarioGameBibilioteca;
using FluentAssertions;

namespace FIAPCloudGames.Domain.Tests.Validators
{
    public class ComprarGameRequestValidatorTests
    {
        private readonly ComprarGameRequestValidator _validator;

        public ComprarGameRequestValidatorTests()
        {
            _validator = new ComprarGameRequestValidator();
        }

        [Fact]
        public void Deve_ter_erro_quando_UsuarioId_estiver_vazio()
        {
            var request = new ComprarGameRequest
            {
                UsuarioId = Guid.Empty,
                GameId = Guid.NewGuid(),
                TipoAquisicao = "Compra",
                PrecoAquisicao = 0m,
                DataAquisicao = null
            };

            var result = _validator.Validate(request);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(e => e.PropertyName == nameof(request.UsuarioId) && e.ErrorMessage == "UsuarioId é obrigatório.");
        }

        [Fact]
        public void Deve_ter_erro_quando_GameId_estiver_vazio()
        {
            var request = new ComprarGameRequest
            {
                UsuarioId = Guid.NewGuid(),
                GameId = Guid.Empty,
                TipoAquisicao = "Compra",
                PrecoAquisicao = 0m,
                DataAquisicao = null
            };

            var result = _validator.Validate(request);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(e => e.PropertyName == nameof(request.GameId) && e.ErrorMessage == "GameId é obrigatório.");
        }

        [Fact]
        public void Deve_ter_erro_quando_TipoAquisicao_estiver_vazio()
        {
            var request = new ComprarGameRequest
            {
                UsuarioId = Guid.NewGuid(),
                GameId = Guid.NewGuid(),
                TipoAquisicao = string.Empty,
                PrecoAquisicao = 0m,
                DataAquisicao = null
            };

            var result = _validator.Validate(request);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(e => e.PropertyName == nameof(request.TipoAquisicao) && e.ErrorMessage == "Tipo de aquisição é obrigatório.");
        }

        [Fact]
        public void Deve_ter_erro_quando_TipoAquisicao_for_invalido()
        {
            var request = new ComprarGameRequest
            {
                UsuarioId = Guid.NewGuid(),
                GameId = Guid.NewGuid(),
                TipoAquisicao = "Troca",
                PrecoAquisicao = 0m,
                DataAquisicao = null
            };

            var result = _validator.Validate(request);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(e => e.PropertyName == nameof(request.TipoAquisicao) && e.ErrorMessage == "Tipo de aquisição inválido. Valores aceitos: Compra, Presente, Assinatura, Gratuito.");
        }

        [Fact]
        public void Deve_aceitar_TipoAquisicao_valido_com_diferente_case()
        {
            var request = new ComprarGameRequest
            {
                UsuarioId = Guid.NewGuid(),
                GameId = Guid.NewGuid(),
                TipoAquisicao = "pReSeNtE",
                PrecoAquisicao = 0m,
                DataAquisicao = null
            };

            var result = _validator.Validate(request);

            result.IsValid.Should().BeTrue();
            result.Errors.Should().BeEmpty();
        }

        [Fact]
        public void Deve_ter_erro_quando_PrecoAquisicao_for_menor_que_zero()
        {
            var request = new ComprarGameRequest
            {
                UsuarioId = Guid.NewGuid(),
                GameId = Guid.NewGuid(),
                TipoAquisicao = "Compra",
                PrecoAquisicao = -1m,
                DataAquisicao = null
            };

            var result = _validator.Validate(request);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(e => e.PropertyName == nameof(request.PrecoAquisicao) && e.ErrorMessage == "Preço de aquisição deve ser maior ou igual a zero.");
        }

        [Fact]
        public void Deve_ter_erro_quando_DataAquisicao_for_futura()
        {
            var request = new ComprarGameRequest
            {
                UsuarioId = Guid.NewGuid(),
                GameId = Guid.NewGuid(),
                TipoAquisicao = "Compra",
                PrecoAquisicao = 10m,
                DataAquisicao = DateTimeOffset.UtcNow.AddDays(1)
            };

            var result = _validator.Validate(request);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(e => e.PropertyName == nameof(request.DataAquisicao) && e.ErrorMessage == "Data de aquisição não pode ser futura.");
        }

        [Fact]
        public void Deve_ser_valido_quando_todos_campos_estiverem_corretos()
        {
            var request = new ComprarGameRequest
            {
                UsuarioId = Guid.NewGuid(),
                GameId = Guid.NewGuid(),
                TipoAquisicao = "Compra",
                PrecoAquisicao = 59.99m,
                DataAquisicao = DateTimeOffset.UtcNow.AddDays(-1)
            };

            var result = _validator.Validate(request);

            result.IsValid.Should().BeTrue();
            result.Errors.Should().BeEmpty();
        }
    }
}

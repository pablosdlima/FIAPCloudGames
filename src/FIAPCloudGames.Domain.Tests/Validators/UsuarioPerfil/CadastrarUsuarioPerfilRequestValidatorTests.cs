using FIAPCloudGames.Domain.Dtos.Request.UsuarioPerfil;
using FIAPCloudGames.Domain.Dtos.Validators.UsuarioPerfil;
using FluentAssertions;

namespace FIAPCloudGames.Domain.Tests.Validators
{
    public class CadastrarUsuarioPerfilRequestValidatorTests
    {
        private readonly CadastrarUsuarioPerfilRequestValidator _validator;

        public CadastrarUsuarioPerfilRequestValidatorTests()
        {
            _validator = new CadastrarUsuarioPerfilRequestValidator();
        }

        [Fact]
        public void Deve_ter_erro_quando_UsuarioId_estiver_vazio()
        {
            var request = new CadastrarUsuarioPerfilRequest
            {
                UsuarioId = Guid.Empty,
                NomeCompleto = "Fulano de Tal",
                DataNascimento = null,
                Pais = "Brasil",
                AvatarUrl = "https://exemplo.com/avatar.png"
            };

            var result = _validator.Validate(request);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(e => e.PropertyName == nameof(request.UsuarioId) && e.ErrorMessage == "UsuarioId é obrigatório.");
        }

        [Fact]
        public void Deve_ter_erro_quando_NomeCompleto_estiver_vazio()
        {
            var request = new CadastrarUsuarioPerfilRequest
            {
                UsuarioId = Guid.NewGuid(),
                NomeCompleto = string.Empty,
                DataNascimento = null,
                Pais = "Brasil",
                AvatarUrl = "https://exemplo.com/avatar.png"
            };

            var result = _validator.Validate(request);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(e => e.PropertyName == nameof(request.NomeCompleto) && e.ErrorMessage == "Nome completo é obrigatório.");
        }

        [Fact]
        public void Deve_ter_erro_quando_NomeCompleto_conter_caracteres_invalidos()
        {
            var request = new CadastrarUsuarioPerfilRequest
            {
                UsuarioId = Guid.NewGuid(),
                NomeCompleto = "Fulano123!",
                DataNascimento = null,
                Pais = "Brasil",
                AvatarUrl = "https://exemplo.com/avatar.png"
            };

            var result = _validator.Validate(request);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(e => e.PropertyName == nameof(request.NomeCompleto) && e.ErrorMessage == "Nome completo deve conter apenas letras e espaços.");
        }

        [Fact]
        public void Deve_ter_erro_quando_NomeCompleto_tiver_mais_de_200_caracteres()
        {
            var longName = new string('a', 201);
            var request = new CadastrarUsuarioPerfilRequest
            {
                UsuarioId = Guid.NewGuid(),
                NomeCompleto = longName,
                DataNascimento = null,
                Pais = "Brasil",
                AvatarUrl = "https://exemplo.com/avatar.png"
            };

            var result = _validator.Validate(request);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(e => e.PropertyName == nameof(request.NomeCompleto) && e.ErrorMessage == "Nome completo deve ter no máximo 200 caracteres.");
        }

        [Fact]
        public void Deve_ter_erro_quando_DataNascimento_for_futura()
        {
            var request = new CadastrarUsuarioPerfilRequest
            {
                UsuarioId = Guid.NewGuid(),
                NomeCompleto = "Fulano de Tal",
                DataNascimento = DateTimeOffset.UtcNow.AddDays(1),
                Pais = "Brasil",
                AvatarUrl = "https://exemplo.com/avatar.png"
            };

            var result = _validator.Validate(request);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(e => e.PropertyName == nameof(request.DataNascimento) && e.ErrorMessage == "Data de nascimento deve ser anterior à data atual.");
        }

        [Fact]
        public void Deve_ter_erro_quando_DataNascimento_for_mais_de_120_anos_atras()
        {
            var request = new CadastrarUsuarioPerfilRequest
            {
                UsuarioId = Guid.NewGuid(),
                NomeCompleto = "Fulano de Tal",
                DataNascimento = DateTimeOffset.UtcNow.AddYears(-121),
                Pais = "Brasil",
                AvatarUrl = "https://exemplo.com/avatar.png"
            };

            var result = _validator.Validate(request);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(e => e.PropertyName == nameof(request.DataNascimento) && e.ErrorMessage == "Data de nascimento inválida.");
        }

        [Fact]
        public void Deve_ter_erro_quando_Pais_estiver_vazio()
        {
            var request = new CadastrarUsuarioPerfilRequest
            {
                UsuarioId = Guid.NewGuid(),
                NomeCompleto = "Fulano de Tal",
                DataNascimento = null,
                Pais = string.Empty,
                AvatarUrl = "https://exemplo.com/avatar.png"
            };

            var result = _validator.Validate(request);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(e => e.PropertyName == nameof(request.Pais) && e.ErrorMessage == "País é obrigatório.");
        }

        [Fact]
        public void Deve_ter_erro_quando_Pais_tiver_mais_de_100_caracteres()
        {
            var longCountry = new string('c', 101);
            var request = new CadastrarUsuarioPerfilRequest
            {
                UsuarioId = Guid.NewGuid(),
                NomeCompleto = "Fulano de Tal",
                DataNascimento = null,
                Pais = longCountry,
                AvatarUrl = "https://exemplo.com/avatar.png"
            };

            var result = _validator.Validate(request);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(e => e.PropertyName == nameof(request.Pais) && e.ErrorMessage == "País deve ter no máximo 100 caracteres.");
        }

        [Fact]
        public void Deve_ter_erro_quando_AvatarUrl_estiver_vazio()
        {
            var request = new CadastrarUsuarioPerfilRequest
            {
                UsuarioId = Guid.NewGuid(),
                NomeCompleto = "Fulano de Tal",
                DataNascimento = null,
                Pais = "Brasil",
                AvatarUrl = string.Empty
            };

            var result = _validator.Validate(request);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(e => e.PropertyName == nameof(request.AvatarUrl) && e.ErrorMessage == "URL do avatar é obrigatória.");
        }

        [Fact]
        public void Deve_ter_erro_quando_AvatarUrl_tiver_mais_de_500_caracteres()
        {
            var longUrl = "https://exemplo.com/" + new string('u', 490);
            var request = new CadastrarUsuarioPerfilRequest
            {
                UsuarioId = Guid.NewGuid(),
                NomeCompleto = "Fulano de Tal",
                DataNascimento = null,
                Pais = "Brasil",
                AvatarUrl = longUrl
            };

            var result = _validator.Validate(request);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(e => e.PropertyName == nameof(request.AvatarUrl) && e.ErrorMessage == "URL do avatar deve ter no máximo 500 caracteres.");
        }

        [Fact]
        public void Deve_ter_erro_quando_AvatarUrl_for_invalida()
        {
            var request = new CadastrarUsuarioPerfilRequest
            {
                UsuarioId = Guid.NewGuid(),
                NomeCompleto = "Fulano de Tal",
                DataNascimento = null,
                Pais = "Brasil",
                AvatarUrl = "ftp://invalido.local/avatar.png"
            };

            var result = _validator.Validate(request);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(e => e.PropertyName == nameof(request.AvatarUrl) && e.ErrorMessage == "URL do avatar inválida.");
        }

        [Fact]
        public void Deve_ser_valido_quando_todos_campos_estiverem_corretos()
        {
            var request = new CadastrarUsuarioPerfilRequest
            {
                UsuarioId = Guid.NewGuid(),
                NomeCompleto = "Fulano de Tal",
                DataNascimento = DateTimeOffset.UtcNow.AddYears(-30),
                Pais = "Brasil",
                AvatarUrl = "https://exemplo.com/avatar.png"
            };

            var result = _validator.Validate(request);

            result.IsValid.Should().BeTrue();
            result.Errors.Should().BeEmpty();
        }
    }
}

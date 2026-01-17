using FIAPCloudGames.Domain.Dtos.Request.Game;
using FIAPCloudGames.Domain.Dtos.Validators.Game;
using FluentAssertions;

namespace FIAPCloudGames.Domain.Tests.Validators
{
    public class AtualizarGameRequestValidatorTests
    {
        private readonly AtualizarGameRequestValidator _validator;

        public AtualizarGameRequestValidatorTests()
        {
            _validator = new AtualizarGameRequestValidator();
        }

        [Fact]
        public void Deve_ter_erro_quando_Id_estiver_vazio()
        {
            var request = new AtualizarGameRequest
            {
                Id = Guid.Empty,
                Nome = "Nome",
                Descricao = "Descricao",
                Genero = "Genero",
                Desenvolvedor = "Dev",
                Preco = 10m,
                DataRelease = null
            };

            var result = _validator.Validate(request);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(e => e.PropertyName == nameof(request.Id) && e.ErrorMessage == "Id é obrigatório.");
        }

        [Fact]
        public void Deve_ter_erro_quando_Nome_estiver_vazio_ou_maior_que_200_caracteres()
        {
            var requestEmpty = new AtualizarGameRequest
            {
                Id = Guid.NewGuid(),
                Nome = string.Empty,
                Descricao = "Descricao",
                Genero = "Genero",
                Desenvolvedor = "Dev",
                Preco = 10m,
                DataRelease = null
            };

            var resultEmpty = _validator.Validate(requestEmpty);

            resultEmpty.IsValid.Should().BeFalse();
            resultEmpty.Errors.Should().Contain(e => e.PropertyName == nameof(requestEmpty.Nome) && e.ErrorMessage == "Nome é obrigatório.");

            var longNome = new string('a', 201);
            var requestLong = new AtualizarGameRequest
            {
                Id = Guid.NewGuid(),
                Nome = longNome,
                Descricao = "Descricao",
                Genero = "Genero",
                Desenvolvedor = "Dev",
                Preco = 10m,
                DataRelease = null
            };

            var resultLong = _validator.Validate(requestLong);

            resultLong.IsValid.Should().BeFalse();
            resultLong.Errors.Should().Contain(e => e.PropertyName == nameof(requestLong.Nome) && e.ErrorMessage == "Nome deve ter no máximo 200 caracteres.");
        }

        [Fact]
        public void Deve_ter_erro_quando_Descricao_estiver_vazia_ou_maior_que_1000_cares()
        {
            var requestEmpty = new AtualizarGameRequest
            {
                Id = Guid.NewGuid(),
                Nome = "Nome",
                Descricao = string.Empty,
                Genero = "Genero",
                Desenvolvedor = "Dev",
                Preco = 10m,
                DataRelease = null
            };

            var resultEmpty = _validator.Validate(requestEmpty);

            resultEmpty.IsValid.Should().BeFalse();
            resultEmpty.Errors.Should().Contain(e => e.PropertyName == nameof(requestEmpty.Descricao) && e.ErrorMessage == "Descrição é obrigatória.");

            var longDesc = new string('d', 1001);
            var requestLong = new AtualizarGameRequest
            {
                Id = Guid.NewGuid(),
                Nome = "Nome",
                Descricao = longDesc,
                Genero = "Genero",
                Desenvolvedor = "Dev",
                Preco = 10m,
                DataRelease = null
            };

            var resultLong = _validator.Validate(requestLong);

            resultLong.IsValid.Should().BeFalse();
            resultLong.Errors.Should().Contain(e => e.PropertyName == nameof(requestLong.Descricao) && e.ErrorMessage == "Descrição deve ter no máximo 1000 caracteres.");
        }

        [Fact]
        public void Deve_ter_erro_quando_Genero_estiver_vazio_ou_maior_que_50_caracteres()
        {
            var requestEmpty = new AtualizarGameRequest
            {
                Id = Guid.NewGuid(),
                Nome = "Nome",
                Descricao = "Descricao",
                Genero = string.Empty,
                Desenvolvedor = "Dev",
                Preco = 10m,
                DataRelease = null
            };

            var resultEmpty = _validator.Validate(requestEmpty);

            resultEmpty.IsValid.Should().BeFalse();
            resultEmpty.Errors.Should().Contain(e => e.PropertyName == nameof(requestEmpty.Genero) && e.ErrorMessage == "Gênero é obrigatório.");

            var longGenero = new string('g', 51);
            var requestLong = new AtualizarGameRequest
            {
                Id = Guid.NewGuid(),
                Nome = "Nome",
                Descricao = "Descricao",
                Genero = longGenero,
                Desenvolvedor = "Dev",
                Preco = 10m,
                DataRelease = null
            };

            var resultLong = _validator.Validate(requestLong);

            resultLong.IsValid.Should().BeFalse();
            resultLong.Errors.Should().Contain(e => e.PropertyName == nameof(requestLong.Genero) && e.ErrorMessage == "Gênero deve ter no máximo 50 caracteres.");
        }

        [Fact]
        public void Deve_ter_erro_quando_Desenvolvedor_estiver_vazio_ou_maior_que_200_caracteres()
        {
            var requestEmpty = new AtualizarGameRequest
            {
                Id = Guid.NewGuid(),
                Nome = "Nome",
                Descricao = "Descricao",
                Genero = "Genero",
                Desenvolvedor = string.Empty,
                Preco = 10m,
                DataRelease = null
            };

            var resultEmpty = _validator.Validate(requestEmpty);

            resultEmpty.IsValid.Should().BeFalse();
            resultEmpty.Errors.Should().Contain(e => e.PropertyName == nameof(requestEmpty.Desenvolvedor) && e.ErrorMessage == "Desenvolvedor é obrigatório.");

            var longDev = new string('d', 201);
            var requestLong = new AtualizarGameRequest
            {
                Id = Guid.NewGuid(),
                Nome = "Nome",
                Descricao = "Descricao",
                Genero = "Genero",
                Desenvolvedor = longDev,
                Preco = 10m,
                DataRelease = null
            };

            var resultLong = _validator.Validate(requestLong);

            resultLong.IsValid.Should().BeFalse();
            resultLong.Errors.Should().Contain(e => e.PropertyName == nameof(requestLong.Desenvolvedor) && e.ErrorMessage == "Desenvolvedor deve ter no máximo 200 caracteres.");
        }

        [Fact]
        public void Deve_ter_erro_quando_Preco_for_menor_que_zero()
        {
            var request = new AtualizarGameRequest
            {
                Id = Guid.NewGuid(),
                Nome = "Nome",
                Descricao = "Descricao",
                Genero = "Genero",
                Desenvolvedor = "Dev",
                Preco = -1m,
                DataRelease = null
            };

            var result = _validator.Validate(request);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(e => e.PropertyName == nameof(request.Preco) && e.ErrorMessage == "Preço deve ser maior ou igual a zero.");
        }

        [Fact]
        public void Deve_ter_erro_quando_DataRelease_for_maior_que_5_anos_no_futuro()
        {
            var future = DateOnly.FromDateTime(DateTime.UtcNow.AddYears(6));
            var request = new AtualizarGameRequest
            {
                Id = Guid.NewGuid(),
                Nome = "Nome",
                Descricao = "Descricao",
                Genero = "Genero",
                Desenvolvedor = "Dev",
                Preco = 10m,
                DataRelease = future
            };

            var result = _validator.Validate(request);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(e => e.PropertyName == nameof(request.DataRelease) && e.ErrorMessage == "Data de lançamento não pode ser maior que 5 anos no futuro.");
        }

        [Fact]
        public void Deve_ser_valido_quando_todos_campos_estiverem_corretos()
        {
            var future = DateOnly.FromDateTime(DateTime.UtcNow.AddYears(2));
            var request = new AtualizarGameRequest
            {
                Id = Guid.NewGuid(),
                Nome = "Nome Válido",
                Descricao = "Descrição válida",
                Genero = "Ação",
                Desenvolvedor = "Dev",
                Preco = 59.99m,
                DataRelease = future
            };

            var result = _validator.Validate(request);

            result.IsValid.Should().BeTrue();
            result.Errors.Should().BeEmpty();
        }
    }
}

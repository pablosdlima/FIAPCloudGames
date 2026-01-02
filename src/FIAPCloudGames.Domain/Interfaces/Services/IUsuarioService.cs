using FIAPCloudGames.Domain.Dtos.Request.Usuario;
using FIAPCloudGames.Domain.Dtos.Responses.Usuario;
using FIAPCloudGames.Domain.Interfaces.Generic;
using FIAPCloudGames.Domain.Models;

namespace FIAPCloudGames.Domain.Interfaces.Services;

public interface IUsuarioService : IGenericService<Usuario>
{
    Task<Usuario> CadastrarUsuario(CadastrarUsuarioRequest request);

    Task<Usuario> ValidarLogin(string usuario, string senha);

    Task<bool> AlterarSenha(AlterarSenhaRequest request);

    Task<AlterarStatusResponse> AlterarStatus(Guid Id);
}
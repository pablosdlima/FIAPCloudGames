using FIAPCloudGames.Domain.Dtos.Request;
using FIAPCloudGames.Domain.Interfaces.Generic;
using FIAPCloudGames.Domain.Models;

namespace FIAPCloudGames.Domain.Interfaces.Services;

public interface IUsuarioService : IGenericService<Usuario>
{
    Task<Usuario> CadastrarUsuario(CadastrarUsuarioRequest request);

    Task<Usuario> ValidarLogin(string usuario, string senha);
}
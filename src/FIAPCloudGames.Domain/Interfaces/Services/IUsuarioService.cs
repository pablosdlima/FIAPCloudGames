using FIAPCloudGames.Domain.Dtos.Request;
using FIAPCloudGames.Domain.Interfaces.Generic;
using FIAPCloudGames.Domain.Models;

namespace FIAPCloudGames.Domain.Interfaces.Services;

public interface IUsuarioService : IGenericService<Usuario>
{
    Usuario CadastrarUsuario(CadastrarUsuarioRequest usuario);

    Task<Usuario> ValidarLogin(string usuario, string senha);
}
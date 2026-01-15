using FIAPCloudGames.Domain.Dtos;
using FIAPCloudGames.Domain.Dtos.Request.Usuario;
using FIAPCloudGames.Domain.Dtos.Responses.Usuario;
using FIAPCloudGames.Domain.Dtos.Responses.Usuarios;
using FIAPCloudGames.Domain.Interfaces.Generic;
using FIAPCloudGames.Domain.Models;

namespace FIAPCloudGames.Domain.Interfaces.Services;

public interface IUsuarioService : IGenericServices<Usuario>
{
    Task<Usuario> CadastrarUsuario(CadastrarUsuarioRequest request);

    Task<Usuario> ValidarLogin(string usuario, string senha);

    Task<bool> AlterarSenha(AlterarSenhaRequest request);

    Task<AlterarStatusResponse> AlterarStatus(Guid Id);

    #region GraphQl
    //---------------------------------------------------
    Task<IDictionary<Guid, UsuarioDtos>> BuscarPorIdsAsync(IEnumerable<Guid> ids);
    //---------------------------------------------------
    #endregion
}
using FIAPCloudGames.Domain.Enums;

namespace FIAPCloudGames.Domain.Dtos.Request.UsuarioRole;

public record UsuarioRoleRequest(Guid idUsuarioRole, Guid UsuarioId, TipoUsuario tipoUsuario);
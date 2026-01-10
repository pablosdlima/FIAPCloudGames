namespace FIAPCloudGames.Application.Dtos;

public class ContatosDtos
{
    public int IdContato { get; set; }
    public Guid UsuarioId { get; set; }
    public string Celular { get; set; }
    public string Email { get; set; }
}
namespace FIAPCloudGames.Domain.Dtos.Request.Contato
{
    public record ContatoResponse
    {
        public Guid Id { get; init; }
        public Guid UsuarioId { get; init; }
        public string Celular { get; init; }
        public string Email { get; init; }
    }
}

namespace Neveria.Models.DTOs
{
    public class UsuarioDTO
    {
        public int TagUser { get; set; }
        public string? Name { get; set; }
        public string? UserName { get; set; }
        public string? Email { get; set; }
        public string? NombreRol { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsActive { get; set; }
        // Sin Password — nunca se expone
    }
}

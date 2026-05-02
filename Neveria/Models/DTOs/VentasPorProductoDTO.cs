namespace Neveria.Models.DTOs
{
    public class VentasPorProductoDTO
    {
        public string? NameProduct { get; set; }
        public string? NameCategorie { get; set; }
        public int TotalUnidades { get; set; }
        public decimal TotalIngresos { get; set; }
    }
}

namespace Neveria.Models.DTOs
{

    public class VentaDetalleDTO
    {
        public string? NameProduct { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public decimal Subtotal => Quantity * Price;
    }
}

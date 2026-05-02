namespace Neveria.Models.DTOs
{
    public class VentaResumenDTO
    {
        public int TaglSale { get; set; }
        public DateTime DateSale { get; set; }
        public decimal DueTotal { get; set; }
        public string? NombreEmpleado { get; set; }
        public List<VentaDetalleDTO> Detalles { get; set; } = new();
    }

    public class VentaDetalleDTO
    {
        public string? NameProduct { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public decimal Subtotal => Quantity * Price;
    }
}
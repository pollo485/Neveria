namespace Neveria.Models.DTOs
{

    public class VentaDetalleTablaDTO
    {
        public string NameProduct { get; set; } = "";
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public decimal Total { get; set; }
        public DateTime DateSale { get; set; }
    }
}

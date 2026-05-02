namespace Neveria.Models.DTOs
{
    public class ProductoDetalleDTO
    {
        public int TagProduct { get; set; }
        public string? NameProduct { get; set; }
        public decimal UnitPrice { get; set; }
        public string? DescriptionProduct { get; set; }
        public string? NameCategorie { get; set; }
        public int TagCategorie { get; set; }
        public int StockQuantity { get; set; }
        public bool StockBajo { get; set; }
    }
}

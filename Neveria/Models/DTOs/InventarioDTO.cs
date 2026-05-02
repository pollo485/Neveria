namespace Neveria.Models.DTOs
{
    public class InventarioDTO
    {
        public int TagInventory { get; set; }
        public string? NameProduct { get; set; }
        public string? NameCategorie { get; set; }
        public int StockQuantity { get; set; }
        public int MinQuantity { get; set; }
        public bool StockBajo => StockQuantity <= MinQuantity;
        public DateTime UpdateAt { get; set; }
    }
}

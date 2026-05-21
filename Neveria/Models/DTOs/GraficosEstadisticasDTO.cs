namespace Neveria.Models.DTOs
{
    public class GraficosEstadisticasDTO
    {
        public int VentasMes { get; set; }
        public decimal IngresosTotales { get; set; }
        public string ProductoMasVendido { get; set; } = "";
        public List<int> VentasSemanaActual { get; set; } = new();
        public List<int> VentasSemanaAnterior { get; set; } = new();
    }
}

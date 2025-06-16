// En: SistemaVenta.Shared/DTOs/CategoriaDTO.cs
namespace Shared.DTOs
{
    public class CategoriaDTO
    {
        public int IdCategoria { get; set; }
        public string? Nombre { get; set; }
        public int IdMedida { get; set; }
        public string? NombreMedida { get; set; } // Para mostrar el nombre de la medida en las vistas
        public bool Activo { get; set; } // Usamos bool para representar el estado (true/false)
    }
}
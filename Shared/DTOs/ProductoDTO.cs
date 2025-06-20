using System.ComponentModel.DataAnnotations;

// Asegúrate de que el namespace coincida con tu proyecto compartido.
namespace Shared.DTOs
{
    public class ProductoDTO
    {
        public int IdProducto { get; set; }

        [Required(ErrorMessage = "El código es obligatorio.")]
        public string? Codigo { get; set; }

        [Required(ErrorMessage = "La descripción es obligatoria.")]
        public string? Descripcion { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar una categoría.")]
        public int IdCategoria { get; set; }

        public string? DescripcionCategoria { get; set; }

        [Range(0.01, double.MaxValue, ErrorMessage = "El precio de compra debe ser mayor a 0.")]
        public decimal PrecioCompra { get; set; }

        [Range(0.01, double.MaxValue, ErrorMessage = "El precio de venta debe ser mayor a 0.")]
        public decimal PrecioVenta { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "La cantidad (stock) debe ser 0 o mayor.")]
        public int Cantidad { get; set; }

        // Mantenemos 'Activo' como booleano para el DTO, es más limpio para el API y el frontend.
        // La conversión a entero (1 o 0) se hará en el controlador.
        public bool Activo { get; set; } = true;
    }
}
using System.Collections.Generic;

namespace Shared.DTOs
{
    public class MenuDTO
    {
        public int IdMenu { get; set; }
        public string? Nombre { get; set; }
        public string? Icono { get; set; }
        public string? Url { get; set; }
        public int IdMenuPadre { get; set; }

        // Propiedad clave para la jerarquía
        public List<MenuDTO>? SubMenus { get; set; }
    }
}
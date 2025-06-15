// En: SVRepository/Entities/MenuRol.cs
namespace SVRepository.Entities
{
    public class MenuRol
    {
        public int IdMenu { get; set; } // Propiedad añadida
        public string NombreMenu { get; set; }
        public int IdMenuPadre { get; set; }
        public bool Activo { get; set; }
    }
}
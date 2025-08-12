using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace SVRepository.Entities
{
    /// <summary>
    /// Representa una unidad de medida en el sistema de ventas.
    /// </summary>
    /// <remarks>
    /// Las unidades de medida definen cómo se cuantifican y venden los productos.
    /// Cada medida tiene una abreviatura, nombre descriptivo y puede tener
    /// equivalencias con otras unidades de medida.
    /// </remarks>
    public class Medida
    {
        /// <summary>
        /// Identificador único de la unidad de medida.
        /// </summary>
        /// <remarks>
        /// Este valor es generado automáticamente por la base de datos
        /// y se utiliza como clave primaria en la tabla de medidas.
        /// </remarks>
        public int IdMedida { get; set; }

        /// <summary>
        /// Abreviatura corta de la unidad de medida.
        /// </summary>
        /// <remarks>
        /// Ejemplos: kg (kilogramos), l (litros), pza (piezas), etc.
        /// Se utiliza para mostrar de forma concisa la unidad en la interfaz de usuario.
        /// </remarks>
        public string Abreviatura { get; set; } = string.Empty;

        /// <summary>
        /// Nombre completo de la unidad de medida.
        /// </summary>
        /// <remarks>
        /// Ejemplos: Kilogramos, Litros, Piezas, Metros, etc.
        /// Se utiliza para mostrar el nombre completo en la interfaz de usuario.
        /// </remarks>
        public string Nombre { get; set; } = string.Empty;

        /// <summary>
        /// Descripción de la equivalencia de esta unidad de medida.
        /// </summary>
        /// <remarks>
        /// Proporciona información sobre cómo esta unidad se relaciona
        /// con otras unidades de medida del sistema.
        /// </remarks>
        public string Equivalente { get; set; } = string.Empty;

        /// <summary>
        /// Valor numérico asociado a la equivalencia de la unidad de medida.
        /// </summary>
        /// <remarks>
        /// Este valor se utiliza para cálculos de conversión entre
        /// diferentes unidades de medida cuando sea necesario.
        /// </remarks>
        public int Valor { get; set; }
    }
}

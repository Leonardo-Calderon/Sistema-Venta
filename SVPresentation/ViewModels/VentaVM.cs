using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SVPresentation.ViewModels
{
    public class VentaVM
    {
        [DisplayName("Fecha Registro")]
        public string FechaRegistro { get; set; }
        public string NumeroVenta { get; set; }
        public string Usuario { get; set; }
        public string Cliente { get; set; }
        public string Total { get; set; }
    }
}

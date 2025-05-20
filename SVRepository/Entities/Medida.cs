using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace SVRepository.Entities
{
    public class Medida
    {
        public int IdMedida { get; set; }
        public string Abreviatura { get; set; }
        public string Nombre { get; set; }
        public string Equivalente { get; set; }
        public int Valor { get; set; }


    }
}

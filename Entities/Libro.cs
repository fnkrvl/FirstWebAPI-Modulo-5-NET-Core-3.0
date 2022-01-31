using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Modulo_5.Entities
{
    public class Libro // Sirve como entidad para guardar información con la base de datos
    {

        public int ID { get; set; }
        public string Titulo { get; set; }
        public int AutorID { get; set; }
        public Autor Autor { get; set; }
    }
}

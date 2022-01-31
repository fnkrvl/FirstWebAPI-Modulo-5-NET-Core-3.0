using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Modulo_5.Models
{
    public class LibroDTO // Representa la información que se envía al cliente
    {

        public int ID { get; set; }
        public string Titulo { get; set; }
        public int AutorID { get; set; }
    }
}

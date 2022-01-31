using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Modulo_5.Entities
{
    public class Autor // Sirve como entidad para guardar información con la base de datos
    {

        public int ID { get; set; }
        [Required]
        public string Nombre { get; set; }
        public string Identificacion { get; set; }
        public DateTime fechaNacimiento { get; set; }
        public List<Libro> Books { get; set; }

    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Modulo_5.Models
{
    public class AutorDTO // Representa la información que se envía al cliente
    {

        public int ID { get; set; }
        [Required]
        public string Nombre { get; set; }
        public DateTime fechaNacimiento { get; set; }
        public List<LibroDTO> Books { get; set; }

    }
}

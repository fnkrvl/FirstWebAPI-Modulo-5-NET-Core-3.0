using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Modulo_5.Context;
using Modulo_5.Entities;
using Modulo_5.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Modulo_5.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AutoresController : ControllerBase
    {

        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;

        public AutoresController(ApplicationDbContext context, IMapper mapper)  // Inyectamos el servicio de AutoMapper
        {
            this.context = context;
            this.mapper = mapper;
        }


        [HttpGet("/listado")]
        [HttpGet("listado")]
        public ActionResult<IEnumerable<Autor>> GetListado()
        {
            return context.Autor.Include(x => x.Books).ToList();
        }


        [HttpGet("/Primer")]
        [HttpGet("Primer")]
        public ActionResult<Autor> GetPrimerAutor()
        {
            return context.Autor.FirstOrDefault();  // Devuelve el primer autor que encuentra
        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<AutorDTO>>> Get()
        {
            var autores = await context.Autor.ToListAsync();
            var autoresDTO = mapper.Map<List<AutorDTO>>(autores);
            return autoresDTO;
        }



        [HttpGet("{id}", Name = "ObtenerAutor")]
        public async Task<ActionResult<AutorDTO>> GetAutor(int id)
        {
            var autor = await context.Autor.Include(x => x.Books).FirstOrDefaultAsync(x => x.ID == id);
            // Ésta línea es la que busca el o los recursos externos a la aplicación y es la qiue podría tardar más, y por ello se hace la acción asíncrona

            if (autor == null)
            {
                return NotFound();
            }

            var autorDTO = mapper.Map<AutorDTO>(autor);

            return autorDTO;
        }
        // Se usa Task para definir uan acción asíncrona, es decir una acción que se ejecuta externa a nuestra aplicación, para que el servidor pueda 
        // seguir ejecutando otras tareas

        [HttpPost]
        public async Task<ActionResult> Post([FromBody] AutorCreacionDTO autorCreacion)
        {          // FromBody -> Indica que la información del autor viene en el cuerpo de la petición HTTP 
            TryValidateModel(autorCreacion);  // Si se requiere volver a hacer las validaciones a un modelo (cumple con las validaciones de atributos) 
            var autor = mapper.Map<Autor>(autorCreacion);
            context.Add(autor); // Add -> EF
            await context.SaveChangesAsync();
            var autorDTO = mapper.Map<AutorDTO>(autor);
            return new CreatedAtRouteResult("ObtenerAutor", new { id = autorDTO.ID }, autorDTO); // 201 Created | Objeto creado
        }


        [HttpPut("{id}")]  // Actualización completa de un recurso | Todos los campos de la clase
        public async Task<ActionResult> Put(int id, [FromBody] AutorCreacionDTO autorActualizacion)
        {
            var autor = mapper.Map<Autor>(autorActualizacion);
            autor.ID = id;
            // Entry - EF                                                                // DB CONTEXT
            context.Entry(autorActualizacion).State = EntityState.Modified;              // EntityState.Added      - INSERT
            await context.SaveChangesAsync();                                            // EntityState.Modified   - UPDATE
            return NoContent(); // Código 204 - Valor modificado | No devuelve nada      // EntityState.Deleted    - DELETE
        }                                                                                // EntityState.Unchanged  - On SaveChanges()
                                                                                         // 204 - The server has successfully fulfilled the request and that there is no additional content to send in the response payload body. 


        [HttpDelete("{id}")]
        public async Task<ActionResult<Autor>> Delete(int id)
        {
            var autorID = await context.Autor.Select(x => x.ID).FirstOrDefaultAsync(x => x == id);

            if (autorID == default(int)) // Porque el campo ID es un entero
            {
                return NotFound();  // Hereda de ActionResult
            }

            context.Autor.Remove(new Autor { ID = autorID});  // Remove -> EF
            context.SaveChangesAsync();
            return NoContent(); // Devuelve el autor que se eliminó
        }


        [HttpPatch("{id}")]
        public async Task<ActionResult> Patch(int id, [FromBody] JsonPatchDocument<AutorCreacionDTO> patchDocument)
        {
            if (patchDocument == null)
            {
                return BadRequest();
            }
            // From the database
            var autorDB = await context.Autor.FirstOrDefaultAsync(x => x.ID == id);

            if (autorDB == null)
            {
                return NotFound();
            }

            var autorDTO = mapper.Map<AutorCreacionDTO>(autorDB);

            patchDocument.ApplyTo(autorDTO, ModelState);

            var isValid = TryValidateModel(autorDB);

            if (!isValid)
            {
                return BadRequest(ModelState);
            }

            mapper.Map(autorDTO, autorDB);

            await context.SaveChangesAsync();
            return NoContent();  // 204 Status
                                 
        }

    }
}

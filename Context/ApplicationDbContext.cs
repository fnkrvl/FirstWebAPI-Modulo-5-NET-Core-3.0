using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Modulo_5.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Modulo_5.Context
{
    public class ApplicationDbContext : DbContext                            // Es necesario para el options.UseSqlServer del ConfigureServices | 
    {                                // IdentityDbContext<ApplicationUser>   // Microsoft.EntityFrameworkCore.SqlServer
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {

        }

        // Propiedades
        public DbSet<Autor> Autor { get; set; }
        public DbSet<Libro> Libro { get; set; }  // Permite hacer queries directamente hacia la tabla de libros

        // Dbset representa una colección de todas las instancias en el contexto, 
        // o que puede ser consultado desde la base de datos

        // Una vez creado el DbSet de cada clase, hago la el Add-Migration, and then Update-Database
        // Install EntityFramework.Core.Tools for that
    }
}

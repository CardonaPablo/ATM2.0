using System;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using Microsoft.Extensions.Options;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;


namespace ATM2._0
{
    public class Banco : DbContext
    {
        public DbSet<Usuario> Usuario { get; set; }
        public DbSet<Detalle> Detalle { get; set; }
        public DbSet<Transaccion> Transaccion { get; set; }
        public DbSet<TipoUsuario> TipoUsuario { get; set; }
        public DbSet<TipoConcepto> TipoConcepto { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuider)
        {
            optionsBuider.UseSqlServer(
            @"Integrated Security=SSPI;Persist Security Info=False;Initial Catalog=Banco;Data Source=PABLO");
        }

    }
    class Program
    {
        static void Main(string[] args)
        {
            using (var db = new Banco())
            {
                var blogs = db.Usuario.ToList();
                var tipos = db.TipoUsuario.ToList();
                Usuario newUsuario = new Usuario();
                foreach (var user in blogs)
                {
                    Console.WriteLine($"Nombre: {user.pNombre}");
                    Console.WriteLine($"Nombre: {user.sNombre}");
                    Console.WriteLine($"Apellido: {user.pApellido}");
                    Console.WriteLine($"Apellido: {user.sApellido}");
                    Console.WriteLine($"nCuenta: {user.nCuenta}");
                    Console.WriteLine($"NIP: {user.NIP}");
                    Console.WriteLine($"TipoUsuario: {user.tipo.nombre}");

                }
            }

        }
    }
}

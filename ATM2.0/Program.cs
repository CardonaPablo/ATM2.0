using System;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using Microsoft.Extensions.Options;
using System.ComponentModel.DataAnnotations.Schema;


namespace ATM2._0
{
    public class Banco : DbContext
    {
        public DbSet<Usuario> Usuarios { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuider)
        {
            optionsBuider.UseSqlServer(
            @"Data Source=(localdb)\mssqllocaldb;" +
            "Initial Catalog=Banco;" +
            "Integrated Security=true;");
        }

    }
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Que onda");
            using (var db = new Banco())
            {
                var newUser = new Usuario( 12345, 12345, "Pablo","Eduardo","Cardona","Fajardo");
                db.Usuarios.Add(newUser);
                db.SaveChanges();
                Console.WriteLine("Usuario Creado");
                foreach (var user in db.Usuarios)
                {
                    Console.WriteLine(user.pNombre);
                }
            }

        }
    }
}

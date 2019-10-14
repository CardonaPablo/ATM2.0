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
}

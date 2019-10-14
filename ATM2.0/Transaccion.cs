using System;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using Microsoft.Extensions.Options;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;

namespace ATM2._0
{
    public class Transaccion
    {
        public int id { get; set; }
        [ForeignKey("usuario")]
        public Usuario _usuario { get; set; }
        public ICollection<Detalle> detalles { get; set; }
        public Transaccion()
        {
            detalles = new List<Detalle>();
        }
    }
    

}

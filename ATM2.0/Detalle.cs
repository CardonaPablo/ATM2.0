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
    public class Detalle
    {
        public int id { get; set; }
        public DateTime fecha { get; set; }
        public int monto { get; set; }
        [ForeignKey("destinatario")]
        public Usuario _destinatario { get; set; }
        
        public string concepto { get; set; }
        [ForeignKey("tipoConcepto")]
        public TipoConcepto tipo { get; set; }

    }
}

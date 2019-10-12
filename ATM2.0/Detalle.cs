using System;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using Microsoft.Extensions.Options;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
namespace ATM2._0
{
    public class Detalle
    {
        public int id { get; set; }
        public DateTime fecha { get; set; }
        public int monto { get; set; }
        public Usuario destinatario { get; set; }
        public Transaccion transacciones{ get; set; }
        public TipoConcepto tipoConcepto { get; set; }



    }
}

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
    public class Usuario
    {
        [Key]
        public int nCuenta { get; set; }
        [Required]
        public string NIP { get; set; }

        [Required]
        public string pNombre { get; set; }
        public string sNombre { get; set; }
        [Required]
        public string pApellido { get; set; }
        public string sApellido { get; set; }

        public double saldo { get; set; }

        [ForeignKey("tipoUsuario")]
        public TipoUsuario tipo { get; set; }

    }
}

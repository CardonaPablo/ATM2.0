using System;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using Microsoft.Extensions.Options;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ATM2._0
{
    public class Usuario
    {
        [Key]
        public int nCuenta { get; set; }
        [Required]
        public int NIP { get; set; }

        [Required]
        public string pNombre { get; set; }
        public string sNombre { get; set; }
        [Required]
        public string pApellido { get; set; }
        public string sApellido { get; set; }

        public Usuario( int nCuenta, int NIP, string pNombre, string sNombre, string pApellido, string sApellido)
        {
            this.nCuenta = nCuenta;
            this.NIP = NIP;
            this.pApellido = pApellido;
            this.sApellido = sApellido;
            this.pNombre = pNombre;
            this.sNombre = sNombre;
        }

    }
}

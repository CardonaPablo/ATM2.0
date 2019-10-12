using System;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using Microsoft.Extensions.Options;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace ATM2._0
{
    public class Transaccion
    {
        public int id { get; set; }
        public Usuario usuario { get; set; }
    }
}

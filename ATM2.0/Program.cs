using System;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using Microsoft.Extensions.Options;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;


namespace ATM2._0
{
    class Program
    {
        static void Main(string[] args)
        {
            Cajero c = new Cajero();
            //using (var db = new Banco())
            //{
            //    Usuario nuevoUsuario = new Usuario();
            //    var tipos = db.TipoUsuario.ToList();
            //    nuevoUsuario.nCuenta = 11111;
            //    var plainTextBytes = System.Text.Encoding.UTF8.GetBytes("12345");
            //    nuevoUsuario.NIP = Convert.ToBase64String(plainTextBytes);
            //    nuevoUsuario.pNombre = "Pruebas";
            //    nuevoUsuario.sNombre = null;
            //    nuevoUsuario.pApellido = "Test";
            //    nuevoUsuario.sApellido = null;
            //    nuevoUsuario.saldo = 15800.0;
            //    nuevoUsuario.tipo = tipos[0];
            //    db.Usuario.Add(nuevoUsuario);
            //    Console.WriteLine(db.SaveChanges());
            //}
            c.Login();
            


            //using (var db = new Banco())
            //{
            //    var users = db.Usuario.ToList();
            //    var tipos = db.TipoUsuario.ToList();
            //    foreach (var user in users)
            //    {
            //        Console.WriteLine($"Nombre: {user.pNombre}");
            //        Console.WriteLine($"Nombre: {user.sNombre}");
            //        Console.WriteLine($"Apellido: {user.pApellido}");
            //        Console.WriteLine($"Apellido: {user.sApellido}");
            //        Console.WriteLine($"nCuenta: {user.nCuenta}");
            //        var base64EncodedBytes = System.Convert.FromBase64String(user.NIP);
            //        Console.WriteLine($"NIP: {System.Text.Encoding.UTF8.GetString(base64EncodedBytes)}");
            //        Console.WriteLine($"TipoUsuario: {user.tipo.nombre}");
            //        Console.WriteLine("<----------------------------------------->");

            //    }
            //}


        }
    }
}

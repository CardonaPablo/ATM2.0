﻿using System;
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
            //    var plainTextBytes = System.Text.Encoding.UTF8.GetBytes("11111");
            //    nuevoUsuario.NIP = Convert.ToBase64String(plainTextBytes);
            //    nuevoUsuario.pNombre = "Usuario";
            //    nuevoUsuario.sNombre = "Eduardo";
            //    nuevoUsuario.pApellido = "Prueba";
            //    nuevoUsuario.sApellido = "Fajardo";
            //    nuevoUsuario.saldo = 15000.0;
            //    nuevoUsuario.tipo = tipos[0];
            //    db.Usuario.Add(nuevoUsuario);
            //    Console.WriteLine(db.SaveChanges());
            //}

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
            //Console.ReadLine();
            c.Login();
        }
    }
}

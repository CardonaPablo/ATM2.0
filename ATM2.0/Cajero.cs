using System;
using System.Collections.Generic;
using System.Text;
using System.Timers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Globalization;

namespace ATM2._0
{
    public class Cajero
    {
        private int moneyAmount { get; set; }
        public Usuario currentUser { get; set; }    
        public Cajero()
        {
            moneyAmount = 10000;
            currentUser = new Usuario();

        }

        //Este metodo dibuja el login del cajero para acceder a los datos del usuario en la base de datos
        public void Login()
        {
            while (true)
            {
                Console.Clear();
                //Lee el número de cuenta y comprueba que sea de cinco digitos
                int numeroDeCuenta = 0;
                int nip = 0;
                Console.WriteLine("Bienvenido");
                Console.Write("N° de Cuenta : ");
                numeroDeCuenta = Convert.ToInt32(Console.ReadLine());
                while (numeroDeCuenta > 99999 || numeroDeCuenta < 10000)
                {
                    Console.Clear();
                    Console.Write("Numero de Cuenta Inválido\n N° de Cuenta: ");
                    numeroDeCuenta = Convert.ToInt32(Console.ReadLine());
                }
                //Lee el NIP y comprueba que sea de cinco digitos
                Console.Write("Introduzca su NIP: ");
                nip = Convert.ToInt32(Console.ReadLine());
                while (nip > 99999 || nip < 10000)
                {
                    Console.Clear();
                    Console.Write("NIP Inválido\n Introduzca su NIP: ");
                    nip = Convert.ToInt32(Console.ReadLine());
                }

                //Comprueba que los datos obtenidos por medio del teclado correspondan con algun registro en la base de datos
                using (var db = new Banco())
                {
                    //Tansformarlo a query
                    var plainTextBytes = Encoding.UTF8.GetBytes(nip.ToString());
                    var tipos = db.TipoUsuario.ToList();
                    try
                    {
                        currentUser = db.Usuario.Single(u => numeroDeCuenta == u.nCuenta && Convert.ToBase64String(plainTextBytes) == u.NIP);
                    }
                    catch (InvalidOperationException)
                    {
                        Console.WriteLine("El numero de cuenta o NIP ingresados son incorrectos");
                        System.Threading.Thread.Sleep(2250);
                        continue;
                    }
                }
                if (currentUser.tipo.id == 1)
                    UsuarioMenuPrincipal();
                else
                    GerenteMenuPrincipal();
            }
        }


        public void ActualizarUsuario()
        {
            using (var db = new Banco())
            currentUser = db.Usuario.Single(s => s.nCuenta == currentUser.nCuenta);
        }

        //Este método dibuja el menú principal y nos permite manejar las opciones de operaciones dispinibles en el cajero
        private void UsuarioMenuPrincipal()
        {
            while (true)
            {
                ActualizarUsuario();
                Console.Clear();
                Console.WriteLine($"Nombre: {currentUser.pNombre} {currentUser.pApellido} ");
                Console.WriteLine($"N° De Cuenta: {currentUser.nCuenta}\n");
                ConsoleKeyInfo opcionMenu;
                //Se muestra un menú con las operaciones disponibles del ATM
                Console.WriteLine("Presione el número de la operación que desea realizar \n[1] Solicitud de Saldo  \n[2] Retiro \n[3] Depósito \n[4] Ver historial\n[5] Salir");
                opcionMenu = Console.ReadKey(true);
                Console.Clear();
                //Maneja las opciones del menu basado en la tecla presionada
                switch (opcionMenu.KeyChar)
                {
                    case '1':
                        currentUser.ConsultaSaldo();
                        break;
                    case '2':
                        moneyAmount = currentUser.Retiro(moneyAmount);
                        break;
                    case '3':
                        moneyAmount += currentUser.Deposito();
                        break;
                    case '4':
                        currentUser.HistorialUsuario();
                        break;
                    case '5':
                        //Vuelve al login
                        Console.WriteLine("Salir");
                        return;
                    default:
                        //Si se presiona una tecla que no corresponda a una de las funciones
                        //Se muestra un error por .75 segundos y vuelve a pedir una opcion
                        Console.WriteLine("\nDigite una opcion válida");
                        System.Threading.Thread.Sleep(750);
                        break;
                }
            }
        }

        public void GerenteMenuPrincipal()
        {
            while (true)
            {

                ActualizarUsuario();
                Console.Clear();
                Console.WriteLine($"Nombre: {currentUser.pNombre} {currentUser.pApellido} ");
                Console.WriteLine($"N° De Cuenta: {currentUser.nCuenta}\n");
                ConsoleKeyInfo opcionMenu;
                //Se muestra un menú con las operaciones disponibles del ATM
                Console.WriteLine("Presione el número de la operación que desea realizar \n[1] Reportes de Transaccion  \n[2] Revisar Usuario \n[3] Salir");
                opcionMenu = Console.ReadKey(true);
                Console.Clear();
                //Maneja las opciones del menu basado en la tecla presionada
                switch (opcionMenu.KeyChar)
                {
                    case '1':
                        Gerente.ReportesTransaccion();
                        break;
                    case '2':
                        Gerente.RevisarUsuario();
                        break;
                    case '3':
                        //Vuelve al login
                        Console.WriteLine("Salir");
                        return;
                    default:
                        Console.WriteLine("\nDigite una opcion válida");
                        System.Threading.Thread.Sleep(750);
                        break;
                }
            }
        }

       







        public static int SeleccionaMes()
        {
            int mes;
            do
            {
                Console.WriteLine("Selecciona un mes (Enter para mes actual)");
                for (int i = 1; i < 13; i++)
                {
                    string fullMonthName = new DateTime(2019, i, 1).ToString("MMMM", CultureInfo.CreateSpecificCulture("es"));
                    Console.WriteLine($"[{i}] {fullMonthName}");
                }
                try 
                {
                   mes = Convert.ToInt32(Console.ReadLine());
                } catch (FormatException)
                {
                    mes = DateTime.Now.Month;
                }
            } while (mes < 1 || mes > 12);
            Console.Clear();
            return mes;
        }


    }
}

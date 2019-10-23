using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace ATM2._0
{
    static class Gerente
    {
        public static void ReportesTransaccion()
        {
            Console.Clear();
            ConsoleKeyInfo opcionMenu;
            //Se muestra un menú con las operaciones disponibles del ATM
            Console.WriteLine("Presione el número de la operación que desea realizar \n" +
                "[1] Total Depositos por Mes  \n[2] Numero de Depositos Por Mes\n" +
                "[3] Total Retiros por Mes  \n[4] Numero de Retiros Por Mes\n" +
                "[5] Salir");
            opcionMenu = Console.ReadKey(true);
            Console.Clear();
            switch (opcionMenu.KeyChar)
            {
                case '1':
                    TransaccionesPorMes(Cajero.SeleccionaMes(), "deposito", true);
                    break;
                case '2':
                    TransaccionesPorMes(Cajero.SeleccionaMes(), "deposito", false);
                    break;
                case '3':
                    TransaccionesPorMes(Cajero.SeleccionaMes(), "retiro", true);
                    break;
                case '4':
                    TransaccionesPorMes(Cajero.SeleccionaMes(), "retiro", false);
                    break;
                case '5':
                    Console.WriteLine("Salir");
                    return;
                default:
                    Console.WriteLine("\nDigite una opcion válida");
                    System.Threading.Thread.Sleep(750);
                    break;
            }
        }

        public static void RevisarUsuario()
        {
            Usuario usuario = null;
            using (var db = new Banco())
            {
                bool valid = false;
                int cuentaD;
                do
                {
                    Console.Clear();
                    Console.WriteLine("Digite el numero de cuenta del usuario: ");
                    cuentaD = Convert.ToInt32(Console.ReadLine());
                    try
                    {
                        usuario = db.Usuario.Single(s => s.nCuenta == cuentaD);
                        valid = true;
                    }
                    catch (InvalidOperationException)
                    {
                        continue;
                    }
                } while (!valid);
            }
            Console.Clear();
            usuario.HistorialUsuario();

        }

        private static void TransaccionesPorMes(int mes, string tipo, bool modo)
        {
            using (var db = new Banco())
            {
                float total = 0;
                string fullMonthName = new DateTime(2019, mes, 1).ToString("MMMM", CultureInfo.CreateSpecificCulture("es"));
                var Transacciones = db.Transaccion.ToList();
                var tiposConcepto = db.TipoConcepto.ToList();
                var usuarios = db.Usuario.ToList();
                var detalles = db.Detalle.ToList();
                var transacciones = db.Transaccion.Where(t => t._detalle.tipo == db.TipoConcepto.Single(s => s.nombre == tipo)
               && t._detalle.fecha.Month == mes).ToList();
                if (modo)
                {
                    foreach (var t in transacciones)
                    {
                        
                        total += t._detalle.monto;
                    }
                    Console.WriteLine($"Reporte de {fullMonthName} de Monto de {tipo}s: ${total}");

                }
                else
                {
                    Console.WriteLine($"Reporte de {fullMonthName} N°de {tipo}s: {transacciones.Count()} {tipo}s");
                }
                Console.ReadLine();
            }
        }
    }
}

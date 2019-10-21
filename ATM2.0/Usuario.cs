using System;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using Microsoft.Extensions.Options;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;
using System.Timers;
using System.Globalization;

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

        public ICollection<Transaccion> transacciones { get; set; }

        [ForeignKey("tipoUsuario")]
        public TipoUsuario tipo { get; set; }

       public Usuario()
        {
            transacciones = new List<Transaccion>();
        }


        public int Retiro( int moneyAmount)
        {
            while (true)
            {
                ConsoleKeyInfo opcionRetiro;
                int montoRetiro = 0;
                Console.Clear();
                //Muestra las opciones posibles de retiro 
                Console.WriteLine("Retiro");
                Console.WriteLine("Presione el número correspondiente al monto que desea retirar \n[1] $20 \n[2] $40 \n[3] $60 \n[4] $100 \n[5] $200 \n[6] Personalizado\n[0] Cancelar Operación");
                opcionRetiro = Console.ReadKey(true);
                Console.Clear();
                //Dependiendo de la opcion elegida se asigna un valor a montoRetiro o se cancela la operación
                switch (opcionRetiro.KeyChar)
                {
                    case '0':
                        Console.WriteLine("Operación Cancelada");
                        System.Threading.Thread.Sleep(750);
                        return 0;
                    case '1':
                        montoRetiro = 20;
                        break;
                    case '2':
                        montoRetiro = 40;
                        break;
                    case '3':
                        montoRetiro = 60;
                        break;
                    case '4':
                        montoRetiro = 100;
                        break;
                    case '5':
                        montoRetiro = 200;
                        break;
                    case '6':
                        Console.Clear();
                        Console.WriteLine("Digite el monto a retirar:");
                        montoRetiro = Convert.ToInt32(Console.ReadLine());
                        break;
                    default:
                        Console.WriteLine("Seleccione una opcion válida");
                        System.Threading.Thread.Sleep(750);
                        continue;
                }
                //Se prueba si el monto a retirar es mayor al saldo del usuario y arroja un error
                if (saldo < montoRetiro)
                {
                    Console.WriteLine("Fondos Insuficientes, seleccione un monto menor");
                    System.Threading.Thread.Sleep(1000);
                    continue;
                }
                //Comprueba si el cajero tiene suficiente dinero físico para el retiro
                else if (montoRetiro > moneyAmount)
                {
                    Console.WriteLine("El cajero no tiene suficiente efectivo, seleccione un monto menor");
                    System.Threading.Thread.Sleep(1000);
                    continue;
                }
                //Si todo es correcto, el montoARetirar se resta al saldo del usuario y se entrega el dinero
                else
                {
                    saldo -= montoRetiro;
                    moneyAmount -= montoRetiro;
                    Console.WriteLine("Tome su dinero ↓");
                    System.Threading.Thread.Sleep(4000);

                    using (var db = new Banco())
                    {
                        //Update el usuario
                        var usuario = db.Usuario.Single(s => s.nCuenta == nCuenta);
                        usuario.saldo -= montoRetiro;
                        //Crea un detalle nuevo
                        Detalle detalle = new Detalle();
                        detalle.fecha = DateTime.Now;
                        detalle.concepto = db.TipoConcepto.Single(s => s.id == 1).nombre;
                        detalle.tipo = db.TipoConcepto.Single(s => s.id == 1);
                        detalle.monto = montoRetiro;
                        //Crea una transaccion nueva
                        Transaccion transaccion = new Transaccion();
                        transaccion._usuario = usuario;
                        transaccion._detalle = detalle;
                        //Actualizar las tablas
                        db.Transaccion.Add(transaccion);
                        db.Detalle.Add(detalle);
                        db.SaveChanges();
                    }
                    return moneyAmount;
                }
            }
        }




        public int Deposito()
        {
            bool valid = false;
            string concepto = null;
            int cuentaD = 0, moneyAmount = 0;

            int montoDeposito;
            //Se crea un nuevo timer con periodo de 2 minutos
            Timer tiempoDeposito = new Timer(120000);
            //Se desactiva el autoreinicio del timer cuando termine
            tiempoDeposito.AutoReset = false;
            //Se asigna la funcion OnTimedEvent para manejar el evento del timer llegando a 0
            tiempoDeposito.Elapsed += OnTimedEvent;
            //Se ejecuta cuando el timer llega a cero
            void OnTimedEvent(Object source, System.Timers.ElapsedEventArgs e)
            {
                //Se muestra en pantalla un mensaje de error por 1.5 segundos y vuelve al menú principal
                Console.WriteLine("Transacción cancelada por timeout");
                System.Threading.Thread.Sleep(1500);
                //Apagamos el timer
                tiempoDeposito.Enabled = false;
                return;
            }

            ConsoleKeyInfo opcionMenu;
            Console.WriteLine("Depósito");
            Console.WriteLine("[1] Deposito a cuenta propia\n[2] Deposito a otra cuenta\n[3] Cancelar");
            opcionMenu = Console.ReadKey(true);
            bool isTransaccion = false;
            switch (opcionMenu.KeyChar)
            {
                case '1':
                    cuentaD = nCuenta;
                    break;
                case '2':
                    using (var db = new Banco())
                    {
                        while (!valid)
                        {
                            Console.WriteLine("Digite el numero de cuenta del destinatario");
                            cuentaD = Convert.ToInt32(Console.ReadLine());
                            try
                            {
                                var usuario = db.Usuario.Include(i => i.tipo).Single(s => s.nCuenta == cuentaD);
                                if (usuario.tipo.nombre != "gerente")
                                valid = true;
                            }
                            catch (InvalidOperationException)
                            {
                                continue;
                            }
                        }
                    }
                    isTransaccion = true;
                    break;
                case '3':
                    return 0;
            }
            //Se pide el monto de depósito en centavos y se almacena en montoDeposito
            do
            {
                Console.Write("Digite el monto de depósito en centavos: ");
                montoDeposito = Convert.ToInt32(Console.ReadLine());
            } while (montoDeposito < 1);
            Console.WriteLine("Digite un concepto de pago (Opcional)");
            concepto = Console.ReadLine();
            if (!isTransaccion)
                Console.WriteLine("Inserte su dinero");
            else
                Console.WriteLine("Presione una tecla para continuar");
            //Una vez ingresada la cantidad se inicia el timer
            //Si el timer llega a 0, se ejecuta la función OnTimedEvent
            tiempoDeposito.Enabled = true;
            //La condicion dentro del while nos permite saber si el timer sigue corriendo
            //Si sigue corriendo nos permite presionar una tecla (Ingresar el dinero)
            //Si el timer ya llego a cero su propiedad Enabled sera false
            
            //Revisa si una tecla es presionada
            while (tiempoDeposito.Enabled)
            if (Console.KeyAvailable == true)
            {
                //Si la tecla es presionada, la lee y  detiene el timer 
                Console.ReadKey(true);
                tiempoDeposito.Enabled = false;
                using (var db = new Banco())
                {
                    //Update el usuario
                    var usuario = db.Usuario.Single(s => s.nCuenta == nCuenta);
                    var destinatario = db.Usuario.Single(s => s.nCuenta == cuentaD);
                        //Modifica la propiedad saldo y le suma la cantidad depositada convertida a pesos
                        destinatario.saldo += (montoDeposito / 100);
                    if ( isTransaccion)
                    {
                        if (usuario.saldo >= montoDeposito / 100)
                            usuario.saldo -= montoDeposito / 100;
                        else
                        {
                            Console.WriteLine("No cuenta con el saldo suficente");
                            Console.ReadLine();
                            return moneyAmount;
                        }
                    }
                    else 
                        moneyAmount += montoDeposito / 100;
                    //Crea un detalle nuevo
                    Detalle detalle = new Detalle();
                    detalle.fecha = DateTime.Now;
                    detalle.tipo = db.TipoConcepto.Single(s => s.id == 3);
                    detalle.monto = (montoDeposito / 100);
                    detalle.concepto = concepto != "" ? concepto : db.TipoConcepto.Single(s => s.id == 3).nombre;
                    detalle._destinatario = destinatario;
                    //Crea una transaccion nueva
                    Transaccion transaccion = new Transaccion();
                    transaccion._usuario = usuario;
                    transaccion._detalle = detalle;
                    //Actualizar las tablas
                    db.Transaccion.Add(transaccion);
                    db.Detalle.Add(detalle);
                    db.SaveChanges();
                    //Muestra un mensaje por un segundo y regresa al menú principal
                    Console.WriteLine("Depósito realizado con éxito");
                    System.Threading.Thread.Sleep(1000);

                }
            }
            return moneyAmount;
        }


        public void ConsultaSaldo()
        {
            //El objeto currentUser contiene la informacion del usuario extraida de la base de datos
            //Se muestra el saldo disponible, almacenado en el atributo saldoActual
            Console.WriteLine("Solicitud de Saldo");
            Console.WriteLine("Su saldo actual es de " + saldo);
            Console.WriteLine("\nPresione una tecla para volver al Menu Principal");
            Console.ReadKey(true);
            using (var db = new Banco())
            {
                //Crea un detalle nuevo
                Detalle detalle = new Detalle();
                detalle.fecha = DateTime.Now;
                detalle.concepto = db.TipoConcepto.Single(s => s.id == 2).nombre;
                detalle.tipo = db.TipoConcepto.Single(s => s.id == 2);
                //Crea una transaccion nueva
                Transaccion transaccion = new Transaccion();
                transaccion._usuario = db.Usuario.Single(s => s.nCuenta == nCuenta);
                transaccion._detalle = detalle;

                //Actualizar las tablas
                db.Transaccion.Add(transaccion);
                db.Detalle.Add(detalle);
                db.SaveChanges();
            }
        }

        public void HistorialUsuario()
        {
            ConsoleKeyInfo opcionMenu;
            //Se muestra un menú con las operaciones disponibles del ATM
            Console.WriteLine("Ver: \n[1] Consultas de Saldo \n[2] Retiros \n[3] Depósitos\n[4] Salir");
            opcionMenu = Console.ReadKey(true);
            Console.Clear();
            //Maneja las opciones del menu basado en la tecla presionada
            switch (opcionMenu.KeyChar)
            {
                case '1':
                    Console.WriteLine("Consultas de Saldo");
                    VerTransaccion(Cajero.SeleccionaMes(), "consulta");
                    break;
                case '2':
                    Console.WriteLine("Retiros");
                    VerTransaccion(Cajero.SeleccionaMes(), "retiro");
                    break;
                case '3':
                    Console.WriteLine("Depositos");
                    VerTransaccion(Cajero.SeleccionaMes(), "deposito");
                    break;
            }
        }

        private void VerTransaccion(int mes, string tipo)
        {
            using (var db = new Banco())
            {
                var Transacciones = db.Transaccion.ToList();
                string fullMonthName = new DateTime(2019, mes, 1).ToString("MMMM", CultureInfo.CreateSpecificCulture("es"));
                var tiposConcepto = db.TipoConcepto.ToList();
                var usuarios = db.Usuario.ToList();
                transacciones = db.Transaccion.Where(t => t._usuario == this
                && t._detalle.tipo == db.TipoConcepto.Single(s => s.nombre == tipo)
                && t._detalle.fecha.Month == mes)
                .Include(transaccion => transaccion._detalle).ToList();
                Console.WriteLine($"Mes: {fullMonthName}\n");
                foreach (var transaccion in transacciones)
                {
                    switch (tipo)
                    {
                        case "consulta":
                            Console.WriteLine($"Fecha: {transaccion._detalle.fecha}\n");
                            break;
                        case "retiro":
                            Console.WriteLine($"Monto: {transaccion._detalle.monto}");
                            Console.WriteLine($"Fecha: {transaccion._detalle.fecha}\n");
                            break;
                        case "deposito":
                            Console.WriteLine($"{tipo} a: {transaccion._detalle._destinatario.nCuenta}");
                            Console.WriteLine($"Monto: {transaccion._detalle.monto}");
                            Console.WriteLine($"Concepto: {transaccion._detalle.concepto}");
                            Console.WriteLine($"Fecha: {transaccion._detalle.fecha}\n");
                            break;
                    }
                }
                if (transacciones.Count() == 0)
                    Console.WriteLine("No hay Resultados");
                Console.WriteLine("Presione una tecla para continuar");
            }
            Console.ReadLine();
        }

    }
}

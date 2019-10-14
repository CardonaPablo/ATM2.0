using System;
using System.Collections.Generic;
using System.Text;
using System.Timers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

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
                    var users = db.Usuario.ToList();
                    foreach (Usuario u in users)
                    {
                        var tipos = db.TipoUsuario.ToList();
                        var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(nip.ToString());
                        if (numeroDeCuenta == u.nCuenta && Convert.ToBase64String(plainTextBytes) == u.NIP)
                        {
                            currentUser = u;
                            MostrarMenuPrincipal();

                        }

                    }
                    if (currentUser == null) 
                    { 
                    Console.WriteLine("El numero de cuenta o NIP ingresados son incorrectos");
                    System.Threading.Thread.Sleep(2250);
                    }
                }
            }
        }

        //Este método dibuja el menú principal y nos permite manejar las opciones de operaciones dispinibles en el cajero
        private void MostrarMenuPrincipal()
        {
            while (true)
            {
                Console.Clear();
                ConsoleKeyInfo opcionMenu;
                //Se muestra un menú con las operaciones disponibles del ATM
                Console.WriteLine("Presione el número de la operación que desea realizar \n[1] Solicitud de Saldo  \n[2] Retiro \n[3] Depósito \n[4] Salir");
                opcionMenu = Console.ReadKey(true);
                Console.Clear();
                //Maneja las opciones del menu basado en la tecla presionada
                switch (opcionMenu.KeyChar)
                {
                    case '1':
                        ConsultaSaldo();
                        break;
                    case '2':
                        Console.WriteLine("Retiro");
                        Retiro();
                        break;
                    case '3':
                        Deposito();
                        break;
                    case '4':
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
        private void ConsultaSaldo()
        {
            //El objeto currentUser contiene la informacion del usuario extraida de la base de datos
            //Se muestra el saldo disponible, almacenado en el atributo saldoActual
            Console.WriteLine("Solicitud de Saldo");
            Console.WriteLine("Su saldo actual es de " + currentUser.saldo);
            Console.WriteLine("Presione una tecla para volver al Menu Principal");
            Console.ReadKey(true);
            using (var db = new Banco())
            {
                //Crea una transaccion nueva
                Transaccion transaccion = new Transaccion();
                transaccion._usuario = db.Usuario.Single(s => s.nCuenta == currentUser.nCuenta);
                //Crea un detalle nuevo
                Detalle detalle = new Detalle();
                detalle.fecha = DateTime.Now;
                detalle._transaccion = transaccion;
                detalle.concepto = db.TipoConcepto.Single(s => s.id == 2).nombre;
                detalle.tipo = db.TipoConcepto.Single(s => s.id == 2);
                //Actualizar las tablas
                db.Transaccion.Add(transaccion);
                db.Detalle.Add(detalle);
                db.SaveChanges();
            }
        }


        public void Deposito()
        {
            bool valid = false;
            string concepto = null;
            int cuentaD = 0;

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
            switch( opcionMenu.KeyChar)
            {
                case '1':
                    cuentaD = currentUser.nCuenta;
                    break;
                case '2':
                    using (var db = new Banco()) {
                        while (!valid)
                        {
                            Console.WriteLine("Digite el numero de cuenta del destinatario");
                            cuentaD = Convert.ToInt32(Console.ReadLine());

                            try
                            {
                                var usuario = db.Usuario.Single(s => s.nCuenta == cuentaD);
                                valid = true;
                            }
                            catch (InvalidOperationException) 
                            {
                                continue;
                            }
                        }
                    }
                    break;
                case '3':
                    return;
            }
            //Se pide el monto de depósito en centavos y se almacena en montoDeposito
            Console.Write("Digite el monto de depósito en centavos: ");
            montoDeposito = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine("Digite un concepto de pago (Opcional)");
            concepto = Console.ReadLine();
            Console.WriteLine("Inserte su dinero");
            //Una vez ingresada la cantidad se inicia el timer
            //Si el timer llega a 0, se ejecuta la función OnTimedEvent
            tiempoDeposito.Enabled = true;
            //La condicion dentro del while nos permite saber si el timer sigue corriendo
            //Si sigue corriendo nos permite presionar una tecla (Ingresar el dinero)
            //Si el timer ya llego a cero su propiedad Enabled sera false
            while (tiempoDeposito.Enabled)
            {
                //Revisa si una tecla es presionada
                if (Console.KeyAvailable == true)
                {
                    //Si la tecla es presionada, la lee y  detiene el timer 
                    Console.ReadKey(true);
                    tiempoDeposito.Enabled = false;
                    using (var db = new Banco())
                    {
                        //Update el usuario
                        var usuario = db.Usuario.Single(s => s.nCuenta == currentUser.nCuenta);
                        var destinatario = db.Usuario.Single(s => s.nCuenta == cuentaD);
                        //Modifica la propiedad saldo y le suma la cantidad depositada convertida a pesos
                        destinatario.saldo += (montoDeposito/100);
                        moneyAmount += montoDeposito / 100;
                        //Crea una transaccion nueva
                        Transaccion transaccion = new Transaccion();
                        transaccion._usuario = usuario;
                        //Crea un detalle nuevo
                        Detalle detalle = new Detalle();
                        detalle.fecha = DateTime.Now;
                        detalle._transaccion = transaccion;
                        detalle.tipo = db.TipoConcepto.Single(s => s.id == 3);
                        detalle.monto = (montoDeposito/100);
                        detalle.concepto = concepto != null ? concepto : db.TipoConcepto.Single(s => s.id == 3).nombre;
                        detalle._destinatario = destinatario;
                        //Actualizar las tablas
                        db.Transaccion.Add(transaccion);
                        db.Detalle.Add(detalle);
                        db.SaveChanges();
                        //Muestra un mensaje por un segundo y regresa al menú principal
                        Console.WriteLine("Depósito realizado con éxito");
                        System.Threading.Thread.Sleep(1000);

                    }
                }
            }
        }



        //Este método dibuja el menú para escoger las opciones de retiro de efectivo
        private void Retiro()
        {
            while (true)
            {
                ConsoleKeyInfo opcionRetiro;
                int montoRetiro = 0;
                Console.Clear();
                //Muestra las opciones posibles de retiro 
                Console.WriteLine("Presione el número correspondiente al monto que desea retirar \n[1] $20 \n[2] $40 \n[3] $60 \n[4] $100 \n[5] $200 \n[0] Cancelar Operación");
                opcionRetiro = Console.ReadKey(true);
                Console.Clear();
                //Dependiendo de la opcion elegida se asigna un valor a montoRetiro o se cancela la operación
                switch (opcionRetiro.KeyChar)
                {
                    case '0':
                        Console.WriteLine("Operación Cancelada");
                        System.Threading.Thread.Sleep(750);
                        MostrarMenuPrincipal();
                        break;
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
                    default:
                        Console.WriteLine("Seleccione una opcion válida");
                        System.Threading.Thread.Sleep(750);
                        continue;
                }
                //Se prueba si el monto a retirar es mayor al saldo del usuario y arroja un error
                if (currentUser.saldo < montoRetiro)
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
                    currentUser.saldo = (currentUser.saldo - montoRetiro);
                    moneyAmount -= montoRetiro;
                    Console.WriteLine("Tome su dinero ↓");
                    System.Threading.Thread.Sleep(4000);

                    using (var db = new Banco())
                    {
                        //Update el usuario
                        var usuario = db.Usuario.Single(s => s.nCuenta == currentUser.nCuenta);
                        usuario.saldo -= montoRetiro;
                        //Crea una transaccion nueva
                        Transaccion transaccion = new Transaccion();
                        transaccion._usuario = usuario;
                        //Crea un detalle nuevo
                        Detalle detalle = new Detalle();
                        detalle.fecha = DateTime.Now;
                        detalle._transaccion = transaccion;
                        detalle.concepto = db.TipoConcepto.Single(s => s.id == 1).nombre;
                        detalle.tipo = db.TipoConcepto.Single(s => s.id == 1);
                        detalle.monto = montoRetiro;
                        //Actualizar las tablas
                        db.Transaccion.Add(transaccion);
                        db.Detalle.Add(detalle);
                        db.SaveChanges();
                    }
                    return;
                }
            }
        }


    }
}

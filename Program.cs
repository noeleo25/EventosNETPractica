using System;
using System.Transactions;

namespace PracticaEventos2
{
    public enum TipoAlerta //add before
    {
        Error = 1,
        Advertencia = 2,
        Exito = 3
    }
    public enum TipoPago //add before
    {
        Tarjeta = 1,
        Transferencia = 2,
        Otros = 3
    }
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Ingresa una forma de pago: ");
            Console.WriteLine("1 - Tarjeta \n2 - Transferencia \n3 - Otros ");
            string tipoPago = Console.ReadLine();

            FormaDePago fm = new FormaDePago();
            fm.CambioFormaPago += fm_seleccionFormaPago;
            fm.Tipo = (TipoPago)Enum.Parse(typeof(TipoPago), tipoPago);
        }
        static void fm_seleccionFormaPago(object emisor, CambioFormaPagoEventArgs args) //eventhandler
        {
            if (args.TipoAlerta.Equals(TipoAlerta.Error))
                Console.WriteLine("Error - Elegiste una forma de pago incorrecta ");
            else if (args.TipoAlerta.Equals(TipoAlerta.Exito))
                Console.WriteLine("Nueva forma de pago seleccionada: {0}", args.TipoPago.ToString());
        }
    }

    public class FormaDePago //event broadcaster/publisher/emisora
    {
        //banco
        //cuenta origen
        private TipoPago tipo; //atributo
        //declara variable/propiedad como evento utilizando la clase EventHandler<TEventArgs>
        public event EventHandler<CambioFormaPagoEventArgs> CambioFormaPago; //declara variable/propiedad como evento
        public TipoPago Tipo
        { //propiedad
            get
            {
                return tipo;
            }
            set
            {
                TipoAlerta tipoAlerta = TipoAlerta.Error;
                if (value.Equals(TipoPago.Tarjeta)
                    || value.Equals(TipoPago.Transferencia)
                    || value.Equals(TipoPago.Otros))
                {
                    tipo = value;
                    tipoAlerta = TipoAlerta.Exito;
                }
                CambioFormaPagoEventArgs args = new CambioFormaPagoEventArgs
                {
                    TipoPago = tipo,
                    TipoAlerta = tipoAlerta
                };

                CambioFormaPago(this, args);
            }
        }
    }

    //declaro esta clase porque requiero como parametro mas de un EventArg en mi evento
    public class CambioFormaPagoEventArgs : EventArgs
    {
        public TipoAlerta TipoAlerta { get; set; }
        public TipoPago TipoPago { get; set; }
    }
}

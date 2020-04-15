using System;
using System.ComponentModel;

namespace ProyectoFinal_DI_AlexisSantana.model
{
    public class Cita : NotifyBase,ICloneable
    {
        #region Atributos
        private string nombreCliente;
        private int? id, producto;
        private DateTime fecha;
        private Producto prod;
        #endregion

        #region Propiedades
        public int? Id
        {
            get => id;
            set
            {
                id = value;
                OnPropertyChanged("Id");
            }
        }

        public string NombreCliente
        {
            get => nombreCliente;
            set
            {
                nombreCliente = value;
                OnPropertyChanged("NombreCliente");
            }
        }

        public int? Producto
        {
            get => producto;
            set
            {
                producto = value;
                OnPropertyChanged("Producto");
            }
        }

        public DateTime Fecha
        {
            get => fecha;
            set
            {
                fecha = value;
                OnPropertyChanged("Fecha");
            }
        }

        public Producto Prod
        {
            get => prod;
        }
        #endregion

        #region Constructores
        public Cita() { }

        public Cita(int? id)
        {
            this.id = id;
        }

        public Cita(int? id, string nombreCliente, int? producto, DateTime fecha)
        {
            this.id = id;
            this.nombreCliente = nombreCliente;
            this.producto = producto;
            this.fecha = fecha;
        }

        public Cita(int? id, string nombreCliente, Producto prod, DateTime fecha)
        {
            this.id = id;
            this.nombreCliente = nombreCliente;
            this.prod = prod;
            this.fecha = fecha;
        }
        #endregion

        #region Metodos
        /*Interfaz ICloneable, necesario para modificar*/
        public object Clone()
        {
            return MemberwiseClone();
        }
        #endregion
    }
}

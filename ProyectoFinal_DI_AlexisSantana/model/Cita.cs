using System;
using System.ComponentModel;

namespace ProyectoFinal_DI_AlexisSantana.model
{
    public class Cita : NotifyBase
    {
        #region Atributos
        private string nombreCliente;
        private int? id, producto;
        private DateTime fecha;
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
        #endregion
    }
}

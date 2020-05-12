using System;

namespace ProyectoFinal_DI_AlexisSantana.model
{
    public class Compra : NotifyBase,ICloneable
    {
        #region Atributos
        private DateTime fecha;
        private int? id, cliente, productos;
        private float total;
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

        public int? Cliente
        {
            get => cliente;
            set
            {
                cliente = value;
                OnPropertyChanged("Cliente");
            }
        }

        public int? Productos
        {
            get => productos;
            set
            {
                productos = value;
                OnPropertyChanged("Productos");
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

        public float Total
        {
            get => total;
            set
            {
                total = value;
                OnPropertyChanged("Total");
            }
        }
        #endregion

        #region Constructores
        public Compra() { }

        public Compra(int? id)
        {
            this.id = id;
        }

        public Compra(int? id, int? cliente, DateTime fecha)
        {
            this.id = id;
            this.cliente = cliente;
            this.fecha = fecha;
        }

        public Compra(int? id, int? cliente, int? productos, DateTime fecha, float total)
        {
            this.id = id;
            this.cliente = cliente;
            this.productos = productos;
            this.fecha = fecha;
            this.total = total;
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

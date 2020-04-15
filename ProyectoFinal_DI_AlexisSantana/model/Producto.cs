using System;

namespace ProyectoFinal_DI_AlexisSantana.model
{
    public class Producto : NotifyBase, ICloneable
    {
        #region Atributos
        private string nombre, descripcion;
        private int? id, cantidad;
        private double precio;
        #endregion

        #region Propiedades
        /// <summary>
        /// Getter y Setter del ID.
        /// </summary>
        public int? Id
        {
            get => id;
            set
            {
                id = value;
                OnPropertyChanged("Nombre");
            } 
        }

        /// <summary>
        /// Getter y Setter del nombre.
        /// </summary>
        public string Nombre
        {
            get => nombre;
            set
            {
                nombre = value;
                OnPropertyChanged("Nombre");
            }
        }

        /// <summary>
        /// Getter y Setter de la descripción.
        /// </summary>
        public string Descripcion
        {
            get => descripcion;
            set
            {
                descripcion = value;
                OnPropertyChanged("Descripcion");
            }
        }

        /// <summary>
        /// Getter y Setter de la cantidad.
        /// </summary>
        public int? Cantidad
        {
            get => cantidad;
            set
            {
                cantidad = value;
                OnPropertyChanged("Cantidad");
            }
        }

        /// <summary>
        /// Getter y Setter del precio.
        /// </summary>
        public double Precio
        {
            get => precio;
            set
            {
                precio = value;
                OnPropertyChanged("Precio");
            }
        }
        #endregion

        #region Constructores
        public Producto() {}

        /// <summary>
        /// Crea un nuevo producto solo con un ID.
        /// </summary>
        /// <param name="id">ID del producto.</param> 
        public Producto(int? id)
        {
            this.id = id;
        }

        /// <summary>
        /// Crea un nuevo producto con todos los atributos.
        /// </summary>
        /// <param name="id">ID del producto.</param> 
        /// <param name="nombre">Nombre del producto.</param> 
        /// <param name="descripcion">Descripción del producto.</param> 
        /// <param name="cantidad">Cantidad de unidades del producto que hay en el almacén.</param> 
        /// <param name="precio">Precio del producto.</param> 
        public Producto(int? id, string nombre, string descripcion, int? cantidad, double precio)
        {
            this.id = id;
            this.nombre = nombre;
            this.descripcion = descripcion;
            this.cantidad = cantidad;
            this.precio = precio;
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

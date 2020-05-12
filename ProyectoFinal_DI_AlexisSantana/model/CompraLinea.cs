using System;

namespace ProyectoFinal_DI_AlexisSantana.model
{
    public class CompraLinea : NotifyBase, ICloneable
    {
        #region Atributos
        private int? compra, producto, cantidad;
        private float precio;
        #endregion

        #region Propiedades
        public int? Compra
        {
            get => compra;
            set
            {
                compra = value;
                OnPropertyChanged("Compra");
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

        public int? Cantidad
        {
            get => cantidad;
            set
            {
                cantidad = value;
                OnPropertyChanged("Cantidad");
            }
        }

        public float Precio
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
        public CompraLinea() { }

        public CompraLinea(int? compra, int? producto)
        {
            this.compra = compra;
            this.producto = producto;
        }

        public CompraLinea(int? compra, int? producto, int? cantidad, float precio)
        {
            this.compra = compra;
            this.producto = producto;
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

using System;
using System.ComponentModel;

namespace ProyectoFinal_DI_AlexisSantana.model
{
    public class Cliente : NotifyBase,ICloneable
    {
        #region Atributos
        private string nombreCliente, direccion, telefono;
        private int? id;
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

        public string Direccion
        {
            get => direccion;
            set
            {
                direccion = value;
                OnPropertyChanged("Direccion");
            }
        }

        public string Telefono
        {
            get => telefono;
            set
            {
                telefono = value;
                OnPropertyChanged("Telefono");
            }
        }
        #endregion

        #region Constructores
        public Cliente() { }

        public Cliente(int? id)
        {
            this.id = id;
        }

        public Cliente(int? id, string nombreCliente, string direccion, string telefono)
        {
            this.id = id;
            this.nombreCliente = nombreCliente;
            this.direccion = direccion;
            this.telefono = telefono;
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

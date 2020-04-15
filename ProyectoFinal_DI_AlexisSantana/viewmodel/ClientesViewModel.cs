using ProyectoFinal_DI_AlexisSantana.data.DB;
using ProyectoFinal_DI_AlexisSantana.model;
using ProyectoFinal_DI_AlexisSantana.view;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

namespace ProyectoFinal_DI_AlexisSantana.viewmodel 
{
    public class ClientesViewModel : ObservableCollection<Cliente>
    {
        private ObservableCollection<Cliente> listaClientes;

        public ObservableCollection<Cliente> ListaClientes
        {
            get => listaClientes;
            set => listaClientes = value;
        }

        public ClientesViewModel()
        {
            listaClientes = new ObservableCollection<Cliente>();

            DBConnection.Instance.ConnectDB("clientes");

            if (DBConnection.Instance.itemsClientes != null)
            {
                UIGlobal.MainWindow.EmptyNotifMenu();
                foreach (Cliente i in DBConnection.Instance.itemsClientes)
                {
                    listaClientes.Add(i);
                }
            }
        }

        #region Metodos
        public void AddCliente(Cliente c)
        {
            if (DBConnection.Instance.InsertCliente(c))
            {
                ListaClientes.Add(c);
                UIGlobal.MainWindow.statusBar.Content = "Cliente añadido correctamente";
            }
        }

        public void EditCliente(Cliente c)
        {
            if (DBConnection.Instance.EditCliente(c))
            {
                var cliente = ListaClientes.FirstOrDefault(i => i.Id == c.Id);
                if (cliente != null)
                {
                    cliente.NombreCliente = c.NombreCliente;
                    cliente.Direccion = c.Direccion;
                    cliente.Telefono = c.Telefono;
                }
                UIGlobal.MainWindow.statusBar.Content = "Cliente editado correctamente";
            }
        }

        public void DeleteCliente(Cliente c)
        {
            if (DBConnection.Instance.DeleteCliente(c))
            {
                listaClientes.Remove(listaClientes.Where(i => i.Id == c.Id).Single());
                UIGlobal.MainWindow.statusBar.Content = "Cliente eliminado correctamente";
            }
        }
        #endregion
    }
}

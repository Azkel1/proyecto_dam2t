using ProyectoFinal_DI_AlexisSantana.data.DB;
using ProyectoFinal_DI_AlexisSantana.model;
using System.Collections.ObjectModel;
using System.Linq;

namespace ProyectoFinal_DI_AlexisSantana.viewmodel
{
    public class ProductoViewModel : ObservableCollection<Producto>
    {
        private ObservableCollection<Producto> listaInventario;

        public ObservableCollection<Producto> ListaInventario
        {
            get => listaInventario;
        }

        public ProductoViewModel()
        {
            listaInventario = new ObservableCollection<Producto>();

            // LLamo a la instancia Singleton y conecto a la bbdds, con la operación 1 Leer
            DBConnection.Instance.ConnectDB("inventario");

            // Recorrer el array de clientes e insertarlos en el arrayList de la clase
            if (DBConnection.Instance.itemsInventario != null)
            {
                foreach (Producto i in DBConnection.Instance.itemsInventario)
                {
                    listaInventario.Add(i);
                }
            }
        }

        #region Metodos
        public void AddInv(Producto i)
        {
            if (DBConnection.Instance.InsertInv(i))
            {
                ListaInventario.Add(i);
                UIGlobal.MainWindow.statusBar.Content = "Producto añadido correctamente";
            }

        }

        public void EditInv(Producto i)
        {
            if (DBConnection.Instance.EditInv(i))
            {
                var prod = ListaInventario.FirstOrDefault(p => p.Id == i.Id);
                if (prod != null)
                {
                    prod.Nombre = i.Nombre;
                    prod.Descripcion = i.Descripcion;
                    prod.Cantidad = i.Cantidad;
                    prod.Precio = i.Precio;
                }
                UIGlobal.MainWindow.statusBar.Content = "Producto editado correctamente";
            }
        }

        public void DeleteInv(Producto i)
        {
            if (DBConnection.Instance.DeleteInv(i))
            {
                ListaInventario.Remove(ListaInventario.Where(p => p.Id == i.Id).Single());
                UIGlobal.MainWindow.statusBar.Content = "Producto eliminado correctamente";
            }
        }
        #endregion
    }
}

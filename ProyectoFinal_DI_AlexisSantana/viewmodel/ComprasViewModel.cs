using ProyectoFinal_DI_AlexisSantana.data.DB;
using ProyectoFinal_DI_AlexisSantana.model;
using System.Collections.ObjectModel;
using System.Linq;

namespace ProyectoFinal_DI_AlexisSantana.viewmodel 
{
    public class ComprasViewModel : ObservableCollection<Compra>
    {
        private ObservableCollection<Compra> listaCompras;

        public ObservableCollection<Compra> ListaCompras
        {
            get => listaCompras;
            set => listaCompras = value;
        }

        public ComprasViewModel()
        {
            listaCompras = new ObservableCollection<Compra>();

            DBConnection.Instance.ConnectDB("compras");

            if (DBConnection.Instance.itemsCompras != null)
            {
                UIGlobal.MainWindow.EmptyNotifMenu();
                foreach (Compra i in DBConnection.Instance.itemsCompras)
                {
                    listaCompras.Add(i);
                }
            }
        }

        #region Metodos
        public void AddCompra(Compra c)
        {
            if (DBConnection.Instance.SearchCli(c.Cliente))
            {
                if (DBConnection.Instance.InsertCompra(c))
                {
                    ListaCompras.Add(c);
                    UIGlobal.MainWindow.statusBar.Content = "Compra añadida correctamente";
                }
            }
            else
            {
                UIGlobal.MainWindow.statusBar.Content = "No existe ningún cliente con ese ID";
                UIGlobal.MainWindow.ShowMessage("No existe ningún cliente con ese ID", "error");
            }
        }

        public void EditCompra(Compra c)
        {
            if (DBConnection.Instance.SearchCli(c.Cliente))
            {
                if (DBConnection.Instance.EditCompra(c))
                {
                    var compra = ListaCompras.FirstOrDefault(i => i.Id == c.Id);
                    if (compra != null)
                    {
                        compra.Cliente = c.Cliente;
                        compra.Fecha = c.Fecha;
                    }
                    UIGlobal.MainWindow.statusBar.Content = "Compra editada correctamente";
                }
            }
            else
            {
                UIGlobal.MainWindow.statusBar.Content = "No existe ningún cliente con ese ID";
                UIGlobal.MainWindow.ShowMessage("No existe ningún cliente con ese ID", "error");
            }
        }

        public void DeleteCompra(Compra c)
        {
            if (DBConnection.Instance.DeleteCompra(c))
            {
                listaCompras.Remove(listaCompras.Where(i => i.Id == c.Id).Single());
                UIGlobal.MainWindow.statusBar.Content = "Compra eliminada correctamente";
            }
        }
        #endregion
    }
}

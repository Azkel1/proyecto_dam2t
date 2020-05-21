using ProyectoFinal_DI_AlexisSantana.data.DB;
using ProyectoFinal_DI_AlexisSantana.model;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Data;
using System.Linq;
using System.Windows.Input;
using System;

namespace ProyectoFinal_DI_AlexisSantana.viewmodel
{
    class ComprasLineasViewModel : ObservableCollection<CompraLinea>, INotifyPropertyChanged
    {
        private ObservableCollection<CompraLinea> listaComprasLineas;
        override protected event PropertyChangedEventHandler PropertyChanged;
        private ICommand buttonEliminarLinea;
        private Compra filtro;

        public ObservableCollection<CompraLinea> ListaComprasLineas
        {
            get => listaComprasLineas;
            set => listaComprasLineas = value;
        }

        public ICollectionView FilteredLineas
        {
            get => CollectionViewSource.GetDefaultView(ListaComprasLineas);
        }

        public ComprasLineasViewModel()
        {
            listaComprasLineas = new ObservableCollection<CompraLinea>();
            DBConnection.Instance.ConnectDB("compras_lineas");

            if (DBConnection.Instance.itemsCompras != null)
            {
                UIGlobal.MainWindow.EmptyNotifMenu();
                foreach (CompraLinea i in DBConnection.Instance.itemsComprasLineas)
                {
                    listaComprasLineas.Add(i);
                }
            }

            FilteredLineas.Filter = i =>
            {
                if (filtro == null) return true;
                string m = (i as CompraLinea).Compra.ToString();
                return m.Contains(filtro.Id.ToString());
            };
        }

        public Compra Filtro
        {
            get { return filtro; }
            set
            {
                if (Equals(value, filtro)) return;
                filtro = value;
                OnPropertyChanged("Filtro");
                FilteredLineas.Refresh();
            }
        }

        #region Metodos
        public void AddLinea(CompraLinea c)
        {
            if (DBConnection.Instance.SearchInv(c.Producto))
            {
                if (DBConnection.Instance.InsertCompraLinea(c))
                {
                    ListaComprasLineas.Add(c);
                    UIGlobal.MainWindow.statusBar.Content = "Línea añadida correctamente";
                    ((ViewModel)UIGlobal.MainWindow.DataContext).SwitchToCompras();
                }
            }
            else
            {
                UIGlobal.MainWindow.statusBar.Content = "No existe ningún producto con ese ID";
                UIGlobal.MainWindow.ShowMessage("No existe ningún producto con ese ID", "error");
            }
            
        }

        public void EditLinea(CompraLinea c)
        {
            if (DBConnection.Instance.SearchInv(c.Producto))
            {
                if (DBConnection.Instance.EditCompraLinea(c))
                {
                    var linea = ListaComprasLineas.FirstOrDefault(i => i.Compra == c.Compra && i.Producto == c.Producto);
                    if (linea != null)
                    {
                        linea.Cantidad = c.Cantidad;
                    }
                    ((ViewModel)UIGlobal.MainWindow.DataContext).SwitchToCompras();
                    UIGlobal.MainWindow.statusBar.Content = "Línea editada correctamente";
                }
            }
            else
            {
                UIGlobal.MainWindow.statusBar.Content = "No existe ningún producto con ese ID";
                UIGlobal.MainWindow.ShowMessage("No existe ningún producto con ese ID", "error");
            }
            
        }

        public void DeleteLinea(object param)
        {
            CompraLinea c = null;

            if (param is CompraLinea)
            {
                c = (CompraLinea)param;
                if (DBConnection.Instance.DeleteCompraLinea(c))
                {

                    ListaComprasLineas.Remove(ListaComprasLineas.Where(i => i.Compra == c.Compra && i.Producto == c.Producto).Single());
                    UIGlobal.MainWindow.statusBar.Content = "Línea eliminada correctamente";
                    ((ViewModel)UIGlobal.MainWindow.DataContext).SwitchToCompras();
                }
            }
        }

        public bool CheckLinea(CompraLinea c)
        {
            if (DBConnection.Instance.SearchCompraLinea(c))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        #endregion

        #region Comandos
        public ICommand ButtonEliminarLinea
        {
            get
            {
                if (buttonEliminarLinea == null)
                {
                    buttonEliminarLinea = new CommandPages(param => this.DeleteLinea(param));
                }
                return buttonEliminarLinea;
            }
        }
        #endregion
    }
}

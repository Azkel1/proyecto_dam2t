using ProyectoFinal_DI_AlexisSantana.model;
using ProyectoFinal_DI_AlexisSantana.viewmodel;
using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

namespace ProyectoFinal_DI_AlexisSantana.view
{
    public partial class ComprasPage : Page
    {
        private ComprasViewModel cpvm;
        private ComprasLineasViewModel cplvm;
        private DataGridRow row;

        public ComprasPage()
        {
            InitializeComponent();
            cpvm = new ComprasViewModel();
            cplvm = new ComprasLineasViewModel();

            DataContext = new
            {
                compras = cpvm.ListaCompras,
                lineas = cplvm
            };
        }

        public Visibility EditPanelIsVisible
        {
            set => EditPanel.Visibility = value;
        }

        public Visibility DeletePanelIsVisible
        {
            set => DeletePanel.Visibility = value;
        }

        #region Métodos de filas
        private void RowClick(object sender, RoutedEventArgs e)
        {
            row = ItemsControl.ContainerFromElement((DataGrid)sender, e.OriginalSource as DependencyObject) as DataGridRow;

            if (row != null)
            {
                cplvm.Filtro = ((Compra)row.Item);
            }
        }

        private void RowDoubleClick(object sender, MouseButtonEventArgs e)
        {
            row = ItemsControl.ContainerFromElement((DataGrid)sender, e.OriginalSource as DependencyObject) as DataGridRow;

            if (row != null)
            {
                if (!ComprasExpander.IsExpanded) ComprasExpander.IsExpanded = true;
                EditPanelIsVisible = Visibility.Visible;
                DeletePanelIsVisible = Visibility.Visible;

                Compra_Edit_ID.Text = ((Compra) row.Item).Id.ToString();
                Compra_Edit_Cliente.Text = ((Compra) row.Item).Cliente.ToString();
                Compra_Edit_Fecha.SelectedDate = ((Compra) row.Item).Fecha;
            }
            else
            {
                e.Handled = true;
                return;
            }
        }

        private void RowEditEnding(object sender, DataGridRowEditEndingEventArgs e)
        {
            CompraLinea currentRow_New = (CompraLinea)e.Row.Item;
            IEditableCollectionView itemsView = ((DataGrid)sender).Items;
            //BindingExpression be = BindingOperations.GetBindingExpression((FrameworkElement)sender, CompraLinea_Producto.);

            if (e.EditAction == DataGridEditAction.Commit && currentRow_New != null)
            {
                if (currentRow_New.Compra.Equals("") || currentRow_New.Producto.Equals("") || currentRow_New.Cantidad.Equals(""))
                {
                    MessageBox.Show("Solo el precio puede estar vacío", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    if (cplvm.CheckLinea(currentRow_New) && !itemsView.IsEditingItem)
                    {
                        MessageBox.Show("Ya existe una linea en esta Compra de ese producto, edita la existente.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    else if ((!cplvm.CheckLinea(currentRow_New) && itemsView.IsEditingItem) || (cplvm.CheckLinea(currentRow_New) && itemsView.IsEditingItem))
                    {
                        cplvm.EditLinea(new CompraLinea(currentRow_New.Compra, currentRow_New.Producto, currentRow_New.Cantidad, 0));
                    }
                    else if (!cplvm.CheckLinea(currentRow_New) && itemsView.IsAddingNew)
                    {
                        cplvm.AddLinea(new CompraLinea(currentRow_New.Compra, currentRow_New.Producto, currentRow_New.Cantidad, 0));
                    }
                }
            }
        }
        #endregion

        #region Métodos Compras
        private void AddCompra(object o, RoutedEventArgs e)
        {
            if (Compra_ID.Text.Equals("") || Compra_Cliente_ID.Text.Equals("") || Compra_Fecha.Text.Trim().Equals(""))
            {
                MessageBox.Show("Ningún campo puede estar vacío", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                int? id = Convert.ToInt32(Compra_ID.Text);
                int? cliente = Convert.ToInt32(Compra_Cliente_ID.Text);
                DateTime fecha = Convert.ToDateTime(Compra_Fecha.Text);

                cpvm.AddCompra(new Compra(id, cliente, fecha));
            }
        }

        private void EditCompra(object o, RoutedEventArgs e)
        {
            if (Compra_Edit_Cliente.Text.Equals("") || Compra_Edit_Fecha.Equals(""))
            {
                MessageBox.Show("Ningún campo puede estar vacío", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                int? id = Convert.ToInt32(Compra_Edit_ID.Text);
                int? cliente = Convert.ToInt32(Compra_Edit_Cliente.Text);
                DateTime fecha = Convert.ToDateTime(Compra_Edit_Fecha.Text);

                cpvm.EditCompra(new Compra(id, cliente, fecha));
            }
        }

        private void DeleteCompra(object o, RoutedEventArgs e)
        {
            if (MessageBox.Show("¿Seguro que quiere eliminar esta compra?", "Aviso", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                int? id = Convert.ToInt32(Compra_Edit_ID.Text);
                cpvm.DeleteCompra(new Compra(id));
            }
        }
        #endregion
    }
}

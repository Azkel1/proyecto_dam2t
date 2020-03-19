using ProyectoFinal_DI_AlexisSantana.model;
using ProyectoFinal_DI_AlexisSantana.viewmodel;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace ProyectoFinal_DI_AlexisSantana.view
{
    public partial class InventarioPage : Page
    {
        private ProductoViewModel ivm;

        public InventarioPage()
        {
            InitializeComponent();
            ivm = new ProductoViewModel();
            DataContext = ivm.ListaInventario;
        }

        private void RowClick(object sender, MouseButtonEventArgs e)
        {
            var row = ItemsControl.ContainerFromElement((DataGrid)sender, e.OriginalSource as DependencyObject) as DataGridRow;

            if (row != null)
            {
                if (!InventarioExpander.IsExpanded) InventarioExpander.IsExpanded = true;
                EditPanelIsVisible = Visibility.Visible;
                DeletePanelIsVisible = Visibility.Visible;

                Inv_Edit_Product_ID.Text = ((Producto)row.Item).Id.ToString();
                Inv_Edit_Product_Name.Text = ((Producto)row.Item).Nombre;
                Inv_Edit_Product_Desc.Text = ((Producto)row.Item).Descripcion;
                Inv_Edit_Product_Quantity.Text = ((Producto)row.Item).Cantidad.ToString();
                Inv_Edit_Product_Price.Text = ((Producto)row.Item).Precio.ToString();
            }
            else
            {
                e.Handled = true;
                return;
            }
        }

        public Visibility EditPanelIsVisible
        {
            set => EditPanel.Visibility = value;
        }

        public Visibility DeletePanelIsVisible
        {
            set => DeletePanel.Visibility = value;
        }

        private void AddInv(object o, RoutedEventArgs e)
        {
            if (Inv_Product_ID.Text.Equals("") || Inv_Product_Name.Text.Equals("") || Inv_Product_Quantity.Equals("") || Inv_Product_Price.Equals(""))
            {
                MessageBox.Show("Solo la descripción puede estar vacía.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                int? id = Convert.ToInt32(Inv_Product_ID.Text);
                string nombre = Inv_Product_Name.Text.ToString();
                string descripcion = Inv_Product_Desc.Text.ToString();
                int? cantidad = Convert.ToInt32(Inv_Product_Quantity.Text);
                double precio = Convert.ToDouble(Inv_Product_Price.Text);

                ivm.AddInv(new Producto(id, nombre, descripcion, cantidad, precio));
            }
        }

        private void EditInv(object o, RoutedEventArgs e)
        {
            if (Inv_Edit_Product_Name.Text.Equals("") || Inv_Edit_Product_Quantity.Equals("") || Inv_Edit_Product_Price.Equals(""))
            {
                MessageBox.Show("Solo la descripción puede estar vacía.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                int? id = Convert.ToInt32(Inv_Edit_Product_ID.Text);
                string nombre = Inv_Edit_Product_Name.Text.ToString();
                string descripcion = Inv_Edit_Product_Desc.Text.ToString();
                int? cantidad = Convert.ToInt32(Inv_Edit_Product_Quantity.Text);
                double precio = Convert.ToDouble(Inv_Edit_Product_Price.Text);

                ivm.EditInv(new Producto(id, nombre, descripcion, cantidad, precio));
            }
        }

        private void DeleteInv(object o, RoutedEventArgs e)
        {
            if (MessageBox.Show("¿Seguro que quiere eliminar este producto?", "Aviso", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                int? id = Convert.ToInt32(Inv_Edit_Product_ID.Text);
                ivm.DeleteInv(new Producto(id));
            }
        }
    }
}

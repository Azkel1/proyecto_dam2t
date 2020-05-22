using ProyectoFinal_DI_AlexisSantana.model;
using ProyectoFinal_DI_AlexisSantana.viewmodel;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace ProyectoFinal_DI_AlexisSantana.view
{
    public partial class ClientesPage : Page
    {
        private ClientesViewModel clvm;

        public ClientesPage()
        {
            InitializeComponent();
            clvm = new ClientesViewModel();
            DataContext = clvm.ListaClientes;
        }

        private void RowClick(object sender, MouseButtonEventArgs e)
        {
            var row = ItemsControl.ContainerFromElement((DataGrid)sender, e.OriginalSource as DependencyObject) as DataGridRow;

            if (row != null)
            {
                if (!ClientesExpander.IsExpanded) ClientesExpander.IsExpanded = true;
                EditPanelIsVisible = Visibility.Visible;
                DeletePanelIsVisible = Visibility.Visible;

                Cliente_Edit_ID.Text = ((Cliente) row.Item).Id.ToString();
                Cliente_Edit_Name.Text = ((Cliente) row.Item).NombreCliente;
                Cliente_Edit_Direccion.Text = ((Cliente) row.Item).Direccion;
                Cliente_Edit_Telefono.Text = ((Cliente)row.Item).Telefono;
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
        
        private void AddCliente(object o, RoutedEventArgs e)
        {
            if (Cliente_ID.Text.Equals("") || Cliente_Name.Text.Equals("") || Cliente_Direccion.Text.Trim().Equals("") || Cliente_Telefono.Equals(""))
            {
                MessageBox.Show("Ningún campo puede estar vacío", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                int? id = Convert.ToInt32(Cliente_ID.Text);
                string nombre = Cliente_Name.Text.ToString();
                string direccion = Cliente_Direccion.Text.ToString();
                string telefono = Cliente_Telefono.Text.ToString();

                clvm.AddCliente(new Cliente(id, nombre, direccion, telefono));
            }
        }

        private void EditCliente(object o, RoutedEventArgs e)
        {
            if (Cliente_Edit_Name.Text.Equals("") || Cliente_Edit_Direccion.Equals("") || Cliente_Edit_Telefono.Equals(""))
            {
                MessageBox.Show("Ningún campo puede estar vacío", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                int? id = Convert.ToInt32(Cliente_Edit_ID.Text);
                string nombre = Cliente_Edit_Name.Text.ToString();
                string direccion = Cliente_Edit_Direccion.Text.ToString();
                string telefono = Cliente_Edit_Telefono.Text.ToString();

                clvm.EditCliente(new Cliente(id, nombre, direccion, telefono));
            }
        }

        private void DeleteCliente(object o, RoutedEventArgs e)
        {
            if (MessageBox.Show("¿Seguro que quiere eliminar este cliente?", "Aviso", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                int? id = Convert.ToInt32(Cliente_Edit_ID.Text);
                clvm.DeleteCliente(new Cliente(id));
            }
        }
    }
}

using ProyectoFinal_DI_AlexisSantana.model;
using ProyectoFinal_DI_AlexisSantana.viewmodel;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace ProyectoFinal_DI_AlexisSantana.view
{
    public partial class CitasPage : Page
    {
        private CitasViewModel cvm;

        public CitasPage()
        {
            InitializeComponent();
            cvm = new CitasViewModel();
            DataContext = cvm.ListaCitas;
        }

        private void RowClick(object sender, MouseButtonEventArgs e)
        {
            var row = ItemsControl.ContainerFromElement((DataGrid)sender, e.OriginalSource as DependencyObject) as DataGridRow;

            if (row != null)
            {
                if (!CitasExpander.IsExpanded) CitasExpander.IsExpanded = true;
                EditPanelIsVisible = Visibility.Visible;
                DeletePanelIsVisible = Visibility.Visible;

                Cita_Edit_ID.Text = ((Cita) row.Item).Id.ToString();
                Cita_Edit_Cliente_Name.Text = ((Cita) row.Item).NombreCliente;
                Cita_Edit_Product_ID.Text = ((Cita) row.Item).Producto.ToString();
                Cita_Edit_Date.SelectedDate = ((Cita)row.Item).Fecha;
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
        
        private void AddCita(object o, RoutedEventArgs e)
        {
            if (Cita_Cita_ID.Text.Equals("") || Cita_Cliente_Name.Text.Equals("") || Cita_Product_ID.Text.Trim().Equals("") || Cita_Date.Equals(""))
            {
                MessageBox.Show("Ningún campo puede estar vacío", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                int? id = Convert.ToInt32(Cita_Cita_ID.Text);
                string nombre = Cita_Cliente_Name.Text.ToString();
                int? product_id = Convert.ToInt32(Cita_Product_ID.Text);
                DateTime fecha = Convert.ToDateTime(Cita_Date.Text);

                cvm.AddCita(new Cita(id, nombre, product_id, fecha));
            }
        }

        private void EditCita(object o, RoutedEventArgs e)
        {
            if (Cita_Edit_Cliente_Name.Text.Equals("") || Cita_Edit_Product_ID.Equals("") || Cita_Edit_Date.Equals(""))
            {
                MessageBox.Show("Ningún campo puede estar vacío", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                int? id = Convert.ToInt32(Cita_Edit_ID.Text);
                string nombre = Cita_Edit_Cliente_Name.Text.ToString();
                int? product_id = Convert.ToInt32(Cita_Edit_Product_ID.Text);
                DateTime fecha = Convert.ToDateTime(Cita_Edit_Date.Text);

                cvm.EditCita(new Cita(id, nombre, product_id, fecha));
            }
        }

        private void DeleteCita(object o, RoutedEventArgs e)
        {
            if (MessageBox.Show("¿Seguro que quiere eliminar esta cita?", "Aviso", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                int? id = Convert.ToInt32(Cita_Edit_ID.Text);
                cvm.DeleteCita(new Cita(id));
            }
        }
    }
}

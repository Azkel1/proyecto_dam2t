using ProyectoFinal_DI_AlexisSantana.model;
using System.Windows;

namespace ProyectoFinal_DI_AlexisSantana.view
{
    public partial class InfoCitaWindow : Window
    {
        private Cita cita;
        public InfoCitaWindow(Cita c)
        {
            InitializeComponent();
            if (c != null)
            {
                cita = (Cita)c.Clone();
                nombre_cliente.Text = cita.NombreCliente;
                fecha.Text = cita.Fecha.ToString();
                producto.Text = cita.Prod.Nombre;
                producto_desc.Text = cita.Prod.Descripcion;
                DataContext = cita;
            }
        }
    }
}

using ProyectoFinal_DI_AlexisSantana.model;
using System.Windows;

namespace ProyectoFinal_DI_AlexisSantana.view
{
    public partial class InfoProductoWindow : Window
    {
        private Producto producto;
        public InfoProductoWindow(Producto p)
        {
            InitializeComponent();

            if (p != null)
            {
                producto = (Producto)p.Clone();

                id_prod.Text = producto.Id.ToString();
                nombre_prod.Text = producto.Nombre;
                desc_prod.Text = producto.Descripcion;
                fecha_salida_prod.Text = "Temp";
                cant_prod.Text = producto.Cantidad.ToString();
                precio_prod.Text = producto.Precio.ToString();

                DataContext = producto;
            }
        }
    }
}

using ProyectoFinal_DI_AlexisSantana.informes;
using System.Windows;
using System.Windows.Controls;

namespace ProyectoFinal_DI_AlexisSantana.view
{
    /// <summary>
    /// Lógica de interacción para InformesPage.xaml
    /// </summary>
    public partial class InformesPage : Page
    {
        public InformesPage()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            CitasPorProducto obj = new CitasPorProducto();
            obj.Load(@"CitasPorProducto.rpt");
            viewerInformes.ViewerCore.ReportSource = obj;
        }
    }
}

using ProyectoFinal_DI_AlexisSantana.model;
using ProyectoFinal_DI_AlexisSantana.viewmodel;
using System.Windows;

namespace ProyectoFinal_DI_AlexisSantana.view
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            UIGlobal.MainWindow = this;
            InitializeComponent();
            DataContext = new ViewModel();
        }

        public void ShowMessage(string texto, string tipo)
        {
            switch (tipo)
            {
                case "info":
                    MessageBox.Show(texto, "Información", MessageBoxButton.OK, MessageBoxImage.Information);
                    break;

                case "error":
                    MessageBox.Show(texto, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    break;
            }
        }
    }
}

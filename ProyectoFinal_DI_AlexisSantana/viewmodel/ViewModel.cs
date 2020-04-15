using ProyectoFinal_DI_AlexisSantana.model;
using ProyectoFinal_DI_AlexisSantana.view;
using System.Windows.Input;

namespace ProyectoFinal_DI_AlexisSantana.viewmodel
{
    public class ViewModel
    {
        public ViewModel() { }

        private ICommand buttonInventario, buttonCitas, buttonInfo, buttonSalir, buttonInformes, buttonInfoCita;

        #region Comandos
        public ICommand ButtonInventario
        {
            get
            {
                if (buttonInventario == null)
                {
                    buttonInventario = new CommandPages(param => this.SwitchToInventario(), null);
                }
                return buttonInventario;
            }
        }

        public ICommand ButtonCitas
        {
            get
            {
                if (buttonCitas == null)
                {
                    buttonCitas = new CommandPages(param => this.SwitchToCitas(), null);
                }
                return buttonCitas;
            }
        }

        public ICommand ButtonInfo
        {
            get
            {
                if (buttonInfo == null)
                {
                    buttonInfo = new CommandPages(param => this.MostrarInfo(), null);
                }
                return buttonInfo;
            }
        }

        public ICommand ButtonSalir
        {
            get
            {
                if (buttonSalir == null)
                {
                    buttonSalir = new CommandPages(param => this.Salir(), null);
                }
                return buttonSalir;
            }
        }

        public ICommand ButtonInformes
        {
            get
            {
                if (buttonInformes == null)
                {
                    buttonInformes = new CommandPages(param => this.SwitchToInformes(), null);
                }
                return buttonInformes;
            }
        }

        public ICommand ButtonInfoCita
        {
            get
            {
                if (buttonInfoCita == null)
                {
                    buttonInfoCita = new CommandPages(param => this.MostrarInfoCita((Cita)param));
                }
                return buttonInfoCita;
            }
        }
        #endregion

        #region Metodos
        private void SwitchToInventario()
        {
            InventarioPage inventoryPage = new InventarioPage();
            UIGlobal.MainWindow.dataFrame.Navigate(inventoryPage);

            UIGlobal.MainWindow.statusBar.Content = "Items del Inventario";
        }

        private void SwitchToCitas()
        {
            CitasPage citasPage = new CitasPage();
            UIGlobal.MainWindow.dataFrame.Navigate(citasPage);

            UIGlobal.MainWindow.statusBar.Content = "Items de Citas";
        }

        private void SwitchToInformes()
        {
            InformesPage informesPage = new InformesPage();
            UIGlobal.MainWindow.dataFrame.Navigate(informesPage);

            UIGlobal.MainWindow.statusBar.Content = "Informe: Citas por cada producto";
        }

        private void MostrarInfo()
        {
            UIGlobal.MainWindow.ShowMessage("Aplicación para la gestión del inventario y de las citas con clientes de VRWorld.", "info");
        }

        public void MostrarInfoCita(Cita c)
        {
            if (c == null)
            {
                UIGlobal.MainWindow.ShowMessage("Error al obtener la información de la cita.", "error");
            }
            else
            {
                InfoCitaWindow ventana = new InfoCitaWindow(c);
                ventana.ShowDialog();
            }
        }

        private void Salir()
        {
            UIGlobal.MainWindow.Close();
            System.Windows.Application.Current.Shutdown();
        }
        #endregion
    }
}

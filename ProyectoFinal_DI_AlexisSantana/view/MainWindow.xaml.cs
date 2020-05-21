using Notifications.Wpf;
using ProyectoFinal_DI_AlexisSantana.model;
using ProyectoFinal_DI_AlexisSantana.viewmodel;
using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace ProyectoFinal_DI_AlexisSantana.view
{
    public partial class MainWindow : Window
    {

        private NotificationManager notifManager;

        public MainWindow()
        {
            UIGlobal.MainWindow = this;
            InitializeComponent();
            DataContext = new ViewModel();
            notifManager = new NotificationManager();

            //Reemplazar evento de cierre de la ventana para que se cierren todos los procesos creados
            UIGlobal.MainWindow.Closing += new System.ComponentModel.CancelEventHandler(WindowClosing);
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
                    string strPath = @"error.log";
                    if (!File.Exists(strPath))
                    {
                        File.Create(strPath).Dispose();
                    }
                    using (StreamWriter sw = File.AppendText(strPath))
                    {
                        sw.WriteLine("============= Error Logging ===========");
                        sw.WriteLine("=========== Start ============= " + DateTime.Now);
                        sw.WriteLine("Error Message: " + texto);
                        sw.WriteLine("Stack Trace: " + texto);
                        sw.WriteLine("===========End============= " + DateTime.Now);

                    }
                    break;
            }
        }

        public void ShowNotif(string titulo, Cita info, string tipo)
        {
            MenuItem curItem = new MenuItem();
            NotificationContent content = new NotificationContent
            {
                Title = titulo,
                Message = "Mañana a las " + info.Fecha.TimeOfDay + " tienes una cita con " + info.NombreCliente,
                Type = NotificationType.Information
            };

            switch (tipo)
            {
                case "info":
                    content.Type = NotificationType.Information;
                    break;

                case "error":
                    content.Type = NotificationType.Error;
                    break;
            }
            
            notifManager.Show(content);

            curItem.Header = ("Cita con " + info.NombreCliente);
            curItem.SetBinding(MenuItem.CommandProperty, new Binding("ButtonInfoCita"));
            curItem.CommandParameter = info;
            Notificaciones.Items.Add(curItem);
        }

        public void EmptyNotifMenu()
        {
            Notificaciones.Items.Clear();
        }

        #region Eventos
        private void WindowClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            System.Windows.Application.Current.Shutdown();
        }
        #endregion
    }
}

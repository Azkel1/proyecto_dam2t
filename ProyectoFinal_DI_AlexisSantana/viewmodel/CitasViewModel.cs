﻿using ProyectoFinal_DI_AlexisSantana.data.DB;
using ProyectoFinal_DI_AlexisSantana.model;
using ProyectoFinal_DI_AlexisSantana.view;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

namespace ProyectoFinal_DI_AlexisSantana.viewmodel 
{
    public class CitasViewModel : ObservableCollection<Cita>
    {
        private ObservableCollection<Cita> listaCitas;
        private ICommand buttonVerDetalles; 

        public ObservableCollection<Cita> ListaCitas
        {
            get => listaCitas;
            set => listaCitas = value;
        }

        public CitasViewModel()
        {
            listaCitas = new ObservableCollection<Cita>();

            DBConnection.Instance.ConnectDB("citas");

            if (DBConnection.Instance.itemsCitas != null)
            {
                UIGlobal.MainWindow.EmptyNotifMenu();
                foreach (Cita i in DBConnection.Instance.itemsCitas)
                {
                    listaCitas.Add(i);
                    Cita i_notif = new Cita(i.Id, i.NombreCliente, DBConnection.Instance.GetProd(i.Producto), i.Fecha);
                    if (i_notif.Fecha.Date == (DateTime.Today.Date.AddDays(1)))
                    {
                        UIGlobal.MainWindow.ShowNotif("Citas", i_notif, "info");
                    }
                }
            }
        }

        #region Metodos
        public void AddCita(Cita c)
        {
            if (DBConnection.Instance.SearchInv(c.Producto))
            {
                if (DBConnection.Instance.InsertCita(c))
                {
                    ListaCitas.Add(c);
                    UIGlobal.MainWindow.statusBar.Content = "Cita añadida correctamente";
                }
            }
            else
            {
                UIGlobal.MainWindow.statusBar.Content = "No existe ningún producto con ese ID";
                UIGlobal.MainWindow.ShowMessage("No existe ningún producto con ese ID", "error");
            }
        }

        public void EditCita(Cita c)
        {
            if (DBConnection.Instance.SearchInv(c.Producto))
            {
                if (DBConnection.Instance.EditCita(c))
                {
                    var cita = ListaCitas.FirstOrDefault(i => i.Id == c.Id);
                    if (cita != null)
                    {
                        cita.NombreCliente = c.NombreCliente;
                        cita.Fecha = c.Fecha;
                        cita.Producto = c.Producto;
                    }
                    UIGlobal.MainWindow.statusBar.Content = "Cita editada correctamente";
                }
            }
            else
            {
                UIGlobal.MainWindow.statusBar.Content = "No existe ningún producto con ese ID";
                UIGlobal.MainWindow.ShowMessage("No existe ningún producto con ese ID", "error");
            }
        }

        public void DeleteCita(Cita c)
        {
            if (DBConnection.Instance.DeleteCita(c))
            {
                listaCitas.Remove(listaCitas.Where(i => i.Id == c.Id).Single());
                UIGlobal.MainWindow.statusBar.Content = "Cita eliminada correctamente";
            }
        }
        #endregion
    }
}

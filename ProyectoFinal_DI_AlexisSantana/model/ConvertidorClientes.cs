using ProyectoFinal_DI_AlexisSantana.data.DB;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Data;

namespace ProyectoFinal_DI_AlexisSantana.model
{
    class ConvertidorClientes : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            IDictionary<int?, string> clients;

            clients = new Dictionary<int?, string>();

            if (DBConnection.Instance.itemsClientes == null)
            {
                DBConnection.Instance.ReadClientes();
            }

            foreach (Cliente i in DBConnection.Instance.itemsClientes)
            {
                clients.Add(i.Id, i.NombreCliente);
            }

            return clients[(int?)value];
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

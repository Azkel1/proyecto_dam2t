﻿using ProyectoFinal_DI_AlexisSantana.data.DB;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Data;

namespace ProyectoFinal_DI_AlexisSantana.model
{
    class Convertidor : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            IDictionary<int?, string> products;

            products = new Dictionary<int?, string>();

            if (DBConnection.Instance.itemsInventario == null)
            {
                DBConnection.Instance.ReadInventario();
            }

            foreach (Producto i in DBConnection.Instance.itemsInventario)
            {
                products.Add(i.Id, i.Nombre);
            }

            return products[(int?)value];
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

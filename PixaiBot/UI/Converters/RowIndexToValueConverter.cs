using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;
using PixaiBot.Data.Models;

namespace PixaiBot.UI.Converters
{
    internal class RowIndexToValueConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values[0] is not DataGridRow dataGridRow) return null;
            if (values[1] is not ObservableCollection<UserAccount> accountsList) return null;
            if (accountsList.Count == 0) return null;
            int rowIndex;

            try
            {
                rowIndex = dataGridRow.GetIndex();
            }
            catch
            {
                return null ;
            }

            if (rowIndex >= 0 && rowIndex < accountsList.Count && !string.IsNullOrEmpty(accountsList[rowIndex].Email))
            {
                // +1 because the index starts from 0, also i added 3 to center the number
                return "   " + (rowIndex + 1).ToString();
            }

            return null;
        }


        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

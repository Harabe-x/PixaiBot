using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using PixaiBot.Data.Models;

namespace PixaiBot.UI.Converters
{
    internal class AddEmptyRowConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is not ObservableCollection<UserAccount> userAccounts ||
                parameter is not string desiredRowCount) return null;
            if (userAccounts.Count == 0) return null;
            if (!int.TryParse(desiredRowCount, out var desiredRowsToAdd)) return null;
            for (var i = userAccounts.Count; i < desiredRowsToAdd; i++)
            {
                userAccounts.Add(new UserAccount());   
            }

            return userAccounts;

        }


        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

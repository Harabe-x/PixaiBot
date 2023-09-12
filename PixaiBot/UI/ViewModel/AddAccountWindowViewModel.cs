using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using PixaiBot.Data.Interfaces;
using PixaiBot.Data.Models;
using PixaiBot.UI.Base;

namespace PixaiBot.UI.ViewModel
{
    public class AddAccountWindowViewModel : BaseViewModel , IWindowHelper
    {
        public ICommand AddAccountCommand { get; }

        public ICommand CloseWindowCommand { get; }


        public AddAccountWindowViewModel(IAccountsManager accountsManager,IDataValidator dataValidator)
        {
            _accountsManger = accountsManager;
            _dataValidator = dataValidator;
            AddAccountCommand = new RelayCommand((obj) => AddAccount());
            CloseWindowCommand = new RelayCommand((obj) => CloseWindow());
        }

        public Action Close { get; set; }

        public bool CanCloseWindow { get; set; }

        private readonly IAccountsManager _accountsManger;

        private readonly IDataValidator _dataValidator;

        private string _email;

        public string Email
        {
            get => _email;
            set
            {
                _email = value;
                OnPropertyChanged();
            }
        }

        private string _password;

        public string Password
        {
            get => _password;
            set
            {
                _password = value;
                OnPropertyChanged();
            }

        }

        private void CloseWindow()
        {
            Close?.Invoke();
        }


        private void AddAccount()
        {
            if (_dataValidator.ValidateEmail(Email) && _dataValidator.ValidatePassword(Password))
            {
                var userAccount = new UserAccount
                {
                    Email = this.Email,
                    Password = this.Password
                };
                _accountsManger.AddAccount(userAccount);
               
                return;
            }

            MessageBox.Show("Invailid Data","EERRRRRRRROORRRRRRR",MessageBoxButton.OK,MessageBoxImage.Error);
            
           
        }


        
    }
}

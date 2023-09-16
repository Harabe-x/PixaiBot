using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Threading;
using PixaiBot.Bussines_Logic;
using PixaiBot.Data.Interfaces;
using PixaiBot.Data.Models;
using PixaiBot.UI.Base;

namespace PixaiBot.UI.ViewModel
{
    internal class DashboardControlViewModel : BaseViewModel
    {
        public ICommand testCommand { get;  }

        public DashboardControlViewModel(IAccountLoginChecker accountLoginChecker,IAccountsStatisticsManager accountsStatisticsManager)
        {
            _accountsStatisticsManager = accountsStatisticsManager;
            _accountLoginChecker = accountLoginChecker;
            testCommand = new RelayCommand((obj) => test());
            _timer = new DispatcherTimer();
            _timer.Interval = TimeSpan.FromSeconds(30);
            _timer.Tick += UpdateStaistics;
            _timer.Start();
            UpdateStaistics(null,null);
           

        }

        private readonly IAccountLoginChecker _accountLoginChecker;

        private readonly IAccountsStatisticsManager _accountsStatisticsManager;

        private DispatcherTimer _timer;

        private string _accountCount;

        public string AccountCount
        {
            get => _accountCount;
            set
            {
                _accountCount = $"Accounts Count : {value}";
                OnPropertyChanged();
            }
        }
        private string _accountWithClaimedCredits;

        public string AccountWithClaimedCredits
        {
            get => _accountWithClaimedCredits;
            set
            {
                _accountWithClaimedCredits = $"Accounts With Claimed Credits : {value}";
                OnPropertyChanged();
            }
        }
        private string _accountWithUnclaimedCredits;

        public string AccountWithUnclaimedCredits
        {
            get => _accountWithUnclaimedCredits;
            set
            {
                _accountWithUnclaimedCredits = $"Accounts With Unclaimed credits: {value}";
                OnPropertyChanged();
            }
        }


        private void test()
        {
          
        }

        private void UpdateStaistics(object? sender, EventArgs e)
        {
            _accountsStatisticsManager.RefreshStatistics();
            AccountCount = _accountsStatisticsManager.AccountsNumber.ToString();
            AccountWithClaimedCredits = _accountsStatisticsManager.AccountsWithClaimedCredits.ToString();
            AccountWithUnclaimedCredits = _accountsStatisticsManager.AccountsWithUnclaimedCredits.ToString();
        }


    }
}

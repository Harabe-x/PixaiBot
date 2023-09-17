using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Threading;
using Accessibility;
using PixaiBot.Bussines_Logic;
using PixaiBot.Data.Interfaces;
using PixaiBot.Data.Models;
using PixaiBot.UI.Base;

namespace PixaiBot.UI.ViewModel
{
    internal class DashboardControlViewModel : BaseViewModel
    {
        public ICommand ClaimCreditsCommand { get; }

        public DashboardControlViewModel(ICreditClaimer creditClaimer, IAccountsManager accountsManager,
            IAccountsStatisticsManager accountsStatisticsManager, ILogger logger)
        {
            _accountsStatisticsManager = accountsStatisticsManager;
            _creditClaimer = creditClaimer;
            _accountsManager = accountsManager;
            ClaimCreditsCommand = new RelayCommand((obj) => ClaimCredits());
            _timer = new DispatcherTimer();
            StartStatisticsRefreshing();
        }

        private readonly IAccountsManager _accountsManager;

        private readonly ICreditClaimer _creditClaimer;

        private readonly IAccountsStatisticsManager _accountsStatisticsManager;

        private readonly DispatcherTimer _timer;

        private string? _accountCount;

        public string? AccountCount
        {
            get => _accountCount;
            set
            {
                _accountCount = $"Accounts Count : {value}";
                OnPropertyChanged();
            }
        }
        private string? _accountWithClaimedCredits;

        public string? AccountWithClaimedCredits
        {
            get => _accountWithClaimedCredits;
            set
            {
                _accountWithClaimedCredits = $"Accounts With Claimed Credits : {value}";
                OnPropertyChanged();
            }
        }
        private string? _accountWithUnclaimedCredits;

        public string? AccountWithUnclaimedCredits
        {
            get => _accountWithUnclaimedCredits;
            set
            {
                _accountWithUnclaimedCredits = $"Accounts With Unclaimed credits: {value}";
                OnPropertyChanged();
            }
        }

        private void StartStatisticsRefreshing()
        {
            _timer.Interval = TimeSpan.FromSeconds(5);
            _timer.Tick += UpdateStatistics;
            _timer.Start();
            UpdateStatistics(null, null);

        }

        private void ClaimCredits()
        {
            var accounts = _accountsManager.GetAllAccounts();

            foreach (var account in accounts)  
            {
                _creditClaimer.ClaimCredits(account);
            }

        }

        private void UpdateStatistics(object? sender, EventArgs? e)
        {
            _accountsStatisticsManager.RefreshStatistics();
            AccountCount = _accountsStatisticsManager.AccountsNumber.ToString();
            AccountWithClaimedCredits = _accountsStatisticsManager.AccountsWithClaimedCredits.ToString();
            AccountWithUnclaimedCredits = _accountsStatisticsManager.AccountsWithUnclaimedCredits.ToString();
        }


    }
}

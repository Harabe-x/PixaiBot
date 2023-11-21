using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using PixaiBot.Data.Interfaces;

namespace PixaiBot.Bussines_Logic.Data_Management
{
    internal class LoginCredentialsMaker : ILoginCredentialsMaker
    {


        public LoginCredentialsMaker(ITempMailApiManager tempMailApiManager,ILogger logger)
        {
            _tempMailApiManager = tempMailApiManager;
            _random = new Random();
            _logger = logger;

        }

        private readonly ITempMailApiManager _tempMailApiManager;

        private readonly Random _random;

        private readonly ILogger _logger;

        private const string Letters = "abcdefghijklmnopqrstuvwxyz";

        private const string Characters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789<>?+_)(*&^%$#@!<>?";


        public string GenerateEmail()
        {
            _logger.Log("Creating fake email", _logger.CreditClaimerLogFilePath);
            return GenerateRandomString(7, Letters) + "@gmail.com";
        }

        public string GenerateEmail(string tempMailApiKey)
        {
            _logger.Log("Creating email with temp mail domain ", _logger.CreditClaimerLogFilePath);
            var domains = _tempMailApiManager.GetDomains(tempMailApiKey);
            return GenerateRandomString(7,Letters) + domains.ToList()[_random.Next(domains.Count())];
        }

        public string GeneratePassword()
        {
            _logger.Log("Creating  strong password",_logger.CreditClaimerLogFilePath);
            return GenerateRandomString(8, Characters);
        }

        private string GenerateRandomString(int length, string characters)
        {
            _logger.Log($"Creating a random string of {length} characters utilizing the {nameof(characters)} set.",_logger.CreditClaimerLogFilePath);
            return new string(Enumerable.Repeat(characters, length).Select(x => x[_random.Next(characters.Length)])
                .ToArray());
        }
    }
}

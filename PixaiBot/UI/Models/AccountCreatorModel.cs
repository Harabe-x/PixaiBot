namespace PixaiBot.UI.Models;

internal class AccountCreatorModel
{
    public bool ShouldVerifyEmail { get; set; }

    public bool ShouldUseProxy { get; set; }

    public bool IsRunning { get; set; }

    public string AccountsAmmount { get; set; }

    public string TempMailApiKey { get; set; }

    public string ProxyFilePath { get; set; }

    public string OperationStatus { get; set; }

    public string AccountsCreatorButtonText { get; set; }
}
namespace PixaiBot.UI.Models;

internal class EditAccountCredentialsModel
{
    public string? Email { get; set; }

    public string? Password { get; set; }

    public UserAccount Account { get; set; }
}
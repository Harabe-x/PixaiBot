![PixaiBot Logo](https://i.imgur.com/jrkmatA.png)

## Download 
You can download the application [here](https://github.com/Harabe-x/PixaiBot/releases/tag/v1.0.0.0). Alternatively, you can clone the repository and compile it.

## Requirements 
- Google Chrome
- If the application doesn't start, you'll need to install [.Net Framework](https://dotnet.microsoft.com/en-us/download).

## Usage 
To run the bot, add an account in the settings and click the `Claim Credits` button.

## User Interface

### Dashboard Tab
The Dashboard panel features a button to start the credit claiming process along with basic bot information.

![Dashboard Tab](https://i.imgur.com/KasvXxo.png)  

### Settings Tab 

In the settings, you'll find two sections:

1. **Accounts Settings**:
   - Clicking "Add account" opens a window where you provide your account login details.
   - "Add Account List" allows you to add accounts from a text file (account format: `Email:Password`).
   - "Check All Accounts Logins" verifies account logins. Correct logins stay on the list, incorrect ones are removed.

2. **Bot Settings**:
   - Check the 'Start With System' checkbox to add the program to autostart.
   - Check "Credits Auto Claim" to collect credits at program start or 24 hours after the last collection.
   - Check the 'Toast Notifications' checkbox to receive notifications about Credit Claimer results. Example: ![Notification Example](https://i.imgur.com/RrwHiJS.png)

![Settings Tab](https://i.imgur.com/9aknzaT.png)

In the navigation panel, you'll also find the option to "Hide Application," which minimizes the application to the system tray.

## Additional Information
- Accounts are stored in the `accounts.json` file located at `C:\Users\[Your Account Name]\AppData\Roaming\PixaiAutoClaimer`. If you have trouble finding this directory, press *Win + R* and type `%appdata%`. From there, you can easily locate the `PixaiAutoClaimer` directory.
- If you encounter any problems, please open a new issue and provide the logs from the `Logs` directory.

## Contribution
Contributions to this project are welcome. If you identify any issues or have suggestions for improvement, feel free to create a new Pull Request.

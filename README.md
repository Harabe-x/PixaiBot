![PixaiBot Logo](https://i.imgur.com/jrkmatA.png)

# About this project
PixaiBot is a multifunctional bot designed for the Pixai.art website. Originally, its primary function was to claim credits, but additional features have been added.

## Download 
You can download the application [here](https://github.com/Harabe-x/PixaiBot/releases/tag/v1.3.0.0). Alternatively, you can clone the repository and compile it.

## Requirements 
- Google Chrome
- If the application doesn't start, you'll need to install [.NET Framework](https://dotnet.microsoft.com/en-us/download).

# Usage 
To run the bot, add an account in the settings and click the `Start Claiming` button.

# User Interface

## Dashboard Tab
The Dashboard panel features a button to start the credit claiming process along with basic bot information.

![Dashboard Tab](https://i.imgur.com/lKVSXp7.png)  

### Account List Tab
In this tab, you can view the list of accounts that have been added to the bot.
At the bottom, you will find a button for editing an account. To edit account details, click on the email address and then click the edit button.
You will also find buttons at the bottom for removing an account. To do this, click on the email address in the list, and then click remove.

![Account List Tab](https://i.imgur.com/NLdwotL.png)  

## Account Creator Tab

In this tab, you can find basic options related to account creation.

Upon selecting 'Verify Email?', a textbox will appear where you need to enter the API Key.

To enable the bot to verify emails during account creation, you must provide the Temp Mail API key, which can be obtained at: https://rapidapi.com/Privatix/api/temp-mail

When 'Use proxy?' is selected, a button will appear that, when clicked, allows you to add a proxy. The program currently supports HTTP/SOCKS4/SOCKS5 proxies.

If the user does not choose a proxy, accounts will be created with a 5-minute interval. This is a cooldown imposed by Pixai for creating new accounts.
After creating the account, it will be automatically added to the account list.
You can see the account login details by clicking the button in the account list tab.

![Account Creator tab](https://i.imgur.com/L2NQQ4T.png)  

## Account Info Logger Tab
In this tab, you can log information about all your accounts to a text file.
You can limit the details included in the log by unchecking specific textboxes.
An example log looks as follows: ![Example log](https://i.imgur.com/mR82sd2.png)  

![Account Info Logger Tab](https://i.imgur.com/VSZ5Qex.png)  

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
   - Check the 'Headless Browser' checkbox to enable invisible browser mode. In some cases, this mode may cause problems with the program, so if you notice a problem with the bot, try unchecking this option
   - Check the 'Multi-Threading' checkbox to show a textbox in which you can specify the number of threads in which the program will run. In plain language, this option will control the maximum number of browsers running at once. Changing this option can consume a lot of computer resources, so I recommend adjusting the number of threads to match your hardware capabilities.
   - 
![Settings Tab](https://i.imgur.com/saXy5dh.png)

In the navigation panel, you'll also find the option to "Hide Application," which minimizes the application to the system tray.

## Additional Information
- Accounts are stored in the `accounts.json` file located at `C:\Users\[Your Account Name]\AppData\Roaming\PixaiAutoClaimer`. If you have trouble finding this directory, press *Win + R* and type `%appdata%`. From there, you can easily locate the `PixaiAutoClaimer` directory.
- If you encounter any problems, please open a new issue and provide the logs from the `Logs` directory.

## Contribution
Contributions to this project are welcome. If you identify any issues or have suggestions for improvement, feel free to create a new Pull Request.

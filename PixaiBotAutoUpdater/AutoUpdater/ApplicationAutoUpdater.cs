using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Net.Http;
using System.Reflection;
using System.Windows;
using PixaiBotAutoUpdater.AutoUpdater;

namespace PixaiBot.AutoUpdater;

internal class ApplicationAutoUpdater
{
    private readonly string _applicationFileDirectoryPath =
        $"{Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)}\\PixaiAutoClaimer";

    private readonly string _applicationVersionFilePath;

    private readonly GithubApi _githubApi;

    private readonly HttpClient _httpClient;

    public ApplicationAutoUpdater()
    {
        _githubApi = new GithubApi();
        _httpClient = new HttpClient();
        if (!Directory.Exists(_applicationFileDirectoryPath)) CreateDirectories();
        _applicationVersionFilePath = _applicationFileDirectoryPath + "\\ApplicationVersion.json";
        GetApplicationVersion();
    }

    private ApplicationVersion GetApplicationVersion()
    {
        if (File.Exists(_applicationVersionFilePath))
            return JsonReader.ReadApplicationVersion(_applicationVersionFilePath);


        var applicationVersion = new ApplicationVersion
        {
            CurrentVersion = GetCurrentFileVersion()
        };

        JsonWriter.WriteJson(applicationVersion, _applicationVersionFilePath);

        return applicationVersion;
    }

    private static void CreateDirectories()
    {
        Directory.CreateDirectory(
            $"{Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)}\\PixaiAutoClaimer\\Logs");
    }

    private string GetCurrentFileVersion()
    {
        return Assembly.GetExecutingAssembly().GetName().Version.ToString();
    }

    public bool IsUpdateAvailable()
    {
        return GetApplicationVersion().CurrentVersion != _githubApi.GetLatestVersion();
    }

    public bool DoesApplicationDirectoryExist()
    {
        return Directory.Exists(Path.GetDirectoryName(Application.ExecutablePath) + "\\bin");
    }

    public async Task<Stream> DownloadUpdate()
    {
        return await _httpClient.GetStreamAsync(_githubApi.GetLatestReleaseDownloadUrl());
    }

    public async Task InstallUpdate(Stream file)
    {
        var tempPath = Path.GetTempPath() + "PixaiBotUpdate.zip";

        await using var fileStream = File.Create(tempPath);


        await file.CopyToAsync(fileStream);

        var executableDirectory = Path.GetDirectoryName(Application.ExecutablePath);

        fileStream.Close();


        var binFolderPath = executableDirectory + "\\bin";

        if (!Directory.Exists(binFolderPath)) Directory.CreateDirectory(binFolderPath);

        ZipFile.ExtractToDirectory(tempPath, binFolderPath, true);

        CallApplication();

        WriteNewVersion();
    }

    public void CloseApplication()
    {
        Application.Current.Dispatcher.Invoke(() => { Application.Current.Shutdown(0); });
    }

    private void WriteNewVersion()
    {
        var applicationVersion = GetApplicationVersion();
        applicationVersion.CurrentVersion = _githubApi.GetLatestVersion();
        JsonWriter.WriteJson(applicationVersion, _applicationVersionFilePath);
    }


    public void CallApplication()
    {
        Process.Start($"{Path.GetDirectoryName(Application.ExecutablePath)}\\bin\\PixaiBot.exe");
    }
}
using System.Configuration;
using Infrastructure.Extensions;

namespace Infrastructure.Utils;

public static class GeneralUtils
{
    public static string GetAppSettings(string settingsName)
        =>
        ConfigurationManager.AppSettings[settingsName]
        .ToAbsString();

    public static int GetAppSettingsInt(string settingsName)
        =>
        GetAppSettings(settingsName)
        .ToInt();

    public static string[] GetAppSettingsArr(string settingsName)
        =>
        GetAppSettings(settingsName)
        .Split(';');
}
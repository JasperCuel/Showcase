using Cue.Core;
using System.Linq;
using UnityEngine;

/// <summary>
/// A wrapper for the default <see cref="Debug"/> which allows categorizing configurable in Assets/Resources/<see cref="DebugLogConfig"/>
/// </summary>
public static class DebugLogger
{
    private static DebugLogConfig _config;

    private static DebugLogConfig Config
    {
        get
        {
            if (_config != null)
                return _config;

            Resources.LoadAll<DebugLogConfig>("");
            Object[] configs = Resources.FindObjectsOfTypeAll(typeof(DebugLogConfig));
            if (configs.Length > 0)
                _config = configs[0] as DebugLogConfig;
            return _config;
        }
    }

    /// <summary>
    /// Logs an error into the Unity Console
    /// </summary>
    /// <param name="message">Message to display in the error</param>
    /// <param name="type">Type category</param>
    /// <param name="context">Optional Object to reference to</param>
    public static void LogError(object message, DebugType type = DebugType.Debugging, Object context = null)
    {
        DebugLogSetting setting = GetSettingByType(type);
        if (setting == null)
        {
            Debug.LogError(message, context);
            return;
        }

        Color color = setting.color;
        if (context == null)
            Debug.LogError(string.Format("<color=#{0:X2}{1:X2}{2:X2}>[{3}]</color> {4}", (byte)(color.r * 255f), (byte)(color.g * 255f), (byte)(color.b * 255f), type.ToString(), message));
        else
            Debug.LogError(string.Format("<color=#{0:X2}{1:X2}{2:X2}>[{3}]</color> {4}", (byte)(color.r * 255f), (byte)(color.g * 255f), (byte)(color.b * 255f), type.ToString(), message), context);
    }

    /// <summary>
    /// Logs a warning into the Unity Console
    /// </summary>
    /// <param name="message">Message to display in the warning</param>
    /// <param name="type">Type category</param>
    /// <param name="context">Optional Object to reference to</param>
    public static void LogWarning(object message, DebugType type = DebugType.Debugging, Object context = null)
    {
        DebugLogSetting setting = GetSettingByType(type);
        if (setting == null)
        {
            Debug.LogError(message, context);
            return;
        }

        Color color = setting.color;
        if (context == null)
            Debug.LogWarning(string.Format("<color=#{0:X2}{1:X2}{2:X2}>[{3}]</color> {4}", (byte)(color.r * 255f), (byte)(color.g * 255f), (byte)(color.b * 255f), type.ToString(), message));
        else
            Debug.LogWarning(string.Format("<color=#{0:X2}{1:X2}{2:X2}>[{3}]</color> {4}", (byte)(color.r * 255f), (byte)(color.g * 255f), (byte)(color.b * 255f), type.ToString(), message), context);
    }

    /// <summary>
    /// Logs a message into the Unity Console
    /// </summary>
    /// <param name="message">Message to display in the log</param>
    /// <param name="type">Type category</param>
    public static void Log(object message, DebugType type = DebugType.Debugging)
    {
        DebugLogSetting setting = GetSettingByType(type);
        if (setting == null || !setting.showInConsole)
            return;

        Color color = setting.color;
        Debug.Log(string.Format("<color=#{0:X2}{1:X2}{2:X2}>[{3}]</color> {4}", (byte)(color.r * 255f), (byte)(color.g * 255f), (byte)(color.b * 255f), type.ToString(), message));
    }

    private static DebugLogSetting GetSettingByType(DebugType type)
    {
        DebugLogSetting setting = Config.debugTypes.SingleOrDefault(x => x.Key.Equals(type)).Value;
        if (setting == null)
        {
            Debug.LogError($"[DEBUGLOGGER] No settings have been found for DebugType {type}");
            return null;
        }
        return setting;
    }
}
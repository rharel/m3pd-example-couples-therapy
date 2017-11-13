using UnityEngine;

/// <summary>
/// Utility class containing debugging methods that are active only in 
/// development builds.
/// </summary>
public static class DevDebug
{
    /// <summary>
    /// Logs a message.
    /// </summary>
    /// <param name="message">The message to log.</param>
    public static void Log(object message)
    {
        if (Debug.isDebugBuild) { Debug.Log(message); }
    }
}

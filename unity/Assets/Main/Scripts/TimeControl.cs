/// <summary>
/// This class represents the time controls of the virtual scene.
/// </summary>
public static class TimeControl
{
    /// <summary>
    /// Indicates whether the scene is paused.
    /// </summary>
    public static bool IsPaused { get; private set; }

    /// <summary>
    /// Pauses the scene.
    /// </summary>
    public static void Pause()
    {
        IsPaused = true;
    }
    /// <summary>
    /// Resumes the scene.
    /// </summary>
    public static void Resume()
    {
        IsPaused = false;
    }
}

using rharel.M3PD.CouplesTherapyExample.Time;

/// <summary>
/// An implementation of the <see cref="Clock"/> interface for unity's
/// <see cref="UnityEngine.Time"/>.  
/// </summary>
public sealed class UnityClock: Clock
{
    /// <summary>
    /// Gets the singleton instance.
    /// </summary>
    public static UnityClock Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new UnityClock();
            }
            return _instance;
        }
    }
    private static UnityClock _instance;

    /// <summary>
    /// Prevents a default instance of the <see cref="UnityClock"/> class from 
    /// being created.
    /// </summary>
    private UnityClock() { }

    /// <summary>
    /// Gets the time.
    /// </summary>
    public float Time
    {
        get { return UnityEngine.Time.time; }
    }
    /// <summary>
    /// Gets the time increment.
    /// </summary>
    public float TimeIncrement
    {
        get { return UnityEngine.Time.deltaTime; }
    }

    /// <summary>
    /// Returns a string that represents this instance.
    /// </summary>
    /// <returns>
    /// Human-readable string.
    /// </returns>
    public override string ToString()
    {
        return string.Format(
            "UnityClock(Time = {0}, TimeIncrement = {1})",
            Time, TimeIncrement
        );
    }
}

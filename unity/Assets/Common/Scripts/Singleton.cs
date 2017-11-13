using UnityEngine;

public abstract class Singleton<T>: MonoBehaviour
    where T: Singleton<T>
{
    /// <summary>
    /// Gets the singleton instance.
    /// </summary>
    public static T Instance { get; private set; }

    /// <summary>
    /// Initializes the singleton instance or self-destructs this game object
    /// if the singleton is already assigned a value.
    /// </summary>
    protected void InitializeSingleton(bool do_persist = false)
    {
        if (Instance == null) { Instance = (T)this; }
        else if (!ReferenceEquals(Instance, this)) { Destroy(gameObject); }

        if (do_persist) { DontDestroyOnLoad(gameObject); }
    }
}
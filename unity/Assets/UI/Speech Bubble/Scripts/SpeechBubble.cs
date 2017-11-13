using rharel.Debug;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// This behavior animates a floating speech bubble UI-element, representing
/// a character's speech through text.
/// </summary>
[RequireComponent(typeof(ScrollRect))]
public sealed class SpeechBubble: MonoBehaviour
{
    /// <summary>
    /// Clears the display.
    /// </summary>
    public void ClearDisplay()
    {
        _typewriter.ClearText();
        _typewriter.ClearQueue();
    }
    /// <summary>
    /// Adds the specified text to the display.
    /// </summary>
    /// <param name="text">The text to display.</param>
    /// <remarks>
    /// <paramref name="text"/> must not be empty.
    /// </remarks>
    public void AddToDisplay(string text)
    {
        Require.IsNotEmpty(text);

        _typewriter.Queue(text);
    }

    void OnValidate()
    {
        if (GetComponentInChildren<Typewriter>() == null)
        {
            Debug.LogWarning("Component 'Typewriter' is missing in children.");
        }
    }

    void Awake()
    {
        _scroll_view = GetComponent<ScrollRect>();
        _typewriter = GetComponentInChildren<Typewriter>();

        Require.IsNotNull(_typewriter);

        _typewriter.DoneTyping += OnTypewriterDoneTyping;
    }

    void Update()
    {
        if (!_typewriter.IsDone) { ScrollToBottom(); }
    }

    /// <summary>
    /// Scrolls the viewport to the bottom.
    /// </summary>
    private void ScrollToBottom()
    {
        _scroll_view.verticalScrollbar.value = 0.0f;
    }

    /// <summary>
    /// Called when the typewriter is done typing.
    /// </summary>
    /// <param name="sender">The event's sender.</param>
    private void OnTypewriterDoneTyping(Typewriter sender)
    {
        ScrollToBottom();
    }

    private ScrollRect _scroll_view;
    private Typewriter _typewriter;
}

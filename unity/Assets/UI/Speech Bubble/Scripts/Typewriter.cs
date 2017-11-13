using rharel.Debug;
using rharel.M3PD.Common.Delegates;
using rharel.M3PD.CouplesTherapyExample.Time;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// This behavior animates text as if it were typed one character at a time.
/// </summary>
[RequireComponent(typeof(Text))]
public sealed class Typewriter: MonoBehaviour
{
    /// <summary>
    /// Occurs after the last text in the queue has been typed.
    /// </summary>
    public event EventHandler<Typewriter> DoneTyping = delegate { };

    /// <summary>
    /// Interval between keystrokes (in seconds).
    /// </summary>
    public float keystrokeInterval = 0.5f;  // in seconds

    /// <summary>
    /// Enables/disables caret.
    /// </summary>
    public bool caretEnabled = true;
    /// <summary>
    /// Character representing the caret.
    /// </summary>
    public char caretCharacter = '\u2588';  // full block: █

    /// <summary>
    /// Gets the text that was already typed (without caret).
    /// </summary>
    public string WrittenText { get; private set; }
    /// <summary>
    /// Gets the text being displayed (includes caret, if enabled).
    /// </summary>
    public string DisplayedText
    {
        get { return _text.text; }
        private set { _text.text = value; }
    }

    /// <summary>
    /// Indicates whether there is text to type left in the queue.
    /// </summary>
    public bool IsDone
    {
        get { return _pending_text.Count == 0; }
    }

    /// <summary>
    /// Clears the text.
    /// </summary>
    public void ClearText()
    {
        WrittenText = string.Empty;
        UpdateDisplay();
    }

    /// <summary>
    /// Clears the pending text queue.
    /// </summary>
    public void ClearQueue()
    {
        _pending_text.Clear();
    }
    /// <summary>
    /// Queues the specified string for typing.
    /// </summary>
    /// <param name="text">The string to type.</param>
    /// <remarks>
    /// <paramref name="text"/> must not be empty.
    /// </remarks>
    public void Queue(string text)
    {
        Require.IsNotEmpty(text);

        foreach (char character in text)
        {
            _pending_text.Enqueue(character);
        }

        if (!_keystroke_timer.IsTicking)
        {
            _keystroke_timer.Start();
        }
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
            "Typewriter(WrittenText = '{0}')",
            WrittenText
        );
    }

    void OnValidate()
    {
        if (keystrokeInterval < 0) { keystrokeInterval = 0; }
    }

    void Awake()
    {
        _text = GetComponent<Text>();
        _keystroke_timer = new Timer(
            UnityClock.Instance, 
            duration: keystrokeInterval
        );
    }
    
    void Start()
    {
        Queue(DisplayedText);
        ClearText();
    }

    void Update()
    {
        if (!IsDone && 
            _keystroke_timer.Update())
        {
            TypeNext();

            _keystroke_timer.Reset();
            if (!IsDone) { _keystroke_timer.Start(); }
            else { OnDoneTyping(); }
        }
    }

    /// <summary>
    /// Types the next word in the queue.
    /// </summary>
    private void TypeNext()
    {
        char next_token = _pending_text.Dequeue();
        Type(next_token);
    }
    /// <summary>
    /// Types the specified token.
    /// </summary>
    /// <param name="token">The token to type.</param>
    private void Type(char token)
    {
        WrittenText += token;
        UpdateDisplay();
    }

    /// <summary>
    /// Updates the display.
    /// </summary>
    private void UpdateDisplay()
    {
        if (caretEnabled)
        {
            DisplayedText = WrittenText + caretCharacter;
        }
        else
        {
            DisplayedText = WrittenText;
        }
    }

    /// <summary>
    /// Raises the <see cref="DoneTyping"/> event. 
    /// </summary>
    private void OnDoneTyping()
    {
        DoneTyping(this);
    }

    private Text _text;

    private Timer _keystroke_timer;

    private Queue<char> _pending_text = new Queue<char>();
}

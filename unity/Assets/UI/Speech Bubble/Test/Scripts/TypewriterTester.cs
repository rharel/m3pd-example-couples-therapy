using UnityEngine;

[RequireComponent(typeof(Typewriter))]
public sealed class TypewriterTester: MonoBehaviour
{
    /// <summary>
    /// The number of debug-text sentences to output.
    /// </summary>
    public int sentenceCount = 3;
    /// <summary>
    /// Determines whether the animation will loop.
    /// </summary>
    public bool doLoop = false;

    void OnValidate()
    {
        if (sentenceCount < 0) { sentenceCount = 0; }
    }

    void Awake()
    {
        _typewriter = GetComponent<Typewriter>();
    }

    void Start()
    {
        _typewriter.Queue(LoremIpsum.Sentences(sentenceCount));
    }

    void Update()
    {
        if (_typewriter.IsDone && doLoop)
        {
            _typewriter.ClearText();
            _typewriter.Queue(LoremIpsum.Sentences(sentenceCount));
        }
    }

    private Typewriter _typewriter;
}

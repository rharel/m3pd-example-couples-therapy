using rharel.M3PD.Agency.Dialogue_Moves;
using rharel.M3PD.CouplesTherapyExample.Communication;
using rharel.M3PD.CouplesTherapyExample.Scene;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// This behavior manages the virtual session.
/// </summary>
public sealed class Director: Singleton<Director>
{
    public string therapistName = "Charles";
    public string firstPatientName = "Alice";
    public string secondPatientName = "Bob";

    public SpeechBubble therapistBubble;
    public SpeechBubble firstPatientBubble;
    public SpeechBubble secondPatientBubble;

    /// <summary>
    /// Gets the therapist agent.
    /// </summary>
    public Therapist Therapist
    {
        get { return _session.Therapist; }
    }

    void Awake()
    {
        InitializeSingleton();

        _session = Session.Create(
            therapistName, 
            firstPatientName, secondPatientName
        );

        _speech_bubbles.Add(_session.Patients.First.ID, firstPatientBubble);
        _speech_bubbles.Add(_session.Patients.Second.ID, secondPatientBubble);
        _speech_bubbles.Add(_session.Therapist.ID, therapistBubble);

        foreach (var agent in _session.Agents)
        {
            _is_expecting_new_phrase.Add(agent.ID, true);
        }
    }

    void Start()
    {
        foreach (var bubble in _speech_bubbles.Values)
        {
            bubble.ClearDisplay();
        }
    }

    private void FixedUpdate()
    {
        if (TimeControl.IsPaused) { return; }

        _session.Step(Time.fixedDeltaTime);
        _new_perceptions_available = true;
    }
    void Update()
    {
        if (TimeControl.IsPaused) { return; }
        if (_session.HasEnded)
        {
            KeyboardInputHandler.Instance.IsEnabled = false;

            PauseMenu.Instance.Title = "THE END";
            PauseMenu.Instance.IsResumeButtonActive = false;

            TimeControl.Pause();

            StartCoroutine(OnScenarioEnd());
            
            return;
        }
        if (!_new_perceptions_available) { return; }

        var recent_speech = ( 
            _session.Channels.GetPackets<Speech>()
                    .Select(packet => packet.Payload)
        );
        foreach (var speech in recent_speech)
        {
            var speaker = speech.SpeakerID;
            SpeechBubble bubble = _speech_bubbles[speaker];
            
            if (_is_expecting_new_phrase[speaker])
            {
                bubble.ClearDisplay();
                bubble.AddToDisplay(speech.Text);
                
                // We will expect a new phrase once we observe the dialog move
                // event associated with the current one, but for now we have 
                // no such expectation:
                _is_expecting_new_phrase[speaker] = false;
            }
            else
            {
                bubble.AddToDisplay(" " + speech.Text);
            }
        }
        foreach (var packet in _session.Channels.GetPackets<DialogueMove>())
        {
            if (Debug.isDebugBuild)
            {
                string source = packet.SenderID;
                string move = packet.Payload.ToString();
                DevDebug.Log($"Dialogue event: {{ " +
                             $"source: {source}, " +
                             $"move: {move} }}");
            }
            _is_expecting_new_phrase[packet.SenderID] = true;
        }
        _new_perceptions_available = false;
    }

    private IEnumerator OnScenarioEnd()
    {
        yield return new WaitForSeconds(1);

        PauseMenu.Instance.Show();
    }

    private Session _session;
    private bool _new_perceptions_available = false;

    private readonly Dictionary<string, SpeechBubble> _speech_bubbles = (
        new Dictionary<string, SpeechBubble>()
    );
    private readonly Dictionary<string, bool> _is_expecting_new_phrase = (
        new Dictionary<string, bool>()
    );
}

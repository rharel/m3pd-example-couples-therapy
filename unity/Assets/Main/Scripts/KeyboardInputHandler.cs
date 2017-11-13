using rharel.Debug;
using rharel.M3PD.Agency.Dialogue_Moves;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// This behavior handles scene-wide input events.
/// </summary>
public sealed class KeyboardInputHandler: Singleton<KeyboardInputHandler>
{
    /// <summary>
    /// The key that toggles in and out of the move selection dialog.
    /// </summary>
    public string toggleMoveSelectionKey = "space";
    /// <summary>
    /// The key that toggles between scene pause/resume.
    /// </summary>
    public string togglePauseKey = "p";
    /// <summary>
    /// Indicates whether the scene should be initially paused.
    /// </summary>
    public bool doStartPaused = false;

    public ItemSelectionDialog moveSelectionDialog;
    public PauseMenu pauseMenu;

    /// <summary>
    /// Gets/sets whether the scene responds to keyboard input. 
    /// </summary>
    public bool IsEnabled { get; set; }

    void Awake()
    {
        InitializeSingleton();
        IsEnabled = true;

        moveSelectionDialog.ItemSelected += OnItemSelected;
    }
    void Start()
    {
        if (doStartPaused) { Pause(); }
        else { Resume(); }

    }
    void Update()
    {
        if (!IsEnabled) { return; }

        if (Input.GetKeyUp(togglePauseKey)) { TogglePause(); }
        else if (!pauseMenu.IsActive &&
                 Input.GetKeyUp(toggleMoveSelectionKey))
        {
            ToggleMoveSelection();
        }
    }

    private void TogglePause()
    {
        if (TimeControl.IsPaused) { Resume(); }
        else { Pause(); }
    }
    private void Pause()
    {
        TimeControl.Pause();
        pauseMenu.Show();
    }
    private void Resume()
    {
        pauseMenu.Hide();

        if (!moveSelectionDialog.IsActive) { TimeControl.Resume(); }
    }

    private void ToggleMoveSelection()
    {
        if (moveSelectionDialog.IsActive)
        {
            CancelMoveSelection();
        }
        else
        {
            ProposeMoveSelection();
        }
    }
    private void ProposeMoveSelection()
    {
        TimeControl.Pause();

        var suggested_moves = Director.Instance.Therapist.GetSuggestedMoves();
        var items = suggested_moves.Select(move =>
        {
            string label = move.GetAddressee().MapSomeOr(
                addressee => $"{move.Type} => {addressee}",
                move.Type
            );
            DevDebug.Log(label); // TODO: remove
            return new KeyValuePair<string, object>(label, move);
        });
        moveSelectionDialog.SetItems(items);
        moveSelectionDialog.Show();
    }
    private void CancelMoveSelection()
    {
        moveSelectionDialog.Hide();
        TimeControl.Resume();
    }

    private void OnItemSelected(
        ItemSelectionDialog _,
        KeyValuePair<string, object> item)
    {
        Require.IsTrue(item.Value is DialogueMove);

        var move = (DialogueMove) item.Value;

        Director.Instance.Therapist.SetTargetMove(move);
        CancelMoveSelection();

        DevDebug.Log("Selected move: " + item.Key);
    }
}

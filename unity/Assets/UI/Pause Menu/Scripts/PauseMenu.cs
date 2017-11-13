using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// This behavior manages the pause menu UI.
/// </summary>
public sealed class PauseMenu: Singleton<PauseMenu>
{
    public string defaultTitle = "PAUSED";
    public bool showResumeButton = true;

    public Text titleText;
    public Button resumeButton;
    public Button restartButton;
    public Button exitButton;

    /// <summary>
    /// Gets/sets the menu's title.
    /// </summary>
    public string Title
    {
        get { return titleText.text; }
        set { titleText.text = value; }
    }
    /// <summary>
    /// Gets/sets the resume button's activation status.
    /// </summary>
    public bool IsResumeButtonActive
    {
        get { return resumeButton.gameObject.activeSelf; }
        set { resumeButton.gameObject.SetActive(value); }
    }

    /// <summary>
    /// Indicates whether the menu is currently active.
    /// </summary>
    public bool IsActive { get { return gameObject.activeSelf; } }
    /// <summary>
    /// Activates the menu.
    /// </summary>
    public void Show() { gameObject.SetActive(true); }
    /// <summary>
    /// Deactivates the menu.
    /// </summary>
    public void Hide() { gameObject.SetActive(false); }

    void Awake()
    {
        InitializeSingleton();

        Title = defaultTitle;
        IsResumeButtonActive = showResumeButton;

        resumeButton.onClick.AddListener(OnResumeButtonClick);
        restartButton.onClick.AddListener(OnRestartButtonClick);
        exitButton.onClick.AddListener(OnExitButtonClick);
    }

    private void OnResumeButtonClick()
    {
        Hide();
        TimeControl.Resume();
    }
    private void OnRestartButtonClick()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    private void OnExitButtonClick()
    {
        Application.Quit();
    }
}

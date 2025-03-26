using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class PauseMenuUI : MonoBehaviour
{
    [SerializeField] GameObject pauseMenuUI; // PauseMenuUI object on Player
    [SerializeField] GameObject settingsMenuUI;
    private SettingsMenuUI settings;
    private Button resumeButton;
    private Button settingsButton;
    private Button exitButton;

    public PlayerInput playerInput;

    public bool isGamePaused = false;


    private void Start()
    {
        settings = GetComponent<SettingsMenuUI>(); 

        InitializeUIElements();
        SubscribeToEvents();

        // ensure Pause Menu is hidden by default
        UIHelper.HideUI(pauseMenuUI, "PauseMenu");
    }

    private void InitializeUIElements()
    {
        resumeButton = UIHelper.GetUIElement<Button>(pauseMenuUI, "ResumeButton");
        settingsButton = UIHelper.GetUIElement<Button>(pauseMenuUI, "SettingsButton");
        exitButton = UIHelper.GetUIElement<Button>(pauseMenuUI, "ExitButton");
    }

    private void SubscribeToEvents()
    {
        resumeButton.clicked += OnResumeButtonClick;
        settingsButton.clicked += OnSettingsButtonClick;
        exitButton.clicked += OnExitButtonClick;

        // When exiting settings menu, return to pause menu
        SettingsMenuUI.LeaveSettingsEvent += DisplayPauseMenu;
    }

    public void TogglePauseMenu()
    {
        if (!isGamePaused)
        {
            DisplayPauseMenu();
        }
        else
        {
            Resume();
        }
    }

    private void OnDisable()
    {
        resumeButton.clicked -= OnResumeButtonClick;
        settingsButton.clicked -= OnSettingsButtonClick;
        exitButton.clicked -= OnExitButtonClick;

        SettingsMenuUI.LeaveSettingsEvent -= DisplayPauseMenu;
    }


    // Displays Pause Menu UI
    public void DisplayPauseMenu()
    {
        if (pauseMenuUI != null)
        {
            UIHelper.ShowUI(pauseMenuUI, "PauseMenu");
            isGamePaused = true;
            UnityEngine.Cursor.lockState = CursorLockMode.None;
        }
        else
        {
            Debug.LogError("PauseMenuUI is null when trying to display it.");
        }
    }

    // Hides Pause Menu UI
    public void Resume()
    {
        UIHelper.HideUI(pauseMenuUI, "PauseMenu");
        settings.CloseSettingsMenu();
        isGamePaused = false;
        UnityEngine.Cursor.lockState = CursorLockMode.Locked;
    }

    private void OnSettingsButtonClick()
    {
        settings.OpenSettingsMenu();
        UIHelper.HideUI(pauseMenuUI, "PauseMenu");
    }

    private void OnResumeButtonClick()
    {
        Resume();
    }

    // Exits the application entirely
    private void OnExitButtonClick()
    {
        // for now, should only exit to application 
        Application.Quit(); // note this command has no effect inside the editor
        Debug.Log("Pressed Exit Button!");
    }
}
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class PauseMenuUI : MonoBehaviour
{
    [SerializeField] GameObject pauseMenu; // PauseMenuUI object on Player
    private UIDocument pauseMenuUIDocument; // UI Document component on PauseMenuUI
    private VisualElement pauseMenuContainer; // Container in Visual Tree Asset
    private SettingsMenuUI settings;
    private Button resumeButton;
    private Button settingsButton;
    private Button exitButton;

    public PlayerInput playerInput;
    private InputAction pauseAction;

    public static bool isGamePaused = false;


    private void Awake()
    {        
        settings = GetComponent<SettingsMenuUI>();

        // initialize pause menu UI elements
        pauseMenuUIDocument = pauseMenu.GetComponent<UIDocument>();
        pauseMenuContainer = pauseMenuUIDocument.rootVisualElement.Q("Container");
        resumeButton = pauseMenuUIDocument.rootVisualElement.Q<Button>("ResumeButton");
        settingsButton = pauseMenuUIDocument.rootVisualElement.Q<Button>("SettingsButton");
        exitButton = pauseMenuUIDocument.rootVisualElement.Q<Button>("ExitButton");

        // ensure Pause Menu is hidden by default
        pauseMenuContainer.style.visibility = Visibility.Hidden; // must not setActive(false), this will break the UI

        // Register a callback on a pointer down event
        resumeButton.RegisterCallback<ClickEvent>(OnResumeButtonClick);
        settingsButton.RegisterCallback<ClickEvent>(OnSettingsButtonClick);
        exitButton.RegisterCallback<ClickEvent>(OnExitButtonClick);
    }

    private void OnSettingsButtonClick(ClickEvent evt)
    {
        Debug.Log("Pressed settings button!");

        settings.OpenSettingsMenu();
    }

    private void OnResumeButtonClick(ClickEvent evt) 
    {
        Debug.Log("Pressed resume button!");
        TogglePauseMenu();
    }

    // Exits the application entirely
    private void OnExitButtonClick(ClickEvent evt)
    {
        // for now, should only exit to application 
        Application.Quit(); // note this command has no effect inside the editor
        Debug.Log("Pressed Exit Button!");
    }

    public void TogglePauseMenu()
    {
        if (!isGamePaused)
        {
            Pause();
        }
        else
        {            
            Resume();
        }
    }

    

    // Displays Pause Menu UI
    public void Pause()
    {
        Debug.Log("opened pause menu!");
        pauseMenuContainer.style.visibility = Visibility.Visible;
        isGamePaused = true;
        UnityEngine.Cursor.lockState = CursorLockMode.None;
    }

    // Hides Pause Menu UI
    public void Resume()
    {
        Debug.Log("closed pause menu!");
        pauseMenuContainer.style.visibility = Visibility.Hidden;
        settings.CloseSettingsMenu();
        isGamePaused = false;
        UnityEngine.Cursor.lockState = CursorLockMode.Locked;
    }
}

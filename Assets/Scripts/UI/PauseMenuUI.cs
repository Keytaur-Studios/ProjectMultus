using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class PauseMenuUI : MonoBehaviour
{
    [SerializeField] GameObject pauseMenuGameObject; // PauseMenuUI object on Player
    private UIDocument pauseMenuUIDocument; // UI Document component on PauseMenuUI
    private VisualElement pauseMenuContainer; // Container in Visual Tree Asset
    public PlayerInput playerInput;
    private InputAction pauseAction;
    
    private Button resumeButton;
    private Button settingsButton;
    private Button exitButton;

    public static bool isGamePaused = false;


    private void Awake()
    {
        Debug.Log("entered pausemenuui");

        // initialize UI elements
        pauseMenuUIDocument = pauseMenuGameObject.GetComponent<UIDocument>();
        resumeButton = pauseMenuUIDocument.rootVisualElement.Q<Button>("ResumeButton");
        settingsButton = pauseMenuUIDocument.rootVisualElement.Q<Button>("SettingsButton");
        exitButton = pauseMenuUIDocument.rootVisualElement.Q<Button>("ExitButton");

        if (pauseMenuUIDocument == null)
            Debug.Log("PauseMenuUIDocument is null");
        else Debug.Log("PauseMenuUIDocument is NOT null");

        // get pause menu container inside UI document
        pauseMenuContainer = pauseMenuUIDocument.rootVisualElement.Q("Container");

        if (pauseMenuContainer == null)
            Debug.Log("PauseMenuContainer is null");
        else Debug.Log("PauseMenuContainer is NOT null");

        // ensure Pause Menu is hidden by default
        pauseMenuContainer.style.visibility = Visibility.Hidden; // must not setActive(false), this will break the UI

        // Register a callback on a pointer down event
        resumeButton.RegisterCallback<ClickEvent>(OnResumeButtonClick);
    }

    

    private void OnResumeButtonClick(ClickEvent evt) 
    {
        Debug.Log("Pressed resume button!");
        TogglePauseMenu();
    }

    public void TogglePauseMenu()
    {
        if (!isGamePaused)
        {
            // display pause menu
            OpenPauseMenu();
            isGamePaused = true;
            UnityEngine.Cursor.lockState = CursorLockMode.None;
        }
        else
        {
            // hides pause menu
            ClosePauseMenu();
            isGamePaused = false;
            UnityEngine.Cursor.lockState = CursorLockMode.Locked;
        }
    }

    

    // Displays Pause Menu UI
    public void OpenPauseMenu()
    {
        Debug.Log("opened pause menu!");
        pauseMenuContainer.style.visibility = Visibility.Visible;
    }

    // Hides Pause Menu UI
    public void ClosePauseMenu()
    {
        Debug.Log("closed pause menu!");
        pauseMenuContainer.style.visibility = Visibility.Hidden;
    }
}

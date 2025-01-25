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

    public static bool isGamePaused = false;

    private void Awake()
    {
        Debug.Log("entered pausemenuui");

        // get pause menu UI document
        pauseMenuUIDocument = pauseMenuGameObject.GetComponent<UIDocument>();

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


    }


    public void TogglePauseMenu()
    {
        // if game is not paused
        if (!isGamePaused)
        {
            // freeze player movement and unlock cursor
            DisableMovement();
            // display pause menu
            OpenPauseMenu();
            isGamePaused = true;
        }
        else
        {
            // else if game is already paused
            ClosePauseMenu();
            EnableMovement();
            isGamePaused = false;
            // if player is not in pause menu

            // go back to pause menu

            // else player is in pause menu

            // hide pause menu and resume game
        }
    }

    // Stops player movement and unlocks cursor
    public void DisableMovement()
    {
        Debug.Log("disabling movement");
        UnityEngine.Cursor.lockState = CursorLockMode.None;

        // disable player movement

    }

    // Resumes player movement and locks cursor
    public void EnableMovement()
    {
        Debug.Log("enabling movement");
        UnityEngine.Cursor.lockState = CursorLockMode.Locked;

        // enable player movement

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

using UnityEngine;
using UnityEngine.UIElements;

public class SettingsMenuUI : MonoBehaviour
{

    [SerializeField] GameObject settingsMenu; // PauseMenuUI object on Player
    private UIDocument settingsMenuUIDocument; // UI Document component on PauseMenuUI
    private VisualElement settingsMenuContainer; // Container in Visual Tree Asset

    private Slider mouseSensitivity;
    private Slider masterVolume;
    private Slider musicVolume;
    private Slider sfxVolume;
    private Toggle fullscreen;
    private DropdownMenu resolution;
    private Button applyButton;
    private Button backButton;

    private void Awake()
    {
        Debug.Log("SettingsMenuUI is awake");
        settingsMenuUIDocument = settingsMenu.GetComponent<UIDocument>();
        settingsMenuContainer = settingsMenuUIDocument.rootVisualElement.Q("Container");
        backButton = settingsMenuContainer.Q<Button>("BackButton");

        backButton.RegisterCallback<ClickEvent>(OnBackButtonClick);

        // ensure Settings Menu is hidden by default
        settingsMenuContainer.style.visibility = Visibility.Hidden; // must not setActive(false), this will break the UI
    }

    public void OnBackButtonClick(ClickEvent evt)
    {
        CloseSettingsMenu();
    }

    public void OpenSettingsMenu()
    {
        Debug.Log("open settings menu!");
        settingsMenuContainer.style.visibility = Visibility.Visible;
    }

    public void CloseSettingsMenu() 
    {
        Debug.Log("SettingsMenuUI is awake");
        settingsMenuContainer.style.visibility = Visibility.Hidden;
    }
}

using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public class SettingsMenuUI : MonoBehaviour
{
    [SerializeField] GameObject settingsMenu; // SettingsMenuUI object on Player
    private UIDocument settingsMenuUIDocument; // UI Document component on SettingsMenuUI
    private VisualElement settingsMenuContainer; // Container in Visual Tree Asset
    private PlayerMotor motor; 

    // Settings controls
    private Slider lookSensitivitySlider;
    private Slider masterVolumeSlider;
    private Slider musicVolumeSlider;
    private Slider sfxVolumeSlider;
    private Toggle fullscreenToggle;
    private DropdownField resolutionDropdown;
    private DropdownField qualityDropdown;
    private Button applyButton;
    private Button backButton;
    private Button resetButton;

    private void Awake()
    {
        motor = GetComponent<PlayerMotor>();

        // initialize all variables
        settingsMenuUIDocument = settingsMenu.GetComponent<UIDocument>();
        settingsMenuContainer = settingsMenuUIDocument.rootVisualElement.Q("Container");
        lookSensitivitySlider = settingsMenuContainer.Q<Slider>("LookSensitivitySlider");
        masterVolumeSlider = settingsMenuContainer.Q<Slider>("MasterVolumeSlider");
        musicVolumeSlider = settingsMenuContainer.Q<Slider>("MusicVolumeSlider");
        sfxVolumeSlider = settingsMenuContainer.Q<Slider>("SFXVolumeSlider");
        fullscreenToggle = settingsMenuContainer.Q<Toggle>("FullscreenToggle");
        resolutionDropdown = settingsMenuContainer.Q<DropdownField>("ResolutionDropdown");
        qualityDropdown = settingsMenuContainer.Q<DropdownField>("GraphicsQualityDropdown");
        applyButton = settingsMenuContainer.Q<Button>("ApplyButton");
        backButton = settingsMenuContainer.Q<Button>("BackButton");
        resetButton = settingsMenuContainer.Q<Button>("ResetButton");

        // subscribe to events
        backButton.clicked += OnBack;
        applyButton.clicked += OnApply;
        

        // initialize graphics settings
        InitDisplayResolutions();
        InitQualitySettings();

        // ensure Settings Menu is hidden by default
        settingsMenuContainer.style.visibility = Visibility.Hidden; // must not setActive(false), this can break the UI
    }

    private void OnBack()
    {
        CloseSettingsMenu();
    }


    // Applies all configured settings
    private void OnApply()
    {
        // set resolution
        var newResolution = Screen.resolutions[resolutionDropdown.index];
        Screen.SetResolution(newResolution.width, newResolution.height, true);
        
        // set graphics quality
        QualitySettings.SetQualityLevel(qualityDropdown.index, true);

        // set fullscreen mode
        ToggleFullscreen();

        // set look sensitivity
        setLookSensitivity();
        

    }

    private void onReset()
    {

    }

    private void setLookSensitivity()
    {
        motor.xSensitivity = lookSensitivitySlider.value;
        motor.ySensitivity = lookSensitivitySlider.value;
    }

    // Applies the current fullscreen setting
    private void ToggleFullscreen()
    {
        if (fullscreenToggle.value == true)
        {
            Screen.fullScreenMode = FullScreenMode.ExclusiveFullScreen;
        }
        else
        {
            Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
        }
    }

    private void InitDisplayResolutions()
    {
        // initialize the resolution drop down menu options
        resolutionDropdown.choices = Screen.resolutions.Select(resolution => $"{resolution.width}x{resolution.height}").ToList();
        // display the player's current resolution as the selected value
        resolutionDropdown.index = Screen.resolutions
                .Select((resolution, index) => (resolution, index))
                .First((value) => value.resolution.width == Screen.currentResolution.width && value.resolution.height == Screen.currentResolution.height) 
                .index; 
    }

    private void InitQualitySettings()
    {
        // initialize the graphics dropdown options with UnityEngine presets (Performant, Balanced, High Fidelity) 
        qualityDropdown.choices = QualitySettings.names.ToList();
        // display the player's current quality level as the selected value
        qualityDropdown.index = QualitySettings.GetQualityLevel();
    }

    public void OpenSettingsMenu()
    {
        settingsMenuContainer.style.visibility = Visibility.Visible;
    }

    public void CloseSettingsMenu() 
    {
        settingsMenuContainer.style.visibility = Visibility.Hidden;
    }
}

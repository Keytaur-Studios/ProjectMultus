using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public class SettingsMenuUI : MonoBehaviour
{
    [SerializeField] GameObject settingsMenu; // SettingsMenuUI object on Player
    private UIDocument settingsMenuUIDocument; // UI Document component on SettingsMenuUI
    private VisualElement settingsMenuContainer; // Container in Visual Tree Asset
    private PlayerMotor motor; // For mouse sensitivity

    // Settings controls
    private SliderInt mouseSensitivitySlider;
    private Slider masterVolumeSlider;
    private Slider musicVolumeSlider;
    private Slider sfxVolumeSlider;
    private Toggle fullscreenToggle;
    private DropdownField resolutionDropdown;
    private DropdownField graphicsQualityDropdown;
    private Button applyButton;
    private Button backButton;
    private Button resetButton;

    // Most recently applied values
    private int savedMouseSens;
    private float savedMasterVol;
    private float savedMusicVol;
    private float savedSfxVolume;
    private bool savedFullscreen;
    private int savedResolution;
    private int savedGraphicsQuality;

    // Default graphics (set when player loads into scene, used to reset changes back to default)
    private int graphicsQualityDefault;
    private int resolutionDefault;

    private bool isApplied;

    private void Awake()
    {
        motor = GetComponent<PlayerMotor>();

        // Initialize all variables
        settingsMenuUIDocument = settingsMenu.GetComponent<UIDocument>();
        settingsMenuContainer = settingsMenuUIDocument.rootVisualElement.Q("SettingsMenu");
        mouseSensitivitySlider = settingsMenuContainer.Q<SliderInt>("MouseSensitivitySlider");
        masterVolumeSlider = settingsMenuContainer.Q<Slider>("MasterVolumeSlider");
        musicVolumeSlider = settingsMenuContainer.Q<Slider>("MusicVolumeSlider");
        sfxVolumeSlider = settingsMenuContainer.Q<Slider>("SFXVolumeSlider");
        fullscreenToggle = settingsMenuContainer.Q<Toggle>("FullscreenToggle");
        resolutionDropdown = settingsMenuContainer.Q<DropdownField>("ResolutionDropdown");
        graphicsQualityDropdown = settingsMenuContainer.Q<DropdownField>("GraphicsQualityDropdown");
        applyButton = settingsMenuContainer.Q<Button>("ApplyButton");
        backButton = settingsMenuContainer.Q<Button>("BackButton");
        resetButton = settingsMenuContainer.Q<Button>("ResetButton");

        // subscribe to events
        backButton.clicked += OnBack;
        applyButton.clicked += OnApply;
        resetButton.clicked += OnReset;
        

        // initialize graphics settings
        InitDisplayResolutions();
        InitQualitySettings();

        // Initialize saved settings values
        SaveSettings();

        // ensure Settings Menu is hidden by default
        settingsMenuContainer.style.visibility = Visibility.Hidden; // must not setActive(false), this can break the UI
    }

    private void OnBack()
    {
        CloseSettingsMenu();
    }


    // Applies all changes
    private void OnApply()
    {
        // apply resolution
        var newResolution = Screen.resolutions[resolutionDropdown.index];
        Screen.SetResolution(newResolution.width, newResolution.height, true);

        // apply graphics quality
        QualitySettings.SetQualityLevel(graphicsQualityDropdown.index, true);

        ToggleFullscreen();
        SetLookSensitivity();
        SaveSettings();

    }

    // Resets all settings to default states
    private void OnReset()
    {
        mouseSensitivitySlider.value = 30; // 30f is the value initialized in PlayerMotor.cs 
        masterVolumeSlider.value = 100; // full volumes are default
        sfxVolumeSlider.value = 100;
        musicVolumeSlider.value = 100;
        fullscreenToggle.value = false;
        resolutionDropdown.index = resolutionDefault;
        graphicsQualityDropdown.index = graphicsQualityDefault; 
    }

    private void SetLookSensitivity()
    {
        motor.xSensitivity = mouseSensitivitySlider.value;
        motor.ySensitivity = mouseSensitivitySlider.value;
    }

    // Applies the saved fullscreen setting
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
        // display the player's default resolution as the selected value
        resolutionDropdown.index = Screen.resolutions
                .Select((resolution, index) => (resolution, index))
                .First((value) => value.resolution.width == Screen.currentResolution.width && value.resolution.height == Screen.currentResolution.height) 
                .index; 
        // save this as the default setting
        resolutionDefault = resolutionDropdown.index;
        
    }

    private void InitQualitySettings()
    {
        // initialize the graphics dropdown options with UnityEngine presets (Performant, Balanced, High Fidelity) 
        graphicsQualityDropdown.choices = QualitySettings.names.ToList();
        // display the player's saved quality level as the selected value
        graphicsQualityDropdown.index = QualitySettings.GetQualityLevel();
        // save this as the default setting
        graphicsQualityDefault = graphicsQualityDropdown.index;
    }

    public void OpenSettingsMenu()
    {
        settingsMenuContainer.style.visibility = Visibility.Visible;
    }

    public void CloseSettingsMenu() 
    {
        settingsMenuContainer.style.visibility = Visibility.Hidden;

        if (!IsChangesApplied())
        {
            DiscardChanges();
        }

    }

    // Reverts changes to most recently saved settings
    private void DiscardChanges()
    {
        mouseSensitivitySlider.value = savedMouseSens;
        masterVolumeSlider.value = savedMasterVol;
        musicVolumeSlider.value = savedMusicVol;
        sfxVolumeSlider.value = savedSfxVolume;
        fullscreenToggle.value = savedFullscreen;

        resolutionDropdown.index = savedResolution;
        var oldResolution = Screen.resolutions[resolutionDropdown.index];
        Screen.SetResolution(oldResolution.width, oldResolution.height, true);

        graphicsQualityDropdown.index = savedGraphicsQuality;
        QualitySettings.SetQualityLevel(graphicsQualityDropdown.index, true);


    }

    // Saves all settings values
    private void SaveSettings()
    {
        savedMouseSens = mouseSensitivitySlider.value;
        savedMasterVol = masterVolumeSlider.value;
        savedMusicVol = musicVolumeSlider.value;
        savedSfxVolume = sfxVolumeSlider.value;
        savedFullscreen = fullscreenToggle.value;
        savedResolution = resolutionDropdown.index;
        savedGraphicsQuality = graphicsQualityDropdown.index;
    }

    // returns true if any settings have been modified without applying
    private bool IsChangesApplied()
    {
        if (savedMouseSens != mouseSensitivitySlider.value ||
            savedMasterVol != masterVolumeSlider.value ||
            savedMusicVol != musicVolumeSlider.value ||
            savedSfxVolume != sfxVolumeSlider.value ||
            savedFullscreen != fullscreenToggle.value ||
            savedResolution != resolutionDropdown.index ||
            savedGraphicsQuality != graphicsQualityDropdown.index)
            return false;
        else
            return true;
    }


    private void OnDisable()
    {
        // unsubscribe
        backButton.clicked -= OnBack;
        applyButton.clicked -= OnApply;
        resetButton.clicked -= OnReset;

    }
}

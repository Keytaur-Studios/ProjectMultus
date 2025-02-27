using System;
using System.Linq;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UIElements;


public class SettingsMenuUI : MonoBehaviour
{
    [SerializeField] GameObject settingsMenu; // SettingsMenuUI object on Player
    [SerializeField] AudioMixer audioMixer;
    private UIDocument settingsMenuUIDocument; // UI Document component on SettingsMenuUI
    private VisualElement settingsMenuContainer; // Container in Visual Tree Asset
    private PlayerLook look; // For mouse sensitivity

    // Tab containers
    private VisualElement graphicsTabContent, audioTabContent, controlsTabContent;
    private Button graphicsTabButton, audioTabButton, controlsTabButton;
    private VisualElement currentTab; // 1 Graphics 2 Audio 3 Controls


    // Settings controls
    private Slider mouseSensitivitySlider;
    private Slider masterVolumeSlider, musicVolumeSlider, sfxVolumeSlider;
    private Toggle fullscreenToggle;
    private DropdownField resolutionDropdown, graphicsQualityDropdown;
    private Button applyButton, backButton, resetButton;

    // Most recently applied values
    private float savedMouseSensitivity;
    private float savedMasterVolume;
    private float savedMusicVolume;
    private float savedSfxVolume;
    private bool savedFullscreen;
    private int savedResolution;
    private int savedGraphicsQuality;

    // Default graphics (set when player loads into scene, used to reset changes back to default)
    private int graphicsQualityDefault, resolutionDefault;

    private bool isApplied;

    private void Awake()
    {
        look = GetComponent<PlayerLook>();
        // audioMixer = GetComponent<AudioMixer>(); // no audiomixer for now

        Debug.Log("settings 0");

        InitUIElements();
        
        InitDisplayResolutions();
        InitQualitySettings();
        SaveSettings();

        Debug.Log("settings 1");

        backButton.clicked += OnBack;
        applyButton.clicked += OnApply;
        resetButton.clicked += OnReset;

        graphicsTabButton.clicked += openGraphicsTab;
        audioTabButton.clicked += openAudioTab;
        controlsTabButton.clicked += openControlsTab;
        
        // ensure Settings Menu is hidden by default
        settingsMenuContainer.style.visibility = Visibility.Hidden;

        // hide all tabs other than graphics (ensure only one tab content is showing at a time)
        currentTab = graphicsTabContent;
        audioTabContent.style.visibility = Visibility.Hidden;
        controlsTabContent.style.visibility = Visibility.Hidden;
        Debug.Log("settings 2");
    }

    private void OnDisable()
    {
        backButton.clicked -= OnBack;
        applyButton.clicked -= OnApply;
        resetButton.clicked -= OnReset;
        graphicsTabButton.clicked -= openGraphicsTab;
        audioTabButton.clicked -= openAudioTab;
        controlsTabButton.clicked -= openControlsTab;
    }

    private void InitUIElements()
    {
        settingsMenuUIDocument = settingsMenu.GetComponent<UIDocument>();
        settingsMenuContainer = settingsMenuUIDocument.rootVisualElement.Q<VisualElement>("SettingsMenu");

        graphicsTabButton = settingsMenuContainer.Q<Button>("GraphicsTabButton");
        audioTabButton = settingsMenuContainer.Q<Button>("AudioTabButton");
        controlsTabButton = settingsMenuContainer.Q<Button>("ControlsTabButton");

        graphicsTabContent = settingsMenuContainer.Q<VisualElement>("GraphicsTabContent");
        audioTabContent = settingsMenuContainer.Q<VisualElement>("AudioTabContent");
        controlsTabContent = settingsMenuContainer.Q<VisualElement>("ControlsTabContent");

        mouseSensitivitySlider = settingsMenuContainer.Q<Slider>("MouseSensitivitySlider");

        masterVolumeSlider = settingsMenuContainer.Q<Slider>("MasterVolumeSlider");
        musicVolumeSlider = settingsMenuContainer.Q<Slider>("MusicVolumeSlider");
        sfxVolumeSlider = settingsMenuContainer.Q<Slider>("SFXVolumeSlider");

        fullscreenToggle = settingsMenuContainer.Q<Toggle>("FullscreenToggle");
        resolutionDropdown = settingsMenuContainer.Q<DropdownField>("DisplayResolutionDropdown");
        graphicsQualityDropdown = settingsMenuContainer.Q<DropdownField>("GraphicsQualityDropdown");
        
        applyButton = settingsMenuContainer.Q<Button>("ApplyButton");
        backButton = settingsMenuContainer.Q<Button>("BackButton");
        resetButton = settingsMenuContainer.Q<Button>("ResetButton");

    }
    
    public void OpenSettingsMenu()
    {
        settingsMenuContainer.style.visibility = Visibility.Visible;
        graphicsTabContent.style.visibility = Visibility.Visible;
    }

    public void CloseSettingsMenu()
    {
        settingsMenuContainer.style.visibility = Visibility.Hidden;
        graphicsTabContent.style.visibility = Visibility.Hidden;
        audioTabContent.style.visibility = Visibility.Hidden;
        controlsTabContent.style.visibility = Visibility.Hidden;
        currentTab = graphicsTabContent; // so graphics tab will be displayed when opened again

        if (!IsChangesApplied())
        {
            DiscardChanges();
        }
        

    }

    private void OnBack()
    {
        CloseSettingsMenu();

    }

    private void OnApply()
    {
        QualitySettings.SetQualityLevel(graphicsQualityDropdown.index, true); // apply graphics quality
        ApplyResolution();
        ToggleFullscreen();
        SetLookSensitivity();
        SaveSettings();
    }

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

    private void switchTab(VisualElement newTab)
    {
        currentTab.style.visibility = Visibility.Hidden;
        newTab.style.visibility = Visibility.Visible;
        currentTab = newTab;
    }

    private void openGraphicsTab()
    {
        switchTab(graphicsTabContent);
    }

    private void openAudioTab()
    {
        switchTab(audioTabContent);
    }

    private void openControlsTab()
    {
        switchTab(controlsTabContent);
    }

    private void SetLookSensitivity()
    {
        look.xSensitivity = mouseSensitivitySlider.value;
        look.ySensitivity = mouseSensitivitySlider.value;
    }

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

    private void ApplyResolution()
    {
        var newResolution = Screen.resolutions[resolutionDropdown.index];
        Screen.SetResolution(newResolution.width, newResolution.height, true);
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

    // Reverts settings to "saved" settings
    private void DiscardChanges()
    {
        mouseSensitivitySlider.value = savedMouseSensitivity;
        masterVolumeSlider.value = savedMasterVolume;
        musicVolumeSlider.value = savedMusicVolume;
        sfxVolumeSlider.value = savedSfxVolume;
        fullscreenToggle.value = savedFullscreen;

        resolutionDropdown.index = savedResolution;
        ApplyResolution();

        graphicsQualityDropdown.index = savedGraphicsQuality;
        QualitySettings.SetQualityLevel(graphicsQualityDropdown.index, true);
    }

    // Saves all current settings values
    private void SaveSettings()
    {
        savedMouseSensitivity = mouseSensitivitySlider.value;
        savedMasterVolume = masterVolumeSlider.value;
        savedMusicVolume = musicVolumeSlider.value;
        savedSfxVolume = sfxVolumeSlider.value;
        savedFullscreen = fullscreenToggle.value;
        savedResolution = resolutionDropdown.index;
        savedGraphicsQuality = graphicsQualityDropdown.index;
    }

    // returns true if any settings have been modified without applying
    private bool IsChangesApplied()
    {
        if (savedMouseSensitivity != mouseSensitivitySlider.value ||
            savedMasterVolume != masterVolumeSlider.value ||
            savedMusicVolume != musicVolumeSlider.value ||
            savedSfxVolume != sfxVolumeSlider.value ||
            savedFullscreen != fullscreenToggle.value ||
            savedResolution != resolutionDropdown.index ||
            savedGraphicsQuality != graphicsQualityDropdown.index)
            return false;
        else
            return true;
    }
}

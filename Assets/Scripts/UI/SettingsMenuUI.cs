using System;
using System.Linq;
using Newtonsoft.Json.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UIElements;


public class SettingsMenuUI : MonoBehaviour
{
    public GameObject settingsMenuUI;

    [SerializeField] AudioMixer audioMixer;

    private PlayerLook look; // For mouse sensitivity

    // Tab elements
    private VisualElement graphicsTabContent, audioTabContent, controlsTabContent;
    private Button graphicsTabButton, audioTabButton, controlsTabButton;
    private VisualElement currentTabContent; 
    private VisualElement currentTabButton;


    // Settings controls
    private Slider mouseSensitivitySlider;
    private Slider masterVolumeSlider, musicVolumeSlider, sfxVolumeSlider;
    private Toggle fullscreenToggle;
    private DropdownField resolutionDropdown, graphicsQualityDropdown;
    private Button applyButton, backButton, resetButton;

    // Default graphics (set when player loads into scene, used to reset changes back to default)
    private int graphicsQualityDefault, resolutionDefault;

    public static event Action OnBackEvent;

    private void Start()
    {
        look = GetComponent<PlayerLook>(); // NOTE: this will be NULL if settings menu is accessed from main menu

        // audioMixer = GetComponent<AudioMixer>(); // no audiomixer for now

        InitUIElements();
        LoadSettings(); // Load settings
        InitDisplayResolutions(); 
        InitQualitySettings();

        ApplySettings(); // Apply loaded settings
        SubscribeToEvents();

        // ensure Settings Menu is hidden by default
        UIHelper.HideUI(settingsMenuUI, "SettingsMenu");
        UIHelper.HideUI(currentTabContent);
    }

    private void OnDisable()
    {
        UnsubscribeToEvents();
    }


    private void InitUIElements()
    {
        // Tab buttons
        graphicsTabButton = UIHelper.GetUIElement<Button>(settingsMenuUI, "GraphicsTabButton");
        audioTabButton = UIHelper.GetUIElement<Button>(settingsMenuUI, "AudioTabButton");
        controlsTabButton = UIHelper.GetUIElement<Button>(settingsMenuUI, "ControlsTabButton");

        // Tab Content Containers
        graphicsTabContent = UIHelper.GetUIElement<VisualElement>(settingsMenuUI, "GraphicsTabContent");
        audioTabContent = UIHelper.GetUIElement<VisualElement>(settingsMenuUI, "AudioTabContent");
        controlsTabContent = UIHelper.GetUIElement<VisualElement>(settingsMenuUI, "ControlsTabContent");

        // Audio Controls
        mouseSensitivitySlider = UIHelper.GetUIElement<Slider>(settingsMenuUI, "MouseSensitivitySlider");

        masterVolumeSlider = UIHelper.GetUIElement<Slider>(settingsMenuUI, "MasterVolumeSlider");
        musicVolumeSlider = UIHelper.GetUIElement<Slider>(settingsMenuUI, "MusicVolumeSlider");
        sfxVolumeSlider = UIHelper.GetUIElement<Slider>(settingsMenuUI, "SFXVolumeSlider");

        // Graphics Controls
        fullscreenToggle = UIHelper.GetUIElement<Toggle>(settingsMenuUI, "FullscreenToggle");
        resolutionDropdown = UIHelper.GetUIElement<DropdownField>(settingsMenuUI, "DisplayResolutionDropdown");
        graphicsQualityDropdown = UIHelper.GetUIElement<DropdownField>(settingsMenuUI, "GraphicsQualityDropdown");

        // Apply, Reset, Back
        applyButton = UIHelper.GetUIElement<Button>(settingsMenuUI, "ApplyButton");
        backButton = UIHelper.GetUIElement<Button>(settingsMenuUI, "BackButton");
        resetButton = UIHelper.GetUIElement<Button>(settingsMenuUI, "ResetButton");

        // first tab is graphics, set up UI to display graphics tab Opened
        currentTabButton = graphicsTabButton;
        currentTabContent = graphicsTabContent;
        OpenGraphicsTab();

        // hide all tabs other than graphics (ensure only one tab content is showing at a time)
        UIHelper.HideUI(audioTabContent);
        UIHelper.HideUI(controlsTabContent);

    }


    private void SubscribeToEvents()
    {
        // Register Button Callbacks
        backButton.clicked += ExitSettings;
        applyButton.clicked += ApplySettings;
        resetButton.clicked += ResetToDefault;
        graphicsTabButton.clicked += OpenGraphicsTab;
        audioTabButton.clicked += OpenAudioTab;
        controlsTabButton.clicked += OpenControlsTab;

        MainMenuUI.OnOpenSettingsFromMainMenu += OpenSettingsMenu;
    }


    private void UnsubscribeToEvents()
    {
        backButton.clicked -= ExitSettings;
        applyButton.clicked -= ApplySettings;
        resetButton.clicked -= ResetToDefault;
        graphicsTabButton.clicked -= OpenGraphicsTab;
        audioTabButton.clicked -= OpenAudioTab;
        controlsTabButton.clicked -= OpenControlsTab;
    }


    private void ExitSettings()
    {
        CloseSettingsMenu();
        OnBackEvent?.Invoke();

    }


    private void ApplySettings()
    {
        QualitySettings.SetQualityLevel(graphicsQualityDropdown.index, true); // apply graphics quality
        ApplyResolution();
        ToggleFullscreen();
        SetLookSensitivity();
        SaveSettings();
    }


    private void ResetToDefault()
    {
        mouseSensitivitySlider.value = 30; // 30f is the value initialized in PlayerMotor.cs 
        masterVolumeSlider.value = 100; // full volumes are default
        sfxVolumeSlider.value = 100;
        musicVolumeSlider.value = 100;
        fullscreenToggle.value = false;
        resolutionDropdown.index = resolutionDefault;
        graphicsQualityDropdown.index = graphicsQualityDefault; 
    }
    public void OpenSettingsMenu()
    {
        UIHelper.ShowUI(settingsMenuUI, "SettingsMenu");
        UIHelper.ShowUI(currentTabContent);
    }


    public void CloseSettingsMenu()
    {
        UIHelper.HideUI(settingsMenuUI, "SettingsMenu");
        OpenGraphicsTab();
        UIHelper.HideUI(currentTabContent);

        if (!IsChangesApplied())
        {
            DiscardChanges();
        }
    }


    private void InitDisplayResolutions()
    {
        // initialize the resolution drop down menu options
        resolutionDropdown.choices = Screen.resolutions
            .Select(resolution => $"{resolution.width}x{resolution.height}")
            .ToList();

        // Check if player already has a saved resolution preference
        if (PlayerPrefs.GetInt("DisplayResolution", -1) == -1)
        {
            // Use current screen resolution as default if no saved preference
            resolutionDropdown.index = Screen.resolutions
                    .Select((resolution, index) => (resolution, index))
                    .First((value) => value.resolution.width == Screen.currentResolution.width && value.resolution.height == Screen.currentResolution.height)
                    .index;

            // save this as the default setting
            resolutionDefault = resolutionDropdown.index;
        }
        else
        {
            // Load player's saved resolution
            resolutionDropdown.index = PlayerPrefs.GetInt("DisplayResolution");

            // Set the default to the player's current screen resolution in case of reset
            resolutionDefault = Screen.resolutions
                    .Select((resolution, index) => (resolution, index))
                    .First((value) => value.resolution.width == Screen.currentResolution.width && value.resolution.height == Screen.currentResolution.height)
                    .index;
        }

        ApplyResolution();
        
    }


    private void InitQualitySettings()
    {
        // initialize the graphics dropdown options with UnityEngine presets (Performant, Balanced, High Fidelity) 
        graphicsQualityDropdown.choices = QualitySettings.names.ToList();

        // Check if player already has a saved quality preference
        if (PlayerPrefs.GetInt("GraphicsQuality", -1) != -1)
            // Initialize with saved quality
            graphicsQualityDropdown.index = PlayerPrefs.GetInt("GraphicsQuality", -1); 
        else
        {
            // Initialized with default quality if no preference
            graphicsQualityDropdown.index = QualitySettings.GetQualityLevel();
        }

        graphicsQualityDefault = graphicsQualityDropdown.index;
    }


    private void ApplyResolution()
    {
        var newResolution = Screen.resolutions[resolutionDropdown.index];
        Screen.SetResolution(newResolution.width, newResolution.height, true);
    }

    private void SetLookSensitivity()
    {
        if (look != null)
        {
            look.xSensitivity = mouseSensitivitySlider.value;
            look.ySensitivity = mouseSensitivitySlider.value;
        }
        else
        {
            // if look is null, player is inside the main menu
            // in this case do nothing, look sensitivity will be applied when player loads into a game
        }

    }


    private void ToggleFullscreen()
    {
        if (fullscreenToggle.value == true)
        {
            Screen.fullScreenMode = FullScreenMode.ExclusiveFullScreen;
        }
        else
        {
            Screen.fullScreenMode = FullScreenMode.Windowed;
        }
    }


    // Reverts settings to "saved" settings
    private void DiscardChanges()
    {
        LoadSettings();

        ApplyResolution();
        QualitySettings.SetQualityLevel(graphicsQualityDropdown.index, true);
    }


    // Saves all current settings values
    private void SaveSettings()
    {
        PlayerPrefs.SetFloat("MouseSensitivity", mouseSensitivitySlider.value);

        PlayerPrefs.SetFloat("MasterVolume", masterVolumeSlider.value);
        PlayerPrefs.SetFloat("MusicVolume", musicVolumeSlider.value);
        PlayerPrefs.SetFloat("SFXVolume", sfxVolumeSlider.value);

        PlayerPrefs.SetInt("FullscreenMode", (fullscreenToggle.value ? 1 : 0)); // 1 if fullscreen enabled : 0  if disabled (i.e., windowed mode)
        PlayerPrefs.SetInt("DisplayResolution", resolutionDropdown.index);
        PlayerPrefs.SetInt("GraphicsQuality", graphicsQualityDropdown.index);
    }


    private void LoadSettings()
    {
        mouseSensitivitySlider.value = PlayerPrefs.GetFloat("MouseSensitivity");
        
        masterVolumeSlider.value = PlayerPrefs.GetFloat("MasterVolume");
        musicVolumeSlider.value = PlayerPrefs.GetFloat("MusicVolume");
        sfxVolumeSlider.value = PlayerPrefs.GetFloat("SFXVolume");

        fullscreenToggle.value = (PlayerPrefs.GetInt("FullscreenMode") == 1) ? true : false; // 1 if fullscreen enabled : 0  if disabled (i.e., windowed mode)
        resolutionDropdown.index = PlayerPrefs.GetInt("DisplayResolution");
        graphicsQualityDropdown.index = PlayerPrefs.GetInt("GraphicsQuality");


    }


    // returns true if any settings have been modified without applying
    private bool IsChangesApplied()
    {
        if (PlayerPrefs.GetFloat("MouseSensitivity") != mouseSensitivitySlider.value ||
            PlayerPrefs.GetFloat("MasterVolume") != masterVolumeSlider.value ||
            PlayerPrefs.GetFloat("MusicVolume") != musicVolumeSlider.value ||
            PlayerPrefs.GetFloat("SFXVolume") != sfxVolumeSlider.value ||
            PlayerPrefs.GetInt("FullscreenMode") == 1 ? true : false != fullscreenToggle.value ||
            PlayerPrefs.GetInt("DisplayResolution") != resolutionDropdown.index ||
            PlayerPrefs.GetInt("GraphicsQuality") != graphicsQualityDropdown.index)
            return false;
        else
            return true;
    }


    private void SwitchTab(VisualElement newTabContent, Button newTabButton, string activeImage, Button inactiveButton1, string inactiveImage1, Button inactiveButton2, string inactiveImage2)
    {
        currentTabContent.style.visibility = Visibility.Hidden;
        currentTabButton.style.minWidth = 125;
        currentTabButton.style.minHeight = 32;

        newTabButton.style.backgroundImage = Resources.Load<Texture2D>(activeImage);
        newTabButton.style.minWidth = 309;
        newTabButton.style.minHeight = 95;
        newTabContent.style.visibility = Visibility.Visible;

        inactiveButton1.style.backgroundImage = Resources.Load<Texture2D>(inactiveImage1);
        inactiveButton2.style.backgroundImage = Resources.Load<Texture2D>(inactiveImage2);

        currentTabContent = newTabContent;
        currentTabButton = newTabButton;
    }

    private void OpenGraphicsTab()
    {
        SwitchTab(graphicsTabContent, graphicsTabButton, "UI Toolkit/Graphics/graphicsTabActive", audioTabButton, "UI Toolkit/Graphics/audioTab", controlsTabButton, "UI Toolkit/Graphics/controlsTab");
    }

    private void OpenAudioTab()
    {
        SwitchTab(audioTabContent, audioTabButton, "UI Toolkit/Graphics/audioTabActive", graphicsTabButton, "UI Toolkit/Graphics/graphicsTab", controlsTabButton, "UI Toolkit/Graphics/controlsTab");
    }

    private void OpenControlsTab()
    {
        SwitchTab(controlsTabContent, controlsTabButton, "UI Toolkit/Graphics/controlsTabActive", graphicsTabButton, "UI Toolkit/Graphics/graphicsTab", audioTabButton, "UI Toolkit/Graphics/audioTab");
    }
}

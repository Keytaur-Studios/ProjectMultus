using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public class SettingsMenuUI : MonoBehaviour
{

    [SerializeField] GameObject settingsMenu; // PauseMenuUI object on Player
    private UIDocument settingsMenuUIDocument; // UI Document component on PauseMenuUI
    private VisualElement settingsMenuContainer; // Container in Visual Tree Asset

    private Slider lookSensitivitySlider;
    private Slider masterVolumeSlider;
    private Slider musicVolumeSlider;
    private Slider sfxVolumeSlider;
    private Toggle fullscreenToggle;
    private DropdownField resolutionDropdown;
    private DropdownField qualityDropdown;
    private Button applyButton;
    private Button backButton;

    private void Awake()
    {
        Debug.Log("SettingsMenuUI is awake");
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

        backButton.clicked += OnBack;
        applyButton.clicked += OnApply;

        InitDisplayResolutions();
        InitQualitySettings();

        // ensure Settings Menu is hidden by default
        settingsMenuContainer.style.visibility = Visibility.Hidden; // must not setActive(false), this will break the UI
    }

    private void OnBack()
    {
        CloseSettingsMenu();
    }

    private void OnApply()
    {
        var setResolution = Screen.resolutions[resolutionDropdown.index];
        Screen.SetResolution(setResolution.width, setResolution.height, true);
        QualitySettings.SetQualityLevel(qualityDropdown.index, true);
        ToggleFullscreen();
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

    private void InitDisplayResolutions()
    {
        resolutionDropdown.choices = Screen.resolutions.Select(resolution => $"{resolution.width}x{resolution.height}").ToList();
        resolutionDropdown.index = Screen.resolutions
                .Select((resolution, index) => (resolution, index))
                .First((value) => value.resolution.width == Screen.currentResolution.width && value.resolution.height == Screen.currentResolution.height)
                .index; 
    }

    private void InitQualitySettings()
    {
        qualityDropdown.choices = QualitySettings.names.ToList();
        qualityDropdown.index = QualitySettings.GetQualityLevel();
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

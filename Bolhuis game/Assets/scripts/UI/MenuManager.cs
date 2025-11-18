using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    [Header("Templates")]
    [SerializeField] private VisualTreeAsset _levelsButtonTemplate;
    [SerializeField] private VisualTreeAsset _settingsPanelTemplate;

    private VisualElement _buttonsWrapper;
    private Button _tutorialButton;
    private Button _levelsButton;
    private Button _settingsButton;
    private Button _exitButton;

    private VisualElement _levelsPanel;
    private List<Button> _levelsPanelButtons = new List<Button>();

    private VisualElement _settingsPanel;
    private List<Button> _settingsPanelButtons = new List<Button>();

    private Slider _volumeSlider;
    public AudioSource BackGroundMusic;

    private void Awake()
    {
        var root = GetComponent<UIDocument>().rootVisualElement;

        _buttonsWrapper = root.Q<VisualElement>("Buttons");
        _tutorialButton = root.Q<Button>("TutorialButton");
        _settingsButton = root.Q<Button>("SettingsButton");
        _levelsButton = root.Q<Button>("LevelsButton");
        _exitButton = root.Q<Button>("ExitButton");

        _tutorialButton.clicked += TutorialButtonClicked;
        _levelsButton.clicked += LevelsButtonClicked;
        _settingsButton.clicked += SettingsButtonClicked;
        _exitButton.clicked += ExitButtonClicked;

        _levelsPanel = _levelsButtonTemplate.CloneTree().Q<VisualElement>("Wrapper");
        _levelsPanel.style.display = DisplayStyle.None;
        _buttonsWrapper.Add(_levelsPanel);

        AddPanelButton(_levelsPanel, _levelsPanelButtons, "LevelBackButton", LevelsBackButtonClicked);
        AddPanelButton(_levelsPanel, _levelsPanelButtons, "Level1", Level1ButtonClicked);
        AddPanelButton(_levelsPanel, _levelsPanelButtons, "Level2", Level2ButtonClicked);
        AddPanelButton(_levelsPanel, _levelsPanelButtons, "Level3", Level3ButtonClicked);
        AddPanelButton(_levelsPanel, _levelsPanelButtons, "Level4", Level4ButtonClicked);
        AddPanelButton(_levelsPanel, _levelsPanelButtons, "Level5", Level5ButtonClicked);

        _settingsPanel = _settingsPanelTemplate.CloneTree().Q<VisualElement>("Wrapper");
        _settingsPanel.style.display = DisplayStyle.None;
        _buttonsWrapper.Add(_settingsPanel);

        AddPanelButton(_settingsPanel, _settingsPanelButtons, "SettingsBackButton", SettingsBackButtonClicked);

        _volumeSlider = _settingsPanel.Q<Slider>("VolumeSlider");

        if (_volumeSlider != null)
        {
            if (!PlayerPrefs.HasKey("musicVolume"))
                PlayerPrefs.SetFloat("musicVolume", 1);

            float savedVolume = PlayerPrefs.GetFloat("musicVolume");
            _volumeSlider.value = savedVolume;

            AudioListener.volume = savedVolume;
            if (BackGroundMusic != null)
                BackGroundMusic.volume = savedVolume;

            _volumeSlider.RegisterValueChangedCallback(evt =>
            {
                AudioListener.volume = evt.newValue;
                if (BackGroundMusic != null)
                    BackGroundMusic.volume = evt.newValue;

                PlayerPrefs.SetFloat("musicVolume", evt.newValue);
            });
        }
        else
        {
            Debug.LogWarning("VolumeSlider not found in UXML Settings Panel.");
        }
    }

    private void AddPanelButton(VisualElement panel, List<Button> list, string name, System.Action callback)
    {
        var button = panel.Q<Button>(name);
        if (button != null)
        {
            button.clicked += callback;
            button.style.display = DisplayStyle.None;
            list.Add(button);
        }
        else
        {
            Debug.LogWarning($"Button '{name}' not found in template.");
        }
    }

    private void ShowPanel(VisualElement panel, List<Button> buttons)
    {
        _tutorialButton.style.display = DisplayStyle.None;
        _levelsButton.style.display = DisplayStyle.None;
        _settingsButton.style.display = DisplayStyle.None;
        _exitButton.style.display = DisplayStyle.None;

        panel.style.display = DisplayStyle.Flex;

        foreach (var b in buttons)
            b.style.display = DisplayStyle.Flex;
    }

    private void HidePanel(VisualElement panel, List<Button> buttons)
    {
        panel.style.display = DisplayStyle.None;

        foreach (var b in buttons)
            b.style.display = DisplayStyle.None;

        _tutorialButton.style.display = DisplayStyle.Flex;
        _levelsButton.style.display = DisplayStyle.Flex;
        _settingsButton.style.display = DisplayStyle.Flex;
        _exitButton.style.display = DisplayStyle.Flex;
    }

    private void TutorialButtonClicked() => SceneManager.LoadScene("Tutorial");
    private void LevelsButtonClicked() => ShowPanel(_levelsPanel, _levelsPanelButtons);
    private void SettingsButtonClicked() => ShowPanel(_settingsPanel, _settingsPanelButtons);

    private void LevelsBackButtonClicked() => HidePanel(_levelsPanel, _levelsPanelButtons);
    private void SettingsBackButtonClicked() => HidePanel(_settingsPanel, _settingsPanelButtons);

    private void Level1ButtonClicked() => SceneManager.LoadScene("Level 1");
    private void Level2ButtonClicked() => SceneManager.LoadScene("Level 2");
    private void Level3ButtonClicked() => SceneManager.LoadScene("Level 3");
    private void Level4ButtonClicked() => SceneManager.LoadScene("Level 4");
    private void Level5ButtonClicked() => SceneManager.LoadScene("Level 5");

    private void ExitButtonClicked() => Application.Quit();
}

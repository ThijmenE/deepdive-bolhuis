using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    [Header("Templates")]
    [SerializeField] private VisualTreeAsset _UitlegButtonTemplate;
    [SerializeField] private VisualTreeAsset _GeluidPanelTemplate;

    private VisualElement _buttonsWrapper;
    private Button _SpelenButton;
    private Button _UitlegButton;
    private Button _GeluidButton;

    private VisualElement _UitlegPanel;
    private List<Button> _UitlegPanelButtons = new List<Button>();

    private VisualElement _GeluidPanel;
    private List<Button> _GeluidPanelButtons = new List<Button>();

    private Slider _volumeSlider;
    public AudioSource BackGroundMusic;

    private void Awake()
    {
        var root = GetComponent<UIDocument>().rootVisualElement;

        _buttonsWrapper = root.Q<VisualElement>("Buttons");
        _SpelenButton = root.Q<Button>("SpelenButton");
        _GeluidButton = root.Q<Button>("GeluidButton");
        _UitlegButton = root.Q<Button>("UitlegButton");

        _SpelenButton.clicked += SpelenButtonClicked;
        _UitlegButton.clicked += UitlegButtonClicked;
        _GeluidButton.clicked += GeluidButtonClicked;

        _UitlegPanel = _UitlegButtonTemplate.CloneTree().Q<VisualElement>("Wrapper");
        _UitlegPanel.style.display = DisplayStyle.None;
        _buttonsWrapper.Add(_UitlegPanel);

        AddPanelButton(_UitlegPanel, _UitlegPanelButtons, "UitlegBackButton", UitlegBackButtonClicked);

        _GeluidPanel = _GeluidPanelTemplate.CloneTree().Q<VisualElement>("Wrapper");
        _GeluidPanel.style.display = DisplayStyle.None;
        _buttonsWrapper.Add(_GeluidPanel);

        AddPanelButton(_GeluidPanel, _GeluidPanelButtons, "GeluidBackButton", GeluidBackButtonClicked);

        _volumeSlider = _GeluidPanel.Q<Slider>("VolumeSlider");

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
        _SpelenButton.style.display = DisplayStyle.None;
        _UitlegButton.style.display = DisplayStyle.None;
        _GeluidButton.style.display = DisplayStyle.None;

        panel.style.display = DisplayStyle.Flex;

        foreach (var b in buttons)
            b.style.display = DisplayStyle.Flex;
    }

    private void HidePanel(VisualElement panel, List<Button> buttons)
    {
        panel.style.display = DisplayStyle.None;

        foreach (var b in buttons)
            b.style.display = DisplayStyle.None;

        _SpelenButton.style.display = DisplayStyle.Flex;
        _UitlegButton.style.display = DisplayStyle.Flex;
        _GeluidButton.style.display = DisplayStyle.Flex;
    }

    private void SpelenButtonClicked() => SceneManager.LoadScene("Boom test");
    private void UitlegButtonClicked() => ShowPanel(_UitlegPanel, _UitlegPanelButtons);
    private void GeluidButtonClicked() => ShowPanel(_GeluidPanel, _GeluidPanelButtons);

    private void UitlegBackButtonClicked() => HidePanel(_UitlegPanel, _UitlegPanelButtons);
    private void GeluidBackButtonClicked() => HidePanel(_GeluidPanel, _GeluidPanelButtons);
}

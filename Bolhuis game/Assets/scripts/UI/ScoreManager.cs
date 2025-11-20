using System;
using UnityEngine;
using UnityEngine.UIElements;

public class GameManager : MonoBehaviour
{
    private Label scoreLabel;
    private Label muntenLabel;
    private Label timerLabel;
    private Button gieterButton;
    private Button mestButton;
    private Button SchepButton;

    private float score = 0f;
    private float munten = 750f;
    [SerializeField] private float mest = 10;
    [SerializeField] private float Gieter = 75f;
    [SerializeField] private float schep = 50f;

    private float elapsedTime = 0f;
    public Animator animator;

    private bool isGieterActive = false;
    private bool isMestActive = false;
    private bool isSchepActive = false;

    public static event Action<bool> gieter;
    public static void SetGieter(bool value) => gieter?.Invoke(value);

    public static event Action<bool> Mest;
    public static void SetMest(bool value) => Mest?.Invoke(value);

    public static event Action<bool> Schep;
    public static void SetSchep(bool value) => Schep?.Invoke(value);

    private void Awake()
    {
        UIDocument uiDocument = GetComponent<UIDocument>();
        if (uiDocument == null)
            uiDocument = FindObjectOfType<UIDocument>();

        if (uiDocument != null)
        {
            var root = uiDocument.rootVisualElement;
            scoreLabel = root.Q<Label>("ScoreLabel");
            muntenLabel = root.Q<Label>("MuntenLabel");
            timerLabel = root.Q<Label>("boom-timer");
            gieterButton = root.Q<Button>("GieterButton");
            mestButton = root.Q<Button>("MestButton");
            SchepButton = root.Q<Button>("SchepButton");
        }

        if (gieterButton != null)
            gieterButton.clicked += OnGieterButtonClicked;

        gieter += OnGieterChanged;

        if (mestButton != null)
            mestButton.clicked += OnMestButtonClicked;

        Mest += OnMestChanged;

        if (SchepButton != null)
            SchepButton.clicked += OnSchepButtonClicked;

        Schep += OnSchepChanged;
    }

    private void Start()
    {
        UpdateScoreLabel();
        UpdateMuntenLabel();
    }

    private void OnEnable()
    {
        Boom.OnScoreAdd += OnScoreAdded;
        Boom.OnMuntenAdd += OnMuntenAdded;
    }

    private void OnDisable()
    {
        Boom.OnScoreAdd -= OnScoreAdded;
        Boom.OnMuntenAdd -= OnMuntenAdded;
    }

    private void OnDestroy()
    {
        if (gieterButton != null)
            gieterButton.clicked -= OnGieterButtonClicked;

        gieter -= OnGieterChanged;

        if (mestButton != null)
            mestButton.clicked -= OnMestButtonClicked;

        Mest -= OnMestChanged;

        if (SchepButton != null)
            SchepButton.clicked -= OnSchepButtonClicked;

        Schep -= OnSchepChanged;
    }

    private void OnScoreAdded(float value)
    {
        score += value;
        UpdateScoreLabel();
    }

    private void OnMuntenAdded(float value)
    {
        munten += value;
        UpdateMuntenLabel();
    }

    private void OnGieterChanged(bool state)
    {
        isGieterActive = state;
    }

    private void OnMestChanged(bool state)
    {
        isMestActive = state;
    }

    private void OnSchepChanged(bool state)
    {
        isSchepActive = state;
    }

    private void OnGieterButtonClicked()
    {
        if (isGieterActive) return;

        if (munten >= Gieter)
        {
            munten -= Gieter;
            UpdateMuntenLabel();
            SetGieter(true);

            gieterButton.SetEnabled(false);
        }
    }

    private void OnMestButtonClicked()
    {
        if (isMestActive) return;

        if (munten >= mest)
        {
            munten -= mest;
            UpdateMuntenLabel();
            SetMest(true);

            mestButton.SetEnabled(false);
        }
    }

    private void OnSchepButtonClicked()
    {
        if (isSchepActive) return;

        if (munten >= schep)
        {
            munten -= schep;
            UpdateMuntenLabel();
            SetSchep(true);

            SchepButton.SetEnabled(false);
        }
    }

    private void UpdateScoreLabel()
    {
        if (scoreLabel != null)
            scoreLabel.text = "Score: " + score;
    }

    private void UpdateMuntenLabel()
    {
        if (muntenLabel != null)
            muntenLabel.text = "Munten: " + munten;
    }

    private void Update()
    {
        float gieterSpeed = isGieterActive ? 1.5f : 1f;
        float mestSpeed   = isMestActive ? 1.2f : 1f;
        float schepSpeed  = isSchepActive ? 1.3f : 1f;

        float totalSpeed = gieterSpeed + mestSpeed + schepSpeed - 2f;

        if (elapsedTime < 20f)
        {
            elapsedTime += Time.deltaTime * totalSpeed;
            if (elapsedTime > 20f) elapsedTime = 20f;
        }

        int minutes = Mathf.FloorToInt(elapsedTime / 60f);
        int seconds = Mathf.FloorToInt(elapsedTime % 60f);

        if (timerLabel != null)
            timerLabel.text = $"{minutes:00}:{seconds:00}";

        if (animator != null)
            animator.SetFloat("Timer", elapsedTime);
    }
}

using System;
using TMPro;
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

    public static event Action<float> OnScoreAdd;
    public static event Action<float> OnMuntenAdd;

    [SerializeField] private float scoreValue = 100;
    [SerializeField] private float muntenValue = 25;

    private Button _ButtonCheck;
    private Button _ButtonPlanted;

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

            _ButtonCheck = root.Q<Button>("ButtonCheck");
            if (_ButtonCheck != null)
                _ButtonCheck.clicked += ButtonCheckClicked;

            _ButtonPlanted = root.Q<Button>("ButtonPlanted");
            if (_ButtonPlanted != null)
                _ButtonPlanted.clicked += ButtonPlantClicked;
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
        _ButtonCheck.SetEnabled(false);
        _ButtonPlanted.SetEnabled(false);
    }

    private void OnEnable()
    {
        OnScoreAdd += OnScoreAdded;
        OnMuntenAdd += OnMuntenAdded;
    }

    private void OnDisable()
    {
        OnScoreAdd -= OnScoreAdded;
        OnMuntenAdd -= OnMuntenAdded;
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

    private void OnGieterChanged(bool state) => isGieterActive = state;
    private void OnMestChanged(bool state) => isMestActive = state;
    private void OnSchepChanged(bool state) => isSchepActive = state;

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

    public void ButtonCheckClicked()
    {
        if (elapsedTime < 20f) return;

        OnScoreAdd?.Invoke(scoreValue);
        OnMuntenAdd?.Invoke(muntenValue);

        animator.SetTrigger("Sold");
        _ButtonCheck.SetEnabled(false);
        _ButtonPlanted.SetEnabled(true);
    }

    public void ButtonCheck2Clicked()
    {
        if (elapsedTime < 20f) return;

        OnScoreAdd?.Invoke(scoreValue);
        OnMuntenAdd?.Invoke(muntenValue);

        animator.SetTrigger("Sold");
    }

    public void ButtonCheck3Clicked()
    {
        if (elapsedTime < 20f) return;

        OnScoreAdd?.Invoke(scoreValue);
        OnMuntenAdd?.Invoke(muntenValue);

        animator.SetTrigger("Sold");
    }

    public void ButtonPlantClicked()
    {
        animator.SetTrigger("Planted");
        elapsedTime = 0f;
        _ButtonPlanted.SetEnabled(false);
    }

    public void ButtonPlant2Clicked()
    {
        animator.SetTrigger("Planted");
        elapsedTime = 0f;
    }

    public void ButtonPlant3Clicked()
    {
        animator.SetTrigger("Planted");
        elapsedTime = 0f;
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
        float mestSpeed = isMestActive ? 1.2f : 1f;
        float schepSpeed = isSchepActive ? 1.3f : 1f;

        float totalSpeed = gieterSpeed + mestSpeed + schepSpeed - 2f;

        if (elapsedTime < 20f)
        {
            elapsedTime += Time.deltaTime * totalSpeed;
        }
        if (elapsedTime > 20f)
        {
            elapsedTime = 20f;
            _ButtonCheck.SetEnabled(true);
        }

        int minutes = Mathf.FloorToInt(elapsedTime / 60f);
        int seconds = Mathf.FloorToInt(elapsedTime % 60f);

        if (timerLabel != null)
            timerLabel.text = $"{minutes:00}:{seconds:00}";

        if (animator != null)
            animator.SetFloat("Timer", elapsedTime);
    }
}

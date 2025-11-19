using System;
using UnityEngine;
using UnityEngine.UIElements;

public class GameManager : MonoBehaviour
{
    private Label scoreLabel;
    private Label muntenLabel;
    private Label timerLabel;
    private Button gieterButton;

    private float score = 0f;
    private float munten = 50f;
    [SerializeField] private float gieterWaarde = 75f;

    private float elapsedTime = 0f;
    public Animator animator;
    private bool isGieterActive = false;

    public static event Action<bool> gieter;
    public static void SetGieter(bool value) => gieter?.Invoke(value);

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
        }
        else
        {
            Debug.LogWarning("No UIDocument found in scene.");
        }

        if (gieterButton != null)
            gieterButton.clicked += OnGieterButtonClicked;
        else
            Debug.LogWarning("GieterButton not found in UIDocument.");

        gieter += OnGieterChanged;
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
    private void OnGieterButtonClicked()
    {
        if (munten >= gieterWaarde)
        {
            munten -= gieterWaarde;
            UpdateMuntenLabel();
            SetGieter(true);

            Debug.Log("Gieter is gekocht!");
        }
        else
        {
            Debug.Log("Niet genoeg munten in de tas! sukkel");
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
        float speed = isGieterActive ? 1.5f : 1f;

        if (elapsedTime < 30f)
        {
            elapsedTime += Time.deltaTime * speed;
            if (elapsedTime > 30f) elapsedTime = 30f;
        }

        int minutes = Mathf.FloorToInt(elapsedTime / 60f);
        int seconds = Mathf.FloorToInt(elapsedTime % 60f);

        if (timerLabel != null)
            timerLabel.text = $"{minutes:00}:{seconds:00}";

        if (animator != null)
            animator.SetFloat("Timer", elapsedTime);
    }

    public void ToggleGieter(bool value)
    {
        SetGieter(value);
    }
}

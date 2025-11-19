using UnityEngine;
using UnityEngine.UIElements;

public class ScoreManager : MonoBehaviour
{
    private Label scoreLabel;
    private Label muntenLabel;
    private Button schepButton;

    private float score = 0;
    private float munten = 0;

    private float schep = 75;

    private Button _SchepButton;

    private void Start()
    {
        UIDocument uiDocument = GetComponent<UIDocument>();
        if (uiDocument == null)
            uiDocument = FindObjectOfType<UIDocument>();

        scoreLabel = uiDocument.rootVisualElement.Q<Label>("ScoreLabel");
        muntenLabel = uiDocument.rootVisualElement.Q<Label>("MuntenLabel");

        UpdateScore();
        UpdateMunten();
    }

    private void Awake()
    {
        var root = GetComponent<UIDocument>().rootVisualElement;
        _SchepButton = root.Q<Button>("SchepButton");
        _SchepButton.clicked += SchepButtonClicked;
    }

    private void SchepButtonClicked()
    {

        if (munten >= 75)
        {
            munten -= schep;
            UpdateMunten();
            Debug.Log("Schep is gekocht!");
        }
        else
        {
            Debug.Log("Niet genoeg munten in de tas!");
        }
    }
    private void OnEnable()
    {
        Boom.OnScoreAdd += ScoreUpdater;
        Boom.OnMuntenAdd += MuntenUpdater;
    }

    private void OnDisable()
    {
        Boom.OnScoreAdd -= ScoreUpdater;
        Boom.OnMuntenAdd -= MuntenUpdater;
    }

    private void ScoreUpdater(float value)
    {
        score += value;
        UpdateScore();
    }

    private void MuntenUpdater(float value)
    {
        munten += value;
        UpdateMunten();
    }
    private void UpdateScore()
    {
        if (scoreLabel != null)
            scoreLabel.text = "Score: " + score;
    }

    private void UpdateMunten()
    {
        if (muntenLabel != null)
            muntenLabel.text = "Munten: " + munten;
    }
}

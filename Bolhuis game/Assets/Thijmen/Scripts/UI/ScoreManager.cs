using UnityEngine;
using UnityEngine.UIElements;

public class ScoreManager : MonoBehaviour
{
    private Label scoreLabel;
    private float score = 0;

    private void Start()
    {
        UIDocument uiDocument = GetComponent<UIDocument>();
        if (uiDocument == null)
        {
            uiDocument = FindObjectOfType<UIDocument>();
        }

        if (uiDocument == null)
        {
            Debug.LogError("ScoreManager: No UIDocument found in the scene!");
            return;
        }

        scoreLabel = uiDocument.rootVisualElement.Q<Label>("ScoreLabel");

        if (scoreLabel == null)
        {
            Debug.LogError("ScoreManager: Could not find a Label named 'ScoreLabel' in the UIDocument.");
        }
        else
        {
            UpdateUI();
        }
    }

    private void OnEnable()
    {
        Fruit.OnFruitCollected += HandleFruitCollected;
    }

    private void OnDisable()
    {
        Fruit.OnFruitCollected -= HandleFruitCollected;
    }

    private void HandleFruitCollected(float value)
    {
        score += value;
        UpdateUI();
    }

    private void UpdateUI()
    {
        if (scoreLabel != null)
        {
            scoreLabel.text = "Score: " + score;
        }
    }
}

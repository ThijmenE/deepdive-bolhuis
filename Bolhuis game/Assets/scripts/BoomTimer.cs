using UnityEngine;
using UnityEngine.UIElements;

public class BoomTimer : MonoBehaviour
{
    private float elapsedTime;
    private Label timerLabel;

    void Awake()
    {
        var uiDoc = GetComponent<UIDocument>();
        timerLabel = uiDoc.rootVisualElement.Q<Label>("boom-timer");
    }

    void Update()
    {
        if (elapsedTime < 30f)
        {
            elapsedTime += Time.deltaTime;
        }
        else
        {
            elapsedTime = 30f;
        }

        int minutes = Mathf.FloorToInt(elapsedTime / 60);
        int seconds = Mathf.FloorToInt(elapsedTime % 60);

        if (timerLabel != null)
            timerLabel.text = $"{minutes:00}:{seconds:00}";
    }
}

using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class Timer : MonoBehaviour
{
    [SerializeField] private float remainingTime = 60f;

    private Label timerLabel;

    void Awake()
    {
        var uiDoc = GetComponent<UIDocument>();

        timerLabel = uiDoc.rootVisualElement.Q<Label>("timer-label");

        if (timerLabel == null)
        {
            Debug.LogError("Label 'timer-label' niet gevonden! Controleer de naam in UI Builder.");
        }
    }

    void Update()
    {
        if (remainingTime > 0)
        {
            remainingTime -= Time.deltaTime;
        }
        else if (remainingTime < 0)
        {
            remainingTime = 0;
            timerLabel.style.color = Color.red;
            SceneManager.LoadScene("WinScene");
        }

        int minutes = Mathf.FloorToInt(remainingTime / 60);
        int seconds = Mathf.FloorToInt(remainingTime % 60);

        if (timerLabel != null)
            timerLabel.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }
}

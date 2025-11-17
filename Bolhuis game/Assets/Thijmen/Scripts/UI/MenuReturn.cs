using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class MenuReturn : MonoBehaviour
{
    private VisualElement _buttonsWrapper;
    private Button _exitButton;

    private void Awake()
    {
        var root = GetComponent<UIDocument>().rootVisualElement;

        _exitButton = root.Q<Button>("ReturnMenuButton");

        _exitButton.clicked += ExitButtonClicked;
    }

    
    private void ExitButtonClicked() => SceneManager.LoadScene("Main Menu");

}

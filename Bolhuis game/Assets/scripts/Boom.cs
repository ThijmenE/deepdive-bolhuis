using System;
using UnityEngine;
using UnityEngine.UIElements;

public class Boom : MonoBehaviour
{
    public static event Action<float> OnScoreAdd;
    public static event Action<float> OnMuntenAdd;

    [SerializeField] private Animator animator;
    [SerializeField] private float scoreValue = 100;
    [SerializeField] private float muntenValue = 25;

    private Button _ButtonCheck;
    private bool collected = false;

    private void Awake()
    {
        var root = GetComponent<UIDocument>().rootVisualElement;
        _ButtonCheck = root.Q<Button>("ButtonCheck");
        _ButtonCheck.clicked += ButtonClicked;
    }

    public void ButtonClicked()
    {
        if (collected) return;
        collected = true;

        OnScoreAdd?.Invoke(scoreValue);
        OnMuntenAdd?.Invoke(muntenValue);

        collected = false;
    }
}

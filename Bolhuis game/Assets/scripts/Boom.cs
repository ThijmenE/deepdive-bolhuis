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

    private Button _ButtonPlanted;

    private void Awake()
    {
        var root = GetComponent<UIDocument>().rootVisualElement;
        _ButtonCheck = root.Q<Button>("ButtonCheck");
        _ButtonCheck.clicked += ButtonCheckClicked;
        _ButtonCheck = root.Q<Button>("ButtonCheck2");
        _ButtonCheck.clicked += ButtonCheck2Clicked;
        _ButtonCheck = root.Q<Button>("ButtonCheck3");
        _ButtonCheck.clicked += ButtonCheck3Clicked;
        _ButtonPlanted = root.Q<Button>("ButtonPlanted");
        _ButtonPlanted.clicked += ButtonPlantClicked;
    }

    public void ButtonCheckClicked()
    {
        OnScoreAdd?.Invoke(scoreValue);
        OnMuntenAdd?.Invoke(muntenValue);

        animator.SetTrigger("Sold");
    }

    public void ButtonCheck2Clicked()
    {
        OnScoreAdd?.Invoke(scoreValue);
        OnMuntenAdd?.Invoke(muntenValue);

        animator.SetTrigger("Sold");
    }

    public void ButtonCheck3Clicked()
    {
        OnScoreAdd?.Invoke(scoreValue);
        OnMuntenAdd?.Invoke(muntenValue);

        animator.SetTrigger("Sold");
    }

    public void ButtonPlantClicked()
    {
        animator.SetTrigger("Planted");
    }

    public void ButtonPlant2Clicked()
    {
        animator.SetTrigger("Planted");
    }

    public void ButtonPlant3Clicked()
    {
        animator.SetTrigger("Planted");
    }
}
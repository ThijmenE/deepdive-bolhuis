using System;
using UnityEngine;

public class Fruit : MonoBehaviour
{
    public AudioSource pickupSound;
    [SerializeField] private Animator animator;
    [SerializeField] private float scoreValue = 10;

    public static Action<float> OnFruitCollected;

    private bool collected = false;

    public void Collect()
    {
        if (collected) return;
        collected = true;

        OnFruitCollected?.Invoke(scoreValue);

        pickupSound.Play();

        animator.SetTrigger("Collect");

        Destroy(gameObject, 1f);
    }
}

using System.Data.Common;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerCollisions : MonoBehaviour
{

    [SerializeField] private Animator animator;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Rigidbody2D rigidBody;
    [SerializeField] private float bounceForce = 6f;

    [SerializeField] private float bounceForceMultiplier = 1.5f;

    private float halfHeight;
    public AudioSource enemyKill;

    private void Start()
    {
        halfHeight = spriteRenderer.bounds.extents.y;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Fruit")){
            other.GetComponent<Fruit>().Collect();
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            CollideWithEnemy(other);
        }
    }

    private void CollideWithEnemy(Collision2D other)
    {
        float force = bounceForce;
        if (Input.GetButton("Jump"))
        {
            force *= bounceForceMultiplier;
        }
        foreach (ContactPoint2D contact in other.contacts)
        {
            if (contact.normal.y > 0.5f)
            {
                rigidBody.linearVelocity = new Vector2(rigidBody.linearVelocity.x, 0f);
                rigidBody.AddForce(Vector2.up * force, ForceMode2D.Impulse);
                Destroy(other.gameObject);
                animator.SetBool("isJumping", true);
                enemyKill.Play();
            }
            else if (contact.normal.y < 0.5f)
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
        }
    }
}
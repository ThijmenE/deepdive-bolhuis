using UnityEngine;
using UnityEngine.SceneManagement;

public class WinCollider : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            RestartScene();
        }
    }

    private void RestartScene()
    {
        SceneManager.LoadScene("Main Menu");
    }
}

using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int points = 10;

    public void Hit()
    {
        if (GameManager.instance != null)
        {
            GameManager.instance.AddScore(points);
        }

        Destroy(gameObject);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<PlayerController>() != null)
        {
            if (GameManager.instance != null)
            {
                GameManager.instance.GameOver(false);
            }
        }
    }
}

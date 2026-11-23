using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    public float speed = 6f;
    public float minY = -6f;

    void Update()
    {
        if (GameManager.instance != null && GameManager.instance.isGameOver)
        {
            Destroy(gameObject);
            return;
        }

        transform.position += Vector3.down * speed * Time.deltaTime;

        if (transform.position.y < minY)
        {
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<PlayerController>() != null)
        {
            if (GameManager.instance != null)
            {
                GameManager.instance.LoseLife();
            }
            Destroy(gameObject);
        }
    }
}

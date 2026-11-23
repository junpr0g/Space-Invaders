using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    public float speed = 8.5f;
    public float minY = -9f;

    private bool hit = false;

    void Update()
    {
        if (GameManager.instance != null && (GameManager.instance.isGameOver || GameManager.instance.isWaveRespawning))
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
        if (hit)
        {
            return;
        }

        BunkerBlock block = other.GetComponent<BunkerBlock>();
        if (block != null)
        {
            hit = true;
            BunkerBlock.SplashHit(block.transform.position);
            Destroy(gameObject);
            return;
        }

        if (other.GetComponent<PlayerController>() != null)
        {
            hit = true;
            if (GameManager.instance != null)
            {
                GameManager.instance.LoseLife();
            }
            Destroy(gameObject);
        }
    }
}

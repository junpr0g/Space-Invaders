using UnityEngine;

public class PlayerBullet : MonoBehaviour
{
    public float speed = 14f;
    public float maxY = 9f;

    private bool hit = false;

    void Update()
    {
        if (GameManager.instance != null && (GameManager.instance.isGameOver || GameManager.instance.isWaveRespawning || GameManager.instance.isPlayerRespawning))
        {
            Destroy(gameObject);
            return;
        }

        transform.position += Vector3.up * speed * Time.deltaTime;

        if (transform.position.y > maxY)
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
            BunkerBlock.SplashHit(block.transform.position, block.transform.lossyScale.x);
            Destroy(gameObject);
            return;
        }

        Enemy enemy = other.GetComponent<Enemy>();
        if (enemy != null)
        {
            hit = true;
            enemy.Hit();
            Destroy(gameObject);
        }
    }
}

using UnityEngine;

public class PlayerBullet : MonoBehaviour
{
    public float speed = 10f;
    public float maxY = 6f;

    void Update()
    {
        if (GameManager.instance != null && GameManager.instance.isGameOver)
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
        Enemy enemy = other.GetComponent<Enemy>();
        if (enemy != null)
        {
            enemy.Hit();
            Destroy(gameObject);
        }
    }
}

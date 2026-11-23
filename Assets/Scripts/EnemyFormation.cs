using UnityEngine;

public class EnemyFormation : MonoBehaviour
{
    public GameObject[] enemyPrefabs;
    public int rows = 5;
    public int columns = 9;
    public float spacingX = 1.15f;
    public float spacingY = 0.8f;
    public float startY = 3.4f;
    public float speed = 1.6f;
    public float stepDown = 0.4f;
    public float edgeLimit = 7.5f;
    public float bottomLimit = -3.3f;
    public GameObject enemyBulletPrefab;
    public float shootInterval = 1.2f;

    private float direction = 1f;
    private float shootTimer = 0f;

    void Start()
    {
        float startX = -(columns - 1) * spacingX / 2f;

        for (int row = 0; row < rows; row++)
        {
            GameObject prefab = enemyPrefabs[2];
            if (row == 0)
            {
                prefab = enemyPrefabs[0];
            }
            else if (row <= 2)
            {
                prefab = enemyPrefabs[1];
            }

            for (int col = 0; col < columns; col++)
            {
                Vector3 pos = new Vector3(startX + col * spacingX, startY - row * spacingY, 0f);
                Instantiate(prefab, pos, Quaternion.identity, transform);
            }
        }
    }

    void Update()
    {
        if (GameManager.instance != null && GameManager.instance.isGameOver)
        {
            return;
        }

        if (transform.childCount == 0)
        {
            if (GameManager.instance != null)
            {
                GameManager.instance.GameOver(true);
            }
            return;
        }

        transform.position += Vector3.right * direction * speed * Time.deltaTime;

        bool hitEdge = false;
        for (int i = 0; i < transform.childCount; i++)
        {
            Transform enemy = transform.GetChild(i);

            if (enemy.position.y <= bottomLimit)
            {
                if (GameManager.instance != null)
                {
                    GameManager.instance.GameOver(false);
                }
                return;
            }

            if (direction > 0f && enemy.position.x >= edgeLimit)
            {
                hitEdge = true;
            }
            else if (direction < 0f && enemy.position.x <= -edgeLimit)
            {
                hitEdge = true;
            }
        }

        if (hitEdge)
        {
            direction *= -1f;
            transform.position += Vector3.down * stepDown;
        }

        shootTimer += Time.deltaTime;
        if (shootTimer >= shootInterval)
        {
            shootTimer = 0f;
            int randomIndex = Random.Range(0, transform.childCount);
            Transform shooter = transform.GetChild(randomIndex);
            Instantiate(enemyBulletPrefab, shooter.position, Quaternion.identity);
        }
    }
}

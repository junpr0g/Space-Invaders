using System.Collections;
using UnityEngine;

public class EnemyFormation : MonoBehaviour
{
    public GameObject[] enemyPrefabs;
    public int rows = 6;
    public int columns = 9;
    public float spacingX = 1.4f;
    public float spacingY = 1.0f;
    public float startY = 7.2f;
    public float baseSpeed = 1f;
    public float speedPerKilledEnemy = 0.0973f; // When half of them have been killed, the speed threshold is reached.

    public float maxSpeedMultiplier = 3.14f; // An Easter egg related to Euler's number.
    public float lastEnemyMultiplier = 6.28f; // An Easter egg related to the Tau number, i think thats cool.
    public float stepDown = 0.4f;
    public float edgeLimit = 10.5f;
    public float bottomLimit = -3.85f;
    public GameObject enemyBulletPrefab;
    public float shootInterval = 0.8f;

    private float direction = 1f;
    private float shootTimer = 0f;
    private int totalEnemies = 0;
    private bool isRespawning = false;

    void Start()
    {
        totalEnemies = rows * columns;
        SpawnAllEnemiesImmediate();
    }

    void SpawnAllEnemiesImmediate()
    {
        float startX = -(columns - 1) * spacingX / 2f;

        for (int row = 0; row < rows; row++)
        {
            GameObject prefab = GetPrefabForRow(row);

            for (int col = 0; col < columns; col++)
            {
                Vector3 pos = new Vector3(startX + col * spacingX, startY - row * spacingY, 0f);
                Instantiate(prefab, pos, Quaternion.identity, transform);
            }
        }
    }

    GameObject GetPrefabForRow(int row)
    {
        if (row == 0)
        {
            return enemyPrefabs[0];
        }
        if (row <= 2)
        {
            return enemyPrefabs[1];
        }
        return enemyPrefabs[2];
    }

    void Update()
    {
        if (GameManager.instance != null && !GameManager.instance.CanPlay())
        {
            return;
        }

        if (isRespawning)
        {
            return;
        }

        if (transform.childCount == 0)
        {
            StartCoroutine(RespawnWave());
            return;
        }

        int killedCount = totalEnemies - transform.childCount;
        float currentSpeed = baseSpeed + killedCount * speedPerKilledEnemy;
        float maxSpeed = baseSpeed * maxSpeedMultiplier;
        if (currentSpeed > maxSpeed)
        {
            currentSpeed = maxSpeed;
        }
        if (transform.childCount == 1)
        {
            currentSpeed = baseSpeed * lastEnemyMultiplier;
        }

        transform.position += Vector3.right * direction * currentSpeed * Time.deltaTime;

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

            float lowestY = float.MaxValue;
            for (int i = 0; i < transform.childCount; i++)
            {
                if (transform.GetChild(i).position.y < lowestY)
                {
                    lowestY = transform.GetChild(i).position.y;
                }
            }

            if (lowestY <= bottomLimit)
            {
                transform.position += Vector3.up * (bottomLimit - lowestY);
                if (GameManager.instance != null)
                {
                    GameManager.instance.GameOver(false);
                }
                return;
            }
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

    IEnumerator RespawnWave()
    {
        isRespawning = true;
        if (GameManager.instance != null)
        {
            GameManager.instance.isWaveRespawning = true;
        }

        transform.position = Vector3.zero;
        direction = 1f;
        shootTimer = 0f;

        float startX = -(columns - 1) * spacingX / 2f;

        for (int row = 0; row < rows; row++)
        {
            GameObject prefab = GetPrefabForRow(row);

            for (int col = 0; col < columns; col++)
            {
                Vector3 pos = new Vector3(startX + col * spacingX, startY - row * spacingY, 0f);
                Instantiate(prefab, pos, Quaternion.identity, transform);
                yield return new WaitForSeconds(0.04f);
            }
        }

        yield return new WaitForSeconds(1f);

        isRespawning = false;
        if (GameManager.instance != null)
        {
            GameManager.instance.isWaveRespawning = false;
        }
    }
}

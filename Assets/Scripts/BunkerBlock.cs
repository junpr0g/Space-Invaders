using UnityEngine;

public class BunkerBlock : MonoBehaviour
{
    public int hp = 1;
    private const float BlockSize = 0.16f;
    private const float MaxSplashBlocks = 3f;
    private SpriteRenderer sr;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    public void TakeDamage(int amount)
    {
        hp -= amount;

        if (hp <= 0)
        {
            Destroy(gameObject);
            return;
        }

        if (sr != null)
        {
            if (hp == 2)
            {
                sr.color = new Color(0.15f, 0.7f, 0.25f, 1f);
            }
            else if (hp == 1)
            {
                sr.color = new Color(0.1f, 0.4f, 0.15f, 1f);
            }
        }
    }

    public static void SplashHit(Vector3 center, float scale = 1f)
    {
        float clampedScale = Mathf.Max(scale, 0.01f);
        float blockStep = BlockSize * clampedScale;
        float maxRadius = (MaxSplashBlocks + 0.1f) * blockStep;

        Collider2D[] hits = Physics2D.OverlapCircleAll(center, maxRadius);
        for (int i = 0; i < hits.Length; i++)
        {
            BunkerBlock block = hits[i].GetComponent<BunkerBlock>();
            if (block == null)
            {
                continue;
            }

            float dist = Vector2.Distance(center, block.transform.position);
            if (dist <= 0.5f * blockStep)
            {
                block.TakeDamage(1);
                continue;
            }

            float normalizedDist = Mathf.Clamp01(dist / maxRadius);
            float destroyChance = Mathf.Lerp(0.85f, 0.12f, normalizedDist);
            if (Random.value <= destroyChance)
            {
                block.TakeDamage(1);
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<Enemy>() != null)
        {
            if (GameManager.instance != null)
            {
                GameManager.instance.GameOver(false);
            }
        }
    }
}

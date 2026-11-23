using UnityEngine;

public class BunkerBlock : MonoBehaviour
{
    public int hp = 3;
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
        Collider2D[] hits = Physics2D.OverlapCircleAll(center, 0.35f * clampedScale);
        for (int i = 0; i < hits.Length; i++)
        {
            BunkerBlock block = hits[i].GetComponent<BunkerBlock>();
            if (block != null)
            {
                float dist = Vector2.Distance(center, block.transform.position);
                if (dist < 0.1f * clampedScale)
                {
                    block.TakeDamage(3);
                }
                else if (dist < 0.24f * clampedScale)
                {
                    block.TakeDamage(2);
                }
                else
                {
                    block.TakeDamage(1);
                }
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

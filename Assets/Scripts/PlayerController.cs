using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public float speed = 7f;
    public float xLimit = 10.5f;
    public GameObject bulletPrefab;
    public Transform firePoint;
    public float fireDelay = 0.75f;
    public float destructionDuration = 0.7f;
    public float respawnBlinkInterval = 0.1f;

    private float fireTimer = 0f;
    private SpriteRenderer spriteRenderer;
    private Collider2D playerCollider;
    private static Sprite debrisSprite;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        playerCollider = GetComponent<Collider2D>();
    }

    void Start()
    {
        fireTimer = fireDelay;
    }

    void Update()
    {
        if (GameManager.instance != null && !GameManager.instance.CanPlay())
        {
            return;
        }

        if (Keyboard.current == null)
        {
            return;
        }

        float move = 0f;

        if (Keyboard.current.leftArrowKey.isPressed || Keyboard.current.aKey.isPressed)
        {
            move = -1f;
        }
        if (Keyboard.current.rightArrowKey.isPressed || Keyboard.current.dKey.isPressed)
        {
            move = 1f;
        }

        transform.position += new Vector3(move * speed * Time.deltaTime, 0f, 0f);

        Vector3 pos = transform.position;
        if (pos.x < -xLimit)
        {
            pos.x = -xLimit;
        }
        if (pos.x > xLimit)
        {
            pos.x = xLimit;
        }
        transform.position = pos;

        fireTimer += Time.deltaTime;
        if (GameManager.instance != null && GameManager.instance.blockShootThisFrame)
        {
            return;
        }

        if (Keyboard.current.spaceKey.wasPressedThisFrame && fireTimer >= fireDelay)
        {
            Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
            fireTimer = 0f;
        }
    }

    public IEnumerator PlayDestructionAnimation()
    {
        if (playerCollider != null)
        {
            playerCollider.enabled = false;
        }

        if (spriteRenderer != null)
        {
            spriteRenderer.enabled = false;
        }

        const int pieceCount = 14;
        GameObject[] pieces = new GameObject[pieceCount];
        SpriteRenderer[] pieceRenderers = new SpriteRenderer[pieceCount];
        Vector2[] velocities = new Vector2[pieceCount];
        float[] angularVelocities = new float[pieceCount];
        Vector3[] initialScales = new Vector3[pieceCount];

        Color baseColor = spriteRenderer != null ? spriteRenderer.color : Color.white;
        int sortingLayerId = spriteRenderer != null ? spriteRenderer.sortingLayerID : 0;
        int sortingOrder = spriteRenderer != null ? spriteRenderer.sortingOrder + 1 : 1;
        Sprite sprite = GetDebrisSprite();

        for (int i = 0; i < pieceCount; i++)
        {
            GameObject piece = new GameObject("PlayerDebris");
            piece.transform.position = transform.position;

            float angle = (Mathf.PI * 2f / pieceCount) * i + Random.Range(-0.25f, 0.25f);
            float speedVal = Random.Range(2.5f, 6.5f);
            velocities[i] = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * speedVal;
            angularVelocities[i] = Random.Range(-360f, 360f);

            float size = Random.Range(0.12f, 0.24f);
            initialScales[i] = new Vector3(size, size, 1f);
            piece.transform.localScale = initialScales[i];

            SpriteRenderer sr = piece.AddComponent<SpriteRenderer>();
            sr.sprite = sprite;
            sr.color = baseColor;
            sr.sortingLayerID = sortingLayerId;
            sr.sortingOrder = sortingOrder;

            pieces[i] = piece;
            pieceRenderers[i] = sr;
        }

        float elapsed = 0f;
        while (elapsed < destructionDuration)
        {
            float dt = Time.deltaTime;
            elapsed += dt;
            float t = Mathf.Clamp01(elapsed / destructionDuration);

            for (int i = 0; i < pieceCount; i++)
            {
                if (pieces[i] == null)
                {
                    continue;
                }

                pieces[i].transform.position += (Vector3)(velocities[i] * dt);
                pieces[i].transform.Rotate(0f, 0f, angularVelocities[i] * dt);
                pieces[i].transform.localScale = Vector3.Lerp(initialScales[i], Vector3.zero, t);

                Color c = baseColor;
                c.a = 1f - t;
                pieceRenderers[i].color = c;
            }

            yield return null;
        }

        for (int i = 0; i < pieceCount; i++)
        {
            if (pieces[i] != null)
            {
                Destroy(pieces[i]);
            }
        }
    }

    public IEnumerator PlayRespawnBlinkAnimation(float duration)
    {
        float elapsed = 0f;
        bool visible = true;

        if (spriteRenderer != null)
        {
            spriteRenderer.enabled = true;
        }

        while (elapsed < duration)
        {
            float wait = Mathf.Min(respawnBlinkInterval, duration - elapsed);
            yield return new WaitForSeconds(wait);
            elapsed += wait;

            visible = !visible;
            if (spriteRenderer != null)
            {
                spriteRenderer.enabled = visible;
            }
        }

        if (spriteRenderer != null)
        {
            spriteRenderer.enabled = true;
        }

        if (playerCollider != null)
        {
            playerCollider.enabled = true;
        }

        fireTimer = fireDelay;
    }

    private static Sprite GetDebrisSprite()
    {
        if (debrisSprite == null)
        {
            Texture2D tex = new Texture2D(1, 1);
            tex.SetPixel(0, 0, Color.white);
            tex.Apply();
            debrisSprite = Sprite.Create(tex, new Rect(0f, 0f, 1f, 1f), new Vector2(0.5f, 0.5f), 1f);
        }
        return debrisSprite;
    }
}

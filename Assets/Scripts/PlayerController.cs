using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public float speed = 7f;
    public float xLimit = 12.2f;
    public GameObject bulletPrefab;
    public Transform firePoint;
    public float fireDelay = 0.75f;

    private float fireTimer = 0f;

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
}

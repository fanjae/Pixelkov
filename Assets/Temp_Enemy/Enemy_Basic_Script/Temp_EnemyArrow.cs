using UnityEngine;

public class Temp_EnemyArrow : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 10.0f;
    [SerializeField] private float lifeTime = 3.0f;
    [SerializeField] private int damage = 1;

    private Vector2 direction;
    private bool hasHit;

    public int Damage => damage;

    private void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    public void SetDirection(Vector2 dir)
    {
        direction = dir.normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        transform.rotation = Quaternion.Euler(0f, 0f, angle);
    }

    private void Update()
    {
        transform.position += (Vector3)(direction * moveSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (hasHit) return;

        if (collision.CompareTag("Enemy"))
            return;

        PlayerHealth playerHealth = collision.GetComponentInParent<PlayerHealth>();

        if (playerHealth == null)
            return;

        hasHit = true;
        playerHealth.TakeDamage(damage);

        Destroy(gameObject);
    }
}
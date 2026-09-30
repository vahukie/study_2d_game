using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 10f;
    [SerializeField] private float timeDestroy = 5f;
    [SerializeField] private float damage = 10f;

    void Start()
    {
        // Thiết lập vật lý cho đạn của quái giống như đạn của Player
        if (GetComponent<Collider2D>() != null) GetComponent<Collider2D>().isTrigger = true;
        if (GetComponent<Rigidbody2D>() != null) GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
        
        Destroy(gameObject, timeDestroy); // Tự hủy sau vài giây
    }

    void Update()
    {
        // Bay thẳng tới trước
        transform.Translate(Vector2.right * moveSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Nếu chạm trúng Player thì trừ máu Player
        if (collision.CompareTag("Player"))
        {
            PlayerController player = collision.GetComponent<PlayerController>();
            if (player != null)
            {
                player.TakeDamage(damage);
                Destroy(gameObject); // Chạm trúng thì nổ đạn
            }
        }
    }
}

using UnityEngine;

public class PlayerBullet : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 10f;
    [SerializeField] private float timeDestroy = 10f;

    void Start()
    {
        // In ra Console để kiểm tra xem script có được gắn vào đạn chưa và tốc độ là bao nhiêu
        // Debug.Log("Đạn đã sinh ra! Tốc độ bay (moveSpeed) đang là: " + moveSpeed);

        // 1. Ép Collider của viên đạn thành dạng Trigger 
        // (Xuyên thấu - để đạn bay xuyên qua Player mà không tạo ra va chạm đẩy lùi)
        if (GetComponent<Collider2D>() != null)
        {
            GetComponent<Collider2D>().isTrigger = true;
        }

        // 2. Ép Rigidbody thành dạng Kinematic 
        // (Vật lý ảo - để đạn bay thẳng đều, không bị rơi do trọng lực và không kẹt lại)
        if (GetComponent<Rigidbody2D>() != null)
        {
            GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
        }

        Destroy(gameObject, timeDestroy);
    }

    void Update()
    {
        MoveBullet();
    }

    void MoveBullet()
    {
        transform.Translate(Vector2.right * moveSpeed * Time.deltaTime);
    }

    [SerializeField] private float damage = 10f; // Lượng sát thương của đạn

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Bỏ qua việc so sánh Tag vì dễ sinh lỗi nếu chưa tạo Tag trong Unity.
        // Thay vào đó, kiểm tra trực tiếp xem vật chạm vào có chứa script Enimy không.
        Enimy enemy = collision.GetComponent<Enimy>();
        
        // Nếu có script Enimy (chứng tỏ đây là quái vật)
        if (enemy != null)
        {
            enemy.TakeDamage(damage); // Trừ máu quái vật
            Destroy(gameObject);      // Phá hủy viên đạn sau khi trúng mục tiêu
        }
    }
}

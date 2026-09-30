using UnityEngine;

public class win : MonoBehaviour
{
    [Header("Âm thanh")]
    [SerializeField] private AudioClip pickupSound; // File âm thanh nhặt đồ

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlaySound();
            
            PlayerController player = collision.GetComponent<PlayerController>();
            if (player != null)
            {
                player.CollectItem(); // Báo cho Player biết vừa nhặt 1 món
            }
            Destroy(gameObject); // Xóa cục vàng đi
        }
    }

    // Khi người chơi chạm vào vật phẩm (dạng va chạm vật lý cứng)
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlaySound();

            PlayerController player = collision.gameObject.GetComponent<PlayerController>();
            if (player != null)
            {
                player.CollectItem(); // Báo cho Player biết vừa nhặt 1 món
            }
            Destroy(gameObject); // Xóa cục vàng đi
        }
    }

    private void PlaySound()
    {
        if (pickupSound != null)
        {
            // Nếu mất Tag MainCamera thì phát âm thanh ngay tại vị trí rơi đồ để tránh lỗi
            Vector3 soundPos = Camera.main != null ? Camera.main.transform.position : transform.position;
            AudioSource.PlayClipAtPoint(pickupSound, soundPos);
        }
    }
}

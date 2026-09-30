using UnityEngine;

public class WinItem : MonoBehaviour
{
    // Khi người chơi chạm vào vật phẩm (dạng xuyên thấu Is Trigger)
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerController player = collision.GetComponent<PlayerController>();
            if (player != null)
            {
                player.WinGame();    // Gọi hàm chiến thắng bên Player
                Destroy(gameObject); // Xóa vật phẩm
            }
        }
    }

    // Khi người chơi chạm vào vật phẩm (dạng va chạm vật lý)
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerController player = collision.gameObject.GetComponent<PlayerController>();
            if (player != null)
            {
                player.WinGame();
                Destroy(gameObject);
            }
        }
    }
}

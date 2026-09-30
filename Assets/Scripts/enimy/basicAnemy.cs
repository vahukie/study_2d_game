using UnityEngine;

public class basicAnemy : Enimy
{
    private void Update()
    {
        MoveToPlayer();
    }
    // Chạy khi Collider được đánh dấu là "Is Trigger" (đi xuyên qua nhau)
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            if(player != null)
            {
                player.TakeDamage(enterDamage);
            }
        }
                
    }

    // Chạy khi Collider KHÔNG phải là Trigger (va chạm vật lý cứng)
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            if(player != null)
            {
                player.TakeDamage(enterDamage);
            }
        }
    }
}

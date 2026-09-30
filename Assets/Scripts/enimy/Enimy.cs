using UnityEngine;
using UnityEngine.UI;

public abstract class Enimy : MonoBehaviour
{
    [SerializeField] protected float maxHp = 50f;
    [SerializeField] protected float currentHp;
    [SerializeField] private Image hpBar;
    [SerializeField] protected float enterDamage = 10f;
    [SerializeField] private float stayDamage = 1f;
    [SerializeField] protected GameObject dropItemPrefab; // Vật phẩm (vàng) rớt ra khi chết

    [SerializeField] protected float enemyMoveSpeed = 1f;
    protected PlayerController player;
    protected virtual void Start()
    {
        player = FindObjectOfType<PlayerController>();
        currentHp = maxHp;
        if (hpBar != null)
        {
            hpBar.fillAmount = currentHp / maxHp;
        }
    }
    protected void MoveToPlayer()
    {
        if( player != null)
        {
            transform.position = Vector2.MoveTowards(transform.position, player.transform.position, enemyMoveSpeed * Time.deltaTime);
            FlipEnemy();
        }
    }
    protected void FlipEnemy()
    {
        if( player != null)
        {
            if(transform.position.x > player.transform.position.x)
            {
                transform.localScale = new Vector3(-1, 1, 1);
            }
            else
            {
                transform.localScale = new Vector3(1, 1, 1);
            }
        }
    }
    public virtual void TakeDamage(float damage)
    {
        currentHp -= damage;
        currentHp = Mathf.Max(currentHp, 0);
        
        // Cập nhật thanh máu UI
        if (hpBar != null)
        {
            hpBar.fillAmount = currentHp / maxHp;
        }

        if(currentHp <= 0)
        {
            Die();
        }
    }
    public virtual void Die()
    {
        // Sinh ra vật phẩm (vàng) tại vị trí của quái trước khi nó chết
        if (dropItemPrefab != null)
        {
            Instantiate(dropItemPrefab, transform.position, Quaternion.identity);
        }
        
        Destroy(gameObject);
    }

}
using UnityEngine;

public class RangedEnemy : Enimy
{
    [Header("Bắn Súng")]
    [SerializeField] private GameObject bulletPrefab; // Đạn của quái
    [SerializeField] private Transform firePos;       // Vị trí bắn
    [SerializeField] private float fireRate = 2f;     // Bắn mấy giây 1 lần
    [SerializeField] private float stoppingDistance = 5f; // Khoảng cách đứng lại để bắn (không lao vào ôm Player nữa)
    
    private float nextFireTime;

    private void Update()
    {
        if (player == null) return;
        
        FlipEnemy(); // Luôn quay mặt nhìn về phía Player

        // Tính khoảng cách giữa quái và Player
        float distanceToPlayer = Vector2.Distance(transform.position, player.transform.position);

        if (distanceToPlayer > stoppingDistance)
        {
            // Nếu Player ở quá xa -> Đi bộ lại gần
            transform.position = Vector2.MoveTowards(transform.position, player.transform.position, enemyMoveSpeed * Time.deltaTime);
        }
        else
        {
            // Nếu Player vào đúng tầm ngắm (dưới 5m) -> Đứng lại và xả súng
            if (Time.time > nextFireTime)
            {
                Shoot();
                nextFireTime = Time.time + fireRate;
            }
        }
    }

    private void Shoot()
    {
        Debug.Log("Quái đang xả đạn! Thời gian hiện tại: " + Time.time);
        
        if (bulletPrefab != null && firePos != null)
        {
            // Sinh ra viên đạn
            GameObject bullet = Instantiate(bulletPrefab, firePos.position, Quaternion.identity);
            
            // Xoay viên đạn đúng hướng mặt của quái (trái hoặc phải)
            if (transform.localScale.x == -1) 
            {
                bullet.transform.rotation = Quaternion.Euler(0, 180, 0); // Bắn sang trái
            }
        }
    }
}

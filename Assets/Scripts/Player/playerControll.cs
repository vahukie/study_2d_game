using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private float moveSpeed =5f ;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private float jumpForce = 15f ;
    private int jumpCount = 0;

    [SerializeField] private float maxHp = 50f;
    [SerializeField] private float currentHp;
    [SerializeField] private Image hpBar;



    [Header("Nhiệm vụ")]
    private int targetItems = 1;
    private int currentItems = 0;

    private Animator animator;

    [Header("Bắn Cung")]
    [SerializeField] private GameObject arrowPrefab; // Prefab mũi tên
    [SerializeField] private Transform firePos;    // Vị trí mũi tên bay ra
    [SerializeField] private float fireRate = 0.5f; // Tốc độ bắn
    private float nextFire;

    [Header("Âm thanh")]
    [SerializeField] private AudioClip shootSound; // File âm thanh bắn cung
    private AudioSource audioSource; // Cái loa phát nhạc

    private void Awake()
    {
        if (rb == null) rb = GetComponent<Rigidbody2D>();
        if (spriteRenderer == null) spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();

        // Tự động lấy cái loa, nếu chưa có thì tự động gắn thêm vào nhân vật
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    private void Start()
    {
        Time.timeScale = 1f; // Đảm bảo game chạy bình thường lúc mới vào
        currentHp = maxHp; // Khởi tạo máu của người chơi đầy khi mới vào game

        if (hpBar != null)
        {
            hpBar.fillAmount = currentHp / maxHp;
        }

        // Tự động đếm số lượng quái vật trên màn chơi hiện tại
        Enimy[] allEnemies = FindObjectsOfType<Enimy>();
        targetItems = allEnemies.Length;
        
        // Nếu màn không có quái nào thì mặc định vẫn là 1 (để khỏi lỗi)
        if (targetItems == 0) targetItems = 1; 
        
        Debug.Log("Màn này có " + targetItems + " con quái. Phải nhặt đủ " + targetItems + " món đồ mới được qua màn!");
    }

    public void CollectItem()
    {
        currentItems++;
        Debug.Log("Đã nhặt: " + currentItems + " / " + targetItems);

        // Đủ số lượng đồ thì mới Win
        if (currentItems >= targetItems)
        {
            WinGame();
        }
    }

    private void Update()
    {
        HandleMovement();
        UpdateHealthBar();
        HandleJump();
        HandleAttack(); // Kiểm tra click chuột để phát hoạt ảnh chém/bắn

        if (animator != null)
        {
            // Nếu không chạm đất (đang nhảy hoặc rơi) thì isJump = true
            animator.SetBool("isJump", !IsGround());
        }
    }

    private void HandleAttack()
    {
        // Khi bấm phím Space và thời gian hiện tại lớn hơn thời gian được phép bắn tiếp
        if (Input.GetKeyDown(KeyCode.Space) && Time.time > nextFire)
        {
            nextFire = Time.time + fireRate;

            if (animator != null)
            {
                animator.SetTrigger("isAttack"); // Kích hoạt hoạt ảnh tấn công
            }

            ShootArrow();
        }
    }

    private void ShootArrow()
    {
        if (arrowPrefab != null && firePos != null)
        {
            // Sinh ra mũi tên
            GameObject arrow = Instantiate(arrowPrefab, firePos.position, Quaternion.identity);

            // Kiểm tra xem nhân vật đang quay mặt trái hay phải
            // Nếu lật trái (flipX = true) thì xoay mũi tên lại 180 độ
            if (spriteRenderer.flipX == true)
            {
                arrow.transform.rotation = Quaternion.Euler(0, 180, 0);
            }

            // Gọi hiệu ứng rung màn hình
            var impulse = GetComponent<Unity.Cinemachine.CinemachineImpulseSource>();
            if (impulse != null)
            {
                impulse.GenerateImpulse(0.2f); // 0.2f là lực rung
            }

            // Phát âm thanh bắn cung
            if (shootSound != null && audioSource != null)
            {
                audioSource.PlayOneShot(shootSound);
            }
        }
    }

    private void UpdateHealthBar()
    {
        if (hpBar != null)
        {
            hpBar.fillAmount = currentHp / maxHp;
        }
    }

    private void HandleMovement()
    {
        float moveX = 0f;
        
        // Đọc phím bấm trực tiếp (Hỗ trợ cả A/D và Mũi tên trái/phải)
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            moveX = 1f;
        }
        else if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            moveX = -1f;
        }

        // Chỉ thay đổi tốc độ trục X (ngang), giữ nguyên tốc độ trục Y (dọc) để trọng lực rơi xuống
        rb.linearVelocity = new Vector2(moveX * moveSpeed, rb.linearVelocity.y);

        if(moveX > 0)
        {
            spriteRenderer.flipX = false;
        }
        else if(moveX < 0)
        {
            spriteRenderer.flipX = true;
        }
        
        if (animator != null)
        {
            // Chỉ chạy hoạt ảnh Run khi nhân vật di chuyển VÀ đang chạm đất
            if (moveX != 0 && IsGround())
            {
                animator.SetBool("isRun", true);
            }
            else
            {
                animator.SetBool("isRun", false);
            }
        }
    }

    private void HandleJump()
    {
        // Nhảy bằng W, hoặc Mũi tên Lên đều được (Phím Space đã được dùng để bắn)
        bool jumpInput = Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow);

        if (jumpInput)
        {
            bool isGrounded = IsGround();
            
            // In ra cảnh báo nếu bấm nhảy mà game không nhận là đang đứng trên mặt đất
            if (!isGrounded && jumpCount == 0)
            {
                Debug.LogWarning("Không thể nhảy! Game báo chưa chạm đất. Bạn hãy kiểm tra:\n1. Mảnh đất (Sàn) đã được gán Layer là 'Ground' chưa?\n2. Ở Player (PlayerController), phần 'Ground Layer' đã chọn đúng layer đó chưa?");
            }

            if (isGrounded)
            {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
                jumpCount = 1; // Cho phép nhảy bước 2
            }
            else if (jumpCount == 1)
            {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
                jumpCount = 0; // Đã nhảy bước 2 thì hết
            }
        }
    }

    private bool IsGround()
    {
        Collider2D col = GetComponent<Collider2D>();
        Vector2 startPos = transform.position;
        float distance = 2.0f;

        if (col != null)
        {
            // Tối ưu: Bắn tia từ dưới gót chân (đáy của collider) thay vì từ tâm nhân vật
            startPos = new Vector2(col.bounds.center.x, col.bounds.min.y);
            distance = 0.2f; // Chỉ cần bắn một tia rất ngắn 0.2f từ dưới chân là đủ
        }
        
        // Vẽ tia laser đỏ trong màn hình Scene để dễ dàng nhìn xem tia có đâm xuống đất chưa
        Debug.DrawRay(startPos, Vector2.down * distance, Color.red);
        
        // Bắn tia raycast
        RaycastHit2D hit = Physics2D.Raycast(startPos, Vector2.down, distance, groundLayer);
        
        return hit.collider != null;
    }

    public void TakeDamage(float damage)
    {
       currentHp -= damage;
        currentHp = Mathf.Max(currentHp, 0);
        if(currentHp <= 0)
        {
            Die();
        }
    }
    public virtual void Die()
    {
        Destroy(gameObject);
    }

    [Header("UI Màn hình kết thúc")]
    [SerializeField] private GameObject winScreenPanel;

    [Header("Chuyển màn")]
    [SerializeField] private string nextSceneName; // Tên màn tiếp theo

    public void WinGame()
    {
        if (winScreenPanel != null)
        {
            winScreenPanel.SetActive(true); // Bật màn hình kết thúc
            Time.timeScale = 0f; // Dừng thời gian trong game lại
        }
        else 
        {
            // Nếu không xài màn hình kết thúc, thì cho nhảy qua màn mới luôn
            LoadNextLevel();
        }
    }

    // Bạn có thể gắn hàm này vào Nút bấm (Button) trên màn hình kết thúc
    // Hoặc nó sẽ tự chạy nếu bạn không gán màn hình kết thúc
    public void LoadNextLevel()
    {
        Time.timeScale = 1f; // Phải nhả thời gian ra trước khi chuyển màn

        // Nếu có gõ tên màn thì chuyển qua màn đó
        if (!string.IsNullOrEmpty(nextSceneName))
        {
            SceneManager.LoadScene(nextSceneName);
        }
        else
        {
            // Tự động tính toán số thứ tự của màn tiếp theo
            int nextIndex = SceneManager.GetActiveScene().buildIndex + 1;
            
            // Kiểm tra xem Màn tiếp theo có tồn tại trong Build Profiles không
            if (nextIndex < SceneManager.sceneCountInBuildSettings)
            {
                SceneManager.LoadScene(nextIndex); // Nếu có thì qua màn bình thường
            }
            else
            {
                // Nếu vượt quá số màn đã có (Ví dụ bạn đang chơi Màn cuối cùng)
                Debug.Log("Chúc mừng! Bạn đã chiến thắng toàn bộ các màn chơi!");
                // Tự động quay cuốn chiếu về màn đầu tiên (Màn số 0)
                SceneManager.LoadScene(0); 
            }
        }
    }
}

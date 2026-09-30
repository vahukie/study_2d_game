using UnityEngine;

public class Gun : MonoBehaviour
{
    private float rotateOffset = 180f;
    [SerializeField] private Transform firePos;
    [SerializeField] private GameObject bulletPrefabs;
    [SerializeField] private float fireRate = 0.5f;
    private float nextFire ;
    [SerializeField] private int maxAmo = 24;
    [SerializeField] private int curAmo;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        curAmo = maxAmo;

    }

    // Update is called once per frame
    void Update()
    {
        RotateGun();
        Shoot();
    }
    void RotateGun()
    {
        if(Input.mousePosition.x < 0|| Input.mousePosition.x > Screen.width || Input.mousePosition.y < 0|| Input.mousePosition.y > Screen.height)
        {
            return;
        }

        Vector3 displacement = transform.position - Camera.main.ScreenToWorldPoint(Input.mousePosition);
        float angle = Mathf.Atan2(displacement.y, displacement.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, angle + rotateOffset);

        if (angle > 90 || angle < -90)
        {
            transform.localScale = new Vector3(1f, -1f, 1f);
        }
        else
        {
            transform.localScale = new Vector3(1f, 1f, 1f);
        }
    }
    void Shoot()
    {
        if(Input.GetMouseButtonDown(0) && Time.time > nextFire && curAmo > 0)
        {
            nextFire = Time.time + fireRate;
            Instantiate(bulletPrefabs, firePos.position, firePos.rotation);
            curAmo--;
        }
    }
}

using UnityEngine;

public class TestRageEnemy : Enemy
{
    [Header("Combat Settings")]
    public float attackRange = 5f;
    public float projectileSpeed = 10f;
    public float fireRate = 1f;
    
    [Header("References")]
    public GameObject projectilePrefab;
    public Transform firePoint;

    private float fireCooldown = 0f;

    
    void Update()
    {
        if (player == null)
        {
            animator.SetBool("Attack", false);
            return;
        }

        timer -= Time.deltaTime;
        fireCooldown -= Time.deltaTime;
        
        // หมุนหน้าหาผู้เล่น
        Turn(player.transform.position - transform.position);

        float dist = Vector3.Distance(transform.position, player.transform.position);
        if (dist < attackRange)
        {
            Attack(player);
        }
        else
        {
            animator.SetBool("Attack", false);
        }
    }

    protected override void Attack(Player _player)
    {
        // กันยิงรัว
        if (fireCooldown > 0) return;

       

        
        animator.SetBool("Attack", true);

        // ยิงกระสุน 
        ShootProjectile();

        // รีเซ็ตเวลา
        fireCooldown = fireRate;
        
       
        timer = 1f; 
    }

    void ShootProjectile()
    {
        if (projectilePrefab == null || firePoint == null) return;

        GameObject p = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);
        
        // ยิงไปที่ตัวผู้เล่น
        Vector3 targetPos = player.transform.position + Vector3.up * 1.0f;
        Vector3 dir = (targetPos - firePoint.position).normalized;
        
        p.transform.forward = dir; // หันหัวกระสุน

        Rigidbody rb = p.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.linearVelocity = dir * projectileSpeed;
        }

        // ลบกระสุนทิ้งใน 5 วิถ้าไม่โดนอะไร
        Destroy(p, 5f);
    }
}
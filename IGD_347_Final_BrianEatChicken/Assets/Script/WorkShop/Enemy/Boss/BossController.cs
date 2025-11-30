using UnityEngine;
using UnityEngine.UI;

public class BossController : Enemy
{
    [Header("Boss UI Settings")]
    [SerializeField] private Slider bossHealthSlider;
    [SerializeField] private GameObject winPanel;

    private bool isDead = false;

   
    protected override void Start()
    {
        base.Start();

        if (bossHealthSlider != null)
        {
            bossHealthSlider.maxValue = maxHealth;
            bossHealthSlider.value = health;
            bossHealthSlider.gameObject.SetActive(false);
        }

        if (winPanel != null)
        {
            winPanel.SetActive(false);
        }
    }

    private void Update()
    {
        // ถ้าไม่มีผู้เล่น หรือบอสตายแล้ว ไม่ต้องทำอะไร
        if (player == null || isDead)
        {
            animator.SetBool("Attack", false);
            if (bossHealthSlider != null) bossHealthSlider.gameObject.SetActive(false);
            return;
        }

        float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);

        // เช็คระยะการมองเห็น (Detection Range)
        if (distanceToPlayer > detectionRange)
        {
            if (bossHealthSlider != null) bossHealthSlider.gameObject.SetActive(false);
            animator.SetBool("Attack", false);
            return; 
        }
        else
        {
            if (bossHealthSlider != null) bossHealthSlider.gameObject.SetActive(true);
        }

        
        Turn(player.transform.position - transform.position);
        
        
        timer -= Time.deltaTime;

       
        if (distanceToPlayer < 1.5f)
        {
            Attack(player);
        }
        else
        {
            // เดินเข้าหาผู้เล่น
            animator.SetBool("Attack", false);
            Vector3 direction = (player.transform.position - transform.position).normalized;
            Move(direction);
        }
    }

    public override void TakeDamage(int amount)
    {
        if (isDead) return;

        health -= amount;

        // อัปเดตหลอดเลือด
        if (bossHealthSlider != null)
        {
            bossHealthSlider.gameObject.SetActive(true);
            bossHealthSlider.value = Mathf.Max(health, 0); // ป้องกันค่าติดลบ
        }

        if (health <= 0)
        {
            BossDeath();
        }
    }

    private void BossDeath()
    {
        isDead = true;

        if (bossHealthSlider != null) bossHealthSlider.gameObject.SetActive(false);

        animator.SetBool("Attack", false);
        animator.SetTrigger("Death");

        // ปิด Collider เพื่อไม่ให้ตีศพได้
        Collider col = GetComponent<Collider>();
        if (col != null) col.enabled = false;

        // รอ 3 วินาทีแล้วค่อยขึ้นหน้าชนะ
        Invoke("ShowWinScreen", 3.0f);
    }

    private void ShowWinScreen()
    {
        if (winPanel != null)
        {
            winPanel.SetActive(true);
            Time.timeScale = 0f; // หยุดเวลาเกม
        }
    }
}
using UnityEngine;
using UnityEngine.AI; // จำเป็นต้องใช้ NavMesh

public class MinionAI : MonoBehaviour
{
    [Header("ค่าพลัง")]
    public float attackRange = 2f; // ระยะโจมตี
    public float damage = 10f;     // พลังโจมตี

    private NavMeshAgent agent;
    private Transform targetEnemy;
    private Transform player;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        // หาตัวคนเล่น (เพื่อให้เดินตามตอนไม่มีศัตรู)
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null) player = playerObj.transform;
    }

    void Update()
    {
        // 1. หาศัตรูที่ใกล้ที่สุด
        FindNearestEnemy();

        // 2. ตัดสินใจเดิน
        if (targetEnemy != null)
        {
            // มีศัตรู -> วิ่งไปหา
            agent.SetDestination(targetEnemy.position);

            // ถ้าถึงระยะตี -> ตีเลย!
            if (Vector3.Distance(transform.position, targetEnemy.position) <= attackRange)
            {
                Attack(targetEnemy);
            }
        }
        else if (player != null)
        {
            // ไม่มีศัตรู -> เดินตามเจ้าของ
            agent.SetDestination(player.position);
        }
    }

    void FindNearestEnemy()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        float shortestDistance = Mathf.Infinity;
        Transform nearest = null;

        foreach (GameObject enemy in enemies)
        {
            float distance = Vector3.Distance(transform.position, enemy.transform.position);
            if (distance < shortestDistance)
            {
                shortestDistance = distance;
                nearest = enemy.transform;
            }
        }
        targetEnemy = nearest;
    }

    void Attack(Transform enemy)
    {
        // ใส่ Animation ตีตรงนี้ได้
        Debug.Log("มอนสเตอร์: ย๊ากกกก! ตี " + enemy.name);

        // ตรงนี้ใส่โค้ดลดเลือดศัตรูจริงๆ (ถ้าศัตรูมีสคริปต์เลือด)
        // เช่น enemy.GetComponent<EnemyHealth>().TakeDamage(damage);

        // (ชั่วคราว) ทำลายศัตรูทิ้งเมื่อตีโดน
        // Destroy(enemy.gameObject, 0.5f); 
    }
}
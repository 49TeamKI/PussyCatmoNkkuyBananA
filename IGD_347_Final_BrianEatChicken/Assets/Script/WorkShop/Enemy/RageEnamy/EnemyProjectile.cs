using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    public int damageAmount = 10; 

   
    private void OnTriggerEnter(Collider other)
    {
        // 1. เช็คว่าสิ่งที่ชน คือ Player หรือไม่?
        if (other.CompareTag("Player"))
        {
           
            Player player = other.GetComponent<Player>();
            
            if (player != null)
            {
                player.TakeDamage(damageAmount); 
                Debug.Log("โดนยิง! เลือดลดแล้ว");
            }

           
            Destroy(gameObject);
        }
        
        else if (other.CompareTag("Ground") || other.CompareTag("Wall"))
        {
            Destroy(gameObject);
        }
    }
}
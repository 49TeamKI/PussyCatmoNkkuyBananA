using UnityEngine;

public class EnemyMovetoPlayer : Enemy
{
    private void Update()
    {
        if (player == null)
        {
            animator.SetBool("Attack", false);
            return;
        }

        //  เช็คระยะมองเห็น
        float distanceToPlayer = GetDistanPlayer();

        if (distanceToPlayer > detectionRange)
        {
            
            animator.SetBool("Attack", false);
            
            return;
        }
        
        
        Turn(player.transform.position - transform.position);
        timer -= Time.deltaTime;

        if (distanceToPlayer < 1.5f)
        {
            Attack(player);
        }
        else
        {
            
            animator.SetBool("Attack", false);
            Vector3 direction = (player.transform.position - transform.position).normalized;
            Move(direction);
        }
    }
    
   
  
}
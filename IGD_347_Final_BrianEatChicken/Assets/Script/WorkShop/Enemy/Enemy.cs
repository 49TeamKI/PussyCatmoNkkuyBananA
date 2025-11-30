using UnityEngine;

public class Enemy : Character
{
    protected enum State { idel, cheses, attack, death }

    [SerializeField]
    private float TimeToAttack = 1f;
    protected State currentState = State.idel;
    protected float timer = 0f;

    [Header("Detection Settings")]
    [SerializeField] protected float detectionRange = 5f;

    
    private Vector3 originalPosition;
    private Quaternion originalRotation;

    [Header("Debuff Settings")]
    [SerializeField] private bool doesAttackDamage = true;
    [SerializeField] private bool appliesPoison = false;
    [SerializeField] private float poisonDuration = 5f;
    [SerializeField] private int poisonDamagePerTick = 1;
    [SerializeField] private bool appliesBleed = false;
    [SerializeField] private float bleedDuration = 3f;
    [SerializeField] private int bleedDamagePerTick = 2;
    [SerializeField] private bool appliesSlow = false;
    [SerializeField] private float slowDuration = 2f;

    
    protected virtual void Start()
    {
        base.SetUP(); 

       
        originalPosition = transform.position;
        originalRotation = transform.rotation;
    }
    

    private void Update()
    {
        if (player == null)
        {
            animator.SetBool("Attack", false);
            return;
        }

        // เช็คระยะมองเห็น
        float currentDistance = Vector3.Distance(transform.position, player.transform.position);

        if (currentDistance > detectionRange)
        {
            animator.SetBool("Attack", false); 
            return; 
        }

        Turn(player.transform.position - transform.position);
        timer -= Time.deltaTime;

        float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);

        if (distanceToPlayer < 1.5)
        {
            Attack(player);
        }
        else
        {
            animator.SetBool("Attack", false);
        }
    }

    protected override void Turn(Vector3 direction)
    {
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        transform.rotation = lookRotation;
    }

    protected virtual void Attack(Player _player)
    {
        if (timer <= 0)
        {
            animator.SetBool("Attack", true);

            if (doesAttackDamage) _player.TakeDamage(Damage);
            if (appliesPoison) _player.ApplyPoisonDebuff(poisonDuration, poisonDamagePerTick);
            if (appliesBleed) _player.ApplyBleedDebuff(bleedDuration, bleedDamagePerTick);
            if (appliesSlow) _player.ApplySlowDebuff(slowDuration);

            timer = TimeToAttack;
        }
    }

    public override void TakeDamage(int amount)
    {
        health -= amount;
        if (health <= 0)
        {
            GameManager.instance.AddScore(10);
            gameObject.SetActive(false);
        }
    }

    
    public void ResetEnemy()
    {
        health = maxHealth;
       
        transform.position = originalPosition;
        transform.rotation = originalRotation;
        gameObject.SetActive(true);

        if (animator != null)
        {
            animator.Rebind();
            animator.Update(0f);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
    }
}
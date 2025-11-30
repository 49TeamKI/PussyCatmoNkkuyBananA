using System.Collections.Generic;
using UnityEngine;

public class Player : Character
{
    // --- Singleton ---
    public static Player instance;

    [Header("Hand setting")]
    public Transform RightHand;
    public Transform LeftHand;

    [Header("--- Effects & VFX ---")]
    public GameObject speedBuffVFX;
    public GameObject damageBuffVFX;
    public GameObject warpVFX;
    public GameObject freezeHitVFX;

    // --- Inventory & Skills ---
    public Dictionary<string, int> inventory = new Dictionary<string, int>();
    public HashSet<string> unlockedSkills = new HashSet<string>();

    Vector3 _inputDirection;
    bool _isAttacking = false;
    bool _isInteract = false;

    // [System] --- Buff & Power-Ups ---
    [Header("Buff Status (เวลาที่เหลือ)")]
    [SerializeField] private float buffSpeedTimer = 0f;
    [SerializeField] private float buffDamageTimer = 0f;
    [SerializeField] private float buffFreezeTimer = 0f;

    // Config
    private const float SPEED_MULTIPLIER = 1.5f;
    private const int DAMAGE_MULTIPLIER = 2;
    private const float WARP_DISTANCE = 3.0f;

    // [System] --- Debuffs ---
    private float poisonTimer = 0f;
    private float poisonTickTimer = 0f;
    private int poisonDamagePerTick = 0;
    private float bleedTimer = 0f;
    private float bleedTickTimer = 0f;
    private int bleedDamagePerTick = 0;
    private float slowTimer = 0f;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            // DontDestroyOnLoad(gameObject);
        }
        else if (instance != this)
        {
            Destroy(gameObject);
            return;
        }
    }

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();

        if (maxHealth <= 0) maxHealth = 100;
        health = maxHealth;

        if (GameManager.instance != null)
            GameManager.instance.UpdateHealthBar(health, maxHealth);
    }

    public void FixedUpdate()
    {
        Vector3 directionToMove = _inputDirection;

        float finalSpeedMult = 1.0f;
        if (slowTimer > 0) finalSpeedMult *= 0.5f;
        if (buffSpeedTimer > 0) finalSpeedMult *= SPEED_MULTIPLIER;

        directionToMove *= finalSpeedMult;

        Move(directionToMove);

        if (_inputDirection.sqrMagnitude > 0.01f)
        {
            Turn(_inputDirection);
        }

        Attack(_isAttacking);
        Interact(_isInteract);
    }

    public void Update()
    {
        HandleInput();
        HandleDebuffs();
        HandleBuffs();
    }

    private void HandleInput()
    {
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");
        _inputDirection = new Vector3(x, 0, y);

        if (Input.GetMouseButtonDown(0)) _isAttacking = true;
        if (Input.GetKeyDown(KeyCode.E)) _isInteract = true;

        if (Input.GetKeyDown(KeyCode.H))
        {
            if (HasItem("Big Potion")) UseItem("Big Potion");
            else if (HasItem("Health Potion")) UseItem("Health Potion");
        }
        if (Input.GetKeyDown(KeyCode.Alpha1)) { if (HasItem("Speed Scroll")) UseItem("Speed Scroll"); }
        if (Input.GetKeyDown(KeyCode.Alpha2)) { if (HasItem("Power Rune")) UseItem("Power Rune"); }
        if (Input.GetKeyDown(KeyCode.Alpha3)) { if (HasItem("Ice Gem")) UseItem("Ice Gem"); }
        if (Input.GetKeyDown(KeyCode.Alpha4)) { if (HasItem("Warp Stone")) UseItem("Warp Stone"); }
        if (Input.GetKeyDown(KeyCode.F)) UseSkill_Fireball();
    }

    public void UseItem(string itemName)
    {
        if (HasItem(itemName))
        {
            RemoveItem(itemName, 1);
            Debug.Log($"Used: {itemName}");

            switch (itemName)
            {
                case "Health Potion": Heal(25); break;
                case "Big Potion": Heal(100); break;
                case "Speed Scroll":
                    buffSpeedTimer = 10f;
                    SpawnVFX(speedBuffVFX, transform.position);
                    if (UIManager.instance != null)
                        UIManager.instance.AddStatusIcon(UIManager.instance.speedBuffIcon, 10f);
                    break;

                case "Power Rune":
                    buffDamageTimer = 10f;
                    SpawnVFX(damageBuffVFX, transform.position);
                    if (UIManager.instance != null)
                        UIManager.instance.AddStatusIcon(UIManager.instance.damageBuffIcon, 10f);
                    break;

                case "Ice Gem":
                    buffFreezeTimer = 5f;
                    SpawnVFX(freezeHitVFX, transform.position + Vector3.up);
                    if (UIManager.instance != null)
                        UIManager.instance.AddStatusIcon(UIManager.instance.freezeBuffIcon, 5f);
                    break;

                case "Warp Stone":
                    SpawnVFX(warpVFX, transform.position);
                    PerformWarp();
                    if (UIManager.instance != null)
                    {
                        UIManager.instance.AddStatusIcon(UIManager.instance.warpStoneBuffIcon, 2f);
                    }
                    break;
            }
        }
    }

    private void SpawnVFX(GameObject vfxPrefab, Vector3 position)
    {
        if (vfxPrefab != null)
        {
            GameObject vfx = Instantiate(vfxPrefab, position, Quaternion.identity);
            Destroy(vfx, 2.0f);
        }
    }

    private void PerformWarp()
    {
        Vector3 targetPos = transform.position + (transform.forward * WARP_DISTANCE);
        transform.position = targetPos;
        SpawnVFX(warpVFX, transform.position);
    }

    private void HandleBuffs()
    {
        if (buffSpeedTimer > 0) buffSpeedTimer -= Time.deltaTime;
        if (buffDamageTimer > 0) buffDamageTimer -= Time.deltaTime;
        if (buffFreezeTimer > 0) buffFreezeTimer -= Time.deltaTime;
    }

    public void ClearAllBuffs()
    {
        buffSpeedTimer = 0f;
        buffDamageTimer = 0f;
        buffFreezeTimer = 0f;
    }

    public void Attack(bool isAttacking)
    {
        if (isAttacking)
        {
            animator.SetTrigger("Attack");
            int finalDamage = Damage;
            if (buffDamageTimer > 0) finalDamage *= DAMAGE_MULTIPLIER;

            var e = InFront as IDestroyable;
            if (e != null)
            {
                e.TakeDamage(finalDamage);
                if (buffFreezeTimer > 0)
                {
                    MonoBehaviour enemyMono = InFront as MonoBehaviour;
                    if (enemyMono != null) SpawnVFX(freezeHitVFX, enemyMono.transform.position);
                }
            }
            _isAttacking = false;
        }
    }

    // +++ แก้ไขตรงนี้: เพิ่มการอัปเดต UI ตอนเก็บของ +++
    public void AddItem(string itemName, int amount)
    {
        if (inventory.ContainsKey(itemName)) inventory[itemName] += amount;
        else inventory.Add(itemName, amount);

        Debug.Log($"[Inventory] เก็บของ: {itemName} (จำนวน {amount}) | รวมเป็น: {inventory[itemName]}");

        // ถ้าของที่ได้คือ Big Potion ให้ไปบอก UI ให้เปลี่ยนตัวเลข
        if (itemName == "Big Potion" && UIManager.instance != null)
        {
            UIManager.instance.UpdateBigPotionUI(inventory[itemName]);
        }
    }

    public bool HasItem(string itemName, int amount = 1) 
    { 
        return inventory.ContainsKey(itemName) && inventory[itemName] >= amount; 
    }

    // +++ แก้ไขตรงนี้: เพิ่มการอัปเดต UI ตอนใช้ของ +++
    public void RemoveItem(string itemName, int amount = 1)
    {
        if (HasItem(itemName, amount)) 
        { 
            inventory[itemName] -= amount; 
            
            // จำยอดคงเหลือไว้ก่อนลบ
            int remaining = inventory[itemName];

            if (inventory[itemName] <= 0) inventory.Remove(itemName); 
            
            Debug.Log($"[Inventory] ใช้ของ: {itemName} (จำนวน {amount})");

            // ถ้าของที่ใช้คือ Big Potion ให้ไปบอก UI ให้เปลี่ยนตัวเลข
            if (itemName == "Big Potion" && UIManager.instance != null)
            {
                UIManager.instance.UpdateBigPotionUI(remaining);
            }
        }
    }

    public void Interact(bool interactable)
    {
        if (interactable)
        {
            IInteractable e = InFront as IInteractable;
            if (e != null) e.Interact(this);
            _isInteract = false;
        }
    }

    public void TryCraft(CraftingRecipe recipe)
    {
        if (recipe == null) 
        {
            Debug.LogError("[Crafting] ล้มเหลว: ไม่พบข้อมูล Recipe (Recipe เป็น null)");
            return;
        }

        Debug.Log($"[Crafting] กำลังพยายามคราฟ: {recipe.recipeName} ...");

        bool canCraft = true;
        foreach (Ingredient ingredient in recipe.ingredients)
        {
            // เช็คว่ามีของไหม
            if (!HasItem(ingredient.itemName, ingredient.amount)) 
            { 
                int currentAmount = inventory.ContainsKey(ingredient.itemName) ? inventory[ingredient.itemName] : 0;
                Debug.LogWarning($"[Crafting] ของไม่พอ! ขาด: {ingredient.itemName} (ต้องการ {ingredient.amount} แต่มี {currentAmount})");
                canCraft = false; 
                break; 
            }
        }

        if (canCraft)
        {
            // ลบของ
            foreach (Ingredient ingredient in recipe.ingredients) 
            {
                RemoveItem(ingredient.itemName, ingredient.amount);
            }
            // เพิ่มของใหม่
            AddItem(recipe.result.itemName, recipe.result.amount);
            Debug.Log($"<color=green>[Crafting] สำเร็จ! ได้รับ {recipe.result.itemName}</color>");
        }
        else
        {
            Debug.LogWarning("<color=red>[Crafting] ล้มเหลว: วัตถุดิบไม่เพียงพอ</color>");
        }
    }

    public void ApplyPoisonDebuff(float duration, int damagePerTick)
    {
        poisonTimer = duration;
        poisonDamagePerTick = damagePerTick;
        poisonTickTimer = 0;
        if (UIManager.instance != null) UIManager.instance.AddStatusIcon(UIManager.instance.poisonIcon, duration);
    }

    public void ApplyBleedDebuff(float duration, int damagePerTick)
    {
        bleedTimer = duration;
        bleedDamagePerTick = damagePerTick;
        bleedTickTimer = 0;
        if (UIManager.instance != null) UIManager.instance.AddStatusIcon(UIManager.instance.bleedIcon, duration);
    }

    public void ApplySlowDebuff(float duration)
    {
        slowTimer = duration;
        if (UIManager.instance != null) UIManager.instance.AddStatusIcon(UIManager.instance.slowIcon, duration);
    }

    private void HandleDebuffs()
    {
        if (poisonTimer > 0) { poisonTimer -= Time.deltaTime; poisonTickTimer -= Time.deltaTime; if (poisonTickTimer <= 0) { TakeDamage(poisonDamagePerTick); poisonTickTimer = 1f; } }
        if (bleedTimer > 0) { bleedTimer -= Time.deltaTime; bleedTickTimer -= Time.deltaTime; if (bleedTickTimer <= 0) { TakeDamage(bleedDamagePerTick); bleedTickTimer = 1f; } }
        if (slowTimer > 0) slowTimer -= Time.deltaTime;
    }

    public void UnlockSkill(string skillName) { if (!unlockedSkills.Contains(skillName)) unlockedSkills.Add(skillName); }
    public bool IsSkillUnlocked(string skillName) { return unlockedSkills.Contains(skillName); }
    private void UseSkill_Fireball() { if (IsSkillUnlocked("Fireball")) Debug.Log("Casting FIREBALL!"); }

    public override void TakeDamage(int amount)
    {
        if (health <= 0) return;
        health -= Mathf.Clamp(amount - Deffent, 1, amount);
        if (GameManager.instance != null) GameManager.instance.UpdateHealthBar(health, maxHealth);

        if (health <= 0)
        {
            ClearAllBuffs();
            if (GameManager.instance != null) GameManager.instance.RespawnPlayer(this);
        }
    }

    public override void Heal(int amount)
    {
        if (amount < 0) return;
        if (maxHealth <= 0) maxHealth = 100;

        health += amount;
        if (health > maxHealth) health = maxHealth;

        if (GameManager.instance != null) GameManager.instance.UpdateHealthBar(health, maxHealth);
    }

    public void ClearAllStatus()
    {
        buffSpeedTimer = 0f;
        buffDamageTimer = 0f;
        buffFreezeTimer = 0f;

        poisonTimer = 0f;
        bleedTimer = 0f;
        slowTimer = 0f;

        poisonTickTimer = 0f;
        bleedTickTimer = 0f;

        Debug.Log("Status Cleared!");
    }
}
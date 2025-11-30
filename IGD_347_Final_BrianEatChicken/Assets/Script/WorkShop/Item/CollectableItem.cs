using UnityEngine;

// ผมเดาว่าคลาสแม่ (Item) มีฟังก์ชัน OnCollect นะครับ
public class CollectableItem : Item
{
    // --- 1. เพิ่ม 2 ช่องนี้ ---
    // ตั้งค่าใน Inspector ว่าไอเทมชิ้นนี้คืออะไร
    [Header("Item Data")]
    public string itemName; // เช่น "ไม้", "เงิน", "Health Potion"
    public int amount = 1;  // จะได้กี่ชิ้นเมื่อเก็บ

    // --- 2. 'value' ผมจะเปลี่ยนชื่อเป็น 'scoreValue' เพื่อกันสับสน ---
    public int scoreValue = 10;

    // (ผมลบ constructor เก่าของคุณออก เพราะไม่จำเป็น
    // ถ้าเราตั้งค่าใน Inspector แทน)

    // --- 3. แก้ไข OnCollect ---
    public override void OnCollect(Player player)
    {
        base.OnCollect(player); // เรียกคลาสแม่ (ถ้ามี)

        // 3a. ตรวจสอบว่า player มีจริง
        if (player != null)
        {
            // 3b. (สำคัญ!) เรียก AddItem เวอร์ชันใหม่
            player.AddItem(itemName, amount);
        }

        // (ไม่บังคับ) ถ้าคุณอยากให้เก็บแล้วได้คะแนนด้วย:
        // if (GameManager.instance != null)
        // {
        //     GameManager.instance.AddScore(scoreValue);
        // }

        // ซ่อนไอเทม (เหมือนเดิม)
        gameObject.SetActive(false);
    }
}

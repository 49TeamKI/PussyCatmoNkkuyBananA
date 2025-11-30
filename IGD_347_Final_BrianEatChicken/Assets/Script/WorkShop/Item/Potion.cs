using UnityEngine;

// (เราจะเปลี่ยนจาก "Item" เป็น "CollectableItem" ที่เราเคยทำ
// หรือถ้า Potion.cs ต้องสืบทอดจาก Item.cs จริงๆ ก็ใช้แบบนี้ครับ)

public class Potion : Item // (สืบทอดจาก Item เหมือนเดิม)
{
    // --- 1. เพิ่ม 2 ช่องนี้ ---
    [Header("Item Data")]
    public string itemName = "Health Potion"; // (สำคัญ!) ชื่อนี้ต้องตรงกับที่ Player.cs ใช้ (ตอนกด 'H')
    public int amount = 1;

    // (เราจะไม่ใช้ AmountHealth ตรงนี้แล้ว)
    // public int AmountHealth = 20; 

    // --- 2. แก้ไข OnCollect ---
    public override void OnCollect(Player player)
    {
        base.OnCollect(player); // (เรียกคลาสแม่ ถ้าจำเป็น)

        // (สำคัญ!) เปลี่ยนจาก "Heal" เป็น "AddItem"
        player.AddItem(itemName, amount);

        // (ลบ player.Heal(AmountHealth); ทิ้งไป)

        Destroy(gameObject); // (เก็บแล้วหายไป เหมือนเดิม)
    }
}
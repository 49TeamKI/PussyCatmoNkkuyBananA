using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    [Header("ตั้งค่าไอเทม")]
    [Tooltip("ชื่อต้องตรงกับ Player.cs เช่น: Speed Scroll, Power Rune, Warp Stone, Ice Gem")]
    public string itemName;

    [Tooltip("จำนวน")]
    public int amount = 1;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Player.instance.AddItem(itemName, amount);
            Destroy(gameObject); 
        }
    }
}
using UnityEngine;

public class Coin : Item
{
    public int ScoreValue = 10;
    public AudioClip SoundCoin;

    public override void OnCollect(Player player)
    {
        base.OnCollect(player);
        GameManager.instance.AddScore(ScoreValue);
        SoundManager.instance.PlaySFX(SoundCoin);

        // --- (เพิ่มบรรทัดนี้ครับ!) ---
        // (สั่งให้ Player "เก็บ" เหรียญนี้เข้า Inventory ด้วย)
        if (player != null)
        {
            player.AddItem("Coin", 10); // (สำคัญ!) "Coin" ต้องสะกดตรงกับที่ SkillManager ใช้
        }
        // --- (สิ้นสุดส่วนที่เพิ่ม) ---

        Destroy(gameObject);
    }
}
using UnityEngine;
using TMPro; // +++ [สำคัญ] เพิ่มบรรทัดนี้เพื่อให้รู้จัก TextMeshPro +++

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    [Header("UI References")]
    [SerializeField] private GameObject statusIconPrefab;
    [SerializeField] private Transform statusPanel;

    // +++ [ส่วนที่เพิ่มใหม่] สำหรับโชว์จำนวน Big Potion +++
    [Header("Inventory UI")]
    public TMP_Text bigPotionText; // ลาก TextMeshPro มาใส่ช่องนี้
    // ++++++++++++++++++++++++++++++++++++++++++++++++

    [Header("Debuff Icons")]
    public Sprite poisonIcon;
    public Sprite bleedIcon;
    public Sprite slowIcon;

    // +++ [เพิ่มส่วนนี้] ไอคอนสำหรับ Buff ของผู้เล่น +++
    [Header("Buff Icons")]
    public Sprite speedBuffIcon;
    public Sprite damageBuffIcon;
    public Sprite freezeBuffIcon; // หรือ Ice Gem
    public Sprite warpStoneBuffIcon;
    // +++++++++++++++++++++++++++++++++++++++++++

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    // +++ [ฟังก์ชันใหม่] สำหรับอัปเดตตัวเลข +++
    public void UpdateBigPotionUI(int amount)
    {
        if (bigPotionText != null)
        {
            // ผลลัพธ์จะออกมาเป็น "x 1", "x 2" เป็นต้น
            bigPotionText.text = "x " + amount.ToString();
        }
    }
    // +++++++++++++++++++++++++++++++++++++++

    public void AddStatusIcon(Sprite icon, float duration)
    {
        if (icon == null) return; // กัน Error กรณีลืมใส่รูป

        GameObject newIcon = Instantiate(statusIconPrefab, statusPanel);
        StatusIcon script = newIcon.GetComponent<StatusIcon>();
        if (script != null)
        {
            script.Setup(icon, duration);
        }
    }
}